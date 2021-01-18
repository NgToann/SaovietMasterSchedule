using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using MasterSchedule.Models;
using Microsoft.Reporting.WinForms;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperAccessoriesReportWindow.xaml
    /// </summary>
    public partial class UpperAccessoriesReportWindow : Window
    {
        BackgroundWorker bwLoad;
        List<ReportUpperAccessoriesSummaryModel> summaryReportList;
        List<ReportUpperAccessoriesModel> reportUpperAccessoriesList;
        List<SupplierModel> supplierList;
        int idRowTotal = -9999;
        int supIdDefault = -999;
        private DateTime dtDefault = new DateTime(2000, 01, 01);
        private bool loaded = false;
        private string header = "UPPER ACCESSORIES DELIVERY REPORT";
        private EReportWhat eRpWhat = EReportWhat.Delivery;

        public UpperAccessoriesReportWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            summaryReportList = new List<ReportUpperAccessoriesSummaryModel>();
            supplierList = new List<SupplierModel>();
            reportUpperAccessoriesList = new List<ReportUpperAccessoriesModel>();

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

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            summaryReportList = ReportController.SelectUpperAccessoriesDeliverySummary();
            supplierList = SupplierController.GetSuppliers();
            reportUpperAccessoriesList = ReportController.GetUpperAccessories();
            Dispatcher.Invoke(new Action(() =>
            {
                CreateReport();
            }));
        }

        private void CreateReport()
        {
            DataTable dt = new UpperAccessoriesDataSet().Tables["UpperAccessoriesTable"];
            var reportList = new List<ReportUpperAccessoriesModel>();
            int showRejectColumn = 0, showDeliveryColumn = 0, showBalanceColumn = 0;
            string totalTitle = "";
            if (eRpWhat == EReportWhat.Delivery)
            {
                reportList = reportUpperAccessoriesList.Where(w => w.QuantityDelivery > 0).ToList();
                totalTitle = "Delivery";
            }
            else if (eRpWhat == EReportWhat.Reject)
            {
                reportList = reportUpperAccessoriesList.Where(w => w.Reject > 0).ToList();
                totalTitle = "Reject";
            }
            else if (eRpWhat == EReportWhat.Balance)
            {
                reportList = reportUpperAccessoriesList.Where(w => w.Balance > 0).ToList();
                totalTitle = "Balance";
            }
            else if (eRpWhat == EReportWhat.BalanceAndReject)
            {
                reportList = reportUpperAccessoriesList.Where(w => w.BalanceAndReject > 0).ToList();
                totalTitle = "Balance";
            }

            var supplierIdList = reportList.Select(s => s.SupplierId).Distinct().ToList();
            
            foreach (var supplierId in supplierIdList)
            {
                var productNolist = reportList.Where(w => w.SupplierId == supplierId).Select(s => s.ProductNo).Distinct().ToList();
                var reportBySuppList = reportList.Where(w => w.SupplierId == supplierId).ToList();
                var reportBySuppFirst = reportBySuppList.FirstOrDefault();
                int totalQtyDisplayBySupp = 0;
                foreach (var productNo in productNolist)
                {
                    var reportByPOList  = reportBySuppList.Where(w => w.ProductNo == productNo).ToList();
                    var reportByPOFirst = reportByPOList.FirstOrDefault();

                    int totalQtyDisplay = 0;
                    var sizeNoList = reportByPOList.Select(s => s.SizeNo).Distinct().ToList();
                    foreach (var sizeNo in sizeNoList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SupplierId"]    = reportByPOFirst.SupplierId;
                        dr["ProductNo"]     = productNo;
                        dr["SupplierName"]  = String.Format("Supplier: {0} - {1}", reportByPOFirst.SupplierName, reportByPOFirst.ProvideAccessories);
                        dr["ArticleNo"]     = reportByPOFirst.ArticleNo;
                        dr["ShoeName"]      = reportByPOFirst.ShoeName;
                        if (reportByPOFirst.SupplierEFD != dtDefault)
                            dr["SupplierEFD"] = String.Format("{0:MM/dd/yyyy}", reportByPOFirst.SupplierEFD);
                        if (reportByPOFirst.ActualDeliveryDate != dtDefault)
                            dr["ActualDeliveryDate"] = String.Format("{0:MM/dd/yyyy}", reportByPOFirst.ActualDeliveryDate);

                        var reportBySize = reportByPOList.FirstOrDefault(f => f.SizeNo == sizeNo);
                        dr["SizeNo"] = sizeNo;

                        double sizeDouble = 0;
                        Double.TryParse(sizeNo, out sizeDouble);
                        sizeDouble = sizeDouble > 0 ? sizeDouble : 100;
                        dr["SizeNoDouble"] = sizeDouble;
                        int qtyDisplay = 0;
                        if (eRpWhat == EReportWhat.Delivery)
                        {
                            qtyDisplay              = reportBySize.QuantityDelivery;
                            totalQtyDisplay         = reportByPOList.Sum(s => s.QuantityDelivery);
                            totalQtyDisplayBySupp   = reportBySuppList.Sum(s => s.QuantityDelivery);
                            showRejectColumn = 1;
                            showBalanceColumn = 1;
                            showDeliveryColumn = 0;
                        }
                        else if (eRpWhat == EReportWhat.Reject)
                        {
                            qtyDisplay              = reportBySize.Reject;
                            totalQtyDisplay         = reportByPOList.Sum(s => s.Reject);
                            totalQtyDisplayBySupp   = reportBySuppList.Sum(s => s.Reject);

                            showRejectColumn = 0;
                            showBalanceColumn = 0;
                            showDeliveryColumn = 1;
                        }
                        else if (eRpWhat == EReportWhat.Balance)
                        {
                            qtyDisplay              = reportBySize.Balance;
                            totalQtyDisplay         = reportByPOList.Sum(s => s.Balance);
                            totalQtyDisplayBySupp   = reportBySuppList.Sum(s => s.Balance);

                            showRejectColumn = 0;
                            showBalanceColumn = 0;
                            showDeliveryColumn = 1;
                        }
                        else if (eRpWhat == EReportWhat.BalanceAndReject)
                        {
                            qtyDisplay              = reportBySize.BalanceAndReject;
                            totalQtyDisplay         = reportByPOList.Sum(s => s.BalanceAndReject);
                            totalQtyDisplayBySupp   = reportBySuppList.Sum(s => s.BalanceAndReject);
                            
                            showRejectColumn = 0;
                            showBalanceColumn = 0;
                            showDeliveryColumn = 1;
                        }

                        if (qtyDisplay  > 0)
                            dr["QuantityDisplay"] = qtyDisplay;

                        dr["TotalQuantityDisplay"] = totalQtyDisplay > 0 ? totalQtyDisplay.ToString() : "";

                        dr["TotalDelivery"] = reportByPOList.Sum(s => s.QuantityDelivery) > 0 ? reportByPOList.Sum(s => s.QuantityDelivery).ToString() : "";
                        dr["TotalReject"]   = reportByPOList.Sum(s => s.Reject) > 0 ? reportByPOList.Sum(s => s.Reject).ToString() : "";
                        dr["TotalBalance"]  = reportByPOList.Sum(s => s.Balance) > 0 ? reportByPOList.Sum(s => s.Balance).ToString() : "";
                        
                        dt.Rows.Add(dr);
                    }
                }


                DataRow drTotal = dt.NewRow();
                drTotal["SupplierId"]   = reportBySuppFirst.SupplierId;
                drTotal["ProductNo"]    = "TOTAL";
                drTotal["SizeNoDouble"] = -1;
                drTotal["SupplierName"] = String.Format("Supplier: {0} - {1}", reportBySuppFirst.SupplierName, reportBySuppFirst.ProvideAccessories);
                drTotal["TotalQuantityDisplay"] = totalQtyDisplayBySupp > 0 ? totalQtyDisplayBySupp.ToString() : "";
                
                drTotal["TotalDelivery"]    = reportBySuppList.Sum(s => s.QuantityDelivery) > 0 ? reportBySuppList.Sum(s => s.QuantityDelivery).ToString() : "";
                drTotal["TotalReject"]      = reportBySuppList.Sum(s => s.Reject) > 0 ? reportBySuppList.Sum(s => s.Reject).ToString() : "";
                drTotal["TotalBalance"]     = reportBySuppList.Sum(s => s.Balance) > 0 ? reportBySuppList.Sum(s => s.Balance).ToString() : "";

                if (productNolist.Count() > 1)
                    dt.Rows.Add(drTotal);
            }

            ReportDataSource rds = new ReportDataSource();
            ReportParameter rp = new ReportParameter("ReportHeader", header);
            ReportParameter rp1 = new ReportParameter("ShowRejectColumn",   showRejectColumn.ToString());
            ReportParameter rp2 = new ReportParameter("ShowDeliveryColumn", showDeliveryColumn.ToString());
            ReportParameter rp3 = new ReportParameter("ShowBalanceColumn",  showBalanceColumn.ToString());
            ReportParameter rp4 = new ReportParameter("TotalTitle", totalTitle);

            rds.Name = "UpperAccessoriesRejectSource";
            rds.Value = dt;

            reportViewer.LocalReport.ReportPath = @"Reports\UpperAccessoriesRejectReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp, rp1, rp2, rp3, rp4 });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var supplierHasDeliveryList = summaryReportList.Select(s => s.SupplierId).Distinct().ToList();
            var supplierFilterList = new List<SupplierModel>();
            supplierFilterList.Add(new SupplierModel
            {
                Name = "-- Select a supplier --",
                SupplierId = supIdDefault
            });

            supplierFilterList.AddRange(supplierList.Where(w => supplierHasDeliveryList.Contains(w.SupplierId)).ToList());
            cboSupplier.ItemsSource     = supplierFilterList;
            cboSupplier.SelectedItem    = supplierFilterList.FirstOrDefault();

            ReloadDatagrid(summaryReportList);
            this.Cursor = null;
            btnRefresh.IsEnabled = true;
            btnRefresh1.IsEnabled = true;
            loaded = true;
        }

        private void ReloadDatagrid(List<ReportUpperAccessoriesSummaryModel> reloadList)
        {
            // Create Column
            dgSummaryReport.Columns.Clear();
            DataTable dt = new DataTable();

            //Column Supplier name
            dt.Columns.Add("AccessoriesName", typeof(String));
            DataGridTemplateColumn colAccessoriesName = new DataGridTemplateColumn();
            colAccessoriesName.Header = String.Format("Accessories Name");
            DataTemplate tmpAccessorisName = new DataTemplate();
            FrameworkElementFactory tblAccessoriesName = new FrameworkElementFactory(typeof(TextBlock));
            tmpAccessorisName.VisualTree = tblAccessoriesName;
            tblAccessoriesName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("AccessoriesName")));
            tblAccessoriesName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblAccessoriesName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colAccessoriesName.CellTemplate = tmpAccessorisName;
            colAccessoriesName.SortMemberPath = "AccessoriesName";
            colAccessoriesName.ClipboardContentBinding = new Binding(String.Format("AccessoriesName"));
            dgSummaryReport.Columns.Add(colAccessoriesName);

            // Column Supplier Name
            dt.Columns.Add("Name", typeof(String));
            dt.Columns.Add("SupplierId", typeof(int));
            DataGridTemplateColumn colSupplierName = new DataGridTemplateColumn();
            colSupplierName.Header = String.Format("Supplier");
            DataTemplate tmpSupplierName = new DataTemplate();
            FrameworkElementFactory tblSupplierName = new FrameworkElementFactory(typeof(TextBlock));
            tmpSupplierName.VisualTree = tblSupplierName;
            tblSupplierName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Name")));
            tblSupplierName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblSupplierName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colSupplierName.CellTemplate = tmpSupplierName;
            colSupplierName.SortMemberPath = "Name";
            colSupplierName.ClipboardContentBinding = new Binding(String.Format("Name"));
            dgSummaryReport.Columns.Add(colSupplierName);

            // Column Qty Order
            dt.Columns.Add("QuantityOrder", typeof(String));
            DataGridTemplateColumn colQtyOrder = new DataGridTemplateColumn();
            colQtyOrder.Header = String.Format("Qty Order");
            DataTemplate tmpQtyOrder = new DataTemplate();
            FrameworkElementFactory tblQtyOrder = new FrameworkElementFactory(typeof(TextBlock));
            tmpQtyOrder.VisualTree = tblQtyOrder;
            tblQtyOrder.SetBinding(TextBlock.TextProperty, new Binding(String.Format("QuantityOrder")));
            tblQtyOrder.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblQtyOrder.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colQtyOrder.CellTemplate = tmpQtyOrder;
            colQtyOrder.SortMemberPath = "QuantityOrder";
            colQtyOrder.ClipboardContentBinding = new Binding(String.Format("QuantityOrder"));
            dgSummaryReport.Columns.Add(colQtyOrder);

            // Column Qty Check Ok
            dt.Columns.Add("QuantityCheckOK", typeof(String));
            DataGridTemplateColumn colQtyCheckOK = new DataGridTemplateColumn();
            colQtyCheckOK.Header = String.Format("Qty Check OK");
            DataTemplate tmpQtyCheckOK = new DataTemplate();
            FrameworkElementFactory tblQtyCheckOK = new FrameworkElementFactory(typeof(TextBlock));
            tmpQtyCheckOK.VisualTree = tblQtyCheckOK;
            tblQtyCheckOK.SetBinding(TextBlock.TextProperty, new Binding(String.Format("QuantityCheckOK")));
            tblQtyCheckOK.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblQtyCheckOK.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colQtyCheckOK.CellTemplate = tmpQtyCheckOK;
            colQtyCheckOK.SortMemberPath = "QuantityCheckOK";
            colQtyCheckOK.ClipboardContentBinding = new Binding(String.Format("QuantityCheckOK"));
            dgSummaryReport.Columns.Add(colQtyCheckOK);

            var rejectIdList = reloadList.Where(w => w.RejectId != 0).Select(s => s.RejectId).Distinct().ToList();
            if (rejectIdList.Count() > 0)
                rejectIdList = rejectIdList.OrderBy(o => o).ToList();
            foreach (var rId in rejectIdList)
            {
                var rejectName = reloadList.FirstOrDefault(f => f.RejectId == rId);
                dt.Columns.Add(String.Format("Column{0}", rId), typeof(String));
                DataGridTemplateColumn colReject = new DataGridTemplateColumn();
                colReject.Header = String.Format("{0}\n{1}", rejectName.RejectName, rejectName.RejectName_1);
                DataTemplate tmpReject = new DataTemplate();
                FrameworkElementFactory tblReject = new FrameworkElementFactory(typeof(TextBlock));
                tmpReject.VisualTree = tblReject;
                tblReject.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", rId)));
                tblReject.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                tblReject.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblReject.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                colReject.CellTemplate = tmpReject;
                colReject.ClipboardContentBinding = new Binding(String.Format("Column{0}", rId));
                dgSummaryReport.Columns.Add(colReject);
            }

            dt.Columns.Add("QuantityReject", typeof(String));
            DataGridTemplateColumn colQtyReject = new DataGridTemplateColumn();
            colQtyReject.Header = String.Format("Qty Reject");
            DataTemplate tmpQtyReject = new DataTemplate();
            FrameworkElementFactory tblQtyReject = new FrameworkElementFactory(typeof(TextBlock));
            tmpQtyReject.VisualTree = tblQtyReject;
            tblQtyReject.SetBinding(TextBlock.TextProperty, new Binding(String.Format("QuantityReject")));
            tblQtyReject.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblQtyReject.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colQtyReject.CellTemplate = tmpQtyReject;
            colQtyReject.SortMemberPath = "QuantityReject";
            colQtyReject.ClipboardContentBinding = new Binding(String.Format("QuantityReject"));
            dgSummaryReport.Columns.Add(colQtyReject);

            // Binding Data
            var supplierIdList = reloadList.Select(s => s.SupplierId).Distinct().ToList();
            if (supplierIdList.Count() > 0)
                supplierIdList = supplierIdList.OrderBy(o => o).ToList();
            int totalQtyOrder = 0, totalQtyCheckOK = 0, totalQtyReject = 0;
            foreach (var suppId in supplierIdList)
            {
                var reloadBySuppList = reloadList.Where(w => w.SupplierId == suppId).ToList();
                var reloadModel = reloadBySuppList.FirstOrDefault();
                DataRow dr = dt.NewRow();
                dr["AccessoriesName"] = reloadModel.AccessoriesName;
                dr["Name"] = reloadModel.Name;
                dr["SupplierId"] = suppId;

                if (reloadModel.QuantityOrder > 0)
                    dr["QuantityOrder"] = String.Format("{0:N0}", reloadModel.QuantityOrder);
                if (reloadModel.QuantityCheckOK > 0)
                    dr["QuantityCheckOK"] = String.Format("{0:N0}", reloadModel.QuantityCheckOK);
                
                foreach(var reload in reloadBySuppList)
                {
                    if (reload.RejectId == 0)
                        continue;
                    dr[String.Format("Column{0}", reload.RejectId)] = reload.Reject;
                }
                if (reloadModel.QuantityReject > 0 )
                    dr["QuantityReject"] = String.Format("{0:N0}", reloadModel.QuantityReject);

                dt.Rows.Add(dr);
                totalQtyOrder += reloadModel.QuantityOrder;
                totalQtyCheckOK += reloadModel.QuantityCheckOK;
                totalQtyReject += reloadModel.QuantityReject;
            }
            // Row Total
            if (supplierIdList.Count() > 1)
            {
                DataRow drTotal = dt.NewRow();
                drTotal["AccessoriesName"]  = "TOTAL";
                drTotal["SupplierId"] = idRowTotal;

                if (totalQtyOrder > 0)
                    drTotal["QuantityOrder"]    = String.Format("{0:N0}", totalQtyOrder);
                if (totalQtyCheckOK > 0)
                    drTotal["QuantityCheckOK"]  = String.Format("{0:N0}", totalQtyCheckOK);
                if (totalQtyReject > 0)
                    drTotal["QuantityReject"]   = String.Format("{0:N0}", totalQtyReject);

                dt.Rows.Add(drTotal);
            }

            dgSummaryReport.ItemsSource = dt.AsDataView();
            dgSummaryReport.Items.Refresh();
        }

        private void cboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!loaded)
                return;

            var cboSelected = sender as ComboBox;
            var supplierSelected = cboSelected.SelectedItem as SupplierModel;
            if (supplierSelected == null)
                return;
            var refreshList = new List<ReportUpperAccessoriesSummaryModel>();
            if (supplierSelected.SupplierId.Equals(supIdDefault))
                refreshList = summaryReportList.ToList();
            else
                refreshList = summaryReportList.Where(w => w.SupplierId.Equals(supplierSelected.SupplierId)).ToList();

            ReloadDatagrid(refreshList);
        }

        private void txtAccessoriesName_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (dgSummaryReport.ItemsSource == null)
                return;

            string searchWhat = txtAccessoriesName.Text.Trim().ToUpper().ToString();
            var refreshList = new List<ReportUpperAccessoriesSummaryModel>();
            var supplierSelected = cboSupplier.SelectedItem as SupplierModel;

            if (supplierSelected.SupplierId.Equals(supIdDefault))
                refreshList = summaryReportList.ToList();
            else
                refreshList = summaryReportList.Where(w => w.SupplierId.Equals(supplierSelected.SupplierId)).ToList();

            if (!String.IsNullOrEmpty(searchWhat))
                refreshList = refreshList.Where(w => w.AccessoriesName.ToUpper().Contains(searchWhat)).ToList();
            else
                refreshList = refreshList.ToList();

            ReloadDatagrid(refreshList);
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgSummaryReport_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dgSummaryReport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dgSummaryReport.ItemsSource == null)
                return;
            var rowClicked = (DataRowView)dgSummaryReport.SelectedItem;
            if (rowClicked == null)
                return;
            if (rowClicked["SupplierId"].ToString().Equals(idRowTotal.ToString()))
                return;

            UpperAccessoriesReportDetailWindow window = new UpperAccessoriesReportDetailWindow(rowClicked);
            window.Show();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy==false)
            {
                btnRefresh.IsEnabled = false;
                btnRefresh1.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void radReject_Checked(object sender, RoutedEventArgs e)
        {
            header = "UPPER ACCESSORIES REJECT REPORT";
            eRpWhat = EReportWhat.Reject;
            if (tcMain.SelectedIndex == 0 && loaded)
                tcMain.SelectedIndex = 1;
            CreateReport();
        }

        private void radBalance_Checked(object sender, RoutedEventArgs e)
        {
            header = "UPPER ACCESSORIES BALANCE REPORT";
            eRpWhat = EReportWhat.Balance;
            if (tcMain.SelectedIndex == 0 && loaded)
                tcMain.SelectedIndex = 1;
            CreateReport();
        }
              
        private void radBalanceAndReject_Checked(object sender, RoutedEventArgs e)
        {
            header = "UPPER ACCESSORIES BALANCE + REJECT REPORT";
            eRpWhat = EReportWhat.BalanceAndReject;
            if (tcMain.SelectedIndex == 0 && loaded)
                tcMain.SelectedIndex = 1;
            CreateReport();
        }
        
        private void radDelivery_Checked(object sender, RoutedEventArgs e)
        {
            header = "UPPER ACCESSORIES DELIVERY REPORT";
            eRpWhat = EReportWhat.Delivery;
            if (tcMain.SelectedIndex == 0 && loaded)
                tcMain.SelectedIndex = 1;
            CreateReport();
        }

        enum EReportWhat
        {
            Delivery, Reject, Balance, BalanceAndReject
        }
    }
}
