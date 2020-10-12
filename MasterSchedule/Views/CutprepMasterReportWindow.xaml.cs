using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.ViewModels;
using System.Linq;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class CutprepMasterReportWindow : Window
    {
        List<CutprepMasterExportViewModel> cutprepMasterExportViewList;
        List<String> columnNeedPrintList;
        string line;
        public CutprepMasterReportWindow(List<CutprepMasterExportViewModel> cutprepMasterExportViewList, string line, List<String> columnNeedPrintList)
        {
            this.cutprepMasterExportViewList = cutprepMasterExportViewList;
            this.line = line;
            this.columnNeedPrintList = columnNeedPrintList;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new CutprepMasterDataSet().Tables["CutprepMasterTable"];

            foreach (CutprepMasterExportViewModel cutprepMasterExportView in cutprepMasterExportViewList)
            {
                DataRow dr = dt.NewRow();
                dr["Sequence"] = cutprepMasterExportView.Sequence;
                dr["ProductNo"] = cutprepMasterExportView.ProductNo;
                dr["Country"] = cutprepMasterExportView.Country;
                dr["ShoeName"] = cutprepMasterExportView.ShoeName;
                dr["ArticleNo"] = cutprepMasterExportView.ArticleNo;
                dr["PatternNo"] = cutprepMasterExportView.PatternNo;
                dr["Quantity"] = cutprepMasterExportView.Quantity;
                dr["ETD"] = cutprepMasterExportView.ETD;
                dr["SewingLine"] = cutprepMasterExportView.SewingLine;
                dr["UpperMatsArrival"] = cutprepMasterExportView.UpperMatsArrival;
                dr["SewingStartDate"] = cutprepMasterExportView.SewingStartDate;
                dr["SewingQuota"] = cutprepMasterExportView.SewingQuota;
                dr["SewingBalance"] = cutprepMasterExportView.SewingBalance;
                dr["CutAStartDate"] = cutprepMasterExportView.CutAStartDate;
                dr["CutAFinishDate"] = cutprepMasterExportView.CutAFinishDate;
                dr["CutAQuota"] = cutprepMasterExportView.CutAQuota;
                dr["AutoCut"] = cutprepMasterExportView.AutoCut;
                dr["LaserCut"] = cutprepMasterExportView.LaserCut;
                dr["HuasenCut"] = cutprepMasterExportView.HuasenCut;
                dr["CutABalance"] = cutprepMasterExportView.CutABalance;
                dr["PrintingBalance"] = cutprepMasterExportView.PrintingBalance;
                dr["H_FBalance"] = cutprepMasterExportView.H_FBalance;
                dr["EmbroideryBalance"] = cutprepMasterExportView.EmbroideryBalance;
                dr["CutBBalance"] = cutprepMasterExportView.CutBBalance;
                dr["IsUpperMatsArrivalOk"] = cutprepMasterExportView.IsUpperMatsArrivalOk;
                dr["IsHaveMemo"] = !string.IsNullOrEmpty(cutprepMasterExportView.MemoId);

                dr["CutBStartDate"] = cutprepMasterExportView.CutBStartDate;
                dr["AtomCutA"] = cutprepMasterExportView.AtomCutA;
                dr["AtomCutB"] = cutprepMasterExportView.AtomCutB;
                dr["LaserCutA"] = cutprepMasterExportView.LaserCutA;
                dr["LaserCutB"] = cutprepMasterExportView.LaserCutB;
                dr["HuasenCutA"] = cutprepMasterExportView.HuasenCutA;
                dr["HuasenCutB"] = cutprepMasterExportView.HuasenCutB;
                dr["ComelzCutA"] = cutprepMasterExportView.ComelzCutA;
                dr["ComelzCutB"] = cutprepMasterExportView.ComelzCutB;

                dt.Rows.Add(dr);
            }


            //ReportParameter rp = new ReportParameter("Line", line);
            // Hidden =iif(Parameters!ShowProductNo.Value="1", false, true)
            var parameterList = new List<ReportParameter>();
            parameterList.Add(new ReportParameter("Line", line));
            var propertyList = typeof(CutprepMasterExportViewModel).GetProperties().ToList();
            foreach (var property in propertyList)
            {
                string value = "0";
                if (columnNeedPrintList.Contains(property.Name))
                    value = "1";
                parameterList.Add(new ReportParameter(String.Format("Show{0}", property.Name), value));
            }

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "CutprepMaster";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\CutprepMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\CutprepMasterReport.rdlc";
            reportViewer.LocalReport.SetParameters(parameterList);
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
