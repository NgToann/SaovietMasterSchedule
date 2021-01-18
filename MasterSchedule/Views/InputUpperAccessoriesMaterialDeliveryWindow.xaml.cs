using MasterSchedule.Controllers;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputMaterialDeliveryWindow.xaml
    /// </summary>
    public partial class InputUpperAccessoriesMaterialDeliveryWindow : Window
    {
        private string productNo;
        private AccountModel account;
        RawMaterialViewModel rawMaterialView;
        BackgroundWorker bwLoad;
        BackgroundWorker bwUpload;
        List<SizeRunModel> sizeRunList;
        List<MaterialDeliveryModel> matsDeliveryByPOList;
        List<MaterialPlanModel> matsPlanByPOList;
        List<SupplierModel> supplierList;
        DataTable dtDelivery;
        private const string RowQuantity = "Quantity", RowReject = "Reject", RowRejectSewing = "Reject Sewing";
        private DateTime dtDefault = new DateTime(2000, 01, 01);
        private EExcute eAction = EExcute.None;
        public InputUpperAccessoriesMaterialDeliveryWindow(string productNo, AccountModel account, RawMaterialViewModel rawMaterialView)
        {
            this.productNo  = productNo;
            this.account    = account;
            this.rawMaterialView = rawMaterialView;
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            bwUpload = new BackgroundWorker();
            bwUpload.DoWork += BwUpload_DoWork; 
            bwUpload.RunWorkerCompleted += BwUpload_RunWorkerCompleted;

            sizeRunList          = new List<SizeRunModel>();
            matsDeliveryByPOList = new List<MaterialDeliveryModel>();
            matsPlanByPOList     = new List<MaterialPlanModel>();
            dtDelivery           = new DataTable();
            supplierList = new List<SupplierModel>();
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy==false)
            {
                tblTitle.Text = tblTitle.Text + productNo;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sizeRunList             = SizeRunController.Select(productNo);
                matsDeliveryByPOList    = MaterialDeliveryController.GetMaterialDeliveryByPO(productNo);
                matsPlanByPOList        = MaterialPlanController.GetMaterialPlanByPO(productNo);
                supplierList            = SupplierController.GetSuppliersAccessories();
                Dispatcher.Invoke(new Action(() =>
                {
                    // Create Column Datagrid
                    dgDeliveryInfo.Columns.Clear();
                    dtDelivery = new DataTable();

                    dtDelivery.Columns.Add("Status", typeof(String));
                    dtDelivery.Columns.Add("Name", typeof(String));
                    dtDelivery.Columns.Add("SupplierId", typeof(String));
                    DataGridTemplateColumn colSuppName = new DataGridTemplateColumn();
                    colSuppName.Header = String.Format("Supplier");
                    DataTemplate templateSuppName = new DataTemplate();
                    FrameworkElementFactory tblSuppName = new FrameworkElementFactory(typeof(TextBlock));
                    templateSuppName.VisualTree = tblSuppName;
                    tblSuppName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Name")));
                    tblSuppName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblSuppName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colSuppName.CellTemplate = templateSuppName;
                    colSuppName.ClipboardContentBinding = new Binding(String.Format("Name"));
                    dgDeliveryInfo.Columns.Add(colSuppName);

                    //Column ETD
                    dtDelivery.Columns.Add("ETD", typeof(String));
                    dtDelivery.Columns.Add("ETDDate", typeof(DateTime));
                    DataGridTemplateColumn colETD = new DataGridTemplateColumn();
                    colETD.Header = String.Format("EFD");
                    colETD.MinWidth = 60;
                    DataTemplate templateETD = new DataTemplate();
                    FrameworkElementFactory tblETD = new FrameworkElementFactory(typeof(TextBlock));
                    templateETD.VisualTree = tblETD;
                    tblETD.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ETD")));
                    tblETD.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblETD.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblETD.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colETD.CellTemplate = templateETD;
                    colETD.ClipboardContentBinding = new Binding(String.Format("ETD"));
                    dgDeliveryInfo.Columns.Add(colETD);

                    //Column Actual Date
                    dtDelivery.Columns.Add("ActualDate", typeof(String));
                    dtDelivery.Columns.Add("ActualDateDate", typeof(DateTime));
                    DataGridTemplateColumn colActualDate = new DataGridTemplateColumn();
                    colActualDate.Header = String.Format("{0}\n{1}\n{2}","Order Size","Qty","Actual Date");
                    DataTemplate templateActualDate = new DataTemplate();
                    FrameworkElementFactory tblActualDate = new FrameworkElementFactory(typeof(TextBlock));
                    templateActualDate.VisualTree = tblActualDate;
                    tblActualDate.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ActualDate")));
                    tblActualDate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblActualDate.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblActualDate.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colActualDate.CellTemplate = templateActualDate;
                    colActualDate.ClipboardContentBinding = new Binding(String.Format("ActualDate"));
                    dgDeliveryInfo.Columns.Add(colActualDate);

                    var regex = new Regex("[a-z]|[A-Z]");
                    if (sizeRunList.Count() > 0)
                        sizeRunList = sizeRunList.OrderBy(s => regex.IsMatch(s.SizeNo) ? Double.Parse(regex.Replace(s.SizeNo, "100")) : Double.Parse(s.SizeNo)).ToList();
                    foreach (var sizeRun in sizeRunList)
                    {
                        string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                        dtDelivery.Columns.Add(String.Format("Column{0}", sizeBinding), typeof(String));
                        dtDelivery.Columns.Add(String.Format("Column{0}Foreground", sizeBinding), typeof(SolidColorBrush));
                        //dtDelDetail.Columns.Add(String.Format("Column{0}ToolTip", sizeBinding), typeof(String));
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.SetValue(TagProperty, sizeRun.SizeNo);
                        column.Header = string.Format("{0}\n\n{1}", sizeRun.SizeNo, sizeRun.Quantity);
                        column.MinWidth = 40;
                        column.MaxWidth = 200;
                        column.Binding = new Binding(String.Format("Column{0}", sizeBinding));

                        Style styleColumn = new Style();
                        Setter setterColumnForecolor = new Setter();
                        setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                        setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", sizeBinding));

                        styleColumn.Setters.Add(setterColumnForecolor);
                        column.CellStyle = styleColumn;

                        dgDeliveryInfo.Columns.Add(column);
                    }

                    //Column Total
                    dtDelivery.Columns.Add("Balance", typeof(String));
                    DataGridTemplateColumn colBalance = new DataGridTemplateColumn();
                    colBalance.Header = String.Format("Total\n\n{0}", sizeRunList.Sum(s => s.Quantity));
                    colBalance.MinWidth = 80;
                    colBalance.MaxWidth = 80;
                    DataTemplate templateBalance = new DataTemplate();
                    FrameworkElementFactory tblBalance = new FrameworkElementFactory(typeof(TextBlock));
                    templateBalance.VisualTree = tblBalance;
                    tblBalance.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Balance")));
                    tblBalance.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblBalance.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblBalance.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colBalance.CellTemplate = templateBalance;
                    colBalance.ClipboardContentBinding = new Binding(String.Format("Balance"));
                    dgDeliveryInfo.Columns.Add(colBalance);

                    DataGridTemplateColumn colButtonOK = new DataGridTemplateColumn();
                    colButtonOK.MinWidth = 40;
                    colButtonOK.MaxWidth = 40;
                    DataTemplate templateButtonOK = new DataTemplate();
                    FrameworkElementFactory fefButtonOK = new FrameworkElementFactory(typeof(Button));
                    templateButtonOK.VisualTree = fefButtonOK;
                    fefButtonOK.SetValue(Button.ContentProperty, "Ok");
                    fefButtonOK.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnOK_Click));
                    colButtonOK.CellTemplate = templateButtonOK;
                    dgDeliveryInfo.Columns.Add(colButtonOK);

                    // Binding Data
                    if (matsDeliveryByPOList.Count() == 0)
                    {
                        foreach (var matsPlan in matsPlanByPOList)
                        {
                            var supplierByPlan = supplierList.FirstOrDefault(f => f.SupplierId == matsPlan.SupplierId);
                            foreach (var sizeRun in sizeRunList)
                            {
                                matsDeliveryByPOList.Add(new MaterialDeliveryModel
                                {
                                    SupplierId          = matsPlan.SupplierId,
                                    SupplierNameDisplay = supplierByPlan != null ? String.Format("{0} - {1}", supplierByPlan.Name, supplierByPlan.ProvideAccessories) : "",
                                    ProductNo           = matsPlan.ProductNo,
                                    ETD                 = matsPlan.ETD,
                                    ActualDeliveryDate  = dtDefault,
                                    SizeNo              = sizeRun.SizeNo,
                                    Quantity            = 0,
                                    Reject              = 0,
                                    RejectSewing        = 0
                                });
                            }
                        }
                    }
                    else
                    {
                        // Put Supplier Name - Accessories Name
                        matsDeliveryByPOList.ForEach(f => f.SupplierNameDisplay = String.Format("{0} - {1}", 
                            supplierList.FirstOrDefault(w => w.SupplierId == f.SupplierId).Name, 
                            supplierList.FirstOrDefault(w => w.SupplierId == f.SupplierId).ProvideAccessories));
                    }
                    
                    var supplierIdList = matsDeliveryByPOList.Select(s => s.SupplierId).Distinct().ToList();
                    foreach (var supplierId in supplierIdList)
                    {
                        var deliveryListBySupp = matsDeliveryByPOList.Where(w => w.SupplierId == supplierId).ToList();
                        var materialInfoBySupp = deliveryListBySupp.FirstOrDefault();

                        // Row Quantity, Reject, RejectSewing
                        DataRow drQuantity      = dtDelivery.NewRow();
                        DataRow drReject        = dtDelivery.NewRow();
                        DataRow drRejectSewing  = dtDelivery.NewRow();

                        drQuantity["Status"]        = RowQuantity;
                        drReject["Status"]          = RowReject;
                        drRejectSewing["Status"]    = RowRejectSewing;

                        drQuantity["Name"]      = materialInfoBySupp.SupplierNameDisplay;
                        drReject["Name"]        = RowReject;
                        drRejectSewing["Name"]  = RowRejectSewing;

                        drQuantity["SupplierId"]        = materialInfoBySupp.SupplierId;
                        drReject["SupplierId"]          = materialInfoBySupp.SupplierId;
                        drRejectSewing["SupplierId"]    = materialInfoBySupp.SupplierId;

                        drQuantity["ETD"]           = string.Format("{0:MM/dd}", materialInfoBySupp.ETD);
                        if (materialInfoBySupp.ActualDeliveryDate != dtDefault)
                            drQuantity["ActualDate"] = string.Format("{0:MM/dd}", materialInfoBySupp.ActualDeliveryDate);
                        
                        drQuantity["ActualDateDate"]        = materialInfoBySupp.ActualDeliveryDate;
                        drQuantity["ETDDate"] = materialInfoBySupp.ETD;

                        foreach (var sizeRun in sizeRunList)
                        {
                            string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                            var deliveryBySizeNo = deliveryListBySupp.FirstOrDefault(f => f.SizeNo == sizeRun.SizeNo);
                            if (deliveryBySizeNo != null)
                            {
                                if (deliveryBySizeNo.Quantity > 0)
                                {
                                    drQuantity[String.Format("Column{0}", sizeBinding)] = deliveryBySizeNo.Quantity;
                                    if (deliveryBySizeNo.Quantity == sizeRun.Quantity)
                                        drQuantity[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
                                }
                                if (deliveryBySizeNo.Reject > 0)
                                {
                                    drReject[String.Format("Column{0}", sizeBinding)] = deliveryBySizeNo.Reject;
                                    drReject[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                                }
                                if (deliveryBySizeNo.RejectSewing > 0)
                                {
                                    drRejectSewing[String.Format("Column{0}", sizeBinding)] = deliveryBySizeNo.RejectSewing;
                                    drRejectSewing[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                                }
                            }
                        }

                        int totalBalanceBySupp  = sizeRunList.Sum(s => s.Quantity) - deliveryListBySupp.Sum(s => s.Quantity);
                        int totalReject         = deliveryListBySupp.Sum(s => s.Reject);
                        int totalRejectSewing   = deliveryListBySupp.Sum(s => s.RejectSewing);
                        
                        drQuantity["Balance"]   = totalBalanceBySupp > 0 ? totalBalanceBySupp.ToString() : "";
                        drReject["Balance"]     = totalReject > 0 ? totalReject.ToString() : "";
                        drRejectSewing["Balance"] = totalRejectSewing > 0 ? totalRejectSewing.ToString() : "";

                        dtDelivery.Rows.Add(drQuantity);
                        dtDelivery.Rows.Add(drReject);
                        dtDelivery.Rows.Add(drRejectSewing);
                    }

                    dgDeliveryInfo.ItemsSource = dtDelivery.AsDataView();
                }));
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.InnerException.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!account.MaterialDelivery)
                return;
            if (dgDeliveryInfo.CurrentItem == null)
                return;
            var drClicked = ((DataRowView)dgDeliveryInfo.CurrentItem).Row;
            if (!drClicked["Status"].ToString().Equals(RowQuantity))
                return;
            var supplierIdOK = drClicked["SupplierId"].ToString();
            foreach (var sizeRun in sizeRunList)
            {
                string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                drClicked[String.Format("Column{0}", sizeBinding)] = sizeRun.Quantity;
                drClicked[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
            }
            drClicked["Balance"] = "";

            // Update Actual Date
            int totalDeliveryOld = matsDeliveryByPOList.Where(w => w.SupplierId.ToString().Equals(supplierIdOK)).Sum(s => s.Quantity);
            if (totalDeliveryOld != sizeRunList.Sum(s => s.Quantity))
            {
                drClicked["ActualDate"] = string.Format("{0:MM/dd}", DateTime.Now);
                drClicked["ActualDateDate"] = DateTime.Now;
            }
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void dgDeliveryInfo_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (dgDeliveryInfo.CurrentItem == null)
                return;
            var drEditting = ((DataRowView)e.Row.Item).Row;
            if (e.Column.GetValue(TagProperty) == null)
                return;
            string sizeNoEditting = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNoEditting) == false)
            {
                return;
            }

            var supplierEditting = drEditting["SupplierId"].ToString();
            var statusEditting = drEditting["Status"].ToString();

            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyCurrent = 0;
            Int32.TryParse(txtCurrent.Text.ToString(), out qtyCurrent);

            // Validate
            int qtyOld = 0;
            var matsOldBySize = matsDeliveryByPOList.FirstOrDefault(f => f.SupplierId.ToString().Equals(supplierEditting) && f.SizeNo.Equals(sizeNoEditting));
            if (matsOldBySize != null)
            {
                if (statusEditting.Equals(RowQuantity))
                    qtyOld = matsOldBySize.Quantity;
                else if (statusEditting.Equals(RowReject))
                    qtyOld = matsOldBySize.Reject;
                else
                    qtyOld = matsOldBySize.RejectSewing;
            }

            int qtyAfterPlusValue = qtyOld + qtyCurrent;
            int qtyOrderBySize = sizeRunList.FirstOrDefault(f => f.SizeNo == sizeNoEditting).Quantity;

            if (qtyAfterPlusValue > qtyOrderBySize)
                qtyAfterPlusValue = qtyOrderBySize;
            else if (qtyAfterPlusValue <= 0)
                qtyAfterPlusValue = 0;

            var sizeNoBind = sizeNoEditting.Contains(".") ? sizeNoEditting.Replace(".", "@") : sizeNoEditting;
            txtCurrent.Text = qtyAfterPlusValue > 0 ? qtyAfterPlusValue.ToString() : "";
            drEditting[String.Format("Column{0}", sizeNoBind)] = txtCurrent.Text;

            // Highlight Cell
            if (statusEditting == RowQuantity)
            {
                drEditting[String.Format("Column{0}Foreground", sizeNoBind)] = Brushes.Black;
                if (qtyAfterPlusValue == qtyOrderBySize)
                    drEditting[String.Format("Column{0}Foreground", sizeNoBind)] = Brushes.Blue;
            }
            else
            {
                drEditting[String.Format("Column{0}Foreground", sizeNoBind)] = Brushes.Red;
            }

            // Collect data at the row editting
            int totalQtyAtRow = 0;
            foreach (var sizeRun in sizeRunList)
            {
                int qty = 0;
                var sizeNo = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                Int32.TryParse(drEditting[String.Format("Column{0}", sizeNo)].ToString(), out qty);
                totalQtyAtRow += qty;
            }

            int totalQtyOrder = sizeRunList.Sum(s => s.Quantity);
            int totalQtyDeliveryOld = matsDeliveryByPOList.Where(w => w.SupplierId.ToString().Equals(supplierEditting)).Sum(s => s.Quantity);

            // Update Balance and Actual Date
            if (statusEditting == RowQuantity)
            {
                drEditting["ActualDate"] = "";
                drEditting["ActualDateDate"] = dtDefault;

                if (totalQtyOrder - totalQtyAtRow >= 0)
                    drEditting["Balance"] = totalQtyOrder - totalQtyAtRow;
                if (totalQtyAtRow == totalQtyOrder &&
                    totalQtyDeliveryOld != totalQtyOrder)
                {
                    drEditting["ActualDate"] = String.Format("{0:MM/dd}", DateTime.Now);
                    drEditting["ActualDateDate"] = DateTime.Now;
                }
            }
            // Display Total Reject Or Reject Sewing
            else
            {
                drEditting["Balance"] = totalQtyAtRow;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (account.MaterialDelivery == false)
            {
                MessageBox.Show("User does not have permission excute data", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (MessageBox.Show(string.Format("Confirm delete upper accessories delivery PO: {0}?", productNo), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            if (bwUpload.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                eAction = EExcute.Delete;
                btnDelete.IsEnabled = false;
                object[] par = new object[] { new List<MaterialDeliveryModel>(), new List<MaterialPlanModel>() };
                bwUpload.RunWorkerAsync(par);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgDeliveryInfo.Items == null)
                return;
            if (account.MaterialDelivery == false)
            {
                MessageBox.Show("User does not have permission excute data", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // Get Data From Datatable
            List<MaterialDeliveryModel> matsFromGridList = new List<MaterialDeliveryModel>();
            var actualDate  = dtDefault;
            var etd         = dtDefault;
            for (int r = 0; r < dtDelivery.Rows.Count; r++)
            {
                var dr = dtDelivery.Rows[r];
                var status = dr["Status"].ToString();
                var supplierId = dr["SupplierId"].ToString();
                if (status == RowQuantity)
                {
                    actualDate = (DateTime)dr["ActualDateDate"];
                    etd = (DateTime)dr["ETDDate"];
                }

                foreach (var sizeRun in sizeRunList)
                {
                    string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                    int qtyAtCell = 0;
                    Int32.TryParse(dr[String.Format("Column{0}", sizeBinding)].ToString(), out qtyAtCell);
                    var materialDeliveryAddModel = new MaterialDeliveryModel
                    {
                        ProductNo           = productNo,
                        SupplierId          = int.Parse(supplierId),
                        ActualDeliveryDate  = actualDate,
                        ETD                 = etd,
                        SizeNo              = sizeRun.SizeNo
                    };
                    if (status == RowQuantity)
                        materialDeliveryAddModel.Quantity = qtyAtCell;
                    else if (status == RowReject)
                        materialDeliveryAddModel.Reject = qtyAtCell;
                    else
                        materialDeliveryAddModel.RejectSewing = qtyAtCell;

                    matsFromGridList.Add(materialDeliveryAddModel);
                }
            }
            
            if (bwUpload.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                eAction = EExcute.AddNew;
                btnSave.IsEnabled = false;

                // Created Add List
                var supplierIdList = matsFromGridList.Select(s => s.SupplierId).Distinct().ToList();
                var matsDeliveryAddList = new List<MaterialDeliveryModel>();
                var matsPlanUpdateList  = new List<MaterialPlanModel>();
                foreach (var supplierId in supplierIdList)
                {
                    var matsBySuppList  = matsFromGridList.Where(w => w.SupplierId == supplierId).ToList();
                    var matsFisrt       = matsBySuppList.FirstOrDefault();
                    matsPlanUpdateList.Add(new MaterialPlanModel
                    {
                        ProductNo   = matsFisrt.ProductNo,
                        SupplierId  = matsFisrt.SupplierId,
                        ActualDeliveryDate = matsFisrt.ActualDeliveryDate,
                        ETD = matsFisrt.ETD,
                        BalancePO   = sizeRunList.Sum(s => s.Quantity) - matsBySuppList.Sum(s => s.Quantity),
                        RejectPO    = matsBySuppList.Sum(s => s.Reject),
                        TotalDeliveryQuantity = matsBySuppList.Sum(s => s.Quantity)
                    });
                    foreach (var sizeRun in sizeRunList)
                    {
                        matsDeliveryAddList.Add(new MaterialDeliveryModel
                        {
                            ProductNo   = matsFisrt.ProductNo,
                            SupplierId  = matsFisrt.SupplierId,
                            SizeNo      = sizeRun.SizeNo,
                            ActualDeliveryDate = matsFisrt.ActualDeliveryDate,
                            Quantity        = matsBySuppList.Where(w => w.SizeNo == sizeRun.SizeNo).Sum(s => s.Quantity),
                            Reject          = matsBySuppList.Where(w => w.SizeNo == sizeRun.SizeNo).Sum(s => s.Reject),
                            RejectSewing    = matsBySuppList.Where(w => w.SizeNo == sizeRun.SizeNo).Sum(s => s.RejectSewing),
                        });
                    }
                }
                object[] par = new object[] { matsDeliveryAddList, matsPlanUpdateList };
                bwUpload.RunWorkerAsync(par);
            }
        }

        private void BwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = e.Argument as object[];
            var matsUploadList      = args[0] as List<MaterialDeliveryModel>;
            var matsPlanUpdateList  = args[1] as List<MaterialPlanModel>;
            e.Result = true;
            try
            {
                if (eAction == EExcute.AddNew)
                {
                    foreach (var item in matsUploadList)
                    {
                        MaterialDeliveryController.Insert(item);
                    }

                    // Update MaterialPlan
                    string remarksPO = "", actualDeliveryPO = "";
                    string etdPO = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", matsPlanUpdateList.Max(m => m.ETD));

                    // Finish Delivery
                    if (matsPlanUpdateList.Where(w => w.ActualDeliveryDate == dtDefault).Count() == 0)
                    {
                        actualDeliveryPO = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", matsPlanUpdateList.Max(m => m.ActualDeliveryDate));
                    }

                    int qtyDeliveryPO   = matsPlanUpdateList.Sum(s => s.TotalDeliveryQuantity);
                    int balancePO       = matsPlanUpdateList.Max(m => m.BalancePO);
                    int rejectPO        = matsPlanUpdateList.Max(m => m.RejectPO);
                    int remarksNumber   = matsPlanUpdateList.Max(m => m.BalancePO + m.RejectPO);

                    // Already Delivery
                    if (qtyDeliveryPO > 0 && (balancePO > 0 || rejectPO > 0) )
                    {
                        //remarksPO = balancePO + rejectPO > 0 ? (balancePO + rejectPO).ToString() : "";
                        remarksPO = remarksNumber.ToString();
                        var etdNotYetFinishDelivery = matsPlanUpdateList.Where(w => w.ActualDeliveryDate == dtDefault).ToList();
                        if (etdNotYetFinishDelivery.Count() > 0)
                            etdPO = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", etdNotYetFinishDelivery.Max(m => m.ETD));
                    }

                    foreach (var item in matsPlanUpdateList)
                    {
                        item.ETDPO              = etdPO;
                        item.ActualDeliveryPO   = actualDeliveryPO;
                        MaterialPlanController.UpdatePlanWhenDelivery(item);
                    }

                    // Update RawMaterialViewModel
                    rawMaterialView.UpperAccessories_ActualDeliveryDate = "";
                    rawMaterialView.UpperAccessories_Remarks = "";
                    rawMaterialView.UpperAccessories_ETD = etdPO;

                    if (!String.IsNullOrEmpty(actualDeliveryPO))
                        rawMaterialView.UpperAccessories_ActualDeliveryDate = actualDeliveryPO;
                    if (!String.IsNullOrEmpty(remarksPO))
                        rawMaterialView.UpperAccessories_Remarks = remarksPO;
                }
                else if (eAction == EExcute.Delete)
                {
                    MaterialDeliveryController.DeleteByPO(productNo);
                    // Update RawMaterialViewModel
                    rawMaterialView.UpperAccessories_ActualDeliveryDate = "";
                    rawMaterialView.UpperAccessories_Remarks = "";
                }
            }
            catch (Exception ex)
            {
                e.Result = false;
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.InnerException.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void dgDeliveryInfo_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!account.MaterialDelivery)
                e.Cancel = true;
            
            // Deny Input Reject
            var drEditting = ((DataRowView)e.Row.Item).Row;
            if (drEditting["Status"].ToString() == RowReject)
                e.Cancel = true;
        }

        private void BwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string msg = "";
            if ((bool)e.Result == false || e.Cancelled == true)
                msg = "An error occurred when excute data !";
            else
            {
                if (eAction == EExcute.AddNew)
                {
                    msg = "Saved !";
                }
                else if (eAction == EExcute.Delete)
                {
                    msg = "Deleted !";
                    dgDeliveryInfo.ItemsSource = null;
                }
            }
            MessageBox.Show(msg, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.Cursor = null;
            btnSave.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void dgDeliveryInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

    }
}
