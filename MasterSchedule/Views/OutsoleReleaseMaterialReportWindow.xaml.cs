using System.Windows;
using System.Data;
using Microsoft.Reporting.WinForms;
using MasterSchedule.Models;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleReleaseMaterialPrintWindow.xaml
    /// </summary>
    public partial class OutsoleReleaseMaterialReportWindow : Window
    {
        string reportId;
        DataTable dt;
        ERelease releaseTo;
        public OutsoleReleaseMaterialReportWindow(string reportId, DataTable dt, ERelease releaseTo)
        {
            this.releaseTo = releaseTo;
            this.reportId = reportId;
            this.dt = dt;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string title = releaseTo == ERelease.ReleaseToOutsole ? "Release Material Report for STOCKFIT" : "Release Material Report To WH INSPECTION";
            ReportParameter rp = new ReportParameter("ReportId", reportId);
            ReportParameter rp1 = new ReportParameter("Title", title);

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleReleaseMaterial";
            rds.Value = dt;

            if (releaseTo == ERelease.ReleaseToOutsole)
            {
                //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleReleaseMaterialReport.rdlc";
                reportViewer.LocalReport.ReportPath = @"Reports\OutsoleReleaseMaterialReport.rdlc";
            }
            else
            {
                //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.2.1.8 Cding\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleReleaseMaterialToWHInspectionReport.rdlc";
                reportViewer.LocalReport.ReportPath = @"Reports\OutsoleReleaseMaterialToWHInspectionReport.rdlc";
            }
            
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
        }

    }
}
