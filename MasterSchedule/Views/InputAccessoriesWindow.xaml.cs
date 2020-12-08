using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Helpers;
using MasterSchedule.Controllers;
using System.Data;
using System.Globalization;
//using MasterSchedule.Helpers;
using System.Windows.Media;
using System.Data.Metadata.Edm;
using System.Configuration;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputAccessoriesWindow.xaml
    /// </summary>
    public partial class InputAccessoriesWindow : Window
    {
        BackgroundWorker bwLoad;
        private string productNo;
        private List<RejectModel> rejectUpperAccessoriesList;
        List<MaterialPlanModel> materialPlanList;
        List<SupplierModel> supplierAccessoriesList;
        List<SizeRunModel> sizeRunList;
        Button btnEditMatsPlan;
        List<MaterialDeliveryModel> matsDeliveryList;
        SupplierModel supplierClicked;
        DataTable dtDelDetail;
        public InputAccessoriesWindow(string productNo, List<RejectModel> rejectUpperAccessoriesList)
        {
            this.productNo = productNo;
            this.rejectUpperAccessoriesList = rejectUpperAccessoriesList;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            materialPlanList = new List<MaterialPlanModel>();
            supplierAccessoriesList = new List<SupplierModel>();
            sizeRunList = new List<SizeRunModel>();

            btnEditMatsPlan = new Button();
            matsDeliveryList = new List<MaterialDeliveryModel>();
            dtDelDetail = new DataTable();

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
            materialPlanList = MaterialPlanController.GetMaterialPlanByPO(productNo);
            supplierAccessoriesList = SupplierController.GetSuppliersAccessories();
            sizeRunList = SizeRunController.Select(productNo);

            matsDeliveryList = MaterialDeliveryController.GetMaterialDeliveryByPO(productNo);
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dpDeliveryDate.SelectedDate = DateTime.Now;
            LoadMaterialPlan(materialPlanList);
            LoadListOfDefects(rejectUpperAccessoriesList);
            LoadDeliveryDetail(matsDeliveryList);
            tblDeliveryDetailOf.Text = String.Format("Delivery detail of: {0}", productNo);
            dgDeliveryDetail.IsReadOnly = true;
            this.Cursor = null;
        }

        private void LoadMaterialPlan(List<MaterialPlanModel> matsPlanList)
        {
            dgAccessoriesInfor.ItemsSource = null;
            dgAccessoriesInfor.ItemsSource = matsPlanList;
            dgAccessoriesInfor.Items.Refresh();
        }

        private void LoadListOfDefects(List<RejectModel> rejectUpperAccessoriesList)
        {
            // binding to error to grid
            int countColumn = gridError.ColumnDefinitions.Count();
            int countRow = countRow = rejectUpperAccessoriesList.Count / countColumn;
            if (rejectUpperAccessoriesList.Count % countColumn != 0)
            {
                countRow = rejectUpperAccessoriesList.Count / countColumn + 1;
            }
            gridError.RowDefinitions.Clear();
            for (int i = 1; i <= countRow; i++)
            {
                RowDefinition rd = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star),
                };
                gridError.RowDefinitions.Add(rd);
            }

            for (int i = 0; i <= rejectUpperAccessoriesList.Count() - 1; i++)
            {
                Button button = new Button();
                var template = FindResource("ButtonDefectTemplate") as ControlTemplate;
                button.Template = template;
                button.Margin = new Thickness(4, 4, 0, 0);
                button.MaxHeight = 68;

                Border br = new Border();
                br.Name = string.Format("border{0}", rejectUpperAccessoriesList[i].RejectKey);

                Grid.SetColumn(button, i % countColumn);
                Grid.SetRow(button, i / countColumn);

                Grid grid = new Grid();
                ColumnDefinition cld1 = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Auto),
                };
                ColumnDefinition cld2 = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star),
                };
                grid.ColumnDefinitions.Add(cld1);
                grid.ColumnDefinitions.Add(cld2);

                TextBlock txtKey = new TextBlock();
                txtKey.Text = rejectUpperAccessoriesList[i].RejectKey;
                txtKey.FontSize = 28;
                txtKey.Foreground = Brushes.Blue;
                txtKey.VerticalAlignment = VerticalAlignment.Center;
                txtKey.Margin = new Thickness(4, 0, 4, 0);
                Grid.SetColumn(txtKey, 0);

                TextBlock txtErrorName = new TextBlock();
                txtErrorName.Text = string.Format("{0}\n{1}", rejectUpperAccessoriesList[i].RejectName, rejectUpperAccessoriesList[i].RejectName_1);
                txtErrorName.FontSize = 15;
                txtErrorName.TextWrapping = TextWrapping.Wrap;
                txtErrorName.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(txtErrorName, 1);

                grid.Children.Add(txtKey);
                grid.Children.Add(txtErrorName);

                br.Child = grid;
                button.Content = br;

                gridError.Children.Add(button);
            }
        }

        private void dgDeliveryDetail_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dgAccessoriesInfor_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btnAddNewAccessory_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddMaterialPlanForProductNoWindow(productNo, supplierAccessoriesList, null, materialPlanList);
            window.ShowDialog();
            if (window.materialUpdate != null)
            {
                materialPlanList.RemoveAll(r => r.ProductNo == window.materialUpdate.ProductNo && r.SupplierId == window.materialUpdate.SupplierId);
                materialPlanList.Add(window.materialUpdate);
                LoadMaterialPlan(materialPlanList);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnEditMatsPlan = sender as Button;
            var rowClicked = dgAccessoriesInfor.CurrentItem as MaterialPlanModel;
            if (rowClicked == null)
                return;
            btnEditMatsPlan.IsEnabled = false;
            var window = new AddMaterialPlanForProductNoWindow(productNo, supplierAccessoriesList, rowClicked, materialPlanList);
            window.ShowDialog();
            if (window.materialUpdate != null)
            {
                var indexOf = materialPlanList.IndexOf(rowClicked);
                try
                {
                    if (window.runModeRespone == EExcute.Update)
                    {
                        materialPlanList.RemoveAt(indexOf);
                        materialPlanList.Insert(indexOf, window.materialUpdate);
                    }
                    else if (window.runModeRespone == EExcute.Delete)
                    {
                        materialPlanList.RemoveAt(indexOf);
                    }
                    LoadMaterialPlan(materialPlanList);
                }
                catch { }
            }
            btnEditMatsPlan.IsEnabled = true;
        }

        private void dgAccessoriesInfor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var cellClicked = dgAccessoriesInfor.CurrentCell;
            var rowClicked = dgAccessoriesInfor.CurrentItem as MaterialPlanModel;
            if (cellClicked == null || rowClicked == null)
                return;
            if (!cellClicked.Column.Equals(colSupplier))
                return;

            // Load Delivery Infor
            var deliveryDetailBySupplierClicked = matsDeliveryList.Where(w => w.SupplierId == rowClicked.SupplierId).ToList();
            var deliveryThisTimeList = new List<MaterialDeliveryModel>();
            if (deliveryDetailBySupplierClicked.Where(w => w.DeliveryDate.Equals(dpDeliveryDate.SelectedDate.Value)).Count() == 0)
            {
                foreach (var sizeRun in sizeRunList)
                {
                    deliveryThisTimeList.Add(
                        new MaterialDeliveryModel
                        {
                            ProductNo = productNo,
                            SupplierId = rowClicked.SupplierId,
                            DeliveryDate = dpDeliveryDate.SelectedDate.Value,
                            SizeNo = sizeRun.SizeNo,
                            Quantity = 0,
                            Reject = 0,
                            RejectId = 0,
                            Reviser = "new"
                        });
                }
            }
            deliveryDetailBySupplierClicked.AddRange(deliveryThisTimeList);

            LoadDeliveryDetail(deliveryDetailBySupplierClicked);
            supplierClicked = supplierAccessoriesList.FirstOrDefault(f => f.SupplierId.Equals(rowClicked.SupplierId));
            tblDeliveryDetailOf.Text = String.Format("Delivery detail of: {0}", supplierClicked.Name);
            dgDeliveryDetail.IsReadOnly = false;
        }

        private void LoadDeliveryDetail(List<MaterialDeliveryModel> deliveryList)
        {
            dgDeliveryDetail.Columns.Clear();
            dtDelDetail = new DataTable();

            //Column Supplier name
            dtDelDetail.Columns.Add("Name", typeof(String));
            dtDelDetail.Columns.Add("SupplierId", typeof(String));
            DataGridTemplateColumn colSuppName = new DataGridTemplateColumn();
            colSuppName.Header = String.Format("Supplier");
            DataTemplate templateSuppName = new DataTemplate();
            FrameworkElementFactory tblSuppName = new FrameworkElementFactory(typeof(TextBlock));
            templateSuppName.VisualTree = tblSuppName;
            tblSuppName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Name")));
            tblSuppName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblSuppName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colSuppName.CellTemplate = templateSuppName;
            colSuppName.ClipboardContentBinding = new Binding(String.Format("Name"));
            dgDeliveryDetail.Columns.Add(colSuppName);

            //Column Delivery Date
            dtDelDetail.Columns.Add("DeliveryDate", typeof(String));
            DataGridTemplateColumn colDelDate = new DataGridTemplateColumn();
            colDelDate.Header = String.Format("Delivery\nDate");
            DataTemplate templateDelDate = new DataTemplate();
            FrameworkElementFactory tblDelDate = new FrameworkElementFactory(typeof(TextBlock));
            templateDelDate.VisualTree = tblDelDate;
            tblDelDate.SetBinding(TextBlock.TextProperty, new Binding(String.Format("DeliveryDate")));
            tblDelDate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblDelDate.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colDelDate.CellTemplate = templateDelDate;
            colDelDate.ClipboardContentBinding = new Binding(String.Format("DeliveryDate"));
            dgDeliveryDetail.Columns.Add(colDelDate);

            //dtDelDetail.Columns.Add("DeliveryDate", typeof(DateTime));
            //DataGridTemplateColumn colDelDate = new DataGridTemplateColumn();
            //colDelDate.Header = String.Format("Delivery Date");
            //DataTemplate templateDelDate = new DataTemplate();
            //FrameworkElementFactory datePickerDelDate = new FrameworkElementFactory(typeof(DatePicker));
            //templateDelDate.VisualTree = datePickerDelDate;
            //datePickerDelDate.SetBinding(DatePicker.SelectedDateProperty, new Binding(String.Format("DeliveryDate")));
            //datePickerDelDate.SetValue(DatePicker.VerticalContentAlignmentProperty, VerticalAlignment.Center);
            //datePickerDelDate.SetValue(DatePicker.PaddingProperty, new Thickness(3, 0, 0, 0));
            //datePickerDelDate.SetValue(DatePicker.FontWeightProperty, FontWeights.Regular);
            //colDelDate.CellTemplate = templateDelDate;
            //colDelDate.ClipboardContentBinding = new Binding(String.Format("DeliveryDate"));
            //dgDeliveryDetail.Columns.Add(colDelDate);

            dtDelDetail.Columns.Add("Title", typeof(String));
            DataGridTemplateColumn colTitle = new DataGridTemplateColumn();
            colTitle.Header = String.Format("");
            DataTemplate templateTitle = new DataTemplate();
            FrameworkElementFactory tblTitle = new FrameworkElementFactory(typeof(TextBlock));
            templateTitle.VisualTree = tblTitle;
            tblTitle.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Title")));
            tblTitle.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblTitle.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colTitle.CellTemplate = templateTitle;
            colTitle.ClipboardContentBinding = new Binding(String.Format("Title"));
            dgDeliveryDetail.Columns.Add(colTitle);

            var regex = new Regex("[a-z]|[A-Z]");
            if (sizeRunList.Count() > 0)
                sizeRunList = sizeRunList.OrderBy(s => regex.IsMatch(s.SizeNo) ? Double.Parse(regex.Replace(s.SizeNo, "100")) : Double.Parse(s.SizeNo)).ToList();
            foreach (var sizeRun in sizeRunList)
            {
                string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                dtDelDetail.Columns.Add(String.Format("Column{0}", sizeBinding), typeof(String));
                dtDelDetail.Columns.Add(String.Format("Column{0}Foreground", sizeBinding), typeof(SolidColorBrush));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);

                StackPanel stkSizeFull = new StackPanel();
                stkSizeFull.Orientation = Orientation.Vertical;

                StackPanel stkSize = new StackPanel();
                stkSize.Orientation = Orientation.Horizontal;
                var tblHeaderSize = new TextBlock();
                tblHeaderSize.Text = string.Format("{0}", sizeRun.SizeNo);
                var chkHeaderSize = new CheckBox();
                chkHeaderSize.Margin = new Thickness(2, 0, 0, 0);
                chkHeaderSize.Cursor = Cursors.Hand;
                chkHeaderSize.VerticalAlignment = VerticalAlignment.Center;
                stkSize.Children.Add(tblHeaderSize);
                stkSize.Children.Add(chkHeaderSize);

                var tblHeaderQty = new TextBlock();
                tblHeaderQty.HorizontalAlignment = HorizontalAlignment.Center;
                tblHeaderQty.Text = string.Format("{0}", sizeRun.Quantity);

                stkSizeFull.Children.Add(stkSize);
                stkSizeFull.Children.Add(tblHeaderQty);

                column.Header = stkSizeFull;

                //column.Header = string.Format("{0}\n{1}", sizeRun.SizeNo, sizeRun.Quantity);
                column.MinWidth = 45;
                column.MaxWidth = 200;
                column.Binding = new Binding(String.Format("Column{0}", sizeBinding));

                Style styleColumn = new Style();
                Setter setterColumnForecolor = new Setter();
                setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", sizeBinding));
                styleColumn.Setters.Add(setterColumnForecolor);
                column.CellStyle = styleColumn;

                dgDeliveryDetail.Columns.Add(column);
            }

            // Column Total
            dtDelDetail.Columns.Add("Total", typeof(String));
            DataGridTemplateColumn colTotal = new DataGridTemplateColumn();
            colTotal.Header = String.Format("Total");
            DataTemplate templateTotal = new DataTemplate();
            FrameworkElementFactory tblTotal = new FrameworkElementFactory(typeof(TextBlock));
            templateTotal.VisualTree = tblTotal;
            tblTotal.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Total")));
            tblTotal.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblTotal.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            tblTotal.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colTotal.CellTemplate = templateTotal;
            colTotal.ClipboardContentBinding = new Binding(String.Format("Total"));
            dgDeliveryDetail.Columns.Add(colTotal);

            var supplierList = deliveryList.Select(s => s.SupplierId).Distinct().ToList();
            
            foreach (var supplierId in supplierList)
            {
                var deliveryBySuppList = deliveryList.Where(w => w.SupplierId == supplierId).ToList();
                var dateList = deliveryBySuppList.Select(s => s.DeliveryDate).Distinct().ToList();
                if (dateList.Count() > 0)
                    dateList = dateList.OrderBy(o => o).ToList();

                bool addSupp = false;
                foreach (var date in dateList)
                {
                    DataRow dr = dtDelDetail.NewRow();
                    DataRow drReject = dtDelDetail.NewRow();

                    if (addSupp == false)
                    {
                        dr["Name"] = supplierAccessoriesList.FirstOrDefault(f => f.SupplierId == supplierId).Name;
                        addSupp = true;
                    }
                    dr["SupplierId"] = supplierId;
                    drReject["SupplierId"] = supplierId;

                    dr["DeliveryDate"] = string.Format("{0:MM/dd/yyyy}", date);

                    dr["Title"] = "Quantity";
                    drReject["Title"] = "Reject";

                    var deliveryByDateList = deliveryBySuppList.Where(w => w.DeliveryDate.Equals(date)).ToList();
                    foreach (var sizeRun in sizeRunList)
                    {
                        var deliveryBySize = deliveryByDateList.Where(w => w.SizeNo == sizeRun.SizeNo).ToList();
                        string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                        if (deliveryBySize.Sum(s => s.Quantity) > 0)
                            dr[String.Format("Column{0}", sizeBinding)] = deliveryBySize.Sum(s => s.Quantity).ToString();

                        drReject[String.Format("Column{0}", sizeBinding)] = "";
                        drReject[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                    }

                    dtDelDetail.Rows.Add(dr);
                    dtDelDetail.Rows.Add(drReject);
                }
            }

            dgDeliveryDetail.ItemsSource = dtDelDetail.AsDataView();
            dgDeliveryDetail.Items.Refresh();
        }

        private void dgDeliveryDetail_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var rowClicked = (DataRowView)e.Row.Item;
            // Not Allow Input Reject
            if (rowClicked["Title"].ToString().Equals("Reject"))
            {
                e.Cancel = true;
            }
            if (e.Column.GetValue(TagProperty) == null)
                return;
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }

            var window = new AddRejectForMaterialWindow(rejectUpperAccessoriesList);
            window.ShowDialog();
        }

        private void dgDeliveryDetail_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var rowEditting = (DataRowView)e.Row.Item;
            if (e.Column.GetValue(TagProperty) == null)
                return;
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            int qtyInput = 0;

            TextBox txtCurrent = (TextBox)e.EditingElement;
            Int32.TryParse(txtCurrent.Text.Trim().ToString(), out qtyInput);

            for (int r = 0; r < dtDelDetail.Rows.Count; r++)
            {
                DataRow dr = dtDelDetail.Rows[r];
                if (!dr["Title"].ToString().Equals("Quantity"))
                    continue;
                if (!dr["DeliveryDate"].ToString().Equals(rowEditting["DeliveryDate"]))
                    continue;
                foreach (var sizeRun in sizeRunList)
                {
                    string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                    if (!sizeRun.SizeNo.Equals(sizeNo))
                        continue;

                    if (qtyInput <= 0)
                    {
                        dr[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                        dr[String.Format("Column{0}", sizeBinding)] = "0";
                    }
                    else if (qtyInput >= qtyOrder)
                    {
                        dr[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
                        dr[String.Format("Column{0}", sizeBinding)] = qtyOrder.ToString();
                    }
                    else
                    {
                        dr[String.Format("Column{0}", sizeBinding)] = qtyInput;
                        dr[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Black;
                    }
                }

                // Update Total Cell
                int totalDelivery = 0;
                foreach (var sizeRun in sizeRunList)
                {
                    string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                    var x = dr[String.Format("Column{0}", sizeBinding)].ToString();
                    int qty = 0;
                    Int32.TryParse(x, out qty);
                    totalDelivery += qty;
                    dr["Total"] = totalDelivery.ToString();
                }
            }
        }

        private void dpDeliveryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (supplierClicked != null)
            {
                // Load Delivery Infor
                var deliveryDetailBySupplierClicked = matsDeliveryList.Where(w => w.SupplierId == supplierClicked.SupplierId).ToList();
                var deliveryThisTimeList = new List<MaterialDeliveryModel>();
                if (deliveryDetailBySupplierClicked.Where(w => w.DeliveryDate.Equals(dpDeliveryDate.SelectedDate.Value)).Count() == 0)
                {
                    foreach (var sizeRun in sizeRunList)
                    {
                        deliveryThisTimeList.Add(
                            new MaterialDeliveryModel
                            {
                                ProductNo = productNo,
                                SupplierId = supplierClicked.SupplierId,
                                DeliveryDate = dpDeliveryDate.SelectedDate.Value,
                                SizeNo = sizeRun.SizeNo,
                                Quantity = 0,
                                Reject = 0,
                                RejectId = 0,
                                Reviser = "new"
                            });
                    }
                }
                deliveryDetailBySupplierClicked.AddRange(deliveryThisTimeList);
                LoadDeliveryDetail(deliveryDetailBySupplierClicked);
            }
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // Binding Error Keys.
        }
    }
}
