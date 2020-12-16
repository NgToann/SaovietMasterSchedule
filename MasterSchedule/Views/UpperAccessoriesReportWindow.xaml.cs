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
                SupplierId = -999
            });

            supplierFilterList.AddRange(supplierList.Where(w => supplierHasDeliveryList.Contains(w.SupplierId)).ToList());
            cboSupplier.ItemsSource = supplierFilterList;
            cboSupplier.SelectedItem = supplierFilterList.FirstOrDefault();

            ReloadDatagrid(summaryReportList);
            this.Cursor = null;
            loaded = true;
        }

        private void ReloadDatagrid(List<ReportUpperAccessoriesSummaryModel> reloadList)
        {
            reloadList.RemoveAll(r => r.SupplierId.Equals(-9999));
            var totalModel = new ReportUpperAccessoriesSummaryModel
            {
                AccessoriesName = "TOTAL",
                QuantityCheckOK = reloadList.Sum(s => s.QuantityCheckOK),
                QuantityReject = reloadList.Sum(s => s.QuantityReject),
                SupplierId = -9999
            };
            if (reloadList.Count() > 1)
                reloadList.Add(totalModel);
            dgSummaryReport.ItemsSource = reloadList;
            dgSummaryReport.Items.Refresh();
        }

        private void cboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!loaded)
                return;

            var cboSelected = sender as ComboBox;
            var supplierSelected = cboSelected.SelectedItem as SupplierModel;
            var refreshList = new List<ReportUpperAccessoriesSummaryModel>();
            if (supplierSelected.SupplierId.Equals(-999))
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

            if (supplierSelected.SupplierId.Equals(-999))
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

    }
}
