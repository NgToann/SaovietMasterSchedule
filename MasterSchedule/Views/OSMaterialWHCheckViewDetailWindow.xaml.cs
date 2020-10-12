using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using MasterSchedule.Models;
using Microsoft.Reporting.WinForms;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OSMaterialWHCheckViewDetailWindow.xaml
    /// </summary>
    public partial class OSMaterialWHCheckViewDetailWindow : Window
    {
        List<OutsoleMaterialCheckingModel> osMatCheckingList;
        List<ErrorModel> errorSourceList;
        OrdersModel orderInformation;
        OutsoleSuppliersModel supplierClicked;
        public OSMaterialWHCheckViewDetailWindow(List<OutsoleMaterialCheckingModel> osMatCheckingList, List<ErrorModel> errorSourceList, OrdersModel orderInformation, OutsoleSuppliersModel supplierClicked)
        {
            this.osMatCheckingList  = osMatCheckingList;
            this.errorSourceList    = errorSourceList;
            this.orderInformation   = orderInformation;
            this.supplierClicked    = supplierClicked;
            InitializeComponent();
        }

        private bool reportBySize = true;
        private bool loaded = false;
        private void LoadData()
        {
            DataTable dtBySize = new OSMatWHCheckDetailBySize().Tables["OSMatWHCheckDetailBySizeTable"];
            DataTable dtByCart = new OSMatWHCheckDetailByCart().Tables["OSMatCheckDetailByCartTable"];

            txtPO.Text          = String.Format("PO      : {0}", orderInformation.ProductNo);
            txtArticleNo.Text   = String.Format("Article : {0}", orderInformation.ArticleNo);
            txtShoeName.Text    = String.Format("Shoe Name: {0}", orderInformation.ShoeName);
            
            txtWorker.Text      = String.Format("Worker   : {0}", String.Join(", ", osMatCheckingList.Select(s => s.WorkerId).Distinct().ToList()));
            txtSupplier.Text    = string.Format("Supplier : {0}", supplierClicked.Name);
            txtOSCode.Text      = string.Format("O/SCode: {0}", orderInformation.OutsoleCode);

            txtQty.Text         = String.Format("Quantity OK      : {0}", osMatCheckingList.Sum(s => s.Quantity));
            txtReject.Text      = String.Format("Quantity Reject : {0}", osMatCheckingList.Sum(s => s.Reject));
            txtReturn.Text      = String.Format("Quantity Return: {0}", osMatCheckingList.Sum(s => s.ReturnReject));

            var regex = new Regex(@"[a-z]|[A-Z]");
            stkMain.Children.Clear();
            // By Size
            if (reportBySize == true)
            {
                var dateCheckList = osMatCheckingList.Select(s => s.CheckingDate).Distinct().ToList();
                if (dateCheckList.Count() > 0)
                    dateCheckList = dateCheckList.OrderBy(o => o).ToList();
                foreach (var dateCheck in dateCheckList)
                {
                    var osMatCheckByDate = osMatCheckingList.Where(w => w.CheckingDate == dateCheck).ToList();
                    var sizeNoList = osMatCheckByDate.Select(s => s.SizeNo).Distinct().ToList();
                    StackPanel stkDate = new StackPanel();
                    stkDate.Orientation = Orientation.Horizontal;
                    stkDate.Margin = new Thickness(0, 0, 0, 15);
                    foreach (var sizeNo in sizeNoList)
                    {
                        string sizeNoString = regex.IsMatch(sizeNo) == true ? regex.Replace(sizeNo, "") : sizeNo;
                        double sizeNoDouble = 0;
                        double.TryParse(sizeNoString, out sizeNoDouble);

                        var osMatCheckBySize = osMatCheckByDate.Where(w => w.SizeNo == sizeNo).ToList();

                        DataRow dr = dtBySize.NewRow();
                        dr["Date"] = String.Format("{0:dd/MM/yyyy}", dateCheck);
                        dr["SizeNo"] = sizeNo;
                        dr["SizeNoDouble"] = sizeNoDouble;

                        List<String> displayDataList = new List<String>();
                        var qtyCheckOKList = osMatCheckBySize.Where(w => w.ErrorId == 0).ToList();
                        if (qtyCheckOKList.Count() > 0)
                        {
                            var cartList = qtyCheckOKList.Select(s => s.WorkingCard).Distinct().ToList();
                            if (cartList.Count() > 1)
                                displayDataList.Add(String.Format("Qty OK: {0}", qtyCheckOKList.Sum(s => s.Quantity)));
                            foreach (var cartNo in cartList)
                            {
                                var osMatCheckByCart = osMatCheckBySize.Where(w => w.WorkingCard == cartNo).Sum(s => s.Quantity);
                                displayDataList.Add(String.Format("Carton: Qty\n{0}: {1}", cartNo, osMatCheckByCart));
                            }
                        }

                        var rejectList = osMatCheckBySize.Where(w => w.ErrorId > 0).ToList();
                        var errorList = rejectList.Select(s => s.ErrorId).Distinct().ToList();
                        if (errorList.Count() > 0)
                        {
                            if (errorList.Count() > 1)
                                displayDataList.Add(String.Format("\nReject: {0}", rejectList.Sum(s => s.Reject)));
                            foreach (var errorId in errorList)
                            {
                                var rejectByErrorList = rejectList.Where(w => w.ErrorId == errorId).ToList();
                                var errorName = errorSourceList.FirstOrDefault(f => f.ErrorID == errorId).ErrorName;
                                displayDataList.Add(String.Format("{0}: {1}", errorName, rejectByErrorList.Sum(s => s.Reject)));
                            }
                        }

                        var returnList = osMatCheckBySize.Where(w => w.ErrorId == -1).ToList();
                        if (returnList.Count() > 0)
                        {
                            displayDataList.Add(String.Format("Return: {0}", returnList.Sum(s => s.ReturnReject)));
                        }

                        dr["DisplayValue"] = String.Join(String.Format("{0}", displayDataList.Count() > 1 ? "\n" : ""), displayDataList);
                        dtBySize.Rows.Add(dr);

                        GroupBox grbSize = new GroupBox();
                        var template = FindResource("GroupBoxTemplate") as ControlTemplate;
                        grbSize.Template = template;
                        grbSize.Margin = new Thickness(0, 0, 10, 0);
                        var brTitle = new Border();
                        brTitle.Background = Brushes.Orange;
                        brTitle.Padding = new Thickness(10, 2, 10, 2);
                        var tblSize = new TextBlock();
                        tblSize.Text = String.Format("Size: {0}", sizeNo);
                        brTitle.Child = tblSize;
                        grbSize.Header = brTitle;

                        var stkContent = new StackPanel();
                        stkContent.Orientation = Orientation.Vertical;
                        var tblContent = new TextBlock();
                        tblContent.Text = String.Join("\n", displayDataList);

                        var tblDate = new TextBlock();
                        tblDate.Foreground = Brushes.OrangeRed;
                        tblDate.Text = String.Format("Date: {0:MM/dd/yyyy}", dateCheck);

                        stkContent.Children.Add(tblContent);
                        stkContent.Children.Add(tblDate);

                        grbSize.Content = stkContent;

                        stkDate.Children.Add(grbSize);
                    }

                    stkMain.Children.Add(stkDate);
                }

                ReportDataSource rdsBySize = new ReportDataSource();
                rdsBySize.Name = "OSMatWHCheckDetailBySizeDetail";
                rdsBySize.Value = dtBySize;

                reportViewer.LocalReport.ReportPath = @"Reports\OSMatWHCheckDetailBySizeReport.rdlc";
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(rdsBySize);
                reportViewer.RefreshReport();
            }
            
            // By Cart
            else
            {
                var dateCheckList = osMatCheckingList.Select(s => s.CheckingDate).Distinct().ToList();
                if (dateCheckList.Count() > 0)
                    dateCheckList = dateCheckList.OrderBy(o => o).ToList();
                foreach (var dateCheck in dateCheckList)
                {
                    var osMatCheckByDate = osMatCheckingList.Where(w => w.CheckingDate == dateCheck).ToList();
                    var cartList = osMatCheckByDate.Where(w => w.WorkingCard > 0).Select(s => s.WorkingCard).Distinct().ToList();
                    StackPanel stkDate = new StackPanel();
                    stkDate.Orientation = Orientation.Horizontal;
                    stkDate.Margin = new Thickness(0, 0, 0, 15);
                    if (cartList.Count() > 0)
                        cartList = cartList.OrderBy(o => o).ToList();
                    foreach (var cartNo in cartList)
                    {
                        DataRow dr = dtByCart.NewRow();
                        dr["Date"] = String.Format("{0:dd/MM/yyyy}", dateCheck);
                        dr["Cart"] = cartNo;

                        var osMatByCart = osMatCheckByDate.Where(w => w.WorkingCard == cartNo).ToList();
                        var sizeNoList = osMatByCart.Select(s => s.SizeNo).Distinct().ToList();
                        List<String> displayDataList = new List<String>();
                        if (sizeNoList.Count() > 1)
                            displayDataList.Add(String.Format("Total: {0}\n", osMatByCart.Sum(s => s.Quantity)));
                        displayDataList.Add("SizeNo: Qty");
                        foreach (var sizeNo in sizeNoList)
                        {
                            var osMatBySize = osMatByCart.Where(w => w.SizeNo == sizeNo).ToList();
                            displayDataList.Add(String.Format("{0}#: {1}", sizeNo, osMatBySize.Sum(s => s.Quantity)));
                        }

                        dr["DisplayValue"] = String.Join("\n", displayDataList);
                        dtByCart.Rows.Add(dr);

                        GroupBox grbCarton = new GroupBox();
                        var template = FindResource("GroupBoxTemplate") as ControlTemplate;
                        grbCarton.Template = template;
                        grbCarton.Margin = new Thickness(0, 0, 10, 0);
                        var brTitle = new Border();
                        brTitle.Background = Brushes.DeepSkyBlue;
                        brTitle.Padding = new Thickness(10, 2, 10, 2);
                        var tblCartonNo = new TextBlock();
                        tblCartonNo.Text = String.Format("Carton: {0}", cartNo);
                        brTitle.Child = tblCartonNo;
                        grbCarton.Header = brTitle;

                        var stkContent = new StackPanel();
                        stkContent.Orientation = Orientation.Vertical;
                        var tblContent = new TextBlock();
                        tblContent.Text = String.Join("\n", displayDataList);

                        var tblDate = new TextBlock();
                        tblDate.Foreground = Brushes.OrangeRed;
                        tblDate.Text = String.Format("Date: {0:MM/dd/yyyy}", dateCheck);

                        stkContent.Children.Add(tblContent);
                        stkContent.Children.Add(tblDate);

                        grbCarton.Content = stkContent;

                        stkDate.Children.Add(grbCarton);

                    }

                    stkMain.Children.Add(stkDate);

                    ReportDataSource rdsByCart = new ReportDataSource();
                    rdsByCart.Name = "OSMatWHCheckByCartDetail";
                    rdsByCart.Value = dtByCart;

                    reportViewer.LocalReport.ReportPath = @"Reports\OSMatWHCheckDetailByCartReport.rdlc";
                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.DataSources.Add(rdsByCart);
                    reportViewer.RefreshReport();
                }
            }
        }

        private void radByCart_Checked(object sender, RoutedEventArgs e)
        {
            reportBySize = false;
            if (loaded == true)
                LoadData();
        }
        private void radBySize_Checked(object sender, RoutedEventArgs e)
        {
            reportBySize = true;
            if (loaded == true)
                LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            LoadData();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
