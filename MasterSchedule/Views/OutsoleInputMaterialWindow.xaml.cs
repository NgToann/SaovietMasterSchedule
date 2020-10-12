using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Helpers;
using MasterSchedule.Controllers;
using System.Data;
using System.Globalization;
//using MasterSchedule.Helpers;
using System.Windows.Media;
using System.Data.Metadata.Edm;
using System.Configuration;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputOutsoleMarterialWindow.xaml
    /// </summary>
    public partial class OutsoleInputMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker threadLoadData;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<SizeRunModel> sizeRunList;
        DataTable dt;
        DataTable dtPainting;

        List<OutsoleMaterialModel> outsoleMaterialToInsertList;
        BackgroundWorker threadInsert;
        BackgroundWorker bwUpdateRackNumber;
        List<OutsoleMaterialModel> outsoleMaterialList;
        public RawMaterialModel rawMaterial;
        public int totalRejectAssemblyAndStockfitRespone;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleSuppliersModel> outsoleSupplierModifiedList;
        DateTime dtDefault;
        DateTime dtNothing;
        List<OutsoleRawMaterialModel> outsoleRawMaterialToInsertList;

        List<OutsoleMaterialRackPositionModel> outsoleMaterialRackPositionList;

        List<OutsoleMaterialDeliveryDetailModel> osMaterialDeliveryDetailList;

        BackgroundWorker threadUpdateRawMaterial;
        private string _REJECT = "Reject", _REJECT_ASSEMBLY = "Reject Assembly", _REJECT_STOCKFIT = "Reject Stockfit";
        AccountModel account;
        private bool _loaded = false;
        List<OutsoleMaterialReleasePaintingModel> currentReleasePaintingList;
        private PrivateDefineModel definePrivate;
        public OutsoleInputMaterialWindow(string productNo, AccountModel account)
        {
            InitializeComponent();
            this.productNo = productNo;
            this.account = account;

            threadLoadData = new BackgroundWorker();
            threadLoadData.WorkerSupportsCancellation = true;
            threadLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            threadLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            sizeRunList = new List<SizeRunModel>();
            dt = new DataTable();
            dtPainting = new DataTable();
            outsoleMaterialToInsertList = new List<OutsoleMaterialModel>();

            threadInsert = new BackgroundWorker();
            threadInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            threadInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            bwUpdateRackNumber = new BackgroundWorker();
            bwUpdateRackNumber.DoWork += new DoWorkEventHandler(bwUpdateRackNumber_DoWork);
            bwUpdateRackNumber.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpdateRackNumber_RunWorkerCompleted);

            outsoleMaterialList = new List<OutsoleMaterialModel>();

            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 6,
                ETD = dtNothing,
                ActualDate = dtNothing,
                Remarks = "",
            };

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleSupplierModifiedList = new List<OutsoleSuppliersModel>();
            outsoleRawMaterialToInsertList = new List<OutsoleRawMaterialModel>();

            outsoleMaterialRackPositionList = new List<OutsoleMaterialRackPositionModel>();

            osMaterialDeliveryDetailList = new List<OutsoleMaterialDeliveryDetailModel>();

            currentReleasePaintingList = new List<OutsoleMaterialReleasePaintingModel>();
            insertReleasePaintingList = new List<OutsoleMaterialReleasePaintingModel>();
            definePrivate = new PrivateDefineModel();

            threadUpdateRawMaterial = new BackgroundWorker();
            threadUpdateRawMaterial.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadUpdateRawMaterial_RunWorkerCompleted);
            threadUpdateRawMaterial.DoWork += new DoWorkEventHandler(threadUpdateRawMaterial_DoWork);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = String.Format("{0} for {1}", this.Title, productNo);
            if (threadLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                threadLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleSupplierList = OutsoleSuppliersController.Select();
            outsoleMaterialList = OutsoleMaterialController.Select(productNo);
            sizeRunList = SizeRunController.Select(productNo);
            outsoleRawMaterialList = OutsoleRawMaterialController.Select(productNo);
            outsoleMaterialRackPositionList = OutsoleMaterialRackPositionController.Select(productNo);

            currentReleasePaintingList  = OutsoleMaterialReleasePaintingController.Select(productNo);
            definePrivate               = PrivateDefineController.GetDefine();
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (OutsoleSuppliersModel outsoleSupplier in outsoleSupplierList)
            {
                outsoleSupplierModifiedList.Add(outsoleSupplier);
                outsoleSupplierModifiedList.Add(new OutsoleSuppliersModel
                {
                    Name = _REJECT,
                    OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                });
                outsoleSupplierModifiedList.Add(new OutsoleSuppliersModel
                {
                    Name = _REJECT_ASSEMBLY,
                    OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                });
                outsoleSupplierModifiedList.Add(new OutsoleSuppliersModel
                {
                    Name = _REJECT_STOCKFIT,
                    OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                });
            }
            colSuppliers.ItemsSource = outsoleSupplierModifiedList;
            //dt.Columns.Clear();
            dt.Columns.Add("Supplier", typeof(OutsoleSuppliersModel));
            dt.Columns.Add("ETD", typeof(String));
            dt.Columns.Add("ActualDate", typeof(String));

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = string.Format("{0}\n{1}\n{2}\n({3})", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize, sizeRun.Quantity);
                column.MinWidth = 45;
                if (account.OutsoleMaterialEdit == false)
                {
                    column.IsReadOnly = true;
                }
                column.Binding = new Binding(String.Format("Column{0}", i));

                Style styleColumn = new Style();
                Setter setterColumnForecolor = new Setter();
                setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", i));
                styleColumn.Setters.Add(setterColumnForecolor);

                column.CellStyle = styleColumn;
                DataColumn columnForeground = new DataColumn(String.Format("Column{0}Foreground", i), typeof(SolidColorBrush));
                columnForeground.DefaultValue = Brushes.Black;

                dt.Columns.Add(columnForeground);
                dgOutsoleMaterial.Columns.Add(column);
            }

            dt.Columns.Add("Balance", typeof(String));
            DataGridTextColumn colBalance = new DataGridTextColumn();
            colBalance.Header = "Balance";
            colBalance.MinWidth = 80;
            colBalance.IsReadOnly = true;
            colBalance.Binding = new Binding("Balance");
            dgOutsoleMaterial.Columns.Add(colBalance);

            colCompleted.DisplayIndex = dgOutsoleMaterial.Columns.Count - 1;
            colAddRack.DisplayIndex = dgOutsoleMaterial.Columns.Count - 1;

            // Create column rack
            CreateColumnRack(dt, dgOutsoleMaterial);

            List<Int32> supplierLoadList = outsoleRawMaterialList.OrderBy(od => od.ETD).Select(o => o.OutsoleSupplierId).Distinct().ToList();
            for (int i = 0; i <= supplierLoadList.Count - 1; i++)
            {
                int supplierIDLoad = supplierLoadList[i];
                DataRow dr = dt.NewRow();
                dr["Supplier"] = outsoleSupplierModifiedList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIDLoad && f.Name != _REJECT && f.Name != _REJECT_ASSEMBLY && f.Name != _REJECT_STOCKFIT);
                DateTime dtETD = outsoleRawMaterialList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIDLoad).ETD;
                {
                    if (dtETD.Date != dtDefault)
                    {
                        dr["ETD"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtETD);
                    }
                }

                DateTime dtActualDate = outsoleRawMaterialList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIDLoad).ActualDate;
                if (dtActualDate.Date != dtDefault)
                {
                    dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtActualDate);
                }

                var deleveriedList = outsoleMaterialList.Where(w => w.OutsoleSupplierId == supplierIDLoad).ToList();
                if (deleveriedList.Sum(s => s.Quantity - s.QuantityReject) == sizeRunList.Sum(s => s.Quantity))
                    dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", deleveriedList.Max(s => s.ModifiedTime));
                else
                    dr["ActualDate"] = "";

                DataRow drReject = dt.NewRow();
                DataRow drRejectAssembly = dt.NewRow();
                DataRow drRejectStockfit = dt.NewRow();

                drReject["Supplier"] = outsoleSupplierModifiedList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIDLoad && f.Name == _REJECT);
                drRejectAssembly["Supplier"] = outsoleSupplierModifiedList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIDLoad && f.Name == _REJECT_ASSEMBLY);
                drRejectStockfit["Supplier"] = outsoleSupplierModifiedList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIDLoad && f.Name == _REJECT_STOCKFIT);

                int qtyBalance = 0;
                int totalReject = 0;
                int totalRejectAssembly = 0;
                int totalRejectStockfit = 0;
                for (int j = 0; j <= sizeRunList.Count - 1; j++)
                {
                    SizeRunModel sizeRun = sizeRunList[j];
                    int qtyDelivery = outsoleMaterialList.Where(o => o.OutsoleSupplierId == supplierIDLoad && o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                    if (qtyDelivery > 0)
                    {
                        dr[String.Format("Column{0}", j)] = qtyDelivery;
                        if (qtyDelivery != sizeRun.Quantity)
                            dr[String.Format("Column{0}Foreground", j)] = Brushes.Red;
                    }

                    int qtyReject = outsoleMaterialList.Where(o => o.OutsoleSupplierId == supplierIDLoad && o.SizeNo == sizeRun.SizeNo).Sum(o => o.QuantityReject);
                    if (qtyReject > 0)
                    {
                        drReject[String.Format("Column{0}", j)] = qtyReject;
                        drReject[String.Format("Column{0}Foreground", j)] = Brushes.Red;
                    }

                    int qtyRejectAssembly = outsoleMaterialList.Where(o => o.OutsoleSupplierId == supplierIDLoad && o.SizeNo == sizeRun.SizeNo).Sum(o => o.RejectAssembly);
                    if (qtyRejectAssembly > 0)
                    {
                        drRejectAssembly[String.Format("Column{0}", j)] = qtyRejectAssembly;
                        drRejectAssembly[String.Format("Column{0}Foreground", j)] = Brushes.Red;
                    }

                    int qtyRejectStockfit = outsoleMaterialList.Where(o => o.OutsoleSupplierId == supplierIDLoad && o.SizeNo == sizeRun.SizeNo).Sum(o => o.RejectStockfit);
                    if (qtyRejectStockfit > 0)
                    {
                        drRejectStockfit[String.Format("Column{0}", j)] = qtyRejectStockfit;
                        drRejectStockfit[String.Format("Column{0}Foreground", j)] = Brushes.Red;
                    }

                    int qtyBalancePerSize = sizeRun.Quantity - qtyDelivery;

                    qtyBalance += qtyBalancePerSize;
                    totalReject += qtyReject;
                    totalRejectAssembly += qtyRejectAssembly;
                    totalRejectStockfit += qtyRejectStockfit;
                }
                dr["Balance"] = qtyBalance > 0 ? qtyBalance.ToString() : "";
                drReject["Balance"] = totalReject > 0 ? totalReject.ToString() : "";
                drRejectAssembly["Balance"] = totalRejectAssembly > 0 ? totalRejectAssembly.ToString() : "";
                drRejectStockfit["Balance"] = totalRejectStockfit > 0 ? totalRejectStockfit.ToString() : "";

                // load column rack
                var rackList_Supplier = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == supplierIDLoad).ToList();
                var racks = rackList_Supplier.Select(s => s.RackNumber).Distinct().ToList();
                foreach (var rack in racks)
                {
                    var rackList_Supplier_Rack = rackList_Supplier.Where(w => w.RackNumber == rack).ToList();
                    var cartons = rackList_Supplier_Rack.Select(s => s.CartonNumber).Distinct().ToList();
                    foreach (var carton in cartons)
                    {
                        var rackList_Supplier_Rack_Carton = rackList_Supplier_Rack.Where(w => w.CartonNumber == carton).ToList();
                        var columnBinding = rackBindingList.Where(w => w.SupplierID == supplierIDLoad && w.RackNumber == rack && w.CartonNo == carton).FirstOrDefault();
                        dr[String.Format("ColumnRack{0}", columnBinding.ColumnID)] = String.Format("{0} ; {1}", rackList_Supplier_Rack_Carton.FirstOrDefault().RackNumber, rackList_Supplier_Rack_Carton.FirstOrDefault().CartonNumber);
                        if (string.IsNullOrEmpty(rackList_Supplier_Rack_Carton.FirstOrDefault().SizeNo) == false)
                        {
                            dr[String.Format("ColumnRackTooltip{0}", columnBinding.ColumnID)] = ToolTipDisplay(rackList_Supplier_Rack_Carton);
                        }
                    }
                }

                dt.Rows.Add(dr);
                if (definePrivate.InputReject == true)
                    dt.Rows.Add(drReject);
                dt.Rows.Add(drRejectAssembly);
                dt.Rows.Add(drRejectStockfit);
            }

            dgOutsoleMaterial.ItemsSource = dt.AsDataView();
            btnSave.IsEnabled = true;
            _loaded = true;

            // Binding data cboReleasePainting 20-2156, 109-2156
            var suppliersPainting = new List<OutsoleSuppliersModel>();
            suppliersPainting.Add(new OutsoleSuppliersModel
            {
                Name = currentReleasePaintingList.Count() > 0 ? "All" : "None",
                OutsoleSupplierId = -1
            });
            suppliersPainting.AddRange(outsoleSupplierList.Where(w => supplierLoadList.Contains(w.OutsoleSupplierId)).ToList());
            cboPaintingSupplier.ItemsSource = suppliersPainting;
            cboPaintingSupplier.SelectedItem = suppliersPainting.FirstOrDefault();

            if (currentReleasePaintingList.Count() > 0)
                LoadDataReleasePainting(currentReleasePaintingList);

            // Bind RadioButtonInputReject
            radInputReject.IsChecked = definePrivate.InputReject;
            radNotInputReject.IsChecked = !definePrivate.InputReject;
            
            this.Cursor = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            dt = ((DataView)dgOutsoleMaterial.ItemsSource).ToTable();
            var osMaterialFromTableList = new List<OutsoleMaterialModel>();
            for (int r = 0; r <= dt.Rows.Count - 1; r++)
            {
                DataRow dr = dt.Rows[r];
                var supplierAtRow = (OutsoleSuppliersModel)dr["Supplier"];

                string dateAtRowString = dr["ActualDate"] as String;
                DateTime actualDateAtRow = TimeHelper.Convert(dateAtRowString);

                // Get RawMaterial of Each Supplier
                if (outsoleSupplierList.Where(w => w.Name.Equals(supplierAtRow.Name)).Count() > 0)
                {
                    // Check NeedToUpdate RawMaterial ActualDate ?
                    var osRawMaterialBySupplier = outsoleRawMaterialList.FirstOrDefault(f => f.OutsoleSupplierId == supplierAtRow.OutsoleSupplierId);
                    if (actualDateAtRow != dtNothing && actualDateAtRow != osRawMaterialBySupplier.ActualDate)
                    {
                        outsoleRawMaterialToInsertList.Add(new OutsoleRawMaterialModel
                        {
                            ProductNo = productNo,
                            OutsoleSupplierId = supplierAtRow.OutsoleSupplierId,
                            ActualDate = actualDateAtRow,
                        });
                    }
                }

                // OSMaterial From Table
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    string sizeNo = sizeRunList[i].SizeNo;
                    int quantity = 0, quantityReject = 0, quantityRejectAssembly = 0, quantityRejectStockfit = 0;

                    if (supplierAtRow.Name.Equals(_REJECT))
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out quantityReject);

                    else if (supplierAtRow.Name.Equals(_REJECT_ASSEMBLY))
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out quantityRejectAssembly);

                    else if (supplierAtRow.Name.Equals(_REJECT_STOCKFIT))
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out quantityRejectStockfit);

                    else
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out quantity);


                    osMaterialFromTableList.Add(new OutsoleMaterialModel
                    {
                        ProductNo           = productNo,
                        OutsoleSupplierId   = supplierAtRow.OutsoleSupplierId,
                        SizeNo          = sizeNo,
                        Quantity        = quantity,
                        QuantityReject = quantityReject,
                        RejectAssembly = quantityRejectAssembly,
                        RejectStockfit = quantityRejectStockfit
                    });
                    
                }
            }
            // Summary By Supplier.
            var supplierIdList = osMaterialFromTableList.Select(s => s.OutsoleSupplierId).Distinct().ToList();
            foreach (var supplierId in supplierIdList)
            {
                var osMaterialFromTableBySupplierList = osMaterialFromTableList.Where(w => w.OutsoleSupplierId == supplierId).ToList();
                var qtyOrder = sizeRunList.Sum(s => s.Quantity);
                var qtyDeliveryExReject = osMaterialFromTableBySupplierList.Sum(s => s.Quantity - s.QuantityReject);
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    string sizeNo = sizeRunList[i].SizeNo;
                    var osMaterialFromTableBySupplierBySize = osMaterialFromTableBySupplierList.Where(w => w.SizeNo == sizeNo).ToList();
                    outsoleMaterialToInsertList.Add(new OutsoleMaterialModel
                    {
                        ProductNo = productNo,
                        OutsoleSupplierId = supplierId,
                        SizeNo = sizeNo,
                        Quantity = osMaterialFromTableBySupplierBySize.Sum(s => s.Quantity),
                        QuantityReject = osMaterialFromTableBySupplierBySize.Sum(s => s.QuantityReject),
                        RejectAssembly = osMaterialFromTableBySupplierBySize.Sum(s => s.RejectAssembly),
                        RejectStockfit = osMaterialFromTableBySupplierBySize.Sum(s => s.RejectStockfit),
                        TotalBalance = qtyOrder - qtyDeliveryExReject >= 0 ? qtyOrder - qtyDeliveryExReject : 0
                    });
                }
            }
            if (threadInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnSave.IsEnabled = false;
                threadInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Insert RawMaterial
                foreach (OutsoleRawMaterialModel model in outsoleRawMaterialToInsertList)
                {
                    OutsoleRawMaterialController.UpdateActualDate(model);
                }
                // Insert OutsoleMaterial
                foreach (var materialInsert in outsoleMaterialToInsertList)
                {
                    var beforeInsert = outsoleMaterialList.FirstOrDefault(f => f.OutsoleSupplierId == materialInsert.OutsoleSupplierId && f.SizeNo == materialInsert.SizeNo);

                    // Insert.
                    if (beforeInsert == null)
                        OutsoleMaterialController.Insert(materialInsert);
                    else
                    {
                        // Update Quantity
                        if (beforeInsert.Quantity != materialInsert.Quantity)
                            OutsoleMaterialController.Update(materialInsert, false, true, false, false);
                        // Update Reject
                        if (beforeInsert.QuantityReject != materialInsert.QuantityReject)
                            OutsoleMaterialController.Update(materialInsert, true, false, false, false);
                        // Update Reject Assembly
                        if (beforeInsert.RejectAssembly != materialInsert.RejectAssembly)
                            OutsoleMaterialController.Update(materialInsert, true, false, true, false);
                        // Update Reject Stockfit
                        if (beforeInsert.RejectStockfit != materialInsert.RejectStockfit)
                            OutsoleMaterialController.Update(materialInsert, true, false, false, true);
                    }
                }
                // Insert Release Painting
                foreach (var insertModel in insertReleasePaintingList)
                {
                    OutsoleMaterialReleasePaintingController.Insert(insertModel);
                }
            }

            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message.ToString());
                }));
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Create RawMaterialToUpDate
            var osRawMaterialBfUpdateList = outsoleRawMaterialList.ToList();
            foreach (var osRawMats in osRawMaterialBfUpdateList)
            {
                var updatedModel = outsoleRawMaterialToInsertList.FirstOrDefault(f => f.OutsoleSupplierId == osRawMats.OutsoleSupplierId);
                if (updatedModel != null)
                    osRawMats.ActualDate = updatedModel.ActualDate;
            }

            if (osRawMaterialBfUpdateList.Where(w => w.ActualDate == dtDefault).Count() > 0)
            {
                rawMaterial.ActualDate = dtDefault;
            }
            else
                rawMaterial.ActualDate = osRawMaterialBfUpdateList.Max(m => m.ActualDate);

            var currentBalance = outsoleMaterialToInsertList.Max(m => m.TotalBalance);
            rawMaterial.Remarks = currentBalance > 0 ? currentBalance.ToString() : "";
            
            // Not yet delivery anything.
            if (outsoleMaterialToInsertList.Sum(s => s.Quantity) == 0 && 
                outsoleMaterialToInsertList.Sum(s => s.QuantityReject) == 0)
                rawMaterial.Remarks = "";

            // Get total reject assembly
            totalRejectAssemblyAndStockfitRespone = outsoleMaterialToInsertList.Sum(s => s.RejectAssembly + s.RejectStockfit);

            if (threadUpdateRawMaterial.IsBusy == false)
            {
                threadUpdateRawMaterial.RunWorkerAsync();
            }
        }

        private void threadUpdateRawMaterial_DoWork(object sender, DoWorkEventArgs e)
        {
            RawMaterialController.Insert(rawMaterial);
        }

        private void threadUpdateRawMaterial_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ((DataRowView)dgOutsoleMaterial.CurrentItem).Row;
            if (dr == null)
            {
                return;
            }
            var outsoleSupplier = dr["Supplier"] as OutsoleSuppliersModel;
            if (outsoleSupplier == null || outsoleSupplier.Name == _REJECT || outsoleSupplier.Name == _REJECT_ASSEMBLY || outsoleSupplier.Name == _REJECT_STOCKFIT)
            {
                return;
            }

            // Get RejectCurrent
            int totalReject = 0;
            for (int r = 0; r <= dt.Rows.Count - 1; r++)
            {
                DataRow drCurrent = dt.Rows[r];
                var outsoleSupplierRowBelow = drCurrent["Supplier"] as OutsoleSuppliersModel;
                if (outsoleSupplierRowBelow == outsoleSupplier)
                {
                    DataRow drRejectCurrent = dt.Rows[r + 1];
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        int rejectPerSize = 0;
                        Int32.TryParse(drRejectCurrent[String.Format("Column{0}", i)].ToString(), out rejectPerSize);
                        totalReject += rejectPerSize;
                    }
                }
            }

            // Check Quantity Old            
            int totalQtyOld = 0;

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
                dr[String.Format("Column{0}Foreground", i)] = Brushes.Black;

                int qtyOld = 0, qtyCurrent = 0;
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                totalQtyOld += qtyOld;

                if (qtyOld > 0)
                    qtyCurrent = sizeRun.Quantity - qtyOld;
                else
                    qtyCurrent = sizeRun.Quantity;

                var osmDeliveryDetail = new OutsoleMaterialDeliveryDetailModel()
                {
                    ProductNo = productNo,
                    OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                    SizeNo = sizeRun.SizeNo,
                    QuantityCurrent = qtyCurrent,
                };
                osMaterialDeliveryDetailList.Add(osmDeliveryDetail);
            }

            dr["Balance"] = "";
            if (totalQtyOld == sizeRunList.Sum(s => s.Quantity) && totalReject > 0)
                dr["ActualDate"] = "";

            if (totalQtyOld != sizeRunList.Sum(s => s.Quantity) && totalReject <= 0)
                dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);

            dgOutsoleMaterial.ItemsSource = null;
            dgOutsoleMaterial.ItemsSource = dt.AsDataView();
        }

        private void dgOutsoleMaterial_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.GetValue(TagProperty) == null)
                return;

            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }
            var outsoleSupplier = (OutsoleSuppliersModel)((DataRowView)e.Row.Item)["Supplier"];

            var osMaterialDeliveryDetail = new OutsoleMaterialDeliveryDetailModel()
            {
                ProductNo = productNo,
                OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                OutsoleSupplierName = outsoleSupplier.Name,
                SizeNo = sizeNo,
            };

            int qtyOld = 0;
            if (outsoleSupplier.Name != _REJECT && outsoleSupplier.Name != _REJECT_ASSEMBLY && outsoleSupplier.Name != _REJECT_STOCKFIT)
            {
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.Quantity);
                osMaterialDeliveryDetail.QuantityOld = qtyOld;
            }
            else if (outsoleSupplier.Name == _REJECT)
            {
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.QuantityReject);
                osMaterialDeliveryDetail.RejectOld = qtyOld;
            }
            else if (outsoleSupplier.Name == _REJECT_ASSEMBLY)
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.RejectAssembly);
            else if (outsoleSupplier.Name == _REJECT_STOCKFIT)
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.RejectStockfit);

            int qtyOrder_Size = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);

            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (Int32.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                if (outsoleSupplier.Name != _REJECT && outsoleSupplier.Name != _REJECT_ASSEMBLY && outsoleSupplier.Name != _REJECT_STOCKFIT)
                    osMaterialDeliveryDetail.QuantityCurrent = qtyNew;
                if (outsoleSupplier.Name == _REJECT)
                    osMaterialDeliveryDetail.RejectCurrent = qtyNew;

                txtCurrent.Text = (qtyOld + qtyNew).ToString();
                if (qtyOld + qtyNew < 0 || qtyOld + qtyNew > qtyOrder_Size)
                    txtCurrent.Text = qtyOld.ToString();
            }

            osMaterialDeliveryDetailList.Add(osMaterialDeliveryDetail);

            int qtyCurrent = 0;
            Int32.TryParse(txtCurrent.Text.ToString(), out qtyCurrent);
            int qtyTotal = 0, rejectTotal = 0, rejectAssemblyTotal = 0, rejectStockfitTotal = 0;
            for (int r = 0; r <= dt.Rows.Count - 1; r++)
            {
                DataRow dr = dt.Rows[r];
                if ((OutsoleSuppliersModel)dr["Supplier"] != outsoleSupplier)
                    continue;
                for (int i = 0; i < sizeRunList.Count; i++)
                {
                    int qty = 0;
                    var sizeRunPerSize = sizeRunList[i];
                    if (sizeRunPerSize.SizeNo == sizeNo)
                    {
                        qty = qtyCurrent;
                        dr[String.Format("Column{0}", i)] = qty;

                        dr[String.Format("Column{0}Foreground", i)] = Brushes.Black;
                        if (qty != 0 && qty != sizeRunPerSize.Quantity)
                            dr[String.Format("Column{0}Foreground", i)] = Brushes.Red;
                    }
                    else
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qty);
                    if (outsoleSupplier.Name == _REJECT)
                        rejectTotal += qty;
                    else if (outsoleSupplier.Name == _REJECT_ASSEMBLY)
                        rejectAssemblyTotal += qty;
                    else if (outsoleSupplier.Name == _REJECT_STOCKFIT)
                        rejectStockfitTotal += qty;
                    else
                        qtyTotal += qty;
                }

                // Update Balance Column
                var drActualDateUpdate = dr;
                if (outsoleSupplier.Name == _REJECT)
                {
                    dr["Balance"] = rejectTotal > 0 ? rejectTotal.ToString() : "";

                    drActualDateUpdate = dt.Rows[r - 1];
                    int qtyTotalCurrent = 0;
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        int qty = 0;
                        Int32.TryParse(drActualDateUpdate[String.Format("Column{0}", i)].ToString(), out qty);
                        qtyTotalCurrent += qty;
                    }

                    if (rejectTotal > 0 || (sizeRunList.Sum(s => s.Quantity) - qtyTotalCurrent) > 0 || rejectAssemblyTotal > 0 || rejectStockfitTotal > 0)
                        drActualDateUpdate["ActualDate"] = "";
                    else
                        drActualDateUpdate["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
                }
                else if (outsoleSupplier.Name == _REJECT_ASSEMBLY)
                    dr["Balance"] = rejectAssemblyTotal > 0 ? rejectAssemblyTotal.ToString() : "";
                else if (outsoleSupplier.Name == _REJECT_STOCKFIT)
                    dr["Balance"] = rejectStockfitTotal > 0 ? rejectStockfitTotal.ToString() : "";
                else
                {
                    dr["Balance"] = (sizeRunList.Sum(s => s.Quantity) - qtyTotal) > 0 ? (sizeRunList.Sum(s => s.Quantity) - qtyTotal).ToString() : "";

                    int rejectTotalCurrent = 0;
                    var drReject = dt.Rows[r + 1];
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        int rejectCurrent = 0;
                        Int32.TryParse(drReject[String.Format("Column{0}", i)].ToString(), out rejectCurrent);
                        rejectTotalCurrent += rejectCurrent;
                    }

                    if (rejectTotalCurrent > 0 || (sizeRunList.Sum(s => s.Quantity) - qtyTotal) > 0 || rejectAssemblyTotal > 0 || rejectStockfitTotal > 0)
                        dr["ActualDate"] = "";
                    else
                        dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
                }
            }
        }

        private void btnAddRack_Click(object sender, RoutedEventArgs e)
        {
            DataRow drCurrent = ((DataRowView)dgOutsoleMaterial.CurrentItem).Row;
            if (drCurrent == null)
            {
                return;
            }
            var outsoleSupplier = drCurrent["Supplier"] as OutsoleSuppliersModel;
            var outsoleSupplierClicked = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId);

            if (outsoleSupplier == null || outsoleSupplierClicked == null)
            {
                return;
            }

            var osMaterialDeliveryDetailTranferList = osMaterialDeliveryDetailList.Where(w => w.OutsoleSupplierId == outsoleSupplierClicked.OutsoleSupplierId && w.QuantityCurrent > 0).ToList();
            if (osMaterialDeliveryDetailTranferList.Count() > 0)
            {
                var window = new AddOutsoleMaterialRackPositionWindow(productNo, outsoleSupplierClicked, null, osMaterialDeliveryDetailTranferList);
                window.Title = String.Format("Add Rack for:  {0}", productNo);
                window.ShowDialog();
            }

            if (bwUpdateRackNumber.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwUpdateRackNumber.RunWorkerAsync();
            }
        }

        private void dgOutsoleMaterial_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var rowClicked = dgOutsoleMaterial.CurrentItem as DataRowView;
                DataRow drCurrent = ((DataRowView)dgOutsoleMaterial.CurrentItem).Row;

                var cellClicked = dgOutsoleMaterial.CurrentCell;
                if (rowClicked == null || cellClicked == null || drCurrent == null)
                    return;
                var supplierClicked = rowClicked[0] as OutsoleSuppliersModel;

                if (supplierClicked.Name == _REJECT || supplierClicked.Name == _REJECT_ASSEMBLY || supplierClicked.Name == _REJECT_STOCKFIT)
                    return;
                if (cellClicked.Column.SortMemberPath.Contains("ColumnRack") == false)
                    return;

                var cellClickedSortMemberPath = cellClicked.Column.SortMemberPath.ToString();

                var contentOnCell = drCurrent[cellClickedSortMemberPath];
                var contentOnCellString = contentOnCell != null ? contentOnCell.ToString() : "";

                string rackNo = ""; int cartonNo = 0;
                if (contentOnCellString.Split(';').Count() <= 1)
                    return;

                rackNo = contentOnCellString.Split(';')[0].ToString().Trim();
                Int32.TryParse(contentOnCellString.Split(';')[1].ToString().Trim(), out cartonNo);

                var rackNeedUpdate = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId &&
                                                                           w.RackNumber == rackNo && w.CartonNumber == cartonNo).ToList();

                if (rackNeedUpdate.Count() <= 0)
                    return;

                // Check the old version (only input SizeNo and Quantity)
                // sizeNo = "", Quantity = ""
                if (rackNeedUpdate.Count() == 1)
                {
                    var rackDetail = rackNeedUpdate.FirstOrDefault();
                    if (rackDetail.Pairs == 0)
                        return;
                }

                var supplierTranfer = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).FirstOrDefault();
                var updateRackWindow = new AddOutsoleMaterialRackPositionWindow(productNo, supplierTranfer, rackNeedUpdate, null);
                updateRackWindow.Title = String.Format("Update Rack for: {0}", productNo);
                updateRackWindow.ShowDialog();

                if (bwUpdateRackNumber.IsBusy == false && updateRackWindow.Reload == true)
                {
                    this.Cursor = Cursors.Wait;
                    bwUpdateRackNumber.RunWorkerAsync();
                }
            }

            catch
            {
                return;
            }
        }

        private void bwUpdateRackNumber_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialRackPositionList = OutsoleMaterialRackPositionController.Select(productNo);
        }

        private void bwUpdateRackNumber_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            // Collect column remove on datagrid
            var columnRemoveList = new List<DataGridColumn>();
            foreach (var dgColumn in dgOutsoleMaterial.Columns)
            {
                if (dgColumn.SortMemberPath.ToString().Contains("ColumnRack"))
                {
                    columnRemoveList.Add(dgColumn);
                }
            }

            // Remove Column on datagrid
            foreach (var colRemove in columnRemoveList)
            {
                dgOutsoleMaterial.Columns.Remove(colRemove);
            }

            // Collect column on datatable
            var columnRemoveOnDTList = new List<DataColumn>();
            foreach (DataColumn dcCheck in dt.Columns)
            {
                if (dcCheck.ColumnName.ToString().Contains("ColumnRack"))
                    columnRemoveOnDTList.Add(dcCheck);
            }
            // Remove column on datatable
            foreach (var colRemove in columnRemoveOnDTList)
            {
                dt.Columns.Remove(colRemove);
            }

            // Create Column
            CreateColumnRack(dt, dgOutsoleMaterial);

            var supplierUpdateList = outsoleMaterialList.Select(s => s.OutsoleSupplierId).Distinct().ToList();
            foreach (var supplierUpdate in supplierUpdateList)
            {
                var rackList_Supplier = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == supplierUpdate).ToList();
                var racks = rackList_Supplier.Select(s => s.RackNumber).Distinct().ToList();

                foreach (var rack in racks)
                {
                    var rackList_Supplier_Rack = rackList_Supplier.Where(w => w.RackNumber == rack).ToList();
                    var cartons = rackList_Supplier_Rack.Select(s => s.CartonNumber).Distinct().ToList();
                    foreach (var carton in cartons)
                    {
                        var rackList_Supplier_Rack_Carton = rackList_Supplier_Rack.Where(w => w.CartonNumber == carton).ToList();
                        foreach (DataRow drUpdate in dt.Rows)
                        {
                            var outsoleSupplier_Row = drUpdate["Supplier"] as OutsoleSuppliersModel;
                            var columnBinding = rackBindingList.Where(w => w.SupplierID == supplierUpdate && w.RackNumber == rack && w.CartonNo == carton).FirstOrDefault();
                            if (outsoleSupplier_Row.OutsoleSupplierId == supplierUpdate && outsoleSupplier_Row.Name != _REJECT && outsoleSupplier_Row.Name != _REJECT_ASSEMBLY && outsoleSupplier_Row.Name != _REJECT_STOCKFIT)
                            {
                                drUpdate[String.Format("ColumnRack{0}", columnBinding.ColumnID)] = String.Format("{0} ; {1}", rackList_Supplier_Rack_Carton.FirstOrDefault().RackNumber, rackList_Supplier_Rack_Carton.FirstOrDefault().CartonNumber);
                                if (string.IsNullOrEmpty(rackList_Supplier_Rack_Carton.FirstOrDefault().SizeNo) == false)
                                {
                                    drUpdate[String.Format("ColumnRackTooltip{0}", columnBinding.ColumnID)] = ToolTipDisplay(rackList_Supplier_Rack_Carton);
                                }
                            }
                        }
                    }
                }
            }
        }

        List<RackBinding> rackBindingList;
        private void CreateColumnRack(DataTable dt, DataGrid dg)
        {
            rackBindingList = new List<RackBinding>();
            var supplierIDList = outsoleMaterialRackPositionList.Select(s => s.OutsoleSupplierId).Distinct().ToList();

            foreach (var supplierID in supplierIDList)
            {
                int columnID = 1;
                var rackList_Supplier = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == supplierID).ToList();
                var racks = rackList_Supplier.Select(s => s.RackNumber).Distinct().ToList();
                foreach (var rack in racks)
                {
                    var rackList_Supplier_Rack = rackList_Supplier.Where(w => w.RackNumber == rack).ToList();
                    var cartons = rackList_Supplier_Rack.Select(s => s.CartonNumber).Distinct().ToList();
                    foreach (var carton in cartons)
                    {
                        RackBinding rackBinding = new RackBinding()
                        {
                            ColumnID = columnID,
                            SupplierID = supplierID,
                            RackNumber = rack,
                            CartonNo = carton,
                        };
                        rackBindingList.Add(rackBinding);
                        columnID++;
                    }
                }
            }

            var columnCreateList = rackBindingList.Select(s => s.ColumnID).Distinct().ToList();
            int location = 1;
            foreach (var columnCreate in columnCreateList)
            {
                dt.Columns.Add(String.Format("ColumnRack{0}", columnCreate), typeof(String));
                DataGridTextColumn colRack = new DataGridTextColumn();
                colRack.Header = string.Format("Location {0}", location);
                colRack.Binding = new Binding(String.Format("ColumnRack{0}", columnCreate));
                colRack.IsReadOnly = true;

                var styleRackColumn = new Style();
                Setter setterTooltip = new Setter();
                setterTooltip.Property = DataGridCell.ToolTipProperty;
                setterTooltip.Value = new Binding(String.Format("ColumnRackTooltip{0}", columnCreate));
                styleRackColumn.Setters.Add(setterTooltip);

                colRack.CellStyle = styleRackColumn;
                DataColumn columnRackTooltip = new DataColumn(String.Format("ColumnRackTooltip{0}", columnCreate), typeof(String));

                dt.Columns.Add(columnRackTooltip);
                dgOutsoleMaterial.Columns.Add(colRack);

                location++;
            }
        }

        private string ToolTipDisplay(List<OutsoleMaterialRackPositionModel> rackDetail)
        {
            string result = "";

            string sizeNoDiplay = sizeNoDiplay = String.Join("  |  ", rackDetail.Select(s => s.SizeNo).ToList());
            if (String.IsNullOrEmpty(sizeNoDiplay) == false)
                sizeNoDiplay = "Size No:      " + sizeNoDiplay;

            string quantityDisplay = String.Join("  |  ", rackDetail.Select(s => s.Pairs).ToList());
            if (String.IsNullOrEmpty(quantityDisplay) == false)
                quantityDisplay = "\n" + "Quantity:     " + quantityDisplay;

            string totalPairsDisplay = rackDetail.Sum(s => s.Pairs).ToString();
            if (totalPairsDisplay == "0")
                totalPairsDisplay = "";
            else
                totalPairsDisplay = "\n" + "Total Pairs:  " + totalPairsDisplay;

            result = String.Format("{0}{1}{2}", sizeNoDiplay, quantityDisplay, totalPairsDisplay);
            return result;
        }

        private void cboPaintingSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loaded == false)
                return;
            var supplierClicked = cboPaintingSupplier.SelectedItem as OutsoleSuppliersModel;

            //if (currentReleasePaintingList.Count() > 0 && supplierClicked.OutsoleSupplierId == -1)
            //    return;

            var releaseLoadList = new List<OutsoleMaterialReleasePaintingModel>();
            if (supplierClicked.OutsoleSupplierId != -1)
                releaseLoadList = currentReleasePaintingList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            else
                releaseLoadList = currentReleasePaintingList.ToList();

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                releaseLoadList.Add(new OutsoleMaterialReleasePaintingModel
                {
                    ProductNo = sizeRun.ProductNo,
                    OutsoleSupplierId = supplierClicked.OutsoleSupplierId,
                    ReleaseDate = DateTime.Now.Date,
                    SizeNo = sizeRun.SizeNo,
                    Quantity = 0
                });
            }
            LoadDataReleasePainting(releaseLoadList);
        }

        private void LoadDataReleasePainting(List<OutsoleMaterialReleasePaintingModel> OSMReleasePaintingList)
        {
            dtPainting = new DataTable();
            dgReleasePainting.Columns.Clear();

            var supplierIdPaintedList = OSMReleasePaintingList.Select(s => s.OutsoleSupplierId).Distinct().ToList();
            var supplierPaintedList = outsoleSupplierList.Where(w => supplierIdPaintedList.Contains(w.OutsoleSupplierId)).ToList();

            dtPainting.Columns.Add("SupplierName", typeof(String));
            DataGridTextColumn colSupplierName = new DataGridTextColumn();
            colSupplierName.Header = "Painting\nSupplier";
            //colSupplierName.Width = colSuppliers.ActualWidth + colETD.ActualWidth;
            colSupplierName.Width = 120;
            colSupplierName.IsReadOnly = true;
            colSupplierName.Binding = new Binding("SupplierName");
            dgReleasePainting.Columns.Add(colSupplierName);

            dtPainting.Columns.Add("SupplierId", typeof(Int32));
            DataGridTextColumn colSupplierId = new DataGridTextColumn();
            colSupplierId.Visibility = Visibility.Collapsed;
            colSupplierId.IsReadOnly = true;
            colSupplierId.Binding = new Binding("SupplierId");
            dgReleasePainting.Columns.Add(colSupplierId);

            dtPainting.Columns.Add("ReleaseDate", typeof(String));
            DataGridTextColumn colReleaseDate = new DataGridTextColumn();
            colReleaseDate.Header = "Release\nDate";
            //colReleaseDate.Width = colActualDate.ActualWidth;
            colReleaseDate.Width = 140;
            colReleaseDate.IsReadOnly = true;
            colReleaseDate.Binding = new Binding("ReleaseDate");
            dgReleasePainting.Columns.Add(colReleaseDate);

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dtPainting.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = string.Format("{0}\n{1}\n{2}", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize);
                column.MinWidth = 45;
                if (account.OutsoleMaterialEdit == false)
                {
                    column.IsReadOnly = true;
                }
                column.Binding = new Binding(String.Format("Column{0}", i));

                Style styleColumn = new Style();
                Setter setterColumnForecolor = new Setter();
                setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", i));

                //Setter setterAlignText = new Setter();
                //setterAlignText.Property = DataGridCell.HorizontalAlignmentProperty;
                //setterAlignText.Value = HorizontalAlignment.Center;

                styleColumn.Setters.Add(setterColumnForecolor);
                //styleColumn.Setters.Add(setterAlignText);

                column.CellStyle = styleColumn;
                DataColumn columnForeground = new DataColumn(String.Format("Column{0}Foreground", i), typeof(SolidColorBrush));
                columnForeground.DefaultValue = Brushes.Black;

                dtPainting.Columns.Add(columnForeground);
                dgReleasePainting.Columns.Add(column);
            }

            dtPainting.Columns.Add("TotalRelease", typeof(String));
            DataGridTextColumn colTotal = new DataGridTextColumn();
            colTotal.Header = "Total";
            colTotal.MinWidth = 80;
            colTotal.IsReadOnly = true;
            colTotal.Binding = new Binding("TotalRelease");
            dgReleasePainting.Columns.Add(colTotal);

            // Binding Data
            foreach (var supplier in supplierPaintedList)
            {
                var releaseDateList = OSMReleasePaintingList.Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).Select(s => s.ReleaseDate.Date).Distinct().ToList();
                if (releaseDateList.Count() > 1)
                    releaseDateList = releaseDateList.OrderBy(o => o).ToList();
                var releasePaintingListBySupplier = OSMReleasePaintingList.Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();

                bool displayedSupplier = false;
                List<Int32> qtyReleaseBySupplierList = new List<int>();
                foreach (var releaseDate in releaseDateList)
                {
                    DataRow dr = dtPainting.NewRow();
                    if (displayedSupplier == false)
                        dr["SupplierName"] = supplier.Name;

                    dr["SupplierId"] = supplier.OutsoleSupplierId;
                    dr["ReleaseDate"] = String.Format("{0:MM/dd/yyyy}", releaseDate);
                    var releasePaintingListByDate = releasePaintingListBySupplier.Where(w => w.ReleaseDate == releaseDate).ToList();
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        var sizeRun = sizeRunList[i];
                        var releasedBySize = releasePaintingListByDate.FirstOrDefault(f => f.SizeNo == sizeRun.SizeNo);
                        if (releasedBySize != null && releasedBySize.Quantity > 0)
                            dr[String.Format("Column{0}", i)] = releasedBySize.Quantity;

                    }

                    int totalReleasedByDate = releasePaintingListByDate.Sum(s => s.Quantity);
                    if (totalReleasedByDate > 0)
                        dr["TotalRelease"] = totalReleasedByDate;

                    dtPainting.Rows.Add(dr);
                    displayedSupplier = true;
                }

                int qtyReleased = releasePaintingListBySupplier.Sum(s => s.Quantity);
                if (qtyReleased == 0)
                    continue;

                // dr Total, Balance Released By Size
                DataRow drTotal = dtPainting.NewRow();
                drTotal["SupplierId"] = supplier.OutsoleSupplierId;
                drTotal["ReleaseDate"] = "Total";

                DataRow drBalance = dtPainting.NewRow();
                drBalance["SupplierId"] = supplier.OutsoleSupplierId;
                drBalance["ReleaseDate"] = "Balance";

                for (int i = 0; i < sizeRunList.Count; i++)
                {
                    var sizeRun = sizeRunList[i];
                    int releaseBySize = releasePaintingListBySupplier.Where(w => w.SizeNo == sizeRun.SizeNo).Sum(s => s.Quantity);
                    int balanceBySize = sizeRun.Quantity - releaseBySize;
                    if (releaseBySize > 0)
                    {
                        drTotal[String.Format("Column{0}", i)] = releaseBySize;
                        if (releaseBySize == sizeRun.Quantity)
                            drTotal[String.Format("Column{0}Foreground", i)] = Brushes.Blue;
                        else
                            drTotal[String.Format("Column{0}Foreground", i)] = Brushes.Red;
                    }
                    if (balanceBySize > 0)
                        drBalance[String.Format("Column{0}", i)] = balanceBySize;
                }
                int totalReleased = releasePaintingListBySupplier.Sum(s => s.Quantity);
                int totalOrder = sizeRunList.Sum(s => s.Quantity);
                int totalBalance = totalOrder - totalReleased;
                if (totalReleased > 0)
                    drTotal["TotalRelease"] = totalReleased;
                if (totalBalance > 0)
                    drBalance["TotalRelease"] = totalBalance;
                if (releaseDateList.Count() > 1)
                    dtPainting.Rows.Add(drTotal);
                dtPainting.Rows.Add(drBalance);
            }
            dgReleasePainting.ItemsSource = dtPainting.AsDataView();
        }

        List<OutsoleMaterialReleasePaintingModel> insertReleasePaintingList;
        private void dgReleasePainting_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.GetValue(TagProperty) == null)
                return;

            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
                return;

            var supplierId = (Int32)((DataRowView)e.Row.Item)["SupplierId"];
            var releaseDateString = (String)((DataRowView)e.Row.Item)["ReleaseDate"];
            if (releaseDateString == "Total" || releaseDateString == "Balance")
                return;
            DateTime releaseDate = TimeHelper.Convert(releaseDateString);

            int qtyOrder = sizeRunList.FirstOrDefault(f => f.SizeNo == sizeNo).Quantity;
            int qtyOldOthers = currentReleasePaintingList.Where(w => w.OutsoleSupplierId == supplierId && w.SizeNo == sizeNo && w.ReleaseDate != releaseDate).Sum(s => s.Quantity);
            int qtyOld = currentReleasePaintingList.Where(w => w.OutsoleSupplierId == supplierId && w.SizeNo == sizeNo && w.ReleaseDate == releaseDate).Sum(s => s.Quantity);

            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyInputting = 0;
            Int32.TryParse(txtCurrent.Text.ToString(), out qtyInputting);

            if (qtyInputting + qtyOld + qtyOldOthers > qtyOrder || qtyInputting + qtyOld < 0)
                txtCurrent.Text = qtyOld.ToString();
            else
            {
                txtCurrent.Text = (qtyInputting + qtyOld).ToString();
                insertReleasePaintingList.RemoveAll(r => r.OutsoleSupplierId == supplierId && r.ReleaseDate == releaseDate && r.SizeNo == sizeNo);
                insertReleasePaintingList.Add(new OutsoleMaterialReleasePaintingModel
                {
                    ProductNo = productNo,
                    OutsoleSupplierId = supplierId,
                    ReleaseDate = releaseDate,
                    SizeNo = sizeNo,
                    Quantity = qtyInputting + qtyOld
                });
            }

            // update column total release.
            int qtyCurrent = 0;
            Int32.TryParse(txtCurrent.Text.ToString(), out qtyCurrent);
            List<ReleaseTemp> releaseTemps = new List<ReleaseTemp>();
            for (int r = 0; r < dtPainting.Rows.Count; r++)
            {
                DataRow dr = dtPainting.Rows[r];
                if ((Int32)dr["SupplierId"] != supplierId)
                    continue;
                // Colect data release
                if (dr["ReleaseDate"].ToString() != releaseDateString &&
                    dr["ReleaseDate"].ToString() != "Total" &&
                    dr["ReleaseDate"].ToString() != "Balance")
                {
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        var sizeRunPerSize = sizeRunList[i];
                        int qtyBySize = 0;
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qtyBySize);
                        releaseTemps.Add(new ReleaseTemp { SizeNo = sizeRunPerSize.SizeNo, QuantityRelease = qtyBySize });
                    }
                }

                int totalRelease = 0;
                // Update row release date 
                if (dr["ReleaseDate"].ToString() == releaseDateString)
                {
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        int qtyBySize = 0;
                        var sizeRunPerSize = sizeRunList[i];

                        if (sizeRunPerSize.SizeNo == sizeNo)
                        {
                            qtyBySize = qtyCurrent;
                            dr[String.Format("Column{0}", i)] = qtyBySize;
                        }
                        else
                            Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qtyBySize);

                        totalRelease += qtyBySize;
                        releaseTemps.Add(new ReleaseTemp { SizeNo = sizeRunPerSize.SizeNo, QuantityRelease = qtyBySize });
                    }
                    dr["TotalRelease"] = totalRelease;
                }

                // Update row total release.
                if (dr["ReleaseDate"].ToString() == "Total")
                {
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        var sizeRunPerSize = sizeRunList[i];
                        var relesedBySize = releaseTemps.Where(w => w.SizeNo == sizeRunPerSize.SizeNo).Sum(s => s.QuantityRelease);
                        dr[String.Format("Column{0}", i)] = relesedBySize;

                        if (relesedBySize == sizeRunPerSize.Quantity)
                            dr[String.Format("Column{0}Foreground", i)] = Brushes.Blue;
                        else
                            dr[String.Format("Column{0}Foreground", i)] = Brushes.Red;
                    }
                    dr["TotalRelease"] = releaseTemps.Sum(s => s.QuantityRelease);
                }

                // Update row total balance
                if (dr["ReleaseDate"].ToString() == "Balance")
                {
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        var sizeRunPerSize = sizeRunList[i];
                        var totalBalance = releaseTemps.Where(w => w.SizeNo == sizeRunPerSize.SizeNo).Sum(s => s.QuantityRelease);
                        dr[String.Format("Column{0}", i)] = sizeRunPerSize.Quantity - totalBalance;
                    }
                    dr["TotalRelease"] = sizeRunList.Sum(s => s.Quantity) - releaseTemps.Sum(s => s.QuantityRelease);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void dgReleasePainting_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var releaseDateColValue = (String)((DataRowView)e.Row.Item)["ReleaseDate"];
            var supplierid = (Int32)((DataRowView)e.Row.Item)["SupplierId"];
            var sources = from myRow in dtPainting.AsEnumerable()
                          where myRow.Field<int>("SupplierId") == supplierid
                          select myRow;

            if (releaseDateColValue == "Total" || releaseDateColValue == "Balance")
            {
                e.Cancel = true;
            }
        }

        private void radInputReject_Checked(object sender, RoutedEventArgs e)
        {
            definePrivate.InputReject = true;
        }

        private void radNotInputReject_Checked(object sender, RoutedEventArgs e)
        {
            definePrivate.InputReject = false;
        }

        private class ReleaseTemp
        {
            public string SizeNo { get; set; }
            public int QuantityRelease { get; set; }
            public string ReleaseDate { get; set; }
        }

        private class RackBinding
        {
            public int ColumnID { get; set; }
            public int SupplierID { get; set; }
            public string RackNumber { get; set; }
            public int CartonNo { get; set; }
            public string RackPosition { get; set; }
        }
    }
}
