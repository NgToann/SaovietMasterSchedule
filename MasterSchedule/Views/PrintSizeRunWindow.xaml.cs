using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using MasterSchedule.Controllers;
using MasterSchedule.Models;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for PrintSockliningRequestWindow.xaml
    /// </summary>
    public partial class PrintSizeRunWindow : Window
    {
        BackgroundWorker bwPreview;
        List<PrintSizeRunModel> printSizeRunList;
        public PrintSizeRunWindow()
        {
            bwPreview = new BackgroundWorker();
            bwPreview.DoWork += BwPreview_DoWork;
            bwPreview.RunWorkerCompleted += BwPreview_RunWorkerCompleted;

            printSizeRunList = new List<PrintSizeRunModel>();
            InitializeComponent();
        }

        private void BwPreview_DoWork(object sender, DoWorkEventArgs e)
        {
            printSizeRunList = new List<PrintSizeRunModel>();
            var lisfOfPOSearch = e.Argument as List<String>;
            foreach (var po in lisfOfPOSearch.Distinct().ToList())
            {
                printSizeRunList.AddRange(PrintSizeRunController.GetByPO(po.Trim().ToUpper().ToString()));
            }

            // CREATE COLUMNS
            Dispatcher.Invoke(new Action(() =>
            {
                DataTable dt = new DataTable();
                dgSizeRun.Columns.Clear();

                // Column ProductNo
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTemplateColumn colPO = new DataGridTemplateColumn();
                colPO.Header = String.Format("ProductNo");
                DataTemplate templatePO = new DataTemplate();
                FrameworkElementFactory tblPO = new FrameworkElementFactory(typeof(TextBlock));
                templatePO.VisualTree = tblPO;
                tblPO.SetValue(TextBlock.TextProperty, "");
                tblPO.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ProductNo")));
                //tblPO.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
                tblPO.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblPO.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colPO.CellTemplate = templatePO;
                colPO.ClipboardContentBinding = new Binding(String.Format("ProductNo"));
                dgSizeRun.Columns.Add(colPO);

                // Column EFD
                dt.Columns.Add("EFD", typeof(String));
                DataGridTemplateColumn colEFD = new DataGridTemplateColumn();
                colEFD.Header = String.Format("EFD");
                DataTemplate templateEFD = new DataTemplate();
                FrameworkElementFactory tblEFD = new FrameworkElementFactory(typeof(TextBlock));
                templateEFD.VisualTree = tblEFD;
                tblEFD.SetValue(TextBlock.TextProperty, "");
                tblEFD.SetBinding(TextBlock.TextProperty, new Binding(String.Format("EFD")));
                tblEFD.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblEFD.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colEFD.CellTemplate = templateEFD;
                colEFD.ClipboardContentBinding = new Binding(String.Format("EFD"));
                dgSizeRun.Columns.Add(colEFD);

                // Column ShoeName
                dt.Columns.Add("ShoeName", typeof(String));
                DataGridTemplateColumn colShoeName = new DataGridTemplateColumn();
                colShoeName.Header = String.Format("Shoe Name");
                DataTemplate templateShoeName = new DataTemplate();
                FrameworkElementFactory tblShoeName = new FrameworkElementFactory(typeof(TextBlock));
                templateShoeName.VisualTree = tblShoeName;
                tblShoeName.SetValue(TextBlock.TextProperty, "");
                tblShoeName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ShoeName")));
                tblShoeName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblShoeName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colShoeName.CellTemplate = templateShoeName;
                colShoeName.ClipboardContentBinding = new Binding(String.Format("ShoeName"));
                dgSizeRun.Columns.Add(colShoeName);

                // Column SewingStart
                dt.Columns.Add("SewingStartDate", typeof(String));
                DataGridTemplateColumn colSewStart = new DataGridTemplateColumn();
                colSewStart.Header = String.Format("Sewing Start");
                DataTemplate templateSewStart = new DataTemplate();
                FrameworkElementFactory tblSewStart = new FrameworkElementFactory(typeof(TextBlock));
                templateSewStart.VisualTree = tblSewStart;
                tblSewStart.SetValue(TextBlock.TextProperty, "");
                tblSewStart.SetBinding(TextBlock.TextProperty, new Binding(String.Format("SewingStartDate")));
                tblSewStart.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblSewStart.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colSewStart.CellTemplate = templateSewStart;
                colSewStart.ClipboardContentBinding = new Binding(String.Format("SewingStartDate"));
                dgSizeRun.Columns.Add(colSewStart);

                var sizeNoRawList = printSizeRunList.Select(s => s.SizeNo).Distinct().ToList();
                var regex = new Regex("[a-z]|[A-Z]");
                var sizeNoList = sizeNoRawList.Select(s => s).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

                foreach (var sizeNo in sizeNoList)
                {
                    string sizeNoVar = sizeNo.Contains(".") ? sizeNo.Replace(".", "@").ToString() : sizeNo;

                    dt.Columns.Add(String.Format("Column{0}", sizeNoVar), typeof(String));
                    DataGridTemplateColumn colQuantity = new DataGridTemplateColumn();
                    colQuantity.Header = String.Format(sizeNo);
                    colQuantity.Width = 40;
                    DataTemplate templateQuantity = new DataTemplate();
                    FrameworkElementFactory tblQuantity = new FrameworkElementFactory(typeof(TextBlock));
                    templateQuantity.VisualTree = tblQuantity;
                    tblQuantity.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", sizeNoVar)));
                    tblQuantity.SetValue(TextBlock.PaddingProperty, new Thickness(3, 3, 3, 3));
                    tblQuantity.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblQuantity.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                    colQuantity.CellTemplate = templateQuantity;
                    colQuantity.ClipboardContentBinding = new Binding(String.Format("Column{0}", sizeNoVar));
                    dgSizeRun.Columns.Add(colQuantity);
                }

                // Binding Data
                var productNoList = printSizeRunList.Select(s => s.ProductNo).Distinct().ToList(); 
                if (productNoList.Count() > 0)
                    productNoList = productNoList.OrderBy(o => o).ToList();
                foreach (var productNo in productNoList)
                {
                    var printSizeRunFirst = printSizeRunList.FirstOrDefault(f => f.ProductNo == productNo);
                    var printSizeListByPO = printSizeRunList.Where(w => w.ProductNo == productNo).ToList();

                    DataRow dr = dt.NewRow();
                    dr["ProductNo"]         = productNo;
                    dr["EFD"]               = String.Format("{0:MM/dd/yyy}", printSizeRunFirst.EFD);
                    dr["SewingStartDate"]   = String.Format("{0:MM/dd/yyy}", printSizeRunFirst.SewingStartDate);
                    dr["ShoeName"]          = printSizeRunFirst.ShoeName;
                    foreach (var printSizeRun in printSizeListByPO)
                    {
                        var psrBySize = printSizeListByPO.FirstOrDefault(f => f.SizeNo == printSizeRun.SizeNo);
                        string sizeNoVar = printSizeRun.SizeNo.Contains(".") ? printSizeRun.SizeNo.Replace(".", "@").ToString() : printSizeRun.SizeNo;
                        dr[String.Format("Column{0}", sizeNoVar)] = psrBySize.Quantity;
                    }
                    dt.Rows.Add(dr);
                }
                dgSizeRun.ItemsSource = dt.AsDataView();
            }));
        }

        private void BwPreview_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnPreview.IsEnabled = true;
            this.Cursor = null;
        }


        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            string productNoListSearch = txtProductNoList.Text.Trim().ToString();
            List<String> listOfPOSearch = productNoListSearch.Split(';').ToList();
            if (bwPreview.IsBusy == false && listOfPOSearch.Count() > 0)
            {
                btnPreview.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwPreview.RunWorkerAsync(listOfPOSearch);
            }
        }

        private void dgSizeRun_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintSizeRunReportWindow window = new PrintSizeRunReportWindow(printSizeRunList);
            window.Show();
        }
    }
}
