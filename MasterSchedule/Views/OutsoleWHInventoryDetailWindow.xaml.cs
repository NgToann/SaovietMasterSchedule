﻿using MasterSchedule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class OutsoleWHInventoryDetailWindow : Window
    {
        List<String> productNoList;
        List<OrdersModel> orderList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OutsoleOutputModel> outsoleOutputList;
        BackgroundWorker bwLoad;
        bool viewByOutsoleLine;

        public OutsoleWHInventoryDetailWindow(List<String> productNoList, List<OrdersModel> orderList, List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList, List<OutsoleSuppliersModel> outsoleSupplierList, List<OutsoleMaterialModel> outsoleMaterialList, List<AssemblyReleaseModel> assemblyReleaseList, List<OutsoleOutputModel> outsoleOutputList, bool viewByOutsoleLine)
        {
            this.productNoList              = productNoList;
            this.orderList                  = orderList;
            this.outsoleMaterialList        = outsoleMaterialList;
            this.outsoleReleaseMaterialList = outsoleReleaseMaterialList;
            this.outsoleSupplierList        = outsoleSupplierList;
            this.assemblyReleaseList        = assemblyReleaseList;
            this.outsoleOutputList          = outsoleOutputList;
            this.viewByOutsoleLine          = viewByOutsoleLine;

            bwLoad          = new BackgroundWorker();
            bwLoad.DoWork   += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

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

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            int baseColumn = 0;
            Dispatcher.Invoke(new Action(() => {
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTextColumn column1 = new DataGridTextColumn();
                column1.Header = "PO No.";
                column1.Binding = new Binding("ProductNo");
                column1.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1);
                Binding bindingWidth1 = new Binding();
                bindingWidth1.Source = column1;
                bindingWidth1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1 = new ColumnDefinition();
                cd1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1);
                gridTotal.ColumnDefinitions.Add(cd1);

                dt.Columns.Add("ArticleNo", typeof(String));
                DataGridTextColumn column1_1_1 = new DataGridTextColumn();
                column1_1_1.Header = "Article No.";
                column1_1_1.Binding = new Binding("ArticleNo");
                dgInventory.Columns.Add(column1_1_1);
                Binding bindingWidth1_1_1 = new Binding();
                bindingWidth1_1_1.Source = column1_1_1;
                bindingWidth1_1_1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_1_1 = new ColumnDefinition();
                cd1_1_1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_1_1);
                gridTotal.ColumnDefinitions.Add(cd1_1_1);

                dt.Columns.Add("OutsoleCode", typeof(String));
                DataGridTextColumn column_OutsoleCode = new DataGridTextColumn();
                column_OutsoleCode.Header = "O/S Code";
                column_OutsoleCode.Binding = new Binding("OutsoleCode");

                dgInventory.Columns.Add(column_OutsoleCode);
                Binding bindingWidth_OutsoleCode = new Binding();
                bindingWidth_OutsoleCode.Source = column_OutsoleCode;
                bindingWidth_OutsoleCode.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd_OutsoleCode = new ColumnDefinition();
                cd_OutsoleCode.SetBinding(ColumnDefinition.WidthProperty, bindingWidth_OutsoleCode);
                if (viewByOutsoleLine == false)
                {
                    baseColumn = 0;
                    column_OutsoleCode.Visibility = Visibility.Collapsed;
                }
                else
                {
                    baseColumn = 1;
                    column_OutsoleCode.Visibility = Visibility.Visible;
                    gridTotal.ColumnDefinitions.Add(cd_OutsoleCode);
                }

                dt.Columns.Add("ETD", typeof(DateTime));
                DataGridTextColumn column1_1 = new DataGridTextColumn();
                column1_1.Header = "EFD";
                Binding binding = new Binding();
                binding.Path = new PropertyPath("ETD");
                binding.StringFormat = "dd-MMM";
                column1_1.Binding = binding;
                column1_1.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1_1);
                Binding bindingWidth1_1 = new Binding();
                bindingWidth1_1.Source = column1_1;
                bindingWidth1_1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_1 = new ColumnDefinition();
                cd1_1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_1);
                gridTotal.ColumnDefinitions.Add(cd1_1);

                dt.Columns.Add("Quantity", typeof(Int32));
                DataGridTextColumn column1_2 = new DataGridTextColumn();
                column1_2.Header = "Quantity";
                Binding bindingQuantity = new Binding();
                bindingQuantity.Path = new PropertyPath("Quantity");
                column1_2.Binding = bindingQuantity;
                dgInventory.Columns.Add(column1_2);
                Binding bindingWidth1_2 = new Binding();
                bindingWidth1_2.Source = column1_2;
                bindingWidth1_2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_2 = new ColumnDefinition();
                cd1_2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_2);
                gridTotal.ColumnDefinitions.Add(cd1_2);

                dt.Columns.Add("Release", typeof(Int32));
                DataGridTextColumn column1_3 = new DataGridTextColumn();
                column1_3.Header = "Release";
                Binding bindingRelease = new Binding();
                bindingRelease.Path = new PropertyPath("Release");
                column1_3.Binding = bindingRelease;
                dgInventory.Columns.Add(column1_3);
                Binding bindingWidth1_3 = new Binding();
                bindingWidth1_3.Source = column1_3;
                bindingWidth1_3.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_3 = new ColumnDefinition();
                cd1_3.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_3);
                gridTotal.ColumnDefinitions.Add(cd1_3);

                for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                {
                    OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList[i];
                    dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.Header = outsoleSupplier.Name;
                    column.Binding = new Binding(String.Format("Column{0}", i));

                    Style style = new Style(typeof(DataGridCell));
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                    Setter setterForeground = new Setter();
                    setterForeground.Property = DataGridCell.ForegroundProperty;
                    setterForeground.Value = Brushes.Black;
                    style.Setters.Add(setterForeground);

                    Setter setterBackground = new Setter();
                    setterBackground.Property = DataGridCell.BackgroundProperty;
                    setterBackground.Value = new Binding(String.Format("Column{0}Background", i));
                    style.Setters.Add(setterBackground);

                    column.CellStyle = style;

                    dgInventory.Columns.Add(column);

                    Binding bindingWidth = new Binding();
                    bindingWidth.Source = column;
                    bindingWidth.Path = new PropertyPath("ActualWidth");
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                    gridTotal.ColumnDefinitions.Add(cd);

                    DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", i), typeof(SolidColorBrush));
                    columnBackground.DefaultValue = Brushes.Transparent;

                    dt.Columns.Add(columnBackground);
                }

                dt.Columns.Add("Matching", typeof(Int32));
                DataGridTextColumn column2 = new DataGridTextColumn();
                column2.Header = "Matching";
                column2.Binding = new Binding("Matching");
                dgInventory.Columns.Add(column2);
                Binding bindingWidth2 = new Binding();
                bindingWidth2.Source = column2;
                bindingWidth2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd2 = new ColumnDefinition();
                cd2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth2);
                gridTotal.ColumnDefinitions.Add(cd2);

                dt.Columns.Add("FinishedOutsole", typeof(Int32));
                DataGridTextColumn columnFinishedOutsole = new DataGridTextColumn();
                columnFinishedOutsole.Header = "Finished Outsole";
                columnFinishedOutsole.Binding = new Binding("FinishedOutsole");
                dgInventory.Columns.Add(columnFinishedOutsole);
                Binding bindingWith3 = new Binding();
                bindingWith3.Source = columnFinishedOutsole;
                bindingWith3.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd3 = new ColumnDefinition();
                cd3.SetBinding(ColumnDefinition.WidthProperty, bindingWith3);
                gridTotal.ColumnDefinitions.Add(cd3);

                foreach (string productNo in productNoList)
                {
                    var outsoleMaterialListByPO = outsoleMaterialList.Where(w => w.ProductNo == productNo).ToList();
                    var outsoleOutputListByPO = outsoleOutputList.Where(w => w.ProductNo == productNo).ToList();
                    var assemblyReleaseListByPO = assemblyReleaseList.Where(w => w.ProductNo == productNo).ToList();
                    var outsoleReleaseListByPO = outsoleReleaseMaterialList.Where(w => w.ProductNo == productNo).ToList();

                    DataRow dr = dt.NewRow();
                    dr["ProductNo"] = productNo;
                    OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();

                    if (order != null)
                    {
                        dr["ETD"] = order.ETD;
                        dr["ArticleNo"] = order.ArticleNo;
                        dr["Quantity"] = order.Quantity;
                        dr["OutsoleCode"] = order.OutsoleCode;
                    }
                    List<String> sizeNoList = outsoleMaterialListByPO.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    int qtyMaterialTotalToCheck = 0;
                    for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                    {
                        var supplier = outsoleSupplierList[i];
                        var outsoleMaterialListBySupp = outsoleMaterialListByPO.Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                        if (outsoleMaterialListBySupp.Count() == 0)
                            continue;

                        int qtyMaterialTotal = 0;
                        int qtyReleaseTotal = 0;
                        foreach (string sizeNo in sizeNoList)
                        {
                            int qtyDelivery = outsoleMaterialListBySupp.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity - s.QuantityReject);
                            int qtyRelease = outsoleReleaseListByPO.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);

                            int qtyMaterial = qtyDelivery - qtyRelease;
                            if (qtyMaterial < 0)
                            {
                                qtyMaterial = 0;
                            }
                            qtyMaterialTotal += qtyMaterial;
                            qtyMaterialTotalToCheck += qtyMaterial;
                            qtyReleaseTotal += qtyRelease;
                        }
                        dr["Release"] = qtyReleaseTotal;
                        dr[String.Format("Column{0}", i)] = qtyMaterialTotal;
                    }
                    int qtyMatchingTotal = 0;
                    foreach (string sizeNo in sizeNoList)
                    {
                        //int qtyMin = outsoleMaterialList_D1.Where(o => o.SizeNo == sizeNo).Select(o => (o.Quantity - o.QuantityReject)).Min();
                        int qtyMin = outsoleMaterialListByPO.Where(o => o.SizeNo == sizeNo).Select(o => (o.Quantity - o.QuantityReject)).Min();
                        int qtyRelease = outsoleReleaseListByPO.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                        int qtyMatching = qtyMin - qtyRelease;
                        if (qtyMatching < 0)
                            qtyMatching = 0;
                        qtyMatchingTotal += qtyMatching;
                    }
                    dr["Matching"] = qtyMatchingTotal;

                    int osOutput = outsoleOutputListByPO.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity);
                    int assRelease = assemblyReleaseListByPO.Sum(s => s.Quantity);
                    int finishedOutsole = osOutput - assRelease;
                    dr["FinishedOutsole"] = finishedOutsole > 0 ? finishedOutsole : 0;

                    if (finishedOutsole == 0 && qtyMatchingTotal == 0 && qtyMaterialTotalToCheck == 0)
                        continue;
                    dt.Rows.Add(dr);
                }

                TextBlock lblTotal = new TextBlock();
                lblTotal.Text = "TOTAL";
                lblTotal.Margin = new Thickness(1, 0, 0, 0);
                lblTotal.FontWeight = FontWeights.Bold;
                Border bdrTotal = new Border();
                Grid.SetColumn(bdrTotal, 2);
                Grid.SetColumnSpan(bdrTotal, baseColumn + 4);
                bdrTotal.BorderThickness = new Thickness(1, 0, 1, 1);
                bdrTotal.BorderBrush = Brushes.Black;
                bdrTotal.Child = lblTotal;
                gridTotal.Children.Add(bdrTotal);

                TextBlock lblQuantityTotal = new TextBlock();
                lblQuantityTotal.Text = dt.Compute("Sum(Quantity)", "").ToString();
                lblQuantityTotal.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityTotal.FontWeight = FontWeights.Bold;
                Border bdrQuantityTotal = new Border();
                Grid.SetColumn(bdrQuantityTotal, baseColumn + 5);
                bdrQuantityTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityTotal.BorderBrush = Brushes.Black;
                bdrQuantityTotal.Child = lblQuantityTotal;
                gridTotal.Children.Add(bdrQuantityTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblReleaseTotal = new TextBlock();
                lblReleaseTotal.Text = dt.Compute("Sum(Release)", "").ToString();
                lblReleaseTotal.Margin = new Thickness(1, 0, 0, 0);
                lblReleaseTotal.FontWeight = FontWeights.Bold;
                Border bdrReleaseTotal = new Border();
                Grid.SetColumn(bdrReleaseTotal, baseColumn + 6);
                bdrReleaseTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrReleaseTotal.BorderBrush = Brushes.Black;
                bdrReleaseTotal.Child = lblReleaseTotal;
                gridTotal.Children.Add(bdrReleaseTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                {
                    TextBlock lblSupplierTotal = new TextBlock();
                    lblSupplierTotal.Text = dt.Compute(String.Format("Sum(Column{0})", i), "").ToString();
                    lblSupplierTotal.Margin = new Thickness(1, 0, 0, 0);
                    lblSupplierTotal.FontWeight = FontWeights.Bold;
                    Border bdrSupplierTotal = new Border();
                    Grid.SetColumn(bdrSupplierTotal, baseColumn + 7 + i);
                    bdrSupplierTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                    bdrSupplierTotal.BorderBrush = Brushes.Black;
                    bdrSupplierTotal.Child = lblSupplierTotal;
                    gridTotal.Children.Add(bdrSupplierTotal);
                }

                TextBlock lblMatchingTotal = new TextBlock();
                lblMatchingTotal.Text = dt.Compute("Sum(Matching)", "").ToString();
                lblMatchingTotal.Margin = new Thickness(1, 0, 0, 0);
                lblMatchingTotal.FontWeight = FontWeights.Bold;
                Border brMT = new Border();
                Grid.SetColumn(brMT, baseColumn + 7 + outsoleSupplierList.Count());
                brMT.BorderThickness = new Thickness(0, 0, 1, 1);
                brMT.BorderBrush = Brushes.Black;
                brMT.Child = lblMatchingTotal;
                gridTotal.Children.Add(brMT);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblFinishedOutsoleTotal = new TextBlock();
                lblFinishedOutsoleTotal.Text = dt.Compute("Sum(FinishedOutsole)", "").ToString();
                lblFinishedOutsoleTotal.Margin = new Thickness(1, 0, 0, 0);
                lblFinishedOutsoleTotal.FontWeight = FontWeights.Bold;
                Border brFOT = new Border();
                Grid.SetColumn(brFOT, baseColumn + 8 + outsoleSupplierList.Count());
                brFOT.BorderThickness = new Thickness(0, 0, 1, 1);
                brFOT.BorderBrush = Brushes.Black;
                brFOT.Child = lblFinishedOutsoleTotal;
                gridTotal.Children.Add(brFOT);
                dgInventory.ItemsSource = dt.AsDataView();
            }));
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }
    }
}
