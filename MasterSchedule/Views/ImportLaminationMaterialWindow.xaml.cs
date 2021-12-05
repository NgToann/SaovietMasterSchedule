using MasterSchedule.Controllers;
using MasterSchedule.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportLaminationMaterialWindow.xaml
    /// </summary>
    public partial class ImportLaminationMaterialWindow : Window
    {
        string[] filePathArray;
        BackgroundWorker bwReadExcel;
        BackgroundWorker bwUpload;
        BackgroundWorker bwLoad;
        List<LaminationMaterialModel> laminationMatsList;
        List<LaminationMaterialModel> laminationMatsSearchList;
        private EExcute eAction = EExcute.None;
        public ImportLaminationMaterialWindow()
        {
            bwReadExcel = new BackgroundWorker();
            bwReadExcel.DoWork += BwReadExcel_DoWork;
            bwReadExcel.RunWorkerCompleted += BwReadExcel_RunWorkerCompleted;

            bwUpload = new BackgroundWorker();
            bwUpload.DoWork += BwUpload_DoWork;
            bwUpload.RunWorkerCompleted += BwUpload_RunWorkerCompleted;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork; 
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;
            
            laminationMatsList = new List<LaminationMaterialModel>();
            laminationMatsSearchList = new List<LaminationMaterialModel>();
            InitializeComponent();
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false && e.Error == null && (bool)e.Result == true)
            {
                dgLaminationMaterial.ItemsSource = laminationMatsSearchList;
                dgLaminationMaterial.Items.Refresh();
            }
            this.Cursor = null;
            btnFilter.IsEnabled = true;

            // Create MenuContext
            dgLaminationMaterial.ContextMenu = null;
            var ctm = new ContextMenu();
            var menuItem = new MenuItem();
            menuItem.Header = "Remove";
            menuItem.Click += MenuItem_Click; ;
            ctm.Items.Add(menuItem);
            dgLaminationMaterial.ContextMenu = ctm;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgLaminationMaterial.ItemsSource == null)
                return;
            var itemsSelected = dgLaminationMaterial.SelectedItems.OfType<LaminationMaterialModel>().ToList();

            if (MessageBox.Show(string.Format("Confirm remove {0} item{1} ?", itemsSelected.Count(), itemsSelected.Count() > 1 ? "s" : ""), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK
                || itemsSelected.Count() == 0)
            {
                return;
            }
            if (bwUpload.IsBusy == false)
            {
                txtStatus.Text = "";
                this.Cursor = Cursors.Wait;
                eAction = EExcute.Delete;
                bwUpload.RunWorkerAsync(itemsSelected);
            }
        }

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            string loadWhat = e.Argument as string;
            try
            {
                laminationMatsSearchList = LaminationMaterialController.GetByOrderNo(loadWhat);
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.InnerException.InnerException.Message.ToString());
                }));
            }
        }

        private void BwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null && e.Cancelled == false && (bool)e.Result == true && eAction == EExcute.AddNew)
            {
                MessageBox.Show(string.Format("Imported !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (e.Error == null && e.Cancelled == false && (bool)e.Result == true && eAction == EExcute.Delete)
            {
                MessageBox.Show(string.Format("Deleted !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                dgLaminationMaterial.ItemsSource = laminationMatsSearchList;
                dgLaminationMaterial.Items.Refresh();
            }

            txtStatus.Text = "Completed !";
            prgStatus.Value = 0;
            this.Cursor = null;
            btnImport.IsEnabled = true;
        }

        private void BwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            var importList = e.Argument as List<LaminationMaterialModel>;
            Dispatcher.Invoke(new Action(() =>
            {
                txtStatus.Text = "Uploading ...";
                prgStatus.Maximum = importList.Count();
            }));
            int index = 1;
            foreach (var item in importList)
            {
                try
                {
                    if (eAction == EExcute.AddNew)
                    {
                        
                        LaminationMaterialController.Insert(item);
                        dgLaminationMaterial.Dispatcher.Invoke(new Action(() =>
                        {
                            dgLaminationMaterial.SelectedItem = item;
                            dgLaminationMaterial.ScrollIntoView(item);
                        }));
                        Dispatcher.Invoke(new Action(() =>
                        {
                            txtStatus.Text = "Importing ... ";
                            prgStatus.Value = index;
                        }));
                    }
                    else if (eAction == EExcute.Delete)
                    {
                        LaminationMaterialController.DeleteByOrderNoId(item.OrderNoId);
                        laminationMatsSearchList.Remove(item);
                    }

                    e.Result = true;
                    index++;
                }
                catch (Exception ex)
                {
                    e.Result = false;
                    Dispatcher.Invoke(new Action(() => {
                        MessageBox.Show(ex.InnerException.InnerException.Message.ToString());
                    }));
                    break;
                }
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (dgLaminationMaterial.ItemsSource == null)
                return;
            var importList = dgLaminationMaterial.ItemsSource.OfType<LaminationMaterialModel>().ToList();
            if (bwUpload.IsBusy==false && importList.Count() > 0)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                eAction = EExcute.AddNew;
                bwUpload.RunWorkerAsync(importList);
            }
        }

        private void btnOpenExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Lamination Material Excel File";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                filePathArray = openFileDialog.FileNames;
                if (bwReadExcel.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    btnOpenExcel.IsEnabled = false;
                    laminationMatsList.Clear();
                    bwReadExcel.RunWorkerAsync();
                }
            }
            else
            {

            }
        }
        
        private void BwReadExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                txtStatus.Text = "Reading ...";
                prgStatus.Maximum = filePathArray.Count();
            }));
            int filePathIndex = 1;
            foreach (string filePath in filePathArray)
            {
                Excel.Application excelApplication  = new Excel.Application();
                Excel.Workbook excelWorkbook        = excelApplication.Workbooks.Open(filePath);
                Excel.Worksheet excelWorksheet;
                Excel.Range excelRange;
                try
                {
                    excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                    excelRange = excelWorksheet.UsedRange;

                    var orderNoCell = excelRange[4, 1].Value2;
                    string orderNo = orderNoCell != null ? orderNoCell.ToString() : "";
                    if (orderNo.Contains("."))
                    {
                        var cellSplit = orderNo.Split('.');
                        orderNo = cellSplit[1].Trim().ToString();
                    }

                    // Looking for purchasedate, delivery date
                    string purchaseDate = "", deliveryDate = "";
                    for (int colPur = 2; colPur < 100; colPur ++)
                    {
                        var dateCell = excelRange[4, colPur].Value2;
                        if (dateCell != null)
                        {
                            string dateString = dateCell.ToString();
                            if (dateString.ToUpper().Contains("PURCHASE"))
                            {
                                if (dateString.Contains(":"))
                                    purchaseDate = dateString.Split(':')[1].ToUpper().ToString();
                            }
                            else if (dateString.ToUpper().Contains("DELIVERY"))
                            {
                                if (dateString.Contains(":"))
                                    deliveryDate = dateString.Split(':')[1].ToUpper().ToString();
                                break;
                            }
                        }

                    }

                    var articleCell = excelRange[5, 1].Value2;
                    string articleNo = articleCell != null ? articleCell.ToString() : "";
                    if (articleNo.Contains(":"))
                    {
                        var cellSplit = articleNo.Split(':');
                        articleNo = cellSplit[1].Trim().ToString();
                    }

                    // Looking for productNo cell x,y
                    int colPO = 0;
                    for (int colPOCheck = 2; colPOCheck < 100; colPOCheck++)
                    {
                        var poCheckCell = excelRange[5, colPOCheck].Value2;
                        if (poCheckCell != null)
                        {
                            string poCheck = poCheckCell.ToString();
                            if (poCheck.ToUpper().Contains("PROD. NO.:"))
                            {
                                colPO = colPOCheck + 1;
                                break;
                            }
                        }
                    }
                    // Get ProductNoList
                    List<String> productNoList = new List<string>();
                    int rowNumberOfMaterial = 0;
                    for (int c = colPO; c < 100; c++)
                    {
                        var poColumnValueCheck = excelRange[5, c].Value2;
                        if (poColumnValueCheck == null)
                            continue;
                        for (int r = 5; r < 100; r++)
                        {
                            var cellBreak = excelRange[r, 1].Value2;
                            if (cellBreak != null)
                            {
                                string cellBreakString = cellBreak.ToString();
                                if (cellBreakString.ToUpper().Equals("NO."))
                                {
                                    rowNumberOfMaterial = r + 1;
                                    break;
                                }
                            }
                            var poCell = excelRange[r, c].Value2;
                            if (poCell == null)
                                continue;

                            string po = poCell.ToString();
                            if (!String.IsNullOrEmpty(po))
                            {
                                if (po.Contains("("))
                                {
                                    var cellSplit = po.Split('(');
                                    po = cellSplit[0].Trim().ToString();
                                }
                                productNoList.Add(po);
                            }

                        }
                    }
                    

                    var shoeNameCell = excelRange[6, 1].Value2;
                    string shoeName = shoeNameCell != null ? shoeNameCell.ToString() : "";
                    if (shoeName.Contains(":"))
                    {
                        var cellSplit = shoeName.Split(':');
                        shoeName = cellSplit[1].Trim().ToString();
                    }

                    var patternCell = excelRange[7, 1].Value2;
                    string patternNo = patternCell != null ? patternCell.ToString() : "";
                    if (patternNo.Contains(":"))
                    {
                        var cellSplit = patternNo.Split(':');
                        patternNo = cellSplit[1].Trim().ToString();
                    }

                    //var purchaseDateCell = excelRange[4, 6].Value2;
                    //string purchaseDate = purchaseDateCell != null ? purchaseDateCell.ToString() : "";
                    //if (purchaseDate.Contains(":"))
                    //{
                    //    var cellSplit = purchaseDate.Split(':');
                    //    purchaseDate = cellSplit[1].Trim().ToString();
                    //}

                    //var deliveryDateCell = excelRange[4, 12].Value2;
                    //string deliveryDate = deliveryDateCell != null ? deliveryDateCell.ToString() : "";
                    //if (deliveryDate.Contains(":"))
                    //{
                    //    var cellSplit = deliveryDate.Split(':');
                    //    deliveryDate = cellSplit[1].Trim().ToString();
                    //}

                    int prodQty = 0, sendQty = 0;
                    string position = "", part = "", materialName = "", unit = "";//, prodQtyString = "", sendQtyString = "";
                    
                    // check by part
                    int colPart = 2;
                    var cellPartCheck = excelRange[rowNumberOfMaterial, colPart].Value2;
                    while (cellPartCheck == null || (cellPartCheck == null && cellPartCheck.ToString().ToUpper() != "PART"))
                    {
                        colPart++;
                        cellPartCheck = excelRange[rowNumberOfMaterial, colPart].Value2;
                    }
                    if (cellPartCheck != null)
                    {
                        int colPosition = 0, colMaterial = 0, colUnit = 0, colProdQty = 0, colSendQty = 0;
                        for (int col = 1; col < 100; col++)
                        {
                            // row position, prodQty, sendQty
                            var cellHeaderCheck = excelRange[rowNumberOfMaterial - 1, col].Value2;
                            // the same row with part cell
                            var cellHeader_1Check = excelRange[rowNumberOfMaterial, col].Value2;
                            
                            // Position
                            if (cellHeaderCheck != null
                                && cellHeaderCheck.ToString().ToUpper() == "POSITION"
                                && colPosition == 0)
                            {
                                colPosition = col;
                            }

                            if (cellHeader_1Check != null
                                && cellHeader_1Check.ToString().ToUpper() == "MATERIAL"
                                && colMaterial == 0)
                            {
                                colMaterial = col;
                            }

                            if (cellHeader_1Check != null
                                && cellHeader_1Check.ToString().ToUpper() == "UNIT"
                                && colUnit == 0)
                            {
                                colUnit = col;
                            }

                            if (cellHeaderCheck != null
                                && cellHeaderCheck.ToString().ToUpper() == "PROD. QTY"
                                && colProdQty == 0)
                            {
                                colProdQty = col;
                            }

                            if (cellHeaderCheck != null
                                && cellHeaderCheck.ToString().ToUpper() == "SEND QTY"
                                && colSendQty == 0)
                            {
                                colSendQty = col;
                                break;
                            }
                        }
                        int id = 1;
                        for (int r = rowNumberOfMaterial + 1; r < excelRange.Rows.Count; r++)
                        {
                            string orderId = "";
                            var cellPart = excelRange[r, colPart].Value2;
                            if (cellPart != null && !String.IsNullOrEmpty(cellPart.ToString()))
                            {
                                part = cellPart.ToString();
                                orderId = string.Format("{0}-{1}", orderNo, id);
                                var latestModel = laminationMatsList.OrderBy(o => o.Id).LastOrDefault();
                                
                                // POSITION
                                var cellPosition = excelRange[r, colPosition].Value2;
                                if (cellPosition != null)
                                {
                                    position = cellPosition.ToString();
                                }
                                else if (latestModel != null)
                                {
                                    position = latestModel.Position;
                                }

                                // Material
                                var cellMaterial = excelRange[r, colMaterial].Value2;
                                if (cellMaterial != null)
                                {
                                    materialName = cellMaterial.ToString();
                                }
                                else if (latestModel != null)
                                {
                                    materialName = latestModel.MaterialName;
                                }

                                // UNIT
                                var cellUnit = excelRange[r, colUnit].Value2;
                                if (cellUnit != null)
                                {
                                    unit = cellUnit.ToString();
                                }
                                else if (latestModel != null)
                                {
                                    unit = latestModel.Unit;
                                }

                                // PROD.QTY
                                var cellProdQty = excelRange[r, colProdQty].Value2;
                                if ( cellProdQty != null)
                                {
                                    Int32.TryParse(cellProdQty.ToString(), out prodQty);
                                }
                                else if (latestModel != null)
                                {
                                    prodQty = latestModel.POQuantity;
                                }

                                // SEND QTY
                                var cellSendQty = excelRange[r, colSendQty].Value2;
                                if (cellProdQty != null)
                                {
                                    Int32.TryParse(cellSendQty.ToString(), out sendQty);
                                }
                                else if (latestModel != null)
                                {
                                    sendQty = latestModel.SendQuantity;
                                }
                                var laminationModel = new LaminationMaterialModel
                                {
                                    Id = id,
                                    OrderNoId = orderId,
                                    OrderNo = orderNo,
                                    ProductNoList = String.Join("; ", productNoList),
                                    ArticleNo = articleNo,
                                    ShoeName = shoeName,
                                    PatternNo = patternNo,
                                    MaterialName = materialName,
                                    MaterialPart = part,
                                    Position = position,
                                    Unit = unit,
                                    POQuantity = prodQty,
                                    SendQuantity = sendQty,
                                    DeliveryDate = deliveryDate,
                                    PurchaseDate = purchaseDate,
                                    SupplierName = "",
                                    Remarks = "",
                                };
                                laminationMatsList.Add(laminationModel);
                                
                                id++;
                            }
                        }
                    }
                    //for (int r = rowNumberOfMaterial; r < excelRange.Rows.Count; r++)
                    //{
                    //    int rowHeader = rowNumberOfMaterial - 1;
                    //    var cellCheck = excelRange[r, 1].Value2;
                    //    string orderId = "";
                        
                        
                    //    if (cellCheck != null)
                    //    {
                    //        string noOfMaterial = cellCheck.ToString();
                    //        int checkNumber = 0;
                    //        Int32.TryParse(noOfMaterial, out checkNumber);
                    //        if (checkNumber == 0)
                    //            break;
                    //        orderId = String.Format("{0}-{1}", orderNo, checkNumber);
                    //        for (int colHeader = 2; colHeader < 100; colHeader++)
                    //        {
                    //            var headerCell = excelRange[rowHeader, colHeader].Value2;
                    //            if (headerCell != null)
                    //            {
                    //                string header = headerCell.ToString();
                    //                header = header.ToUpper();
                    //                if (header.Equals("POSITION"))
                    //                {
                    //                    var positionCell = excelRange[r, colHeader].Value2;
                    //                    position = positionCell != null ? positionCell.ToString() : "";
                    //                }
                    //                else if (header.Equals("MATERIAL DESCRIPTION"))
                    //                {
                    //                    for (int colHeaderDetail = colHeader; colHeaderDetail < 100; colHeaderDetail++)
                    //                    {
                    //                        var headerDetailCell = excelRange[rowHeader + 1, colHeaderDetail].Value2;
                    //                        if (headerDetailCell != null)
                    //                        {
                    //                            string headerDetail = headerDetailCell.ToString();
                    //                            headerDetail = headerDetail.ToUpper();
                    //                            if (headerDetail.Equals("PART"))
                    //                            {
                    //                                var partCell = excelRange[r, colHeaderDetail].Value2;
                    //                                part = partCell != null ? partCell.ToString() : "";
                    //                            }
                    //                            else if (headerDetail.Equals("MATERIAL"))
                    //                            {
                    //                                var materialNameCell = excelRange[r, colHeaderDetail].Value2;
                    //                                materialName = materialNameCell != null ? materialNameCell.ToString() : "";
                    //                            }
                    //                            else if (headerDetail.Equals("UNIT"))
                    //                            {
                    //                                var unitCell = excelRange[r, colHeaderDetail].Value2;
                    //                                unit = unitCell != null ? unitCell.ToString() : "";
                    //                                break;
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                else if (header.Equals("PROD. QTY"))
                    //                {
                    //                    var prodQtyCell = excelRange[r, colHeader].Value2;
                    //                    prodQtyString = prodQtyCell != null ? prodQtyCell.ToString() : "";
                    //                    Int32.TryParse(prodQtyString, out prodQty);
                    //                }
                    //                else if (header.Equals("SEND QTY"))
                    //                {
                    //                    var sendQtyCell = excelRange[r, colHeader].Value2;
                    //                    sendQtyString = sendQtyCell != null ? sendQtyCell.ToString() : "";
                    //                    Int32.TryParse(sendQtyString, out sendQty);
                    //                    break;
                    //                }
                    //            }
                    //        }
                            
                    //        var laminationModel = new LaminationMaterialModel
                    //        {
                    //            OrderNoId = orderId,
                    //            OrderNo = orderNo,
                    //            ProductNoList = String.Join("; ", productNoList),
                    //            ArticleNo = articleNo,
                    //            ShoeName = shoeName,
                    //            PatternNo = patternNo,
                    //            MaterialName = materialName,
                    //            MaterialPart = part,
                    //            Position = position,
                    //            Unit = unit,
                    //            POQuantity = prodQty,
                    //            SendQuantity = sendQty,
                    //            DeliveryDate = deliveryDate,
                    //            PurchaseDate = purchaseDate,
                    //            SupplierName = "",
                    //            Remarks = "",
                    //        };
                    //        laminationMatsList.Add(laminationModel);
                    //    }
                    //}
                    Dispatcher.Invoke(new Action(() =>
                    {
                        txtStatus.Text = String.Format("Reading OrderNo: {0}  total file: {1}/{2}", orderNo, filePathIndex, filePathArray.Count());
                        prgStatus.Value = filePathIndex;
                    }));

                }
                catch
                {
                }
                finally
                {
                    excelWorkbook.Close(false, Missing.Value, Missing.Value);
                    excelApplication.Quit();
                }
                filePathIndex++;
            }
        }
        
        private void BwReadExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (e.Cancelled == true)
            {
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show(string.Format("Error\n{0}", e.Error.Message), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (laminationMatsList.Count <= 0)
            {
                MessageBox.Show("Cannot Read Excel File. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show(string.Format("Read Completed !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            txtStatus.Text = String.Format("Read completed: {0} records", laminationMatsList.Count());

            prgStatus.Value = 0;
            dgLaminationMaterial.ItemsSource = laminationMatsList;
            dgLaminationMaterial.Items.Refresh();
            btnOpenExcel.IsEnabled = true;
        }

        private void dgLaminationMaterial_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void txtOrderNo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            btnFilter.IsDefault = true;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string loadWhat = "";
            loadWhat = txtOrderNo.Text.Trim().ToString();
            if (bwLoad.IsBusy == false && !string.IsNullOrEmpty(loadWhat))
            {
                this.Cursor = Cursors.Wait;
                btnFilter.IsDefault = false;
                dgLaminationMaterial.ItemsSource = null;
                laminationMatsSearchList.Clear();
                bwLoad.RunWorkerAsync(loadWhat);
            }
        }
    }
}
