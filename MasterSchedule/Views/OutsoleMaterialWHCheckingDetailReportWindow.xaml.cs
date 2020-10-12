using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using MasterSchedule.Models;
using Microsoft.Reporting.WinForms;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleMaterialWHCheckingDetailReportWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialWHCheckingDetailReportWindow : Window
    {
        BackgroundWorker bwSearch;
        List<ReportOSMWHCheckingModel> reportOSMWHCheckingList;
        List<OutsoleMaterialModel> outsoleMaterialList;

        private POStatus poStatus = POStatus.All;
        public OutsoleMaterialWHCheckingDetailReportWindow()
        {
            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += BwSearch_DoWork;
            bwSearch.RunWorkerCompleted += BwSearch_RunWorkerCompleted;

            reportOSMWHCheckingList = new List<ReportOSMWHCheckingModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpFrom.SelectedDate = DateTime.Now;
            dpTo.SelectedDate   = DateTime.Now;
        }
        private void BwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            var searchFrom = DateTime.Now;
            var searchTo = DateTime.Now;

            Dispatcher.Invoke(new Action(() => {
                searchFrom = dpFrom.SelectedDate.Value.Date;
                searchTo = dpTo.SelectedDate.Value.Date;
            }));
                
            reportOSMWHCheckingList = ReportController.GetOSMWHCheckingFromTo(searchFrom, searchTo);
            var productNoList = reportOSMWHCheckingList.Select(s => s.ProductNo).Distinct().ToList();
            if (productNoList.Count() > 0)
                productNoList = productNoList.OrderBy(o => o).ToList();
            // Outole Material
            foreach (var po in productNoList)
            {
                outsoleMaterialList.AddRange(OutsoleMaterialController.Select(po));
            }

            DataTable dt = new OSMWHCheckingDetailDataset().Tables["OSMWHCheckingDetailTable"];

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

                        var totalQtyOrder   = reportOSMWHCheckingListByPO.FirstOrDefault().Quantity;
                        var totalChecked    = reportOSMWHCheckingListByPO.Sum(s => s.QuantityDelivery + s.Reject);

                        if (poStatus == POStatus.POFinished && totalQtyOrder != totalChecked)
                            continue;
                        if (poStatus == POStatus.PONotYetFinish && totalQtyOrder == totalChecked)
                            continue;

                        int index = 1;
                        foreach (var errorId in errorIdListByPO)
                        {
                            DataRow dr = dt.NewRow();
                            var regex = new Regex("[a-z]|[A-Z]");

                            dr["SupplierAndOutsoleCode"]    = String.Format("{0}@{1}", xReport.Name, xReport.OutsoleCode);
                            dr["SupplierName"]              = String.Format("To: {0}", xReport.Name);
                            dr["OutsoleCode"]               = xReport.OutsoleCode;
                            dr["ProductNo"]                 = xReport.ProductNo;
                            dr["WorkerId"]                  = String.Join(", ", reportOSMWHCheckingListByPO.Select(s => s.WorkerId).Distinct().ToList());
                            dr["ArticleNo"]                 = xReport.ArticleNo;
                            dr["EFD"]                       = String.Format("{0: MM/dd}", xReport.EFD);
                            
                            var sizeNoListContainsError = reportOSMWHCheckingListByPO.Where(w => w.ErrorId > 0).Select(s => s.SizeNo).Distinct().ToList();
                            if (sizeNoListContainsError.Count() > 0)
                                sizeNoListContainsError = sizeNoListContainsError.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

                            List<String> detailViewList = new List<string>();
                            foreach (var sizeNo in sizeNoListContainsError)
                            {
                                var totalRejectBySize = reportOSMWHCheckingListByPO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Reject);
                                detailViewList.Add(String.Format("#{0} = {1}", sizeNo, totalRejectBySize));
                            }
                            if (detailViewList.Count() > 0)
                                dr["Detail"] = String.Join("; ", detailViewList);
                            

                            string fromTo = "";
                            if (searchFrom != searchTo)
                                fromTo = String.Format("From: {0: MM/dd/yyyy} to {1: MM/dd/yyyy}", searchFrom, searchTo);
                            else
                                fromTo = String.Format("From: {0: MM/dd/yyyy}", searchFrom);
                            dr["FromTo"] = fromTo;

                            // Delivery From MasterFile
                            int qtyDelivery = outsoleMaterialList.Where(w => w.ProductNo == productNo && w.OutsoleSupplierId == suppId).Sum(s => s.Quantity);
                            if (qtyDelivery > 0)
                                dr["QuantityDelivery"] = qtyDelivery;

                            // Qty Check = QtyDelivery at Outsole WH
                            int qtyChecked = reportOSMWHCheckingListByPO.Where(w => w.ErrorId == 0).Sum(s => s.QuantityDelivery);
                            if (qtyChecked > 0)
                                dr["QuantityCheck"] = qtyChecked;


                            var reportByErrorIdList = reportOSMWHCheckingListByPO.Where(w => w.ErrorId == errorId && w.ErrorId > 0).ToList();
                            var reportByErrorIdFirst = reportByErrorIdList.FirstOrDefault();

                            List<string> errorAndQtyViewList = new List<string>();
                            if (errorId > 0)
                            {
                                dr["ErrorId"]   = errorId;
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
                        
                            dt.Rows.Add(dr);
                            index++;
                        }
                    }
                }
            }

            e.Result = dt;
        }

        private void BwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var searchFrom  = dpFrom.SelectedDate.Value.Date;
            var searchTo    = dpTo.SelectedDate.Value.Date;

            var dt = e.Result as DataTable;
            //ReportParameter rp = new ReportParameter("FromTo", fromTo);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OSMWHCheckingDetail";
            rds.Value = dt;

            reportViewer.LocalReport.ReportPath = @"Reports\OSMWHCheckingDetailReport.rdlc";
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            btnSearch.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (bwSearch.IsBusy == false)
            {
                btnSearch.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                outsoleMaterialList.Clear();
                bwSearch.RunWorkerAsync();
            }
        }

        private enum POStatus
        {
            All,
            POFinished,
            PONotYetFinish
        }

        private void radPOAll_Checked(object sender, RoutedEventArgs e)
        {
            poStatus = POStatus.All;
        }

        private void radPOFinished_Checked(object sender, RoutedEventArgs e)
        {
            poStatus = POStatus.POFinished;
        }

        private void radPONotYetFinished_Checked(object sender, RoutedEventArgs e)
        {
            poStatus = POStatus.PONotYetFinish;
        }
    }
}
