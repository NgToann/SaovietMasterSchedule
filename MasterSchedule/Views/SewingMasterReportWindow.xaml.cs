using System.Collections.Generic;
using System.Windows;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.ViewModels;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class SewingMasterReportWindow : Window
    {
        List<SewingMasterExportViewModel> sewingMasterExportViewList;
        string line;
        int typeOfReport;
        public SewingMasterReportWindow(List<SewingMasterExportViewModel> sewingMasterExportViewList, string line, int typeOfReport)
        {
            this.sewingMasterExportViewList = sewingMasterExportViewList;
            this.line = line;
            // 0 Report to print ( for planning office ) ...
            // 1 Report to create schedule ( for purchasing ) ...
            this.typeOfReport = typeOfReport;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new SewingMasterDataSet().Tables["SewingMasterTable"];

            foreach (SewingMasterExportViewModel sewingMasterExportView in sewingMasterExportViewList)
            {
                DataRow dr = dt.NewRow();
                dr["Sequence"] = sewingMasterExportView.Sequence;
                dr["ProductNo"] = sewingMasterExportView.ProductNo;
                dr["Country"] = sewingMasterExportView.Country;
                dr["ShoeName"] = sewingMasterExportView.ShoeName;
                dr["ArticleNo"] = sewingMasterExportView.ArticleNo;
                dr["PatternNo"] = sewingMasterExportView.PatternNo;
                dr["Quantity"] = sewingMasterExportView.Quantity;
                dr["ETD"] = sewingMasterExportView.ETD;
                dr["SewingLine"] = sewingMasterExportView.SewingLine;
                dr["UpperMatsArrival"] = sewingMasterExportView.UpperMatsArrival;
                dr["CutAStartDate"] = sewingMasterExportView.CutAStartDate;
                dr["CutBStartDate"] = sewingMasterExportView.CutBStartDate;
                dr["SewingMatsArrival"] = sewingMasterExportView.SewingMatsArrival;
                dr["SewingStartDate"] = sewingMasterExportView.SewingStartDate;
                dr["SewingFinishDate"] = sewingMasterExportView.SewingFinishDate;
                dr["OSMatsArrival"] = sewingMasterExportView.OSMatsArrival;
                dr["OSBalance"] = sewingMasterExportView.OSBalance;
                dr["SewingQuota"] = sewingMasterExportView.SewingQuota;
                dr["SewingBalance"] = sewingMasterExportView.SewingBalance;
                dr["CutABalance"] = sewingMasterExportView.CutABalance;
                dr["CutBBalance"] = sewingMasterExportView.CutBBalance;

                dr["IsUpperMatsArrivalOk"] = sewingMasterExportView.IsUpperMatsArrivalOk;
                dr["IsSewingMatsArrivalOk"] = sewingMasterExportView.IsSewingMatsArrivalOk;
                dr["IsOSMatsArrivalOk"] = sewingMasterExportView.IsOSMatsArrivalOk;
                dr["IsHaveMemo"] = !string.IsNullOrEmpty(sewingMasterExportView.MemoId);

                dt.Rows.Add(dr);
            }

            ReportParameter rp = new ReportParameter("Line", line);
            ReportDataSource rds = new ReportDataSource();
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\SewingMasterReport.rdlc";

            if (typeOfReport == 1)
            {
                //rds.Name = "SewingMasterDataSet_1";
                rds.Name = "SewingMaster";
                rds.Value = dt;
                reportViewer.LocalReport.ReportPath = @"Reports\SewingMasterReport_2.rdlc";
            }
            else
            {
                rds.Name = "SewingMaster";
                rds.Value = dt;
                reportViewer.LocalReport.ReportPath = @"Reports\SewingMasterReport.rdlc";
            }

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
