using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using MasterSchedule.Models;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperAccessoriesReportDetailWindow.xaml
    /// </summary>
    public partial class UpperAccessoriesReportDetailWindow : Window
    {
        private DataRowView rowClicked;
        List<ReportUpperAccessoriesDetailBySupplierModel> reportListDetailBySupp;
        BackgroundWorker bwLoad;
        DateTime dtDefault = new DateTime(2000, 01, 01);

        public UpperAccessoriesReportDetailWindow(DataRowView rowClicked)
        {
            this.rowClicked = rowClicked;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            reportListDetailBySupp = new List<ReportUpperAccessoriesDetailBySupplierModel>();

            InitializeComponent();
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            int supplierId = (Int32)e.Argument;
            reportListDetailBySupp = ReportController.GetUpperAccessoriesMaterialDeliveryBySupplier(supplierId);

            //Create Grid
            Dispatcher.Invoke(new Action(() => {

                DataTable dt = new DataTable();
                dgReportBySupplier.Columns.Clear();

                // Column PO
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTemplateColumn colPO = new DataGridTemplateColumn();
                colPO.Header = String.Format("Product No");
                DataTemplate tmpPO = new DataTemplate();
                FrameworkElementFactory tblPO = new FrameworkElementFactory(typeof(TextBlock));
                tmpPO.VisualTree = tblPO;
                tblPO.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ProductNo")));
                tblPO.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblPO.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colPO.CellTemplate = tmpPO;
                colPO.SortMemberPath = "ProductNo";
                colPO.ClipboardContentBinding = new Binding(String.Format("ProductNo"));
                dgReportBySupplier.Columns.Add(colPO);

                dt.Columns.Add("Reviser", typeof(String));
                DataGridTemplateColumn colReviser = new DataGridTemplateColumn();
                colReviser.Header = String.Format("Reviser");
                DataTemplate tmpReviser = new DataTemplate();
                FrameworkElementFactory tbReviser = new FrameworkElementFactory(typeof(TextBlock));
                tmpReviser.VisualTree = tbReviser;
                tbReviser.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Reviser")));
                tbReviser.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tbReviser.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colReviser.CellTemplate = tmpReviser;
                colReviser.SortMemberPath = "Reviser";
                colReviser.ClipboardContentBinding = new Binding(String.Format("Reviser"));
                dgReportBySupplier.Columns.Add(colReviser);

                // Col Supplier
                dt.Columns.Add("Name", typeof(String));
                DataGridTemplateColumn colSupp = new DataGridTemplateColumn();
                colSupp.Header = String.Format("Supplier");
                DataTemplate tmpSupp = new DataTemplate();
                FrameworkElementFactory tblSupp = new FrameworkElementFactory(typeof(TextBlock));
                tmpSupp.VisualTree = tblSupp;
                tblSupp.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Name")));
                tblSupp.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblSupp.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colSupp.CellTemplate = tmpSupp;
                colSupp.SortMemberPath = "Name";
                colSupp.ClipboardContentBinding = new Binding(String.Format("Name"));
                dgReportBySupplier.Columns.Add(colSupp);

                // Column ETD
                dt.Columns.Add("ETD", typeof(String));
                DataGridTemplateColumn colETD = new DataGridTemplateColumn();
                colETD.Header = String.Format("ETD");
                DataTemplate tmpETD = new DataTemplate();
                FrameworkElementFactory tblETD = new FrameworkElementFactory(typeof(TextBlock));
                tmpETD.VisualTree = tblETD;
                tblETD.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ETD")));
                tblETD.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblETD.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colETD.CellTemplate = tmpETD;
                colETD.SortMemberPath = "ETD";
                colETD.ClipboardContentBinding = new Binding(String.Format("ETD"));
                dgReportBySupplier.Columns.Add(colETD);

                // Column ActualDate
                dt.Columns.Add("ActualDate", typeof(String));
                DataGridTemplateColumn colActual = new DataGridTemplateColumn();
                colActual.Header = String.Format("ActualDate");
                DataTemplate tmpActual = new DataTemplate();
                FrameworkElementFactory tblActual = new FrameworkElementFactory(typeof(TextBlock));
                tmpActual.VisualTree = tblActual;
                tblActual.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ActualDate")));
                tblActual.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblActual.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colActual.CellTemplate = tmpActual;
                colActual.SortMemberPath = "ActualDate";
                colActual.ClipboardContentBinding = new Binding(String.Format("ActualDate"));
                dgReportBySupplier.Columns.Add(colActual);

                var sizeList = reportListDetailBySupp.Select(s => s.SizeNo).Distinct().ToList();
                var regex = new Regex("[a-z]|[A-Z]");
                var sizeNoList = sizeList.Select(s => s).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();
                
                foreach(var sizeNo in sizeNoList)
                {
                    string sizeBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "@") : sizeNo;
                    dt.Columns.Add(String.Format("Column{0}", sizeBinding), typeof(String));
                    DataGridTemplateColumn colSize = new DataGridTemplateColumn();
                    colSize.Header = String.Format("{0}", sizeNo);
                    colSize.MinWidth = 40;
                    DataTemplate tmpSize = new DataTemplate();
                    FrameworkElementFactory tblSize = new FrameworkElementFactory(typeof(TextBlock));
                    tmpSize.VisualTree = tblSize;
                    tblSize.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", sizeBinding)));
                    tblSize.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    tblSize.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblSize.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    colSize.CellTemplate = tmpSize;
                    colSize.ClipboardContentBinding = new Binding(String.Format("Column{0}", sizeBinding));
                    dgReportBySupplier.Columns.Add(colSize);
                }

                dt.Columns.Add("QuantityOK", typeof(String));
                DataGridTemplateColumn colQtyOK = new DataGridTemplateColumn();
                colQtyOK.Header = String.Format("Qty OK");
                DataTemplate tmpQtyOK = new DataTemplate();
                FrameworkElementFactory tblQtyOK = new FrameworkElementFactory(typeof(TextBlock));
                tmpQtyOK.VisualTree = tblQtyOK;
                tblQtyOK.SetBinding(TextBlock.TextProperty, new Binding(String.Format("QuantityOK")));
                tblQtyOK.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblQtyOK.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                tblQtyOK.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colQtyOK.CellTemplate = tmpQtyOK;
                colQtyOK.SortMemberPath = "QuantityOK";
                colQtyOK.ClipboardContentBinding = new Binding(String.Format("QuantityOK"));
                dgReportBySupplier.Columns.Add(colQtyOK);

                dt.Columns.Add("QuantityReject", typeof(String));
                DataGridTemplateColumn colQtyReject = new DataGridTemplateColumn();
                colQtyReject.Header = String.Format("Total Reject");
                DataTemplate tmpQtyReject = new DataTemplate();
                FrameworkElementFactory tblQtyReject = new FrameworkElementFactory(typeof(TextBlock));
                tmpQtyReject.VisualTree = tblQtyReject;
                tblQtyReject.SetBinding(TextBlock.TextProperty, new Binding(String.Format("QuantityReject")));
                tblQtyReject.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblQtyReject.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                tblQtyReject.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                colQtyReject.CellTemplate = tmpQtyReject;
                colQtyReject.SortMemberPath = "QuantityReject";
                colQtyReject.ClipboardContentBinding = new Binding(String.Format("QuantityReject"));
                dgReportBySupplier.Columns.Add(colQtyReject);


                // Binding Data
                var productNoList = reportListDetailBySupp.Select(s => s.ProductNo).Distinct().ToList();
                if (productNoList.Count() > 0)
                    productNoList = productNoList.OrderBy(o => o).ToList();
                int totalQtyOK = 0, totalQtyReject = 0;
                foreach (var productNo in productNoList)
                {
                    var reportByPO = reportListDetailBySupp.Where(w => w.ProductNo.Equals(productNo)).ToList();
                    var reportByPOFirst = reportListDetailBySupp.FirstOrDefault();
                    DataRow dr = dt.NewRow();

                    dr["ProductNo"]     = productNo;
                    dr["Name"]          = reportByPOFirst.Name;
                    dr["Reviser"]       = String.Join("\n", reportByPO.Select(s => s.Reviser).Distinct().ToList());
                    dr["ETD"]           = String.Format("{0:MM/dd/yyyy}", reportByPOFirst.ETD);
                    dr["ActualDate"]    = reportByPOFirst.ActualDate != dtDefault ? String.Format("{0:MM/dd/yyyy}", reportByPOFirst.ActualDate) : "";

                    var sizeNoListByPO = reportByPO.Select(s => s.SizeNo).Distinct().ToList();
                    foreach (var sizeNo in sizeNoListByPO)
                    {
                        var reportBySizeList = reportByPO.Where(w => w.SizeNo.Equals(sizeNo)).ToList();
                        string sizeBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "@") : sizeNo;

                        var qtyOKBySize = reportBySizeList.FirstOrDefault(f => f.RejectId == 0);
                        List<String> displayCellList = new List<string>();
                        if (qtyOKBySize != null)
                            displayCellList.Add(qtyOKBySize.Quantity.ToString());
                        var rejectsBySize = reportBySizeList.Where(f => f.RejectId > 0);
                        foreach (var reject in rejectsBySize)
                        {
                            displayCellList.Add(String.Format("{0} - {1}", reject.RejectName, reject.Reject));
                        }

                        dr[String.Format("Column{0}", sizeBinding)] = String.Join("\n", displayCellList);
                    }

                    var qtyOK = reportByPO.Where(w => w.RejectId == 0).Sum(s => s.Quantity);
                    var qtyReject = reportByPO.Where(w => w.RejectId > 0).Sum(s => s.Reject);
                    totalQtyOK += qtyOK;
                    totalQtyReject += qtyReject;
                    if (qtyOK > 0)
                        dr["QuantityOK"] = String.Format("{0:N0}", qtyOK);
                    if (qtyReject > 0)
                        dr["QuantityReject"] = String.Format("{0:N0}", qtyReject);
                    dt.Rows.Add(dr);
                }

                // drTotal
                if (productNoList.Count() > 1)
                {
                    DataRow drTotal = dt.NewRow();
                    drTotal["ProductNo"] = "TOTAL";
                    if (totalQtyOK > 0)
                        drTotal["QuantityOK"] = String.Format("{0:N0}", totalQtyOK);
                    if (totalQtyReject > 0)
                        drTotal["QuantityReject"] = String.Format("{0:N0}", totalQtyReject);

                    dt.Rows.Add(drTotal);
                }

                dgReportBySupplier.ItemsSource = dt.AsDataView();
                dgReportBySupplier.Items.Refresh();
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title      = this.Title + " - " + String.Format("{0} - {1}", rowClicked["Name"].ToString(), rowClicked["AccessoriesName"].ToString());
            tblTitle.Text   = String.Format("UPPER ACCESSORIES REPORT DETAIL - {0} - {1}", rowClicked["Name"].ToString(), rowClicked["AccessoriesName"].ToString());
            int supplierId = 0;
            Int32.TryParse(rowClicked["SupplierId"].ToString(), out supplierId);
            if (bwLoad.IsBusy == false && supplierId > 0)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync(supplierId);
            }
        }

        private void dgReportBySupplier_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
