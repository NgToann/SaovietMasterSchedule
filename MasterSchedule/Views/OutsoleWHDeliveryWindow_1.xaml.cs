using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
using System.ComponentModel;
using System.Data;
using System.Threading;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHDeliveryWindow_1.xaml
    /// </summary>
    public partial class OutsoleWHDeliveryWindow_1 : Window
    {
        BackgroundWorker bwLoad;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OutsoleSuppliersModel> supplierList;
        List<OrdersModel> ordersList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SizeRunModel> sizeRunList;
        List<ReportOutsoleMaterialDeliverySummary> osDeliverySummaryList;
        public OutsoleWHDeliveryWindow_1()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            outsoleMaterialList         = new List<OutsoleMaterialModel>();
            outsoleReleaseMaterialList  = new List<OutsoleReleaseMaterialModel>();
            supplierList                = new List<OutsoleSuppliersModel>();

            ordersList              = new List<OrdersModel>();
            outsoleMasterList       = new List<OutsoleMasterModel>();
            sizeRunList             = new List<SizeRunModel>();
            osDeliverySummaryList   = new List<ReportOutsoleMaterialDeliverySummary>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                lblStatus.Text = String.Format("Creating Columns");
            }));

            outsoleMaterialList         = OutsoleMaterialController.Select();
            outsoleReleaseMaterialList  = OutsoleReleaseMaterialController.SelectByOutsoleMaterial();
            supplierList                = OutsoleSuppliersController.Select();
            ordersList                  = OrdersController.Select();
            outsoleMasterList           = OutsoleMasterController.Select();
            sizeRunList                 = SizeRunController.SelectIsEnable();

            osDeliverySummaryList = ReportController.GetOSDeliverySummary();

            var productNoList = ordersList.Select(s => s.ProductNo).Distinct().ToList();
            var outsoleCodeList = osDeliverySummaryList.Select(s => s.OutsoleCode).Distinct().ToList();            

            // Create Column
            DataTable dt = new DataTable();
            Dispatcher.Invoke(new Action(() =>
            {
                // Column OutsoleCode
                dt.Columns.Add("OutsoleCode", typeof(String));
                DataGridTemplateColumn colOutsoleCode = new DataGridTemplateColumn();
                colOutsoleCode.Header = String.Format("Outsole Code");
                DataTemplate templateOSCode = new DataTemplate();
                FrameworkElementFactory tblOSCodeFactory = new FrameworkElementFactory(typeof(TextBlock));
                templateOSCode.VisualTree = tblOSCodeFactory;
                tblOSCodeFactory.SetValue(TextBlock.TextProperty, "");
                tblOSCodeFactory.SetBinding(TextBlock.TextProperty, new Binding(String.Format("OutsoleCode")));
                tblOSCodeFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
                tblOSCodeFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblOSCodeFactory.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colOutsoleCode.CellTemplate = templateOSCode;
                colOutsoleCode.ClipboardContentBinding = new Binding(String.Format("OutsoleCode"));
                dgWHDelivery.Columns.Add(colOutsoleCode);

                // Column OutsoleCode
                dt.Columns.Add("OrderQuantity", typeof(String));
                DataGridTemplateColumn colOrderQty = new DataGridTemplateColumn();
                colOrderQty.Header = String.Format("Order Quantity");
                DataTemplate templateOrderQty = new DataTemplate();
                FrameworkElementFactory tblOderQty = new FrameworkElementFactory(typeof(TextBlock));
                templateOrderQty.VisualTree = tblOderQty;
                tblOderQty.SetValue(TextBlock.TextProperty, "");
                tblOderQty.SetBinding(TextBlock.TextProperty, new Binding(String.Format("OrderQuantity")));
                tblOderQty.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblOderQty.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colOrderQty.CellTemplate = templateOrderQty;
                colOrderQty.ClipboardContentBinding = new Binding(String.Format("OrderQuantity"));
                dgWHDelivery.Columns.Add(colOrderQty);

                // Column WHQuantity
                dt.Columns.Add("WHQuantity", typeof(String));
                DataGridTemplateColumn colWHQuantity = new DataGridTemplateColumn();
                colWHQuantity.Header = String.Format("WH Quantity");
                DataTemplate templateWHQuantity = new DataTemplate();
                FrameworkElementFactory tblWHQuantity = new FrameworkElementFactory(typeof(TextBlock));
                templateWHQuantity.VisualTree = tblWHQuantity;
                tblWHQuantity.SetBinding(TextBlock.TextProperty, new Binding(String.Format("WHQuantity")));
                tblWHQuantity.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblWHQuantity.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colWHQuantity.CellTemplate = templateWHQuantity;
                colWHQuantity.ClipboardContentBinding = new Binding(String.Format("WHQuantity"));
                dgWHDelivery.Columns.Add(colWHQuantity);

                // Column WHQuantity
                dt.Columns.Add("Reject", typeof(String));
                DataGridTemplateColumn colReject = new DataGridTemplateColumn();
                colReject.Header = String.Format("Reject");
                DataTemplate templateReject = new DataTemplate();
                FrameworkElementFactory tblReject = new FrameworkElementFactory(typeof(TextBlock));
                templateReject.VisualTree = tblReject;
                tblReject.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Reject")));
                tblReject.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                tblReject.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                colReject.CellTemplate = templateReject;
                colReject.ClipboardContentBinding = new Binding(String.Format("Reject"));
                dgWHDelivery.Columns.Add(colReject);

                // Column Matching
                dt.Columns.Add("Matching", typeof(String));
                DataGridTemplateColumn colMatching = new DataGridTemplateColumn();
                colMatching.Header = String.Format("Matching");
                DataTemplate templateMatching = new DataTemplate();
                FrameworkElementFactory tblMatching = new FrameworkElementFactory(typeof(TextBlock));
                templateMatching.VisualTree = tblMatching;
                tblMatching.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Matching")));
                tblMatching.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                tblMatching.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                colMatching.CellTemplate = templateMatching;
                colMatching.ClipboardContentBinding = new Binding(String.Format("Matching"));
                dgWHDelivery.Columns.Add(colMatching);

                // Column Release
                dt.Columns.Add("Release", typeof(String));
                DataGridTemplateColumn colRelease = new DataGridTemplateColumn();
                colRelease.Header = String.Format("Release");
                DataTemplate templateRelease = new DataTemplate();
                FrameworkElementFactory tblRelease = new FrameworkElementFactory(typeof(TextBlock));
                templateRelease.VisualTree = tblRelease;
                tblRelease.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Release")));
                tblRelease.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                tblRelease.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                colRelease.CellTemplate = templateRelease;
                colRelease.ClipboardContentBinding = new Binding(String.Format("Release"));
                dgWHDelivery.Columns.Add(colRelease);

                // Column Suppliers
                int maxNumberOfSupplier = osDeliverySummaryList.Max(m => m.NoOfSupp);
                for (int i = 0; i < maxNumberOfSupplier; i++)
                {
                    dt.Columns.Add(String.Format("Column{0}", i), typeof(String));
                    dt.Columns.Add(String.Format("ColumnSupplierID{0}", i), typeof(Int32));
                    DataGridTemplateColumn colSupplier = new DataGridTemplateColumn();
                    colSupplier.Header = String.Format("Supplier {0}", i + 1);
                    DataTemplate templateSupplier = new DataTemplate();
                    FrameworkElementFactory tblSupplier = new FrameworkElementFactory(typeof(TextBlock));
                    //FrameworkElementFactory tblSupplier = new FrameworkElementFactory(typeof(TextBox));
                    templateSupplier.VisualTree = tblSupplier;
                    tblSupplier.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", i)));
                    //tblSupplier.SetBinding(TextBlock.TagProperty, new Binding(String.Format("Column{0}", i)));
                    tblSupplier.SetValue(TextBlock.PaddingProperty, new Thickness(3, 3, 3, 3));
                    //tblSupplier.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblSupplier.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

                    colSupplier.CellTemplate = templateSupplier;
                    colSupplier.ClipboardContentBinding = new Binding(String.Format("Column{0}", i));
                    dgWHDelivery.Columns.Add(colSupplier);
                }
            }));

            // Binding Data
            int indexProgressBar = 1;
            Dispatcher.Invoke(new Action(() => {
                progressBar.Maximum = outsoleCodeList.Count();
            }));
            int totalQtyOrder = 0, totalQtyWH = 0, totalQtyReject = 0, totalQtyMatch = 0, totalQtyRelease = 0;
            foreach (var outsoleCode in outsoleCodeList)
            {
                //var osDeliverySummaryByOSCode = osDeliverySummaryList.FirstOrDefault(f => f.OutsoleCode == outsoleCode);

                DataRow dr = dt.NewRow();
                //dr["OutsoleCode"]   = outsoleCode;
                //dr["OrderQuantity"] = string.Format("{0:#,0}", osDeliverySummaryByOSCode.QtyOrder);
                //dr["WHQuantity"]    = string.Format("{0:#,0}", osDeliverySummaryByOSCode.QtyWH);
                //dr["Reject"]        = string.Format("{0:#,0}", osDeliverySummaryByOSCode.QtyReject);
                //dr["Matching"]      = string.Format("{0:#,0}", osDeliverySummaryByOSCode.QtyMatch);
                //dr["Release"]       = string.Format("{0:#,0}", osDeliverySummaryByOSCode.QtyRelease);
                
                //totalQtyOrder       += osDeliverySummaryByOSCode.QtyOrder;
                //totalQtyWH          += osDeliverySummaryByOSCode.QtyWH;
                //totalQtyReject      += osDeliverySummaryByOSCode.QtyReject;
                //totalQtyMatch       += osDeliverySummaryByOSCode.QtyMatch;
                //totalQtyRelease     += osDeliverySummaryByOSCode.QtyRelease;

                var osDeliveryByOScode  = osDeliverySummaryList.Where(w => w.OutsoleCode == outsoleCode).ToList();

                dr["OutsoleCode"]       = outsoleCode;
                dr["OrderQuantity"]     = string.Format("{0:#,0}", osDeliveryByOScode.Max(m => m.QtyOrder));
                dr["WHQuantity"]        = string.Format("{0:#,0}", osDeliveryByOScode.Max(m => m.QtyWH));
                dr["Reject"]            = string.Format("{0:#,0}", osDeliveryByOScode.Max(m => m.QtyReject));
                dr["Matching"]          = string.Format("{0:#,0}", osDeliveryByOScode.Max(m => m.QtyMatch));
                dr["Release"]           = string.Format("{0:#,0}", osDeliveryByOScode.Max(m => m.QtyRelease));

                totalQtyOrder           += osDeliveryByOScode.Max(m => m.QtyOrder);
                totalQtyWH              += osDeliveryByOScode.Max(m => m.QtyWH);
                totalQtyReject          += osDeliveryByOScode.Max(m => m.QtyReject);
                totalQtyMatch           += osDeliveryByOScode.Max(m => m.QtyMatch);
                totalQtyRelease         += osDeliveryByOScode.Max(m => m.QtyRelease);

                var supplierIDList_OSCode = osDeliverySummaryList.Where(w => w.OutsoleCode == outsoleCode).Select(s => s.OutsoleSupplierId).Distinct().ToList();
                for (int i = 0; i < supplierIDList_OSCode.Count(); i++)
                {
                    int supplierID = supplierIDList_OSCode[i];
                    var osDeliverySummaryByOSCodeBySupplier = osDeliverySummaryList.FirstOrDefault(w => w.OutsoleCode == outsoleCode && w.OutsoleSupplierId == supplierID);
                    
                    string numberOfPO = osDeliverySummaryByOSCodeBySupplier.NoOfPO > 0 ? 
                                                                                    string.Format("({0:#,0})", osDeliverySummaryByOSCodeBySupplier.NoOfPO) :
                                                                                    "";
                    string quantityDelivery_SupplierString  = osDeliverySummaryByOSCodeBySupplier.QtyDelivery > 0 ? string.Format("\nDelivery: {0:#,0}", osDeliverySummaryByOSCodeBySupplier.QtyDelivery)   : "";
                    string reject_SupplierString            = osDeliverySummaryByOSCodeBySupplier.QtyReject > 0 ?   string.Format("\nReject:     {0:#,0}", osDeliverySummaryByOSCodeBySupplier.QtyReject)   : "";
                    string balance_SupplierString           = osDeliverySummaryByOSCodeBySupplier.QtyBalance > 0 ?  string.Format("\nBalance:  {0:#,0}", osDeliverySummaryByOSCodeBySupplier.QtyBalance)    : "";

                    dr[String.Format("Column{0}", i)] = String.Format("{0} {1}{2}{3}{4}",
                                                                        osDeliverySummaryByOSCodeBySupplier.Name,
                                                                        numberOfPO,
                                                                        quantityDelivery_SupplierString,
                                                                        reject_SupplierString,
                                                                        balance_SupplierString);
                    dr[String.Format("ColumnSupplierID{0}", i)] = supplierIDList_OSCode[i];
                }
                dt.Rows.Add(dr);
                Dispatcher.Invoke(new Action(() =>
                {
                    progressBar.Value = indexProgressBar;
                    lblStatus.Text = String.Format("Writing  {0} rows / {1}", indexProgressBar, outsoleCodeList.Count());
                    Thread.Sleep(50);
                }));
                indexProgressBar++;
            }

            DataRow drTotal = dt.NewRow();
            drTotal["OutsoleCode"]      = "Total";
            drTotal["OrderQuantity"]    = string.Format("{0:#,0}", totalQtyOrder);
            drTotal["WHQuantity"]       = string.Format("{0:#,0}", totalQtyWH);
            drTotal["Reject"]           = string.Format("{0:#,0}", totalQtyReject);
            drTotal["Matching"]         = string.Format("{0:#,0}", totalQtyMatch);
            drTotal["Release"]          = string.Format("{0:#,0}", totalQtyRelease);

            dt.Rows.Add(drTotal);
            e.Result = dt;
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            var dt = e.Result as DataTable;
            dgWHDelivery.ItemsSource = dt.AsDataView();
            lblStatus.Text = "";
            progressBar.Value = 0;
        }

        protected class SupplierPerOSCode 
        {
            public string OutsoleCode { get; set; }
            public int NumberOfSupplier { get; set; }
        }

        private void dgWHDelivery_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dgWHDelivery.ItemsSource == null)
                return;
            int columnNo = dgWHDelivery.CurrentColumn.DisplayIndex;
            var cellClicked = dgWHDelivery.CurrentItem as DataRowView;
            Int32[] columnsReturn = { 0, 1, 2, 3, 4, 5 };
            if (columnsReturn.Contains(columnNo))
                return;
            int columnIndexArray = columnNo + columnNo - columnsReturn.Max();
            if (columnIndexArray > cellClicked.Row.ItemArray.Count())
                return;
            var supplierIDClicked = cellClicked.Row.ItemArray[columnIndexArray].ToString();
            if (String.IsNullOrEmpty(supplierIDClicked))
                return;

            var supplierClicked = supplierList.FirstOrDefault(w => w.OutsoleSupplierId.ToString() == supplierIDClicked);
            var outsoleCode = cellClicked.Row.ItemArray[0].ToString();
            //var productHaveMaterialScheduleList = outsoleMaterialList.Select(s => s.ProductNo).Distinct().ToList();
            //var productNoList = ordersList.Select(s => s.ProductNo).Distinct().ToList();
            var orderList_OSCode                = ordersList.Where(w => w.OutsoleCode == outsoleCode).ToList();
            var productNoList_OSCode            = orderList_OSCode.Select(s => s.ProductNo).Distinct().ToList();
            var osMaterialList_OSCode_Supplier  = outsoleMaterialList.Where(w => productNoList_OSCode.Contains(w.ProductNo) && w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            var osReleaseList_OSCode            = outsoleReleaseMaterialList.Where(w => productNoList_OSCode.Contains(w.ProductNo)).ToList();
            var sizeRunList_OSCode              = sizeRunList.Where(w => productNoList_OSCode.Contains(w.ProductNo)).ToList();
            var poHasMaterial_Supplier          = osMaterialList_OSCode_Supplier.Select(s => s.ProductNo).Distinct().ToList();
            var orderHasMaterial_SupplierList   = ordersList.Where(w => poHasMaterial_Supplier.Contains(w.ProductNo) && w.OutsoleCode == outsoleCode).ToList();

            OutsoleWHDeliveryDetailWindow_1 window = new OutsoleWHDeliveryDetailWindow_1(outsoleCode,
                                                                                            osMaterialList_OSCode_Supplier,
                                                                                            osReleaseList_OSCode,
                                                                                            sizeRunList_OSCode,
                                                                                            supplierClicked,
                                                                                            orderHasMaterial_SupplierList);
            window.Show();
        }
    }
}
