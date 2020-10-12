using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        //List<ReportOSMWHCheckingModel> reportOSMWHCheckingList;
        List<ReportOSMWHCheckingModel> reportViewList;
        List<OutsoleSuppliersModel> supplierList;

        private SearchWhat searchMode   = SearchWhat.ByPO;
        private POStatus poStatus       = POStatus.POFinished;
        OutsoleSuppliersModel supplierClicked;
        DateTime searchFrom, searchTo;
        DateTime dtDefault = new DateTime(2000, 01, 01);

        List<ReportOSMWHCheckingModel> reportNewList;

        public OutsoleMaterialWHCheckingReportWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += BwSearch_DoWork;
            bwSearch.RunWorkerCompleted += BwSearch_RunWorkerCompleted;

            //reportOSMWHCheckingList = new List<ReportOSMWHCheckingModel>();
            reportViewList          = new List<ReportOSMWHCheckingModel>();
            supplierList            = new List<OutsoleSuppliersModel>();
            supplierClicked         = new OutsoleSuppliersModel();

            reportNewList           = new List<ReportOSMWHCheckingModel>();

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
        }
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            supplierList.Add(new OutsoleSuppliersModel { Name = "All", OutsoleSupplierId = -999 });
            var supplierFromSourceList = OutsoleSuppliersController.Select();
            if (supplierFromSourceList.Count() > 0)
                supplierFromSourceList = supplierFromSourceList.OrderBy(o => o.Name).ToList();
            supplierList.AddRange(supplierFromSourceList);
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
                                var totalRejectBySize       = reportOSMWHCheckingListByPO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Reject);
                                var totalReturnRejectBySize = reportOSMWHCheckingListByPO.Where(w => w.SizeNo == sizeNo).Sum(s => s.ReturnReject);
                                if (totalRejectBySize - totalReturnRejectBySize > 0)
                                    detailViewList.Add(String.Format("#{0} = {1}", sizeNo, totalRejectBySize - totalReturnRejectBySize));
                            }
                            if (detailViewList.Count() > 0)
                                dr["Detail"] = String.Join("; ", detailViewList);

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
    }
}
