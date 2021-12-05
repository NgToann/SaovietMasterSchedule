using System.Collections.Generic;
using System.Windows;
using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.ViewModels;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Globalization;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class SewingMasterReport_NewWindow : Window
    {
        List<SewingMasterViewModel> reportList;
        BackgroundWorker bwLoad;
        public SewingMasterReport_NewWindow(List<SewingMasterViewModel> reportList)
        {
            this.reportList = reportList;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

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
            Dispatcher.Invoke(new Action(() =>
            {
                DataTable dt = new SewingMasterFileDataSet().Tables["SMFDatatable"];
                foreach (var report in reportList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductNo"] = report.ProductNo;
                    dr["Country"] = report.Country;
                    dr["ShoeName"] = report.ShoeName;
                    dr["ArticleNo"] = report.ArticleNo;
                    dr["PatternNo"] = report.PatternNo;
                    dr["Quantity"] = report.Quantity;
                    dr["ETD"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", report.ETD);
                    dr["UpperMatsArrival"] = report.UpperMatsArrival;
                    dr["SewingMatsArrival"] = report.SewingMatsArrival;
                    dr["OSMatsArrival"] = report.OSMatsArrival;
                    dr["AssemblyMatsArrival"] = report.AssemblyMatsArrival;
                    dr["SewingLine"] = report.SewingLine;
                    dr["SewingPrep"] = report.SewingPrep;
                    dr["SewingStartDate"] = report.SewingStartDate;
                    dr["SewingFinishDate"] = report.SewingFinishDate;
                    dr["SewingQuota"] = report.SewingQuota;
                    dr["SewingActualStartDate"] = report.SewingActualStartDate;
                    dr["SewingActualFinishDate"] = report.SewingActualFinishDate;
                    dr["SewingActualStartDateAuto"] = report.SewingActualStartDateAuto;
                    dr["SewingActualFinishDateAuto"] = report.SewingActualFinishDateAuto;
                    dr["SewingBalance"] = report.SewingBalance;
                    dr["OSFinishDate"] = String.Format("{0:MM/dd}", report.OSFinishDate);
                    dr["OSBalance"] = report.OSBalance;
                    dr["CutAStartDate"] = String.Format("{0:MM/dd}", report.CutAStartDate);
                    dr["CutAFinishDate"] = String.Format("{0:MM/dd}", report.CutAFinishDate);
                    dr["CutBStartDate"] = report.CutBStartDate;
                    dr["CutAQuota"] = report.CutAQuota;
                    dr["AtomCutA"] = report.AtomCutA;
                    dr["AtomCutB"] = report.AtomCutB;
                    dr["LaserCutA"] = report.LaserCutA;
                    dr["LaserCutB"] = report.LaserCutB;
                    dr["HuasenCutA"] = report.HuasenCutA;
                    dr["HuasenCutB"] = report.HuasenCutB;
                    dr["ComelzCutA"] = report.ComelzCutA;
                    dr["ComelzCutB"] = report.ComelzCutB;
                    dr["CutAActualStartDate"] = report.CutAActualStartDate;
                    dr["CutAActualFinishDate"] = report.CutAActualFinishDate;
                    dr["CutABalance"] = report.CutABalance;
                    dr["H_FBalance"] = report.H_FBalance;
                    dr["CutBBalance"] = report.CutBBalance;

                    dt.Rows.Add(dr);
                }
                //ReportParameter rp = new ReportParameter("Line", line);
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "SMFReportDataset";
                rds.Value = dt;
                //
                //reportViewer.LocalReport.ReportPath = @"F:\SV PROJECT\MS\1.2.8.9 Cding\Saoviet Master Schedule Solution\SaovietMasterSchedule\MasterSchedule\Reports\SewingMasterFileReport.rdlc";
                reportViewer.LocalReport.ReportPath = @"Reports\SewingMasterFileReport.rdlc";
                //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.RefreshReport();
                this.Cursor = null;
            }));
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }
    }
}
