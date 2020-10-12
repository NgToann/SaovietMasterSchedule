using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class OutsoleDeliveryReportWindow : Window
    {
        BackgroundWorker bwReport;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<SizeRunModel> sizeRunList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OrdersModel> ordersList;

        List<ReportOutsoleMaterialDeliveryFromToModel> reportList;
        public OutsoleDeliveryReportWindow()
        {
            bwReport = new BackgroundWorker();
            bwReport.DoWork += new DoWorkEventHandler(bwReport_DoWork);
            bwReport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwReport_RunWorkerCompleted);

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            sizeRunList = new List<SizeRunModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            ordersList = new List<OrdersModel>();
            reportList = new List<ReportOutsoleMaterialDeliveryFromToModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDateFrom.SelectedDate = DateTime.Now;
            dpDate.SelectedDate = DateTime.Now;
        }

        DateTime dateSearchTo = new DateTime(2000, 1, 1);
        DateTime dateSearchFrom = new DateTime(2000, 1, 1);
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (bwReport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnReport.IsEnabled = false;
                dateSearchTo = dpDate.SelectedDate.Value;
                dateSearchFrom = dpDateFrom.SelectedDate.Value;
                bwReport.RunWorkerAsync();
            }
        }

        private void bwReport_DoWork(object sender, DoWorkEventArgs e)
        {
            reportList = ReportController.GetOutsoleDeliveryFromTo(dateSearchFrom, dateSearchTo);
            //outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            //outsoleMaterialList = OutsoleMaterialController.SelectByOutsoleRawMaterial();
            //sizeRunList = SizeRunController.SelectByOutsoleRawMaterial();
            //outsoleSupplierList = OutsoleSuppliersController.Select();
            //ordersList = OrdersController.SelectByOutsoleRawMaterial();

            DataTable dt = new OutsoleDeliveryDataSet().Tables["OutsoleDeliveryTable"];

            foreach (var report in reportList)
            {
                DataRow dr = dt.NewRow();
                dr["ProductNo"]         = report.ProductNo;
                dr["ArticleNo"]         = report.ArticleNo;
                dr["OutsoleCode"]       = report.OutsoleCode;
                dr["Quantity"]          = report.Quantity;
                dr["ETD"]               = report.ETD;
                dr["QuantityDelivery"]  = report.QuantityDelivery;
                dr["Reject"]            = report.Reject > 0 ? report.Reject.ToString() : "";
                dr["Remarks"]           = report.Balance > 0 ? report.Balance.ToString() : "";
                dr["SupplierETD"]       = report.SupplierEFD;
                dr["Supplier"]          = report.SupplierName;

                dt.Rows.Add(dr);
            }

            //List<String> productNoList = outsoleRawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            //foreach (string productNo in productNoList)
            //{
            //    OrdersModel order = ordersList.FirstOrDefault(f => f.ProductNo == productNo);
            //    List<OutsoleRawMaterialModel> outsoleRawMaterialOfProductNoList = outsoleRawMaterialList.Where(o => o.ProductNo == productNo).ToList();
            //    List<SizeRunModel> sizeRunOfProductNoList = sizeRunList.Where(s => s.ProductNo == productNo).ToList();
            //    List<OutsoleMaterialModel> outsoleMaterialOfProductNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).ToList();
            //    foreach (OutsoleRawMaterialModel outsoleRawMaterial in outsoleRawMaterialOfProductNoList)
            //    {
            //        DateTime etd = outsoleRawMaterial.ETD.Date;
            //        bool isFull = OutsoleRawMaterialController.IsFull(sizeRunOfProductNoList, new List<OutsoleRawMaterialModel>() { outsoleRawMaterial, }, outsoleMaterialOfProductNoList);
            //        if (etd != new DateTime(2000, 1, 1) && (etd >= dateSearchFrom.Date && etd <= dateSearchTo.Date) && isFull == false)
            //        {
            //            DataRow dr = dt.NewRow();
            //            dr["ProductNo"] = productNo;
            //            if (order != null)
            //            {
            //                dr["ArticleNo"] = order.ArticleNo;
            //                dr["OutsoleCode"] = order.OutsoleCode;
            //                dr["Quantity"] = order.Quantity;
            //                dr["ETD"] = order.ETD;
            //            }
            //            dr["SupplierETD"] = etd;
            //            dr["Remarks"] = sizeRunOfProductNoList.Sum(s => (s.Quantity - outsoleMaterialOfProductNoList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == s.SizeNo).Sum(o => (o.Quantity - o.QuantityReject)))).ToString();
            //            OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId);
            //            if (outsoleSupplier != null)
            //            {
            //                dr["Supplier"] = outsoleSupplier.Name;
            //            }

            //            dt.Rows.Add(dr);
            //        }
            //    }
            //}
            e.Result = dt;
        }
        private void bwReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleDelivery";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleDeliveryReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleDeliveryReport.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            btnReport.IsEnabled = true;
            this.Cursor = null;
        }
    }
}
