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

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperAccessoriesReportWindow.xaml
    /// </summary>
    public partial class UpperAccessoriesReportWindow : Window
    {
        BackgroundWorker bwLoad;
        List<ReportUpperAccessoriesSummaryModel> summaryReportList;
        List<SupplierModel> supplierList;
        int idRowTotal = -9999;
        int supIdDefault = -999;
        private bool loaded = false;
        
        public UpperAccessoriesReportWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            summaryReportList = new List<ReportUpperAccessoriesSummaryModel>();
            supplierList = new List<SupplierModel>();

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
            cboSupplier.ItemsSource = supplierFilterList;
            cboSupplier.SelectedItem = supplierFilterList.FirstOrDefault();

            ReloadDatagrid(summaryReportList);
            this.Cursor = null;
            btnRefresh.IsEnabled = true;
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
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
    }
}
