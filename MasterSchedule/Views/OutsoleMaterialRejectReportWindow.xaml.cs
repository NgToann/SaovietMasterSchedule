using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialRejectReportWindow : Window
    {
        BackgroundWorker bwLoadData;
        BackgroundWorker bwLoadBalance;
        BackgroundWorker bwLoadAll;

        List<OutsoleMaterialModel> outsoleMaterialRejectList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OrdersModel> orderList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<SizeRunModel> sizeRunList;

        List<OutsoleMaterialBalanceReportModel> outsoleMaterialBalanceReportList;
        //List<OutsoleMaterialBalanceReportModel> outsoleMaterialRejectReportList;
        List<OutsoleMaterialBalanceReportModel> outsoleMaterialAllReportList;
        private PrivateDefineModel def;

        DataTable dtRejectAssemblyStockfit;
        string parTitle = "Reject/Lacking Report for STOCKFIT";

        public OutsoleMaterialRejectReportWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            bwLoadBalance = new BackgroundWorker();
            bwLoadBalance.WorkerSupportsCancellation = true;
            bwLoadBalance.DoWork +=new DoWorkEventHandler(bwLoadBalance_DoWork);
            bwLoadBalance.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoadBalance_RunWorkerCompleted);

            bwLoadAll = new BackgroundWorker();
            bwLoadAll.WorkerSupportsCancellation = true;
            bwLoadAll.DoWork +=new DoWorkEventHandler(bwLoadAll_DoWork);
            bwLoadAll.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoadAll_RunWorkerCompleted);

            outsoleMaterialRejectList = new List<OutsoleMaterialModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            orderList = new List<OrdersModel>();
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialBalanceReportList = new List<OutsoleMaterialBalanceReportModel>();
            //outsoleMaterialRejectReportList = new List<OutsoleMaterialBalanceReportModel>();

            outsoleMaterialAllReportList = new List<OutsoleMaterialBalanceReportModel>();

            sizeRunList = new List<SizeRunModel>();
            def = new PrivateDefineModel();

            dtRejectAssemblyStockfit = new DataTable();

            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialRejectList = OutsoleMaterialController.SelectReject();
            outsoleSupplierList = OutsoleSuppliersController.Select();
            orderList = OrdersController.SelectByOutsoleMaterialReject();
            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            sizeRunList = SizeRunController.SelectIsEnable();
            def = PrivateDefineController.GetDefine();

            DataTable dt = new OutsoleMaterialRejectDataSet().Tables["OutsoleMaterialRejectTable"];
            DataTable dt1 = new OutsoleMaterialRejectDataSet().Tables["OutsoleMaterialRejectTable"];

            var regex = new Regex(@"[a-z]|[A-Z]");
            foreach (OutsoleMaterialModel outsoleMaterialReject in outsoleMaterialRejectList)
            {
                var outsoleSupplier = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == outsoleMaterialReject.OutsoleSupplierId);
                var order = orderList.FirstOrDefault(f => f.ProductNo == outsoleMaterialReject.ProductNo);
                var outsoleRawMaterial = outsoleRawMaterialList.FirstOrDefault(f => f.ProductNo == outsoleMaterialReject.ProductNo && f.OutsoleSupplierId == outsoleMaterialReject.OutsoleSupplierId);
                DataRow dr = dt.NewRow();
                DataRow dr1 = dt1.NewRow();

                dr["ProductNo"]     = outsoleMaterialReject.ProductNo;
                dr1["ProductNo"]    = outsoleMaterialReject.ProductNo;
                var productNoList   = new List<String>();
                if (order != null)
                {
                    dr["OutsoleCode"]   = order.OutsoleCode;
                    dr1["OutsoleCode"]  = order.OutsoleCode;
                    dr["ETD"]           = order.ETD;
                    dr1["ETD"]          = order.ETD;
                    dr["ArticleNo"]     = order.ArticleNo;
                    dr1["ArticleNo"]    = order.ArticleNo;
                    productNoList = orderList.Where(w => w.OutsoleCode == order.OutsoleCode).Select(s => s.ProductNo).ToList();
                }
                if (outsoleRawMaterial != null)
                {
                    dr["SupplierETD"]   = outsoleRawMaterial.ETD;
                    dr1["SupplierETD"]  = outsoleRawMaterial.ETD;
                }
                if (outsoleSupplier != null)
                {
                    dr["OutsoleSupplier"]   = outsoleSupplier.Name;
                    dr1["OutsoleSupplier"]  = outsoleSupplier.Name;
                }

                string sizeNoOutsole = outsoleMaterialReject.SizeNo;
                var sizeNo_OutsoleCode = sizeRunList.Where(w => productNoList.Contains(w.ProductNo) && w.SizeNo == sizeNoOutsole).FirstOrDefault();
                if (sizeNo_OutsoleCode != null)
                {
                    if (String.IsNullOrEmpty(sizeNo_OutsoleCode.OutsoleSize) == false)
                        sizeNoOutsole = sizeNo_OutsoleCode.OutsoleSize;
                }

                string sizeNoString = regex.IsMatch(sizeNoOutsole) == true ? regex.Replace(sizeNoOutsole, "") : sizeNoOutsole;
                double sizeNoDouble = 0;
                Double.TryParse(sizeNoString, out sizeNoDouble);
                dr["SizeNoDouble"] = sizeNoDouble;
                dr1["SizeNoDouble"] = sizeNoDouble;

                dr["SizeNo"] = outsoleMaterialReject.SizeNo;
                dr1["SizeNo"] = outsoleMaterialReject.SizeNo;
                if (def.ShowOSSizeValue)
                {
                    dr["SizeOutsole"] = sizeNoOutsole;
                    dr1["SizeOutsole"] = sizeNoOutsole;
                }
                

                if (outsoleMaterialReject.QuantityReject > 0)
                {
                    dr["QuantityReject"] = outsoleMaterialReject.QuantityReject;
                    dt.Rows.Add(dr);
                }

                if (outsoleMaterialReject.RejectAssembly > 0)
                {
                    dr1["QuantityReject"] = outsoleMaterialReject.RejectAssembly;
                    dt1.Rows.Add(dr1);
                }
            }
            dtRejectAssemblyStockfit = dt1;
            e.Result = dt;
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaterialReject";
            rds.Value = dt;
            ReportParameter rp = new ReportParameter("Title", parTitle);
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            btnShowReject.IsEnabled = true;
            btnShowBalance.IsEnabled = true;
            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void btnShowReject_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                parTitle = "Reject/Lacking Report for STOCKFIT";
                this.Cursor = Cursors.Wait;
                btnShowReject.IsEnabled = false;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void btnShowBalance_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoadBalance.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnShowBalance.IsEnabled = false;
                bwLoadBalance.RunWorkerAsync();
            }
        }

        private void bwLoadBalance_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialBalanceReportList = ReportController.GetOutsoleBalanceReportList();

            DataTable dtBalance = new OutsoleMaterialRejectDataSet().Tables["OutsoleMaterialRejectTable"];
            var regex = new Regex(@"[a-z]|[A-Z]");
            foreach (var outsoleMaterialBalance in outsoleMaterialBalanceReportList)
            {
                DataRow dr = dtBalance.NewRow();
                var productNoList = new List<String>();
                productNoList = orderList.Where(w => w.OutsoleCode == outsoleMaterialBalance.OutsoleCode).Select(s => s.ProductNo).ToList();

                dr["ProductNo"] = outsoleMaterialBalance.ProductNo;

                dr["OutsoleCode"] = outsoleMaterialBalance.OutsoleCode;
                dr["ETD"] = outsoleMaterialBalance.ETD;
                dr["ArticleNo"] = outsoleMaterialBalance.ArticleNo;
                dr["SupplierETD"] = outsoleMaterialBalance.SupplierETD;
                dr["OutsoleSupplier"] = outsoleMaterialBalance.SupplierName;

                string sizeNoDisplay = outsoleMaterialBalance.SizeNo;
                var sizeNo_OutsoleCode = sizeRunList.Where(w => productNoList.Contains(w.ProductNo) && w.SizeNo == sizeNoDisplay).FirstOrDefault();
                if (sizeNo_OutsoleCode != null)
                {
                    if (String.IsNullOrEmpty(sizeNo_OutsoleCode.OutsoleSize) == false)
                        sizeNoDisplay = sizeNo_OutsoleCode.OutsoleSize;
                }

                string sizeNoString = regex.IsMatch(sizeNoDisplay) == true ?
                                        regex.Replace(sizeNoDisplay, "100")
                                        : sizeNoDisplay;
                double sizeNoDouble = 0;
                Double.TryParse(sizeNoString, out sizeNoDouble);
                dr["SizeNoDouble"] = sizeNoDouble;
                dr["SizeNo"] = outsoleMaterialBalance.SizeNo;
                if (def.ShowOSSizeValue)
                    dr["SizeOutsole"] = sizeNoDisplay;
                dr["QuantityDelivery"] = outsoleMaterialBalance.QuantityDelivery;
                dr["QuantityBalance"] = outsoleMaterialBalance.QuantityBalance;
                dr["QuantityOrder"] = outsoleMaterialBalance.QuantityOrder;

                dtBalance.Rows.Add(dr);
            }
            e.Result = dtBalance;
        }

        private void bwLoadBalance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dtBalance = e.Result as DataTable;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaterialBalance";
            rds.Value = dtBalance;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialBalanceReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            btnShowBalance.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoadAll.IsBusy == false)
            {
                btnShowAll.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwLoadAll.RunWorkerAsync();
            }
        }

        private void bwLoadAll_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialAllReportList = ReportController.GetOutsoleMaterialHasReject();
            
            DataTable dtAll = new OutsoleMaterialRejectDataSet().Tables["OutsoleMaterialRejectTable"];
            var regex = new Regex(@"[a-z]|[A-Z]");
            foreach (var outsoleMaterialAll in outsoleMaterialAllReportList)
            {
                DataRow dr = dtAll.NewRow();
                dr["ProductNo"] = outsoleMaterialAll.ProductNo;
                dr["OutsoleCode"] = outsoleMaterialAll.OutsoleCode;
                dr["ETD"] = outsoleMaterialAll.ETD;
                dr["ArticleNo"] = outsoleMaterialAll.ArticleNo;
                dr["SupplierETD"] = outsoleMaterialAll.SupplierETD;
                dr["OutsoleSupplier"] = outsoleMaterialAll.SupplierName;

                var productNoList = new List<String>();
                productNoList = orderList.Where(w => w.OutsoleCode == outsoleMaterialAll.OutsoleCode).Select(s => s.ProductNo).ToList();

                string sizeNoDisplay = outsoleMaterialAll.SizeNo;
                var sizeNo_OutsoleCode = sizeRunList.Where(w => productNoList.Contains(w.ProductNo) && w.SizeNo == sizeNoDisplay).FirstOrDefault();
                if (sizeNo_OutsoleCode != null)
                {
                    if (String.IsNullOrEmpty(sizeNo_OutsoleCode.OutsoleSize) == false)
                        sizeNoDisplay = sizeNo_OutsoleCode.OutsoleSize;
                }

                string sizeNoString = regex.IsMatch(sizeNoDisplay) == true ?
                                        regex.Replace(sizeNoDisplay, "100")
                                        : sizeNoDisplay;
                double sizeNoDouble = 0;
                Double.TryParse(sizeNoString, out sizeNoDouble);
                dr["SizeNoDouble"] = sizeNoDouble;
                dr["SizeNo"] = outsoleMaterialAll.SizeNo;
                if (def.ShowOSSizeValue)
                    dr["SizeOutsole"] = sizeNoDisplay;

                int balance = outsoleMaterialAll.QuantityBalance + outsoleMaterialAll.QuantityReject;
                if (balance != 0)
                    dr["QuantityBalance"] = balance;

                dr["HasReject"] = "False";
                if (outsoleMaterialAll.QuantityReject != 0 && outsoleMaterialAll.QuantityBalance != 0)
                {
                    dr["HasReject"] = "True";
                    dr["BackgroundColor"] = "Red";
                }

                dr["QuantityOrder"] = outsoleMaterialAll.QuantityOrder;

                dtAll.Rows.Add(dr);
            }
            e.Result = dtAll;
        }

        private void bwLoadAll_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dtAll = e.Result as DataTable;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaterialAll";
            rds.Value = dtAll;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialAllReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            btnShowAll.IsEnabled = true;
            this.Cursor = null;
        }

        private void radAssemblyReject_Click(object sender, RoutedEventArgs e)
        {
            parTitle = "Reject Assembly / Stockfit Report";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaterialReject";
            rds.Value = dtRejectAssemblyStockfit;

            ReportParameter rp = new ReportParameter("Title", parTitle);
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            btnShowReject.IsEnabled = true;
            btnShowBalance.IsEnabled = true;
        }
    }
}
