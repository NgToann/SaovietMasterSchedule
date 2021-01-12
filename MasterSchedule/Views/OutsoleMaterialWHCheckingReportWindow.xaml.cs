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
    /// Interaction logic for OutsoleMaterialWHCheckingReportWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialWHCheckingReportWindow : Window
    {
        BackgroundWorker bwSearch;
        BackgroundWorker bwLoad;
        BackgroundWorker bwLoadWHInventory;
        BackgroundWorker bwViewDetail;
        //List<ReportOSMWHCheckingModel> reportOSMWHCheckingList;
        List<ReportOSMWHCheckingModel> reportViewList;
        List<OutsoleSuppliersModel> supplierList;

        private SearchWhat searchMode   = SearchWhat.ByPO;
        private POStatus poStatus       = POStatus.POFinished;
        OutsoleSuppliersModel supplierClicked;
        DateTime searchFrom, searchTo;
        DateTime dtDefault = new DateTime(2000, 01, 01);

        List<ReportOSMWHCheckingModel> reportNewList;
        List<ReportOSMaterialCheckWHInventoryModel> osMaterialCheckWHInventoryList;
        List<ReportOSMaterialCheckDetailModel> osMatCheckDetailList;
        List<SizeRunModel> sizeRunList;
        List<OutsoleMaterialModel> OSMaterialList;

        List<OutsoleMaterialModel> OSMaterialFilter;
        List<ReportOSMaterialCheckDetailModel> OSMatCheckFilter;

        Button btnViewDetailClicked;
        public OutsoleMaterialWHCheckingReportWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += BwSearch_DoWork;
            bwSearch.RunWorkerCompleted += BwSearch_RunWorkerCompleted;

            bwLoadWHInventory = new BackgroundWorker();
            bwLoadWHInventory.DoWork += BwLoadWHInventory_DoWork; 
            bwLoadWHInventory.RunWorkerCompleted += BwLoadWHInventory_RunWorkerCompleted;

            bwViewDetail = new BackgroundWorker();
            bwViewDetail.DoWork += BwViewDetail_DoWork;
            bwViewDetail.RunWorkerCompleted += BwViewDetail_RunWorkerCompleted;

            //reportOSMWHCheckingList = new List<ReportOSMWHCheckingModel>();
            reportViewList          = new List<ReportOSMWHCheckingModel>();
            supplierList            = new List<OutsoleSuppliersModel>();
            supplierClicked         = new OutsoleSuppliersModel();

            reportNewList           = new List<ReportOSMWHCheckingModel>();
            osMaterialCheckWHInventoryList  = new List<ReportOSMaterialCheckWHInventoryModel>();
            osMatCheckDetailList            = new List<ReportOSMaterialCheckDetailModel>();
            sizeRunList     = new List<SizeRunModel>();
            OSMaterialList  = new List<OutsoleMaterialModel>();

            OSMaterialFilter = new List<OutsoleMaterialModel>();
            OSMatCheckFilter = new List<ReportOSMaterialCheckDetailModel>();

            btnViewDetailClicked = new Button();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                dpFrom.SelectedDate = DateTime.Now;
                dpTo.SelectedDate = DateTime.Now;
                searchFrom = dpFrom.SelectedDate.Value;
                searchTo = dpTo.SelectedDate.Value;
                bwLoad.RunWorkerAsync();
            }

            if (bwLoadWHInventory.IsBusy == false)
            {
                btnRefreshWHInventory.IsEnabled = false;
                bwLoadWHInventory.RunWorkerAsync();
            }
        }
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            supplierList.Add(new OutsoleSuppliersModel { Name = "All", OutsoleSupplierId = -999 });
            var supplierFromSourceList = OutsoleSuppliersController.Select();
            if (supplierFromSourceList.Count() > 0)
                supplierFromSourceList = supplierFromSourceList.OrderBy(o => o.Name).ToList();
            supplierList.AddRange(supplierFromSourceList);

            osMaterialCheckWHInventoryList = ReportController.SelectOSMaterialWHInventory();
        }
        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cbSuppliers.ItemsSource = supplierList;
            cbSuppliers.SelectedItem = supplierList.FirstOrDefault();
            this.Cursor = null;
        }
     
        private void BwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            string poSearch = "";
            Dispatcher.Invoke(new Action(() => {
                poSearch = txtPoSearch.Text.Trim().ToString();
                searchFrom = dpFrom.SelectedDate.Value.Date;
                searchTo = dpTo.SelectedDate.Value.Date;
            }));

            if (searchMode == SearchWhat.ByPO)
            {
                reportNewList = ReportController.SelectOSMWHCheckingByPO(poSearch);
                //reportOSMWHCheckingList = ReportController.GetOSMWHCheckingByPO(poSearch);
            }
            if (searchMode == SearchWhat.ByDateTime)
            {
                //reportOSMWHCheckingList = ReportController.GetOSMWHCheckingFromTo(searchFrom, searchTo);
                reportNewList = ReportController.SelectOSWHMCheckingFromTo(searchFrom, searchTo);
            }

            Dispatcher.Invoke(new Action(() => {
                //reportViewList = reportOSMWHCheckingList.ToList();
                reportViewList = reportNewList.ToList();
                if (supplierClicked.OutsoleSupplierId != -999)
                {
                    reportViewList = reportNewList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    CreateData(reportViewList, searchFrom, searchTo);
                }));

                // Limit Supplier combobox
                var supplierIdList = reportNewList.Select(s => s.OutsoleSupplierId).Distinct().ToList();
                var supplierDisplayList = new List<OutsoleSuppliersModel>();
                supplierDisplayList.Add(new OutsoleSuppliersModel { Name = "All", OutsoleSupplierId = -999 });
                supplierDisplayList.AddRange(supplierList.Where(w => supplierIdList.Contains(w.OutsoleSupplierId)).ToList());
                cbSuppliers.ItemsSource     = supplierDisplayList;
                cbSuppliers.SelectedItem    = supplierDisplayList.FirstOrDefault();
            }));
        }

        private void CreateData(List<ReportOSMWHCheckingModel> reportOSMWHCheckingList, DateTime searchFrom, DateTime searchTo)
        {
            var productNoList = reportOSMWHCheckingList.Select(s => s.ProductNo).Distinct().ToList();

            #region Summary Report

            DataTable dt = new OSMWHCheckingDataSet().Tables["OSMWHCheckingTable"];
            foreach (var po in productNoList)
            {
                var supplierPerPOList = reportOSMWHCheckingList.Where(w => w.ProductNo == po).Select(s => s.OutsoleSupplierId).Distinct().ToList();
                foreach (var suppID in supplierPerPOList)
                {
                    var reportByPOBySupplierList = reportOSMWHCheckingList.Where(w => w.ProductNo == po && w.OutsoleSupplierId == suppID).ToList();
                    var reportByPOSuppFirst      = reportByPOBySupplierList.FirstOrDefault();

                    var errorIDListByPO     = reportByPOBySupplierList.Select(s => s.ErrorId).Distinct().ToList();
                    
                    var totalQtyOK          = reportByPOBySupplierList.Sum(s => s.Quantity);
                    var totalReturnReject   = reportByPOBySupplierList.Sum(s => s.ReturnReject);
                    var totalReject         = reportByPOBySupplierList.Sum(s => s.Reject);

                    if (poStatus == POStatus.POFinished && reportByPOSuppFirst.POStatus.Equals("Not"))
                        continue;
                    if (poStatus == POStatus.PONotYetFinish && reportByPOSuppFirst.POStatus.Equals("Finished"))
                        continue;

                    foreach (var errorId in errorIDListByPO)
                    {
                        DataRow dr = dt.NewRow();

                        dr["POSupplier"] = String.Format("{0}@{1}", po, reportByPOSuppFirst.Name);
                        dr["ProductNo"] = po;
                        dr["SupplierName"] = reportByPOSuppFirst.Name;
                        dr["OutsoleCode"] = reportByPOSuppFirst.OutsoleCode;
                        dr["SupplierETD"] = String.Format("{0:yyyy/MM/dd}", reportByPOSuppFirst.SupplierETD);
                        dr["DeliveryTimes"] = reportByPOSuppFirst.DeliveryTimes;

                        dr["FinishedDate"] = "";
                        if (reportByPOSuppFirst.FinishedDate != dtDefault)
                            dr["FinishedDate"] = String.Format("{0:yyyy/MM/dd}", reportByPOSuppFirst.FinishedDate);

                        // Order Qty
                        dr["Quantity"] = reportByPOSuppFirst.QuantityOrder;

                        // Quantity CheckedOK
                        if (totalQtyOK > 0)
                            dr["DeliveryQuantity"] = totalQtyOK;

                        // Return Reject Quantity
                        if (totalReturnReject > 0)
                            dr["ReturnQuantity"] = totalReturnReject;

                        // Has error
                        if (errorId > 0)
                        {
                            dr["ErrorId"] = errorId;
                            dr["ErrorName"] = String.Format("{0}\n{1}", reportByPOBySupplierList.FirstOrDefault(f => f.ErrorId == errorId).ErrorName, reportByPOBySupplierList.FirstOrDefault(f => f.ErrorId == errorId).ErrorVietNamese);
                            dr["ErrorQuantity"] = reportByPOBySupplierList.Where(w => w.ErrorId == errorId).Sum(s => s.Reject);
                        }

                        // Total Reject
                        if (totalReject > 0)
                            dr["TotalReject"] = totalReject;

                        dt.Rows.Add(dr);
                    }
                }
            }

            #endregion

            #region Reviser Incentives
            DataTable dtReviser = new OSMWHReviserIncentiveDataSet().Tables["ReviserIncentiveTable"];

            var workerIdList = reportOSMWHCheckingList.Select(s => s.WorkerId).Distinct().ToList();

            foreach (var workerId in workerIdList)
            {
                var reportOSMWHByWorkerId = reportOSMWHCheckingList.Where(w => w.WorkerId == workerId).ToList();

                var supplierIdByWorkerList = reportOSMWHByWorkerId.Select(s => s.OutsoleSupplierId).Distinct().ToList();
                if (supplierIdByWorkerList.Count() > 0)
                    supplierIdByWorkerList = supplierIdByWorkerList.OrderBy(o => o).ToList();
                foreach (var supplierId in supplierIdByWorkerList)
                {
                    var reportOSWHByWorkerBySupplier = reportOSMWHByWorkerId.Where(w => w.OutsoleSupplierId == supplierId).ToList();
                    var outsoleCodeList = reportOSWHByWorkerBySupplier.Select(s => s.OutsoleCode).Distinct().ToList();
                    foreach (var outsoleCode in outsoleCodeList)
                    {
                        var reportOSWHByWorkerBySupplierByOSCode = reportOSWHByWorkerBySupplier.Where(w => w.OutsoleCode == outsoleCode).ToList();
                        var productNoListByOSCode = reportOSWHByWorkerBySupplierByOSCode.Select(s => s.ProductNo).Distinct().ToList();

                        int totalCheckedByOSCode = 0;
                        int totalRejectedByOSCode = 0;
                        int totalReturnRejectByOSCode = 0;

                        var poNeedRemoveList = new List<String>();
                        double totalInspectionHrs = 0;
                        foreach (var po in productNoListByOSCode)
                        {
                            var reportByPO      = reportOSWHByWorkerBySupplierByOSCode.Where(w => w.ProductNo == po).ToList();
                            var reportFirstByPO = reportByPO.FirstOrDefault();

                            var totalCheckOK        = reportByPO.Sum(s => s.Quantity);
                            var totalReject         = reportByPO.Sum(s => s.Reject);
                            var totalReturnReject   = reportByPO.Sum(s => s.ReturnReject);

                            if (poStatus == POStatus.POFinished && reportFirstByPO.POStatus.Equals("Not"))
                            {
                                poNeedRemoveList.Add(po);
                                continue;
                            }
                            if (poStatus == POStatus.PONotYetFinish  && reportFirstByPO.POStatus.Equals("Finished"))
                            {
                                poNeedRemoveList.Add(po);
                                continue;
                            }

                            totalCheckedByOSCode        += totalCheckOK;
                            totalRejectedByOSCode       += totalReject;
                            totalReturnRejectByOSCode   += totalReturnReject;
                            totalInspectionHrs          += reportFirstByPO.TotalHours;
                        }

                        int totalQtyOK = totalCheckedByOSCode + totalReturnRejectByOSCode;
                        
                        if (totalQtyOK > 0)
                        {
                            var totalQtyOKByWorker = reportOSMWHByWorkerId.Where(w => poNeedRemoveList.Contains(w.ProductNo) == false).Sum(s => s.Quantity + s.ReturnReject); // - s.Reject
                            DataRow dr = dtReviser.NewRow();

                            dr["WorkerId"]              = workerId;
                            dr["WorkerTotalCheck"]      = totalQtyOKByWorker;
                            dr["SupplierName"]          = reportOSWHByWorkerBySupplierByOSCode.FirstOrDefault().Name;
                            dr["SupplierOperation"]     = reportOSWHByWorkerBySupplierByOSCode.FirstOrDefault().SupplierOperation;
                            if (totalInspectionHrs > 0)
                                dr["InspectionTime"] = String.Format("{0}", Math.Round(totalInspectionHrs / 3600, 3, MidpointRounding.AwayFromZero));
                            dr["OSCode"]                = outsoleCode;
                            dr["QuantityChecked"]       = totalCheckedByOSCode > 0 ? totalCheckedByOSCode.ToString() : "";
                            dr["QuantityRejected"]      = totalRejectedByOSCode > 0 ? totalRejectedByOSCode.ToString() : "";
                            dr["QuantityReturnRejected"] = totalReturnRejectByOSCode > 0 ? totalReturnRejectByOSCode.ToString() : "";

                            dr["Total"]                 = totalQtyOK > 0 ? totalQtyOK.ToString() : "";

                            dtReviser.Rows.Add(dr);
                        }
                    }
                }
            }
            #endregion

            #region RejectDetailReport

            DataTable dtRejectDetail = new OSMWHCheckingDetailDataset().Tables["OSMWHCheckingDetailTable"];

            var supplierIdList = reportOSMWHCheckingList.Select(s => s.OutsoleSupplierId).Distinct().ToList();
            foreach (var suppId in supplierIdList)
            {
                var reportOSMWHCheckingListBySuppId = reportOSMWHCheckingList.Where(w => w.OutsoleSupplierId == suppId).ToList();
                var osCodeListBySuppId = reportOSMWHCheckingListBySuppId.Select(s => s.OutsoleCode).Distinct().ToList();

                foreach (var osCode in osCodeListBySuppId)
                {
                    var reportOSMWHCheckingListBySuppIdByOSCode = reportOSMWHCheckingListBySuppId.Where(w => w.OutsoleCode == osCode).ToList();
                    var poListBySuppByOSCode = reportOSMWHCheckingListBySuppIdByOSCode.Select(s => s.ProductNo).Distinct().ToList();                    
                    foreach (var productNo in poListBySuppByOSCode)
                    {
                        var reportOSMWHCheckingListByPO = reportOSMWHCheckingListBySuppIdByOSCode.Where(w => w.ProductNo == productNo).ToList();
                        var errorIdListByPO = reportOSMWHCheckingListByPO.Select(s => s.ErrorId).Distinct().ToList();
                        var xReport = reportOSMWHCheckingListByPO.FirstOrDefault();
                        
                        var totalQtyOK = reportOSMWHCheckingListByPO.Sum(s => s.Quantity);
                        var totalReturnReject = reportOSMWHCheckingListByPO.Sum(s => s.ReturnReject);

                        if (poStatus == POStatus.POFinished && xReport.POStatus.Equals("Not"))
                            continue;
                        if (poStatus == POStatus.PONotYetFinish && xReport.POStatus.Equals("Finished"))
                            continue;

                        int index = 1;
                        foreach (var errorId in errorIdListByPO)
                        {
                            DataRow dr = dtRejectDetail.NewRow();
                            var regex = new Regex("[a-z]|[A-Z]");

                            dr["SupplierAndOutsoleCode"] = String.Format("{0}@{1}", xReport.Name, xReport.OutsoleCode);
                            dr["SupplierName"] = String.Format("To: {0}", xReport.Name);
                            dr["OutsoleCode"] = xReport.OutsoleCode;
                            dr["ProductNo"] = xReport.ProductNo;
                            dr["WorkerId"] = String.Join(", ", reportOSMWHCheckingListByPO.Select(s => s.WorkerId).Distinct().ToList());
                            dr["ArticleNo"] = xReport.ArticleNo;
                            dr["EFD"] = String.Format("{0: MM/dd}", xReport.EFD);

                            var sizeNoListContainsError = reportOSMWHCheckingListByPO.Where(w => w.ErrorId > 0).Select(s => s.SizeNo).Distinct().ToList();
                            if (sizeNoListContainsError.Count() > 0)
                                sizeNoListContainsError = sizeNoListContainsError.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

                            List<String> detailViewList = new List<string>();
                            foreach (var sizeNo in sizeNoListContainsError)
                            {
                                var totalRejectBySize = reportOSMWHCheckingListByPO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Reject);
                                var totalReturnRejectBySize = reportOSMWHCheckingListByPO.Where(w => w.SizeNo == sizeNo).Sum(s => s.ReturnReject);
                                if (totalRejectBySize - totalReturnRejectBySize > 0)
                                    detailViewList.Add(String.Format("#{0} = {1}", sizeNo, totalRejectBySize - totalReturnRejectBySize));
                            }
                            if (detailViewList.Count() > 0)
                                dr["Detail"] = String.Join("; ", detailViewList);

                            //if (!String.IsNullOrEmpty(xReport.RejectCurrentDetail))
                            //    dr["Detail"] = xReport.RejectCurrentDetail;

                            string fromTo = "";
                            if (searchFrom != searchTo)
                                fromTo = String.Format("From: {0: MM/dd/yyyy} to {1: MM/dd/yyyy}", searchFrom, searchTo);
                            else
                                fromTo = String.Format("From: {0: MM/dd/yyyy}", searchFrom);
                            dr["FromTo"] = fromTo;

                            // Delivery
                            dr["QuantityDelivery"] = xReport.QuantityDelivery > 0 ? xReport.QuantityDelivery.ToString() : "";

                            // Qty Check = QtyDelivery at Outsole WH
                            var qtyCheckOK = reportOSMWHCheckingListByPO.Sum(s => s.Quantity);
                            if (qtyCheckOK > 0)
                                dr["QuantityCheck"] = qtyCheckOK;

                            // Quantity Return Reject
                            var returnReject = reportOSMWHCheckingListByPO.Sum(s => s.ReturnReject);
                            if (returnReject > 0)
                                dr["QuantityReturn"] = returnReject;

                            var reportByErrorIdList = reportOSMWHCheckingListByPO.Where(w => w.ErrorId == errorId && w.ErrorId > 0).ToList();
                            var reportByErrorIdFirst = reportByErrorIdList.FirstOrDefault();

                            List<string> errorAndQtyViewList = new List<string>();
                            if (errorId > 0)
                            {
                                dr["ErrorId"] = errorId;
                                dr["ErrorName"] = String.Format("{0}\n{1}", reportByErrorIdFirst.ErrorName, reportByErrorIdFirst.ErrorVietNamese);

                                var sizeNoListHasError = reportByErrorIdList.Select(s => s.SizeNo).Distinct().ToList();
                                if (sizeNoListHasError.Count() > 0)
                                    sizeNoListHasError = sizeNoListHasError.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();
                                foreach (var sizeNo in sizeNoListHasError)
                                {
                                    int errorQtyBySizeNo = reportByErrorIdList.Where(w => w.SizeNo == sizeNo).Sum(s => s.Reject);
                                    if (errorQtyBySizeNo > 0)
                                        errorAndQtyViewList.Add(String.Format("#{0} = {1}", sizeNo, errorQtyBySizeNo));
                                }
                                if (errorAndQtyViewList.Count() > 0)
                                    dr["ErrorView"] = String.Join("; ", errorAndQtyViewList);
                            }

                            dtRejectDetail.Rows.Add(dr);
                            index++;
                        }
                    }
                }
            }
            #endregion

            //results = new object[] { dt, dtReviser, dtRejectDetail };
            //return results;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OSWHCheckingDetail";
            rds.Value = dt;

            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialWHCheckingReport.rdlc";
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            ReportDataSource rdsReviser = new ReportDataSource();
            rdsReviser.Name = "OSMWHReviserIncentiveDetail";
            rdsReviser.Value = dtReviser;

            reportViewerReviserIncentive.LocalReport.ReportPath = @"Reports\OSWHReviserIncentiveReport.rdlc";
            reportViewerReviserIncentive.LocalReport.DataSources.Clear();
            reportViewerReviserIncentive.LocalReport.DataSources.Add(rdsReviser);
            reportViewerReviserIncentive.RefreshReport();


            ReportDataSource rdsRejectDetail = new ReportDataSource();
            rdsRejectDetail.Name = "OSMWHCheckingDetail";
            rdsRejectDetail.Value = dtRejectDetail;

            reportViewerRejectDetail.LocalReport.ReportPath = @"Reports\OSMWHCheckingDetailReport.rdlc";
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewerRejectDetail.LocalReport.DataSources.Clear();
            reportViewerRejectDetail.LocalReport.DataSources.Add(rdsRejectDetail);
            reportViewerRejectDetail.RefreshReport();
        }

        private void BwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSearch.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (bwSearch.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnSearch.IsEnabled = false;
                bwSearch.RunWorkerAsync();
            }
        }

        private enum SearchWhat
        {
            ByPO,
            ByDateTime,
        }
        private enum POStatus
        {
            All,
            POFinished,
            PONotYetFinish
        }

        private void radPO_Checked(object sender, RoutedEventArgs e)
        {
            searchMode = SearchWhat.ByPO;
        }

        private void radDateTime_Checked(object sender, RoutedEventArgs e)
        {
            searchMode = SearchWhat.ByDateTime;
        }

        private void radPOFinished_Checked(object sender, RoutedEventArgs e)
        {
            poStatus = POStatus.POFinished;
        }

        private void radPONotYetFinished_Checked(object sender, RoutedEventArgs e)
        {
            poStatus = POStatus.PONotYetFinish;
        }

        private void radPOAll_Checked(object sender, RoutedEventArgs e)
        {
            poStatus = POStatus.All;
        }

        private void cbSuppliers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            supplierClicked = cbSuppliers.SelectedItem as OutsoleSuppliersModel;
            reportViewList = reportNewList.ToList();
            if (supplierClicked != null && supplierClicked.OutsoleSupplierId != -999)
                reportViewList = reportNewList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            CreateData(reportViewList, searchFrom, searchTo);
        }

        private void btnRefreshWHInventory_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoadWHInventory.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnRefreshWHInventory.IsEnabled = false;
                bwLoadWHInventory.RunWorkerAsync();
            }
        }
        private void BwLoadWHInventory_DoWork(object sender, DoWorkEventArgs e)
        {
            // Excute Query
            try {
                osMaterialCheckWHInventoryList = ReportController.SelectOSMaterialWHInventory();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void dgInventoryByOutsole_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dgDetail_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void BwLoadWHInventory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnRefreshWHInventory.IsEnabled = true;
            dgInventoryByOutsole.ItemsSource = osMaterialCheckWHInventoryList;
            if (osMaterialCheckWHInventoryList.Count() > 0)
                dgInventoryByOutsole.SelectedItem = osMaterialCheckWHInventoryList.FirstOrDefault();
            dgInventoryByOutsole.Items.Refresh();
            this.Cursor = null;
        }

        private void btnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            btnViewDetailClicked = sender as Button;
            var rowClicked = dgInventoryByOutsole.CurrentItem as ReportOSMaterialCheckWHInventoryModel;
            if (rowClicked == null)
                return;
            if (bwViewDetail.IsBusy == false)
            {
                firstLoadDetail = false;
                tblTitleReportDetail.Text = "";
                cboSupplierInventory.ItemsSource = null;
                txtPOFilter.Text = "";

                this.Cursor = Cursors.Wait;
                btnViewDetailClicked.IsEnabled = false;
                bwViewDetail.RunWorkerAsync(rowClicked);
            }
        }
        
        bool firstLoadDetail = false;
        private void BwViewDetail_DoWork(object sender, DoWorkEventArgs e)
        {
            var rowClicked = e.Argument as ReportOSMaterialCheckWHInventoryModel;
            osMatCheckDetailList    = ReportController.SelectOSMaterialCheckByOSCode(rowClicked.OutsoleCode);
            sizeRunList             = SizeRunController.SelectPerOutsoleCode(rowClicked.OutsoleCode);
            OSMaterialList          = OutsoleMaterialController.SelectByOSCode(rowClicked.OutsoleCode);

            // CREATE COLUMNS
            Dispatcher.Invoke(new Action(() =>
            {
                tblTitleReportDetail.Text = String.Format("List of detail O/S Code: {0}", rowClicked.OutsoleCode);
                LoadDataInventory(OSMaterialList, osMatCheckDetailList);
            }));
        }
        private void BwViewDetail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Binding Combobox Supplier
            var supplierInventoryList = new List<OutsoleSuppliersModel>();
            supplierInventoryList.Add(new OutsoleSuppliersModel { Name = "-- Select a Supplier --", OutsoleSupplierId = -999 });
            supplierInventoryList.AddRange(supplierList.Where(w => OSMaterialList.Select(s => s.OutsoleSupplierId).Distinct().ToList().Contains(w.OutsoleSupplierId)).ToList());
            cboSupplierInventory.ItemsSource = supplierInventoryList;
            cboSupplierInventory.SelectedItem = supplierInventoryList.FirstOrDefault();

            btnViewDetailClicked.IsEnabled = true;
            this.Cursor = null;

            firstLoadDetail = true;
        }
        private void LoadDataInventory(List<OutsoleMaterialModel> OSMaterialList, List<ReportOSMaterialCheckDetailModel> osMatCheckDetailList)
        {
            var sizeList = OSMaterialList.Select(s => s.SizeNo).Distinct().ToList();
            var regex = new Regex("[a-z]|[A-Z]");
            var sizeNoList = sizeList.Select(s => s).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

            DataTable dt = new DataTable();
            dgDetail.Columns.Clear();

            //Column ProductNo
            dt.Columns.Add("ProductNo", typeof(String));
            DataGridTemplateColumn colPO = new DataGridTemplateColumn();
            colPO.Header = String.Format("ProductNo");
            DataTemplate templatePO = new DataTemplate();
            FrameworkElementFactory tblPO = new FrameworkElementFactory(typeof(TextBlock));
            templatePO.VisualTree = tblPO;
            tblPO.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ProductNo")));
            tblPO.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblPO.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colPO.CellTemplate = templatePO;
            colPO.ClipboardContentBinding = new Binding(String.Format("ProductNo"));
            dgDetail.Columns.Add(colPO);

            dt.Columns.Add("Name", typeof(String));
            DataGridTemplateColumn colSupplierName = new DataGridTemplateColumn();
            colSupplierName.Header = String.Format("Supplier");
            DataTemplate templateSupplierName = new DataTemplate();
            FrameworkElementFactory tblSupplierName = new FrameworkElementFactory(typeof(TextBlock));
            templateSupplierName.VisualTree = tblSupplierName;
            tblSupplierName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Name")));
            tblSupplierName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblSupplierName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colSupplierName.CellTemplate = templateSupplierName;
            colSupplierName.ClipboardContentBinding = new Binding(String.Format("Name"));
            dgDetail.Columns.Add(colSupplierName);

            foreach (var sizeNo in sizeNoList)
            {
                string sizeBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "@") : sizeNo;

                dt.Columns.Add(String.Format("Column{0}", sizeBinding), typeof(String));
                dt.Columns.Add(String.Format("ColumnForeground{0}", sizeBinding), typeof(SolidColorBrush));
                DataGridTemplateColumn colQuantity = new DataGridTemplateColumn();
                colQuantity.Header = String.Format(sizeNo);
                colQuantity.Width = 40;
                DataTemplate templateQuantity = new DataTemplate();
                FrameworkElementFactory tblQuantity = new FrameworkElementFactory(typeof(TextBlock));
                templateQuantity.VisualTree = tblQuantity;
                tblQuantity.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", sizeBinding)));
                tblQuantity.SetValue(TextBlock.ForegroundProperty, new Binding(String.Format("ColumnForeground{0}", sizeBinding)));
                tblQuantity.SetValue(TextBlock.PaddingProperty, new Thickness(3, 3, 3, 3));
                tblQuantity.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblQuantity.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                colQuantity.CellTemplate = templateQuantity;
                colQuantity.ClipboardContentBinding = new Binding(String.Format("Column{0}", sizeBinding));
                dgDetail.Columns.Add(colQuantity);
            }

            dt.Columns.Add("Total", typeof(String));
            DataGridTemplateColumn colTotal = new DataGridTemplateColumn();
            colTotal.Header = String.Format("Total");
            DataTemplate templateTotal = new DataTemplate();
            FrameworkElementFactory tblTotal = new FrameworkElementFactory(typeof(TextBlock));
            templateTotal.VisualTree = tblTotal;
            tblTotal.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Total")));
            tblTotal.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblTotal.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            tblTotal.SetValue(TextBlock.PaddingProperty, new Thickness(3, 3, 3, 3));
            colTotal.CellTemplate = templateTotal;
            colTotal.ClipboardContentBinding = new Binding(String.Format("Total"));
            dgDetail.Columns.Add(colTotal);

            // Binding Data
            var productNoList = OSMaterialList.Select(s => s.ProductNo).Distinct().ToList();
            if (productNoList.Count() > 0)
                productNoList = productNoList.OrderBy(o => o).ToList();
            int totaltotal = 0;
            foreach (var productNo in productNoList)
            {
                var osMatCheckByPO = osMatCheckDetailList.Where(w => w.ProductNo.Equals(productNo)).ToList();
                var sizeRunByPO = sizeRunList.Where(w => w.ProductNo.Equals(productNo)).ToList();

                var osMaterialByPO = OSMaterialList.Where(w => w.ProductNo.Equals(productNo)).ToList();
                var supplierIdList = osMaterialByPO.Select(s => s.OutsoleSupplierId).Distinct().ToList();

                // If PO not yet deliver
                if (osMaterialByPO.Count() == 0)
                    continue;

                var addPO = false;
                foreach (var supplierId in supplierIdList)
                {
                    var osMaterialBySupp = osMaterialByPO.Where(w => w.OutsoleSupplierId.Equals(supplierId)).ToList();
                    // If Supplier not yet deliver
                    if (osMaterialBySupp.Count() == 0)
                        continue;
                    var sizeNoListBySupp = osMaterialBySupp.Select(s => s.SizeNo).Distinct().ToList();
                    var osMatCheckBySupp = osMatCheckByPO.Where(w => w.OutsoleSupplierId.Equals(supplierId)).ToList();

                    int totalDelivery = osMaterialBySupp.Sum(s => s.Quantity);
                    int totalChecked = osMatCheckBySupp.Sum(s => s.Checked + s.QtyReturn);

                    if (totalDelivery == totalChecked)
                        continue;

                    DataRow dr = dt.NewRow();
                    if (addPO == false)
                    {
                        dr["ProductNo"] = productNo;
                        addPO = true;
                    }
                    var rowColor = Brushes.Black;
                    if (totalChecked == 0)
                        rowColor = Brushes.Red;

                    dr["Name"] = osMaterialBySupp.FirstOrDefault().Name;
                    int totalNotCheck = 0;
                    List<string> sizeOSAddedList = new List<string>();
                    foreach (var sizeNo in sizeNoListBySupp)
                    {
                        string sizeBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "@") : sizeNo;
                        var sizeRunByOrderSize = sizeRunByPO.FirstOrDefault(f => f.SizeNo == sizeNo);
                        var sizeOSListBySizeOrder = sizeRunByPO.Where(w => w.OutsoleSize == sizeRunByOrderSize.OutsoleSize
                                                                        && !String.IsNullOrEmpty(w.OutsoleSize)).Select(s => s.OutsoleSize).ToList();

                        int qtyDelivery = osMaterialBySupp.FirstOrDefault(f => f.SizeNo.Equals(sizeNo)).Quantity;
                        int qtyChecked = 0, qtyNotCheck = 0;
                        
                        if (sizeOSListBySizeOrder.Count() > 1)
                        {
                            if (!sizeOSAddedList.Contains(sizeOSListBySizeOrder.FirstOrDefault()))
                            {
                                foreach (var sizeOS in sizeOSListBySizeOrder.Distinct())
                                {
                                    var sizeOrderListBySizeOS = sizeRunByPO.Where(w => w.OutsoleSize.Equals(sizeOS)).Select(s => s.SizeNo).ToList();
                                    qtyDelivery = osMaterialBySupp.Where(w => sizeOrderListBySizeOS.Contains(w.SizeNo)).Sum(s => s.Quantity);

                                    var osMatCheckBySize = osMatCheckBySupp.FirstOrDefault(f => f.SizeNo.Equals(sizeOS));
                                    if (osMatCheckBySize != null)
                                        qtyChecked += osMatCheckBySize.Checked + osMatCheckBySize.QtyReturn;
                                    sizeOSAddedList.Add(sizeOSListBySizeOrder.FirstOrDefault());
                                }
                                qtyNotCheck = qtyDelivery - qtyChecked;
                            }
                        }
                        else if (sizeOSListBySizeOrder.Count() > 0)
                        {
                            qtyChecked = osMatCheckBySupp.Where(w => w.SizeNo == sizeOSListBySizeOrder.FirstOrDefault()).Sum(s => s.Checked + s.QtyReturn);
                            qtyNotCheck = qtyDelivery - qtyChecked;
                        }
                        else
                        {
                            qtyChecked = osMatCheckBySupp.Where(w => w.SizeNo == sizeRunByOrderSize.SizeNo).Sum(s => s.Checked + s.QtyReturn);
                            qtyNotCheck = qtyDelivery - qtyChecked;
                        }

                        if (qtyNotCheck > 0)
                        {
                            dr[String.Format("Column{0}", sizeBinding)] = qtyNotCheck;
                            dr[String.Format("ColumnForeground{0}", sizeBinding)] = rowColor;
                            totalNotCheck += qtyNotCheck;
                        }
                    }
                    if (totalNotCheck > 0)
                    {
                        dr["Total"] = String.Format("{0:N0}",totalNotCheck);
                        totaltotal += totalNotCheck;
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                DataRow drTotal = dt.NewRow();
                drTotal["ProductNo"] = "TOTAL";
                drTotal["Total"] = String.Format("{0:N0}", totaltotal);

                dt.Rows.Add(drTotal);
            }
            dgDetail.ItemsSource = dt.AsDataView();
            dgDetail.Items.Refresh();
        }

        private void cboSupplierInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filterWhat = txtPOFilter.Text.Trim().ToUpper().ToString();
            if (firstLoadDetail == true)
            {
                var supplierClicked = cboSupplierInventory.SelectedItem as OutsoleSuppliersModel;
                if (supplierClicked.OutsoleSupplierId != -999)
                {
                    OSMaterialFilter = OSMaterialList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                    OSMatCheckFilter = osMatCheckDetailList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                }
                else
                {
                    OSMaterialFilter = OSMaterialList.ToList();
                    OSMatCheckFilter = osMatCheckDetailList.ToList();
                }
                if (!String.IsNullOrEmpty(filterWhat))
                {
                    OSMaterialFilter = OSMaterialFilter.Where(w => w.ProductNo.Contains(filterWhat)).ToList();
                    OSMatCheckFilter = osMatCheckDetailList.Where(w => w.ProductNo.Contains(filterWhat)).ToList();
                }
                LoadDataInventory(OSMaterialFilter, OSMatCheckFilter);
            }
        }
        
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterWhat = txtPOFilter.Text.Trim().ToUpper().ToString();
            var supplierClicked = cboSupplierInventory.SelectedItem as OutsoleSuppliersModel;
            if (firstLoadDetail == true)
            {
                if (!String.IsNullOrEmpty(filterWhat))
                {
                    OSMaterialFilter = OSMaterialList.Where(w => w.ProductNo.Contains(filterWhat)).ToList();
                    OSMatCheckFilter = osMatCheckDetailList.Where(w => w.ProductNo.Contains(filterWhat)).ToList();
                }
                else
                {
                    OSMaterialFilter = OSMaterialList.ToList();
                    OSMatCheckFilter = osMatCheckDetailList.ToList();
                }

                if (supplierClicked.OutsoleSupplierId != -999)
                {
                    OSMaterialFilter = OSMaterialFilter.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                    OSMatCheckFilter = OSMatCheckFilter.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                }
                else
                {
                    OSMaterialFilter = OSMaterialFilter.ToList();
                    OSMatCheckFilter = OSMatCheckFilter.ToList();
                }
                LoadDataInventory(OSMaterialFilter, OSMatCheckFilter);
            }
        }
        
        private void txtPOFilter_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            btnSearch.IsDefault = false;
            btnFilter.IsDefault = true;
        }
        private void txtPoSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            btnFilter.IsDefault = false;
            btnSearch.IsDefault = true;
        }
    }
}
