using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.Models;
using System;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for PrintSizeRunReportWindow.xaml
    /// </summary>
    public partial class PrintSizeRunReportWindow : Window
    {
        List<PrintSizeRunModel> printSizeRunList;
        PrivateDefineModel privateDef;
        public PrintSizeRunReportWindow(List<PrintSizeRunModel> printSizeRunList, PrivateDefineModel privateDef)
        {
            this.printSizeRunList = printSizeRunList;
            this.privateDef = privateDef;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new PrintSizeRunDataSet().Tables["PrintSizeRunDataTable"];

            var productNoList = printSizeRunList.Select(s => s.ProductNo).Distinct().ToList();
            //if (productNoList.Count() > 0)
            //    productNoList = productNoList.OrderBy(o => o).ToList();
            foreach (var productNo in productNoList)
            {
                var printSizeRunFirst = printSizeRunList.FirstOrDefault(f => f.ProductNo == productNo);
                var printSizeListByPO = printSizeRunList.Where(w => w.ProductNo == productNo).ToList();
                foreach (var printSizeRun in printSizeListByPO)
                {
                    DataRow dr = dt.NewRow();

                    dr["ProductNo"]         = productNo;
                    dr["EFD"]               = String.Format("{0:MM/dd/yyy}", printSizeRunFirst.EFD);
                    dr["SewingStartDate"]   = String.Format("{0:MM/dd/yyy}", printSizeRunFirst.SewingStartDate);
                    dr["ShoeName"]          = printSizeRunFirst.ShoeName;

                    var psrBySize = printSizeListByPO.FirstOrDefault(f => f.SizeNo == printSizeRun.SizeNo);
                    double sizeNoDouble = 100;
                    Double.TryParse(printSizeRun.SizeNo, out sizeNoDouble);
                    dr["SizeNoDouble"] = sizeNoDouble;
                    dr["SizeNo"] = printSizeRun.SizeNo;

                    // Show the size
                    dr["LastSize"] = printSizeRun.LastSize;
                    dr["MidsoleSize"] = printSizeRun.MidsoleSize;

                    dr["Quantity"] = psrBySize.Quantity;
                    dr["TotalQuantity"] = printSizeListByPO.Sum(s => s.Quantity);

                    //if (productNo == productNoList.LastOrDefault())
                    //    dr["TotalQuantityPrint"] = String.Format("Total Quantity: {0}", printSizeRunList.Sum(s => s.Quantity));

                    dt.Rows.Add(dr);
                }
            }

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "PrintSizeRunDataSource";
            rds.Value = dt;
            ReportParameter rp = new ReportParameter("TotalQuantity", String.Format("Total Quantity: {0}", printSizeRunList.Sum(s => s.Quantity)));
            //ReportParameter rp = new ReportParameter("ShowOSSize", privateDef.ShowOSSizeValue == true ? "1" : "0");
            reportViewer.LocalReport.ReportPath = @"Reports\PrinSizeRunReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
