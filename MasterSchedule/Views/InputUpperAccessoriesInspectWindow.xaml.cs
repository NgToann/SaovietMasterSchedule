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
using MasterSchedule.Models;
using MasterSchedule.ViewModels;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputAccessoriesWindow.xaml
    /// </summary>
    public partial class InputUpperAccessoriesInspectWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwUpload;
        private string productNo;
        private List<RejectModel> rejectUpperAccessoriesList;
        public List<MaterialPlanModel> materialPlanList;
        MaterialPlanModel materialPlanChecking;
        List<SupplierModel> supplierAccessoriesList;
        List<SizeRunModel> sizeRunList;
        Button btnEditMatsPlan;
        List<MaterialInspectModel> matsInspectionList;
        List<MaterialDeliveryModel> matsDeliveryList;
        SupplierModel supplierClicked;
        DataTable dtInspectionDetail;
        List<String> buttonSizeKeyList;
        private EExcute eAction = EExcute.None;
        private AccountModel account;
        string[] keysTemp = new string[] {  "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
                                            "11", "12", "13", "14", "15", "16", "17", "18", "19" };
        private string _qtyOK = "Quantity OK";
        private DateTime dtDefault = new DateTime(2000, 01, 01);
        public InputUpperAccessoriesInspectWindow(string productNo, List<RejectModel> rejectUpperAccessoriesList, AccountModel account)
        {
            this.productNo = productNo;
            this.rejectUpperAccessoriesList = rejectUpperAccessoriesList;
            this.account = account;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            bwUpload = new BackgroundWorker();
            bwUpload.DoWork += BwUpload_DoWork;
            bwUpload.RunWorkerCompleted += BwUpload_RunWorkerCompleted;

            materialPlanList = new List<MaterialPlanModel>();
            matsDeliveryList = new List<MaterialDeliveryModel>();
            supplierAccessoriesList = new List<SupplierModel>();
            sizeRunList = new List<SizeRunModel>();

            btnEditMatsPlan = new Button();
            matsInspectionList = new List<MaterialInspectModel>();
            dtInspectionDetail = new DataTable();
            buttonSizeKeyList = new List<string>();
            InitializeComponent();
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.Title + " - " + account.FullName;
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            materialPlanList = MaterialPlanController.GetMaterialPlanByPO(productNo);
            materialPlanList.ForEach(t => t.ActualDateString = t.ActualDate != dtDefault ? String.Format("{0:MM/dd}", t.ActualDate) : "");
            supplierAccessoriesList = SupplierController.GetSuppliersAccessories();
            sizeRunList = SizeRunController.Select(productNo);
            matsInspectionList  = MaterialInspectController.GetMaterialInspectByPO(productNo);
            matsDeliveryList    = MaterialDeliveryController.GetMaterialDeliveryByPO(productNo);
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dpDeliveryDate.SelectedDate = DateTime.Now;
            LoadMaterialPlan(materialPlanList);
            LoadListOfSizeNo(sizeRunList);
            LoadInspectionDetail(matsInspectionList);
            tblDeliveryDetailOf.Text = String.Format("Inspect detail of: {0}", productNo);
            dgDeliveryDetail.IsReadOnly = true;

            this.Cursor = null;
        }

        private void LoadMaterialPlan(List<MaterialPlanModel> matsPlanList)
        {
            dgAccessoriesInfor.ItemsSource = null;
            dgAccessoriesInfor.ItemsSource = matsPlanList;
            dgAccessoriesInfor.Items.Refresh();
        }

        private void LoadListOfSizeNo(List<SizeRunModel> sizeRunList)
        {
            // binding to error to grid
            int countColumn = gridError.ColumnDefinitions.Count();
            int countRow = countRow = sizeRunList.Count / countColumn;
            if (sizeRunList.Count % countColumn != 0)
            {
                countRow = sizeRunList.Count / countColumn + 1;
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

            //for (int i = 0; i <= sizeRunList.Count() - 1; i++)

            for (int i = 0; i <= sizeRunList.Count() - 1; i++)
            {
                var sizeBinding = sizeRunList[i].SizeNo.Contains(".") ? sizeRunList[i].SizeNo.Replace(".", "") : sizeRunList[i].SizeNo;
                Button btnSizeNo = new Button();
                var template = FindResource("ButtonSizeNoTemplate") as ControlTemplate;
                btnSizeNo.Template = template;
                btnSizeNo.Margin = new Thickness(4, 4, 0, 0);
                if (i / countColumn == 0)
                {
                    if (i != 0)
                        btnSizeNo.Margin = new Thickness(4, 0, 0, 0);
                    else
                        btnSizeNo.Margin = new Thickness(0, 0, 0, 0);
                }
                if (i % countColumn == 0 && i / countColumn != 0)
                    btnSizeNo.Margin = new Thickness(0, 4, 0, 0);
                btnSizeNo.MaxHeight = 50;
                btnSizeNo.Tag = sizeRunList[i];
                btnSizeNo.Name = string.Format("button{0}", sizeBinding);
                btnSizeNo.Click += BtnSizeNo_Click;

                Border br = new Border();

                Grid.SetColumn(btnSizeNo, i % countColumn);
                Grid.SetRow(btnSizeNo, i / countColumn);

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

                Border brKey = new Border();
                brKey.Background = Brushes.LightGray;
                brKey.Padding = new Thickness(3, 3, 3, 3);
                brKey.CornerRadius = new CornerRadius(3, 0, 0, 3);
                brKey.MinWidth = 50;
                TextBlock tbltSizeNoKey= new TextBlock();

                var keyDisplay = "";
                if (keysTemp.Count() > sizeRunList.Count())
                    keyDisplay = keysTemp[i].ToString();

                tbltSizeNoKey.Text = keyDisplay;
                tbltSizeNoKey.Tag = sizeRunList[i];
                tbltSizeNoKey.FontSize = 25;
                tbltSizeNoKey.Foreground = Brushes.Blue;
                tbltSizeNoKey.VerticalAlignment = VerticalAlignment.Center;
                tbltSizeNoKey.HorizontalAlignment = HorizontalAlignment.Center;
                brKey.Child = tbltSizeNoKey;
                Grid.SetColumn(brKey, 0);

                TextBlock tblSizeNoName = new TextBlock();
                tblSizeNoName.Text = string.Format("#{0}", sizeRunList[i].SizeNo);
                tblSizeNoName.FontSize = 30;
                tblSizeNoName.Margin = new Thickness(5, 0, 0, 0);
                tblSizeNoName.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(tblSizeNoName, 1);

                grid.Children.Add(brKey);
                grid.Children.Add(tblSizeNoName);

                br.Child = grid;
                btnSizeNo.Content = br;

                gridError.Children.Add(btnSizeNo);
            }
        }

        private void BtnSizeNo_Click(object sender, RoutedEventArgs e)
        {
            if (supplierClicked == null || materialPlanChecking == null)
                return;

            var buttonClicked = sender as Button;
            var sizeClicked = buttonClicked.Tag as SizeRunModel;
            var sizeBinding = sizeClicked.SizeNo.Contains(".") ? sizeClicked.SizeNo.Replace(".", "") : sizeClicked.SizeNo;
            HighLightError(sizeBinding);
        }

        private void HighLightError(string rejectKey)
        {
            try
            {
                var childrenCount = VisualTreeHelper.GetChildrenCount(gridError);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(gridError, i);
                    if (child != null)
                    {
                        Button buttonClicked = child as Button;
                        var template = FindResource("ButtonSizeNoTemplate") as ControlTemplate;
                        var templateClicked = FindResource("ButtonSizeNoClickedTemplate") as ControlTemplate;
                        buttonClicked.Template = template;
                        if (buttonClicked.Name.Equals(String.Format("button{0}", rejectKey)))
                            buttonClicked.Template = templateClicked;
                    }
                }
            }
            catch { }
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
            if (!account.MaterialPlan)
                return;
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
            if (!account.MaterialPlan)
                return;
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
                        foreach (var matsDelivery in matsInspectionList)
                        {
                            if (matsDelivery.SupplierId.Equals(rowClicked.SupplierId))
                                matsDelivery.SupplierId = window.materialUpdate.SupplierId;
                        }
                    }
                    else if (window.runModeRespone == EExcute.Delete)
                    {
                        materialPlanList.RemoveAt(indexOf);
                        matsInspectionList.RemoveAll(r => r.SupplierId.Equals(rowClicked.SupplierId));
                    }
                    LoadMaterialPlan(materialPlanList);
                    LoadInspectionDetail(matsInspectionList);
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

            supplierClicked = supplierAccessoriesList.FirstOrDefault(f => f.SupplierId.Equals(rowClicked.SupplierId));
            materialPlanChecking = rowClicked;

            if (string.IsNullOrEmpty(workerId) && account.MaterialDelivery)
            {
                popInputWorkerId.IsOpen = true;
                txtWorkerId.Focus();
                txtWorkerId.SelectAll();
            }
            else
            {
                LoadSupplierClicked();
            }
        }

        private void LoadSupplierClicked()
        {
            // Load Delivery Infor
            var deliveryDetailBySupplierClicked = matsInspectionList.Where(w => w.SupplierId == materialPlanChecking.SupplierId).ToList();
            var deliveryThisTimeList = new List<MaterialInspectModel>();
            if (deliveryDetailBySupplierClicked.Where(w => w.DeliveryDate.Equals(dpDeliveryDate.SelectedDate.Value.Date)).Count() == 0
                && account.MaterialDelivery)
            {
                foreach (var sizeRun in sizeRunList)
                {
                    deliveryThisTimeList.Add(
                        new MaterialInspectModel
                        {
                            ProductNo = productNo,
                            SupplierId = materialPlanChecking.SupplierId,
                            DeliveryDate = dpDeliveryDate.SelectedDate.Value,
                            SizeNo = sizeRun.SizeNo,
                            Quantity = 0,
                            Reject = 0,
                            RejectId = 0,
                            Reviser = workerId
                        });
                }
            }
            deliveryDetailBySupplierClicked.AddRange(deliveryThisTimeList);
            LoadInspectionDetail(deliveryDetailBySupplierClicked);
            dgDeliveryDetail.IsReadOnly = false;
            tblDeliveryDetailOf.Text = String.Format("Inspection detail of: {0}", supplierClicked.Name);
        }

        private void LoadInspectionDetail(List<MaterialInspectModel> inspectionList)
        {
            HighLightError("");
            dgDeliveryDetail.Columns.Clear();
            dtInspectionDetail = new DataTable();

            //Column Supplier name
            dtInspectionDetail.Columns.Add("Name", typeof(String));
            dtInspectionDetail.Columns.Add("SupplierId", typeof(String));
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
            dtInspectionDetail.Columns.Add("InspectionDate", typeof(String));
            dtInspectionDetail.Columns.Add("InspectionDateDate", typeof(DateTime));
            DataGridTemplateColumn colInspDate = new DataGridTemplateColumn();
            colInspDate.Header = String.Format("Inspection\nDate");
            DataTemplate templateInspDate = new DataTemplate();
            FrameworkElementFactory tblInspDate = new FrameworkElementFactory(typeof(TextBlock));
            templateInspDate.VisualTree = tblInspDate;
            tblInspDate.SetBinding(TextBlock.TextProperty, new Binding(String.Format("InspectionDate")));
            tblInspDate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblInspDate.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colInspDate.CellTemplate = templateInspDate;
            colInspDate.ClipboardContentBinding = new Binding(String.Format("InspectionDate"));
            dgDeliveryDetail.Columns.Add(colInspDate);

            dtInspectionDetail.Columns.Add("Title", typeof(String));
            DataGridTemplateColumn colTitle = new DataGridTemplateColumn();
            colTitle.Header = String.Format("Order Size\nQty");
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

                dtInspectionDetail.Columns.Add(String.Format("Column{0}", sizeBinding), typeof(String));
                dtInspectionDetail.Columns.Add(String.Format("Column{0}Foreground", sizeBinding), typeof(SolidColorBrush));
                dtInspectionDetail.Columns.Add(String.Format("Column{0}ToolTip", sizeBinding), typeof(String));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = string.Format("{0}\n{1}", sizeRun.SizeNo, sizeRun.Quantity);
                column.MinWidth = 45;
                column.MaxWidth = 200;
                column.Binding = new Binding(String.Format("Column{0}", sizeBinding));

                Style styleColumn = new Style();
                Setter setterColumnForecolor = new Setter();
                setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", sizeBinding));

                Setter setterToolTip = new Setter();
                setterToolTip.Property = DataGridCell.ToolTipProperty;
                setterToolTip.Value = new Binding(String.Format("Column{0}ToolTip", sizeBinding));

                styleColumn.Setters.Add(setterColumnForecolor);
                styleColumn.Setters.Add(setterToolTip);
                column.CellStyle = styleColumn;

                dgDeliveryDetail.Columns.Add(column);
            }

            // Column Total
            dtInspectionDetail.Columns.Add("Total", typeof(String));
            DataGridTemplateColumn colTotal = new DataGridTemplateColumn();
            colTotal.Header = String.Format("Total\n{0}", sizeRunList.Sum(s => s.Quantity));
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

            DataGridTemplateColumn colButtonOK = new DataGridTemplateColumn();
            colButtonOK.MinWidth = 40;
            colButtonOK.MaxWidth = 40;
            DataTemplate templateButtonOK = new DataTemplate();
            FrameworkElementFactory fefButtonOK = new FrameworkElementFactory(typeof(Button));
            templateButtonOK.VisualTree = fefButtonOK;
            fefButtonOK.SetValue(Button.ContentProperty, "Ok");
            fefButtonOK.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnOK_Click));
            colButtonOK.CellTemplate = templateButtonOK;
            dgDeliveryDetail.Columns.Add(colButtonOK);

            // Binding Data
            var supplierList = inspectionList.Select(s => s.SupplierId).Distinct().ToList();
            foreach (var supplierId in supplierList)
            {
                var inspectionListBySupp = inspectionList.Where(w => w.SupplierId == supplierId).ToList();
                var dateList = inspectionListBySupp.Select(s => s.DeliveryDate).Distinct().ToList();
                if (dateList.Count() > 0)
                    dateList = dateList.OrderBy(o => o).ToList();

                bool addSupp = false;
                foreach (var date in dateList)
                {
                    DataRow dr = dtInspectionDetail.NewRow();
                    DataRow drReject = dtInspectionDetail.NewRow();

                    if (addSupp == false)
                    {
                        dr["Name"] = supplierAccessoriesList.FirstOrDefault(f => f.SupplierId == supplierId).Name;
                        addSupp = true;
                    }
                    dr["SupplierId"] = supplierId;
                    drReject["SupplierId"] = supplierId;
                    
                    dr["InspectionDate"]      = string.Format("{0:MM/dd/yyyy}", date);
                    dr["InspectionDateDate"]  = date;
                    drReject["InspectionDateDate"] = date;

                    dr["Title"] = _qtyOK;
                    drReject["Title"] = "Reject";

                    var inspectionListByDate = inspectionListBySupp.Where(w => w.DeliveryDate.Equals(date)).ToList();
                    foreach (var sizeRun in sizeRunList)
                    {
                        var deliveryBySize = inspectionListByDate.Where(w => w.SizeNo == sizeRun.SizeNo).ToList();
                        string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                        var qtyOK = deliveryBySize.Sum(s => s.Quantity);
                        if (qtyOK > 0)
                        {
                            dr[String.Format("Column{0}", sizeBinding)] = qtyOK.ToString();
                            if (qtyOK >= sizeRun.Quantity)
                                dr[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
                        }

                        var rejectIdList = deliveryBySize.Where(w => w.RejectId > 0).Select(s => s.RejectId).ToList();
                        var rejectDisplayList = new List<String>();
                        var rejectDisplayEnglishList = new List<String>();
                        foreach (var rId in rejectIdList)
                        {
                            var rejectById = rejectUpperAccessoriesList.Where(w => w.RejectId.Equals(rId)).FirstOrDefault();
                            var noOfReject = deliveryBySize.Where(w => w.RejectId.Equals(rId)).Sum(s => s.Reject);
                            rejectDisplayList.Add(String.Format("{0}: {1}", rejectById.RejectName_1, noOfReject));
                            rejectDisplayEnglishList.Add(String.Format("{0}: {1}", rejectById.RejectName, noOfReject));
                        }
                        if (rejectDisplayList.Count() > 0)
                        {
                            drReject[String.Format("Column{0}", sizeBinding)] = String.Join("\n", rejectDisplayList);
                            drReject[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                            drReject[String.Format("Column{0}ToolTip", sizeBinding)] = String.Join("\n", rejectDisplayEnglishList);
                        }
                    }

                    var totalQtyOK = inspectionListByDate.Sum(s => s.Quantity);
                    var totalReject = inspectionListByDate.Sum(s => s.Reject);
                    if (totalQtyOK > 0)
                        dr["Total"] = totalQtyOK.ToString();
                    if (totalReject > 0)
                        drReject["Total"] = totalReject.ToString();

                    dtInspectionDetail.Rows.Add(dr);
                    dtInspectionDetail.Rows.Add(drReject);
                }
            }

            dgDeliveryDetail.ItemsSource = dtInspectionDetail.AsDataView();
            dgDeliveryDetail.Items.Refresh();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (dgDeliveryDetail.CurrentItem == null)
                return;
            if (materialPlanChecking == null)
                return;

            var drClicked = ((DataRowView)dgDeliveryDetail.CurrentItem).Row;
            if (!drClicked["Title"].ToString().Equals(_qtyOK))
                return;
            var supplierIdOK = drClicked["SupplierId"].ToString();
            foreach (var sizeRun in sizeRunList)
            {
                string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                int qtyDeliveryBySize = matsDeliveryList.Where(w => w.SupplierId.ToString().Equals(supplierIdOK) && w.SizeNo == sizeRun.SizeNo).Sum(s => s.Quantity);
                drClicked[String.Format("Column{0}", sizeBinding)] = qtyDeliveryBySize;
                drClicked[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
            }
            drClicked["Total"] = "";
            drClicked["Total"] = matsDeliveryList.Where(w => w.SupplierId.ToString().Equals(supplierIdOK)).Sum(s => s.Quantity);
        }

        private void dgDeliveryDetail_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!account.MaterialDelivery)
            {
                e.Cancel = true;
                return;
            }

            HighLightError("");
            var rowEditting = (DataRowView)e.Row.Item;
            if (rowEditting["Title"].ToString().Equals(_qtyOK))
            {
                return;
            }
            // Not Allow Input Reject
            if (rowEditting["Title"].ToString().Equals("Reject"))
            {
                e.Cancel = true;
            }

            if (e.Column.GetValue(TagProperty) == null)
                return;

            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            var sizeRunClicked = sizeRunList.FirstOrDefault(f => f.SizeNo.Equals(sizeNo));
            var dateEditting = (DateTime)rowEditting["InspectionDateDate"];
            
            var supplierIdEdditing = rowEditting["SupplierId"].ToString();
            int qtyDeliveryBySize = matsDeliveryList.Where(w => w.SupplierId.ToString().Equals(supplierIdEdditing) && w.SizeNo.Equals(sizeNo)).Sum(s => s.Quantity);
            if (qtyDeliveryBySize == 0)
            {
                MessageBox.Show(String.Format("Size: {0} not yet Delivery !", sizeNo), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var matsDeliveryListBySuppTranfer = matsInspectionList.Where(w => w.SupplierId.Equals(supplierClicked.SupplierId)).ToList();
            var matsDeliveryListByDate = matsDeliveryListBySuppTranfer.Where(w => w.RejectId > 0
                                                                                && w.SizeNo.Equals(sizeRunClicked.SizeNo)
                                                                                && w.DeliveryDate.Equals(dateEditting)).ToList();
            int totalRejectBySizeCurrent = matsDeliveryListBySuppTranfer.Where(w => w.RejectId > 0 && w.SizeNo.Equals(sizeRunClicked.SizeNo)).Count();
            var window = new AddRejectForMaterialWindow(rejectUpperAccessoriesList, sizeRunClicked, materialPlanChecking, rowEditting, matsDeliveryListByDate, totalRejectBySizeCurrent, workerId, sizeRunList);
            window.ShowDialog();
            if (window.eAction == EExcute.AddNew && window.inspectListHasReject.Count() > 0)
            {
                matsInspectionList.RemoveAll(r => r.DeliveryDate.Equals(dateEditting)
                                                && r.RejectId > 0
                                                && r.SizeNo.Equals(sizeRunClicked.SizeNo));

                matsInspectionList.AddRange(window.inspectListHasReject);
                LoadInspectionDetail(matsInspectionList.Where(w => w.SupplierId.Equals(supplierClicked.SupplierId)).ToList());
            }
            else if (window.eAction == EExcute.Delete && window.inspectListHasReject.Count() > 0)
            {
                matsInspectionList.RemoveAll(r => r.DeliveryDate.Equals(dateEditting)
                                                && r.RejectId > 0
                                                && r.SizeNo.Equals(sizeRunClicked.SizeNo));
                LoadInspectionDetail(matsInspectionList.Where(w => w.SupplierId.Equals(supplierClicked.SupplierId)).ToList());
            }
        }

        private void dgDeliveryDetail_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var rowEditting = ((DataRowView)e.Row.Item).Row;
            if (e.Column.GetValue(TagProperty) == null)
                return;
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            string sizeCurrent = sizeNo.Contains(".") ? sizeNo.Replace(".", "@") : sizeNo;
            
            var supplierIdEdditing  = rowEditting["SupplierId"].ToString();
            int qtyDeliveryBySize   = matsDeliveryList.Where(w => w.SupplierId.ToString().Equals(supplierIdEdditing) && w.SizeNo.Equals(sizeNo)).Sum(s => s.Quantity);
            int qtyOrderBySize      = sizeRunList.Where(w => w.SizeNo.Equals(sizeNo)).Sum(s => s.Quantity);
            int qtyInput = 0;
            TextBox txtCurrent = (TextBox)e.EditingElement;
            if (qtyDeliveryBySize == 0)
            {
                MessageBox.Show(String.Format("Size: {0} not yet Delivery !", sizeNo), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtCurrent.Text = "";
                return;
            }
            Int32.TryParse(txtCurrent.Text.Trim().ToString(), out qtyInput);

            // Get total qty the others day
            int totalQtyOfTheOthersDay = 0;
            for (int r = 0; r < dtInspectionDetail.Rows.Count; r++)
            {
                DataRow dr = dtInspectionDetail.Rows[r];
                if (!dr["Title"].ToString().Equals(_qtyOK))
                    continue;
                if (!dr["InspectionDate"].ToString().Equals(rowEditting["InspectionDate"]))
                {
                    int qtyAtCell = 0;
                    Int32.TryParse(dr[String.Format("Column{0}", sizeCurrent)].ToString(), out qtyAtCell);
                    totalQtyOfTheOthersDay += qtyAtCell;
                }
            }

            rowEditting[String.Format("Column{0}Foreground", sizeCurrent)] = Brushes.Black;
            // Validate
            if (qtyInput + totalQtyOfTheOthersDay > qtyDeliveryBySize)
            {
                MessageBox.Show(String.Format("Qty Inspect :{0} grather than qty delivery {1} !", qtyInput + totalQtyOfTheOthersDay, qtyDeliveryBySize), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtCurrent.Text = "";
                return;
            }
            
            rowEditting[String.Format("Column{0}", sizeCurrent)] = txtCurrent.Text;
            if (qtyInput == qtyDeliveryBySize)
                rowEditting[String.Format("Column{0}Foreground", sizeCurrent)] = Brushes.Blue;
            
            // Calculate total qty at row
            int totalQtyAtRow = 0;
            foreach (var sizeRun in sizeRunList)
            {
                int qty = 0;
                var sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                Int32.TryParse(rowEditting[String.Format("Column{0}", sizeBinding)].ToString(), out qty);
                totalQtyAtRow += qty;
            }
            rowEditting["Total"] = "";
            if (totalQtyAtRow > 0)
                rowEditting["Total"] = totalQtyAtRow;
        }

        private void dpDeliveryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (supplierClicked != null)
            {
                if (string.IsNullOrEmpty(workerId))
                {
                    popInputWorkerId.IsOpen = true;
                    dpDeliveryDate.Focus();
                    txtWorkerId.Focus();
                    txtWorkerId.SelectAll();
                }
                else
                {
                    LoadSupplierClicked();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (supplierClicked == null || materialPlanChecking == null || account.MaterialDelivery == false)
                return;

            List<MaterialInspectModel> deliveryOKList = new List<MaterialInspectModel>();
            for (int r = 0; r < dtInspectionDetail.Rows.Count; r++)
            {
                DataRow dr = dtInspectionDetail.Rows[r];
                if (!dr["Title"].ToString().Equals(_qtyOK))
                    continue;

                var deliveryDate = (DateTime)dr["InspectionDateDate"];

                // Get Qty
                foreach (var sizeRun in sizeRunList)
                {
                    string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                    int qtyBySize = 0;
                    Int32.TryParse(dr[String.Format("Column{0}", sizeBinding)].ToString(), out qtyBySize);
                    deliveryOKList.Add(
                        new MaterialInspectModel
                        {
                            ProductNo       = productNo,
                            SupplierId      = supplierClicked.SupplierId,
                            DeliveryDate    = deliveryDate,
                            SizeNo          = sizeRun.SizeNo,
                            Quantity        = qtyBySize,
                            Reviser         = workerId
                        });
                }
            }

            if (bwUpload.IsBusy==false)
            {
                eAction = EExcute.AddNew;
                btnSave.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwUpload.RunWorkerAsync(deliveryOKList);
            }
        }

        private void GetDataFromDatagrid(DataTable tableSource)
        {
            
        }

        private void BwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            var deliveryOKList = e.Argument as List<MaterialInspectModel>;
            try
            {
                if (eAction == EExcute.AddNew)
                {
                    foreach (var itemInsert in deliveryOKList)
                    {
                        MaterialInspectController.Insert(itemInsert, insertQty: true, insertReject: false, deleteReject: false);
                        matsInspectionList.RemoveAll(r => r.SupplierId.Equals(itemInsert.SupplierId)
                                                    && r.DeliveryDate.Equals(itemInsert.DeliveryDate)
                                                    && r.SizeNo.Equals(itemInsert.SizeNo)
                                                    && r.Quantity > 0);
                        matsInspectionList.Add(itemInsert);
                    }
                }
                else if (eAction == EExcute.Delete)
                {
                    MaterialInspectController.DeleteByPOBySupplier(productNo, supplierClicked.SupplierId);
                    matsInspectionList.RemoveAll(r => r.SupplierId.Equals(supplierClicked.SupplierId));
                }
                e.Result = true;

                // Update ActualDate
                var deliveryListBySupp = matsInspectionList.
                                        GroupBy(g => g.SupplierId).
                                        Select(s => new
                                        {
                                            SupplierId = s.Key,
                                            TotalInspect    = matsInspectionList.Where(w => w.SupplierId.Equals(s.Key)).Sum(sum => sum.Quantity),
                                            TotalReject     = matsInspectionList.Where(w => w.SupplierId.Equals(s.Key)).Sum(r => r.Reject),
                                            MaxDeliveryDate = matsInspectionList.Where(w => w.SupplierId.Equals(s.Key)).Max(m => m.DeliveryDate)
                                        }).ToList();
                foreach (var materialPlan in materialPlanList)
                {
                    var deliveryBySupp = deliveryListBySupp.FirstOrDefault(f => f.SupplierId.Equals(materialPlan.SupplierId));
                    if (deliveryBySupp != null)
                    {
                        if (deliveryBySupp.TotalInspect.Equals(sizeRunList.Sum(s => s.Quantity)))
                            materialPlan.ActualDate = deliveryBySupp.MaxDeliveryDate.Date;
                        else
                            materialPlan.ActualDate = dtDefault;
                    }
                    else
                    {
                        materialPlan.ActualDate = dtDefault;
                    }
                    MaterialPlanController.Insert(materialPlan, isUpdateActualDate: true);
                    materialPlanList.ForEach(t => t.ActualDateString = t.ActualDate != dtDefault ? String.Format("{0:MM/dd}", t.ActualDate) : "");
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
                e.Result = false;
            }
        }
        private void BwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result == true)
            {
                if (eAction == EExcute.AddNew)
                {
                    MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    btnSave.IsEnabled = true;
                }
                else if (eAction == EExcute.Delete)
                {
                    MessageBox.Show("Deleted !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    btnDelete.IsEnabled = true;
                }

                LoadInspectionDetail(matsInspectionList.Where(w => w.SupplierId.Equals(supplierClicked.SupplierId)).ToList());
                LoadMaterialPlan(materialPlanList);
            }
            this.Cursor = null;
        }

        string sizePressKey = "";
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if ((int)e.Key >= 74 && (int)e.Key <= 83)
                sizePressKey += e.Key.ToString().Replace("NumPad", "");
            else if ((int)e.Key >= 34 && (int)e.Key <= 43)
                sizePressKey += e.Key.ToString().Replace("D", "");

            if (e.Key.Equals(Key.Escape))
                popInputWorkerId.IsOpen = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (supplierClicked == null || materialPlanChecking == null || account.MaterialDelivery == false)
                return;
            if (MessageBox.Show(string.Format("Confirm delete accessories of: {0}?", supplierClicked.Name), this.Title , MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            if (bwUpload.IsBusy == false)
            {
                eAction = EExcute.Delete;
                this.Cursor = Cursors.Wait;
                btnDelete.IsEnabled = false;
                bwUpload.RunWorkerAsync();
            }
        }

        private void btnClearReviser_Click(object sender, RoutedEventArgs e)
        {
            stkReviser.Visibility = Visibility.Collapsed;
            workerId = "";
            txtReviser.Text = "";
            tblDeliveryDetailOf.Text = "";
            LoadInspectionDetail(new List<MaterialInspectModel>());
        }

        string workerId = "";
        private void btnWorkderId_Click(object sender, RoutedEventArgs e)
        {
            btnWorkderId.IsDefault = false;
            workerId = txtWorkerId.Text.Trim().ToUpper().ToString();
            if (String.IsNullOrEmpty(workerId))
            {
                MessageBox.Show("Input WorkerId !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtWorkerId.Focus();
                txtWorkerId.SelectAll();
                return;
            }

            LoadSupplierClicked();

            txtReviser.Text = String.Format("Reviser: {0}", workerId);
            stkReviser.Visibility = Visibility.Visible;
            popInputWorkerId.IsOpen = false;
        }

        private void txtWorkerId_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            btnWorkderId.IsDefault = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Update Reject Reject By Supp
            foreach (var materialPlan in materialPlanList)
            {
                var totalRejectBySupp = matsInspectionList.Where(w => w.SupplierId == materialPlan.SupplierId).Sum(s => s.Reject);
                materialPlan.RejectPO = totalRejectBySupp;
            }
        }
    }
}
