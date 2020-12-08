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
using System.Windows.Threading;
using MasterSchedule.Controllers;
using MasterSchedule.Models;
using MasterSchedule.Helpers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleMaterialWHCheckingWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialWHCheckingWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwWindowLoad;
        BackgroundWorker bwInsertCouterTime;
        List<OutsoleMaterialModel>  outsoleMaterialList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OutsoleMaterialCheckingModel> outsoleMatCheckingList;
        List<SizeRunModel> sizeRunList;
        List<SizeRunModel> sizeRunByOutsoleSizeDuplicateList;
        List<ErrorModel> errorList;
        private OutsoleSuppliersModel supplierClicked;
        private string poSearch = "", workerId = "";
        private int columnWidth = 45;
        List<OutsoleMaterialCheckingModel> currentOutsoleMaterialCheckingList;
        List<OutsoleMaterialCheckingModel> osMatWHCheckViewDetailList;
        List<OutsoleMaterialCheckingModel> osmCheckFromZeroList;

        List<OutsoleSuppliersModel> supplierListByPO;
        private OrdersModel orderInformation;

        DispatcherTimer timerCounterWT;
        DispatcherTimer clockTimer;
        List<OSWHWorkingTimeModel> oswhWorkingTimeList;
        private string _TypeOfMachine = "WorkStation";
        private PrivateDefineModel definePrivate;

        public OutsoleMaterialWHCheckingWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            bwWindowLoad = new BackgroundWorker();
            bwWindowLoad.DoWork += BwWindowLoad_DoWork;
            bwWindowLoad.RunWorkerCompleted += BwWindowLoad_RunWorkerCompleted;

            bwInsertCouterTime = new BackgroundWorker();
            bwInsertCouterTime.DoWork += BwInsertCouterTime_DoWork;
            bwInsertCouterTime.RunWorkerCompleted += BwInsertCouterTime_RunWorkerComplete;

            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            outsoleMatCheckingList = new List<OutsoleMaterialCheckingModel>();
            currentOutsoleMaterialCheckingList = new List<OutsoleMaterialCheckingModel>();
            osMatWHCheckViewDetailList = new List<OutsoleMaterialCheckingModel>();
            osmCheckFromZeroList = new List<OutsoleMaterialCheckingModel>();

            sizeRunList = new List<SizeRunModel>();
            sizeRunByOutsoleSizeDuplicateList = new List<SizeRunModel>();
            errorList = new List<ErrorModel>();
            supplierListByPO = new List<OutsoleSuppliersModel>();
            orderInformation = new OrdersModel();

            oswhWorkingTimeList = new List<OSWHWorkingTimeModel>();
            definePrivate = new PrivateDefineModel();
            timerCounterWT = new DispatcherTimer();
            timerCounterWT.Tick += WorkingTimeCal_Tick;

            clockTimer = new DispatcherTimer();
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();

            _TypeOfMachine = AppSettingsHelper.ReadSetting("TypeOfMachine");

            InitializeComponent();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            tblClock.Text = String.Format("Clock:  {0:HH:mm:ss}", DateTime.Now);
        }

        private void BwInsertCouterTime_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void BwInsertCouterTime_DoWork(object sender, DoWorkEventArgs e)
        {
            var insertList = e.Argument as List<OSWHWorkingTimeModel>;
            foreach (var insertModel in insertList)
            {
                OSWHWorkingTimeController.Insert(insertModel);
            }
        }

        private void WorkingTimeCal_Tick(object sender, EventArgs e)
        {
            var oswhWorkingTimeListBySupp = oswhWorkingTimeList.Where(w => w.ProductNo == poSearch &&
                                                                           w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId &&
                                                                           w.WorkerId == workerId)
                                                                .ToList();
            if (oswhWorkingTimeListBySupp.Count() == 0)
                return;

            var lastCounter                 = oswhWorkingTimeListBySupp.OrderBy(o => o.StartingTime).LastOrDefault();
            var totalPairChecked            = currentOutsoleMaterialCheckingList.Where(w => w.ProductNo == poSearch && w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId && w.WorkerId == workerId).ToList().Sum(s => s.Quantity + s.ReturnReject + s.Reject);
            var totalPairCheckedFromZero    = osmCheckFromZeroList.Where(w => w.ProductNo == poSearch && w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId && w.WorkerId == workerId).ToList().Sum(s => s.Quantity + s.ReturnReject + s.Reject);
            
            var totalHoursCounted   = oswhWorkingTimeListBySupp.Sum(s => s.TotalHours) - lastCounter.TotalHours;

            lastCounter.Pairs       = totalPairChecked - totalPairCheckedFromZero;
            var totalHoursNow       = (DateTime.Now - lastCounter.StartingTime).TotalSeconds;
            lastCounter.TotalHours  = totalHoursNow;

            tblStartingTime.Text    = String.Format("Start:     {0:HH:mm:ss}", lastCounter.StartingTime);
            tblTotalHours.Text      = String.Format("Time:     {0} (s)", Math.Round(totalHoursCounted + totalHoursNow, 0, MidpointRounding.AwayFromZero));
            tblTotalPais.Text       = String.Format("Pairs:     {0}", lastCounter.Pairs);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwWindowLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwWindowLoad.RunWorkerAsync();
                this.Title = this.Title + " - " + _TypeOfMachine;
            }
        }
        
        private void BwWindowLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            errorList.Add(new ErrorModel {
                ErrorID         = 0,
                ErrorKey        = "Add",
                ErrorName       = "   Press Add",
                ErrorVietNamese = "   Bấm Phím +"
            });
            errorList.Add(new ErrorModel
            {
                ErrorID         = -1,
                ErrorKey        = "Multiply",
                ErrorName       = "Return: Press *",
                ErrorVietNamese = "Hàng bù: *"
            });
            errorList.AddRange(ErrorController.GetError());
            definePrivate = PrivateDefineController.GetDefine();

        }
        
        private void BwWindowLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // binding to error to grid
            int countColumn = gridError.ColumnDefinitions.Count();
            int countRow = countRow = errorList.Count / countColumn;
            if (errorList.Count % countColumn != 0)
            {
                countRow = errorList.Count / countColumn + 1;
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

            for (int i = 0; i <= errorList.Count() - 1; i++)
            {
                Border br = new Border();
                br.BorderBrush = Brushes.Gray;
                br.BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
                br.Margin = new Thickness(4, 4, 0, 0);
                br.Name = string.Format("border{0}", errorList[i].ErrorKey);
                Grid.SetColumn(br, i % countColumn);
                Grid.SetRow(br, i / countColumn);

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
                txtKey.Text = errorList[i].ErrorKey;
                txtKey.FontSize = 28;
                txtKey.Foreground = Brushes.Blue;
                txtKey.VerticalAlignment = VerticalAlignment.Center;
                txtKey.Margin = new Thickness(4, 0, 4, 0);
                Grid.SetColumn(txtKey, 0);

                TextBlock txtErrorName = new TextBlock();
                txtErrorName.Text = string.Format("{0}\n{1}", errorList[i].ErrorName, errorList[i].ErrorVietNamese);
                txtErrorName.FontSize = 15;
                txtErrorName.TextWrapping = TextWrapping.Wrap;
                txtErrorName.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(txtErrorName, 1);

                //if (errorList[i].ErrorID == 0)
                //{
                //    br.Background = Brushes.Green;
                //    txtErrorName.FontWeight = FontWeights.SemiBold;
                //}

                grid.Children.Add(txtKey);
                grid.Children.Add(txtErrorName);

                br.Child = grid;

                gridError.Children.Add(br);
            }

            this.Cursor = null;
            txtProductNo.Focus();
            dpCheckingDate.SelectedDate = DateTime.Now;
        }
       
        private void txtProductNo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            allowPressKey = false;
            btnSearch.IsDefault = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            allowPressKey = false;
            // Clear PO Information
            ClearPOInformation();
            poSearch = txtProductNo.Text.Trim().ToUpper().ToString();
            if (String.IsNullOrEmpty(poSearch))
            {
                MessageBox.Show("Input ProductNo !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                txtProductNo.SelectAll();

                return;
            }
            btnSearch.IsDefault = false;
            popInputWorkerId.IsOpen = true;
            txtWorkerId.Focus();
            txtWorkerId.SelectAll();
        }
        
        private void txtWorkerId_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            allowPressKey = false;
            btnWorkderId.IsDefault = true;
        }

        private void btnWorkderId_Click(object sender, RoutedEventArgs e)
        {
            allowPressKey = false;
            btnWorkderId.IsDefault = false;
            timerCounterWT.Stop();

            workerId = txtWorkerId.Text.Trim().ToUpper().ToString();
            if (String.IsNullOrEmpty(workerId))
            {
                MessageBox.Show("Input WorkerId !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtWorkerId.Focus();
                txtWorkerId.SelectAll();
                return;
            }

            // Confirm saving the working time.
            var insertList = oswhWorkingTimeList.Where(w => w.Pairs != 0).ToList();
            if (insertList.Count() > 0)
            {
                var totalHours = Math.Round(insertList.Sum(s => s.TotalHours) / 3600, 4, MidpointRounding.AwayFromZero);
                if (bwInsertCouterTime.IsBusy == false)
                {
                    MessageBox.Show(string.Format("Worker: {0}\nProductNo: {1}\nTotal Hours: {2} (hrs)", workerId, poSearch, totalHours), "Infor working time", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Cursor = Cursors.Wait;
                    bwInsertCouterTime.RunWorkerAsync(insertList);
                }
            }
            oswhWorkingTimeList.Clear();

            if (bwLoad.IsBusy == false)
            {
                // Clear PO Information
                ClearPOInformation();
                this.Cursor = Cursors.Wait;
                sizeRunByOutsoleSizeDuplicateList.Clear();
                poSearch = txtProductNo.Text.ToUpper().ToString();
                txtProductNo.Text = poSearch;

                bwLoad.RunWorkerAsync(poSearch);
            }
        }

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            var poSearch = e.Argument as String;
            outsoleMaterialList                 = OutsoleMaterialController.Select(poSearch);
            outsoleSupplierList                 = OutsoleSuppliersController.Select();
            outsoleMatCheckingList              = OutsoleMaterialCheckingController.SelectByPO(poSearch);
            osmCheckFromZeroList                = outsoleMatCheckingList.ToList();
            sizeRunList                         = SizeRunController.Select(poSearch);

            
            var osSizeList = sizeRunList.Select(s => s.OutsoleSize).ToList();
            foreach(var sr in sizeRunList)
            {
                if (osSizeList.Where(w => w.Equals(sr.OutsoleSize)).Count() > 1)
                    sizeRunByOutsoleSizeDuplicateList.Add(sr);
            }
            currentOutsoleMaterialCheckingList  = OutsoleMaterialCheckingController.SelectByPO(poSearch);
            orderInformation                    = OrdersController.SelectTop1(poSearch);
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            popInputWorkerId.IsOpen = false;
            if (outsoleMaterialList.Count() == 0)
            {
                MessageBox.Show("PO Not Found !\nPlease try again !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                txtProductNo.Focus();
                txtProductNo.SelectAll();
                return;
            }
            var outsoleMaterialSupplierIdPerPOList = outsoleMaterialList.OrderBy(o => o.OutsoleSupplierId).Select(s => s.OutsoleSupplierId).Distinct().ToList();

            // Create Supplier Button
            int indexSupp = 1;
            foreach (var supplierId in outsoleMaterialSupplierIdPerPOList)
            {
                var supplier = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == supplierId);
                supplierListByPO.Add(supplier);

                Button btnSupplier = new Button();

                btnSupplier.FontSize = 16;
                btnSupplier.MinHeight = 30;
                btnSupplier.Margin = new Thickness(0, 0, 10, 0);
                btnSupplier.ToolTip = String.Format("Press Key F{0}", indexSupp);
                btnSupplier.Tag = supplier;
                btnSupplier.Content = supplier == null ? "" : String.Format("   F{0}. {1}   ", indexSupp, supplier.Name);
                btnSupplier.Name = String.Format("SupplierId{0}", supplierId);
                var template = FindResource("ButtonSupplierTemplate") as ControlTemplate;
                btnSupplier.Template = template;

                btnSupplier.Click += new RoutedEventHandler(btnSupplier_Click);
                stkSuppliers.Children.Add(btnSupplier);
                indexSupp++;
            }

            tblWorkerId.Text    = string.Format("Worker ID: {0}", workerId);
            txtArticleNo.Text   = string.Format("ArticleNo\n{0}", orderInformation.ArticleNo);
            txtOSCode.Text      = string.Format("O/S Code\n{0}", orderInformation.OutsoleCode);
            txtShoeName.Text    = string.Format("Shoe Name\n{0}", orderInformation.ShoeName);

            allowPressKey       = true;            
        }

        private void ClearPOInformation()
        {
            stkSuppliers.Children.Clear();
            supplierListByPO.Clear();

            dgDeliveryStatus.Columns.Clear();
            dgDeliveryStatus.ItemsSource = null;

            dgSupplierDeliveryDetail.Columns.Clear();
            dgSupplierDeliveryDetail.ItemsSource = null;
            txtSupplierDeliveryDetail.Text = "";

            tblWorkerId.Text = "";
            txtArticleNo.Text = "";
            txtOSCode.Text = "";
            txtShoeName.Text = "";

            tblTotalPais.Text = "";
            tblTotalHours.Text = "";
            tblStartingTime.Text = "";

            allowPressKey = false;
        }

        private void btnSupplier_Click(object sender, RoutedEventArgs e)
        {
            var buttonClicked = sender as Button;
            if (buttonClicked == null)
                return;
            supplierClicked = buttonClicked.Tag as OutsoleSuppliersModel;
            LoadSupplierClicked();
        }

        private void LoadSupplierClicked()
        {
            dgDeliveryStatus.Columns.Clear();

            txtSupplierDeliveryDetail.Text = "";
            dgSupplierDeliveryDetail.Columns.Clear();
            dgSupplierDeliveryDetail.ItemsSource = null;

            DataTable dt = new DataTable();

            // Title
            DataGridTemplateColumn colTitle = new DataGridTemplateColumn();
            dt.Columns.Add(String.Format("Title"), typeof(String));
            colTitle.Header = String.Format("Order Size\nO/S Size");
            colTitle.Width = 120;
            DataTemplate tplTitle = new DataTemplate();
            FrameworkElementFactory tblTitle = new FrameworkElementFactory(typeof(TextBlock));
            tplTitle.VisualTree = tblTitle;
            tblTitle.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Title")));
            tblTitle.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblTitle.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colTitle.CellTemplate = tplTitle;
            colTitle.SortMemberPath = "Title";
            colTitle.ClipboardContentBinding = new Binding(String.Format("Title"));
            dgDeliveryStatus.Columns.Add(colTitle);

            foreach (var sizeRun in sizeRunList)
            {
                var size = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                dt.Columns.Add(String.Format("{0}", size), typeof(String));
                dt.Columns.Add(String.Format("Foreground{0}", size), typeof(SolidColorBrush));

                DataGridTemplateColumn colSize = new DataGridTemplateColumn();
                colSize.Header = String.Format("{0}\n{1}", sizeRun.SizeNo, sizeRun.OutsoleSize);
                colSize.Width = columnWidth;
                DataTemplate tplSize = new DataTemplate();
                FrameworkElementFactory tblSize = new FrameworkElementFactory(typeof(TextBlock));
                tplSize.VisualTree = tblSize;

                tblSize.SetBinding(TextBlock.TextProperty, new Binding(String.Format("{0}", size)));
                tblSize.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblSize.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblSize.SetValue(TextBlock.ForegroundProperty, new Binding(String.Format("Foreground{0}", size)));

                colSize.CellTemplate = tplSize;
                colSize.ClipboardContentBinding = new Binding(String.Format("{0}", size));
                dgDeliveryStatus.Columns.Add(colSize);
            }

            DataGridTemplateColumn colTotal = new DataGridTemplateColumn();
            dt.Columns.Add(String.Format("Total"), typeof(String));
            dt.Columns.Add(String.Format("ForegroundTotal"), typeof(SolidColorBrush));
            colTotal.Header = String.Format("Total");
            DataTemplate tplTotal = new DataTemplate();
            FrameworkElementFactory tblTotal = new FrameworkElementFactory(typeof(TextBlock));
            tplTotal.VisualTree = tblTotal;
            tblTotal.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Total")));
            tblTotal.SetBinding(TextBlock.ForegroundProperty, new Binding(String.Format("ForegroundTotal")));
            tblTotal.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblTotal.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colTotal.CellTemplate = tplTotal;
            colTotal.SortMemberPath = "Total";
            colTotal.ClipboardContentBinding = new Binding(String.Format("Total"));
            dgDeliveryStatus.Columns.Add(colTotal);

            // Binding data
            var outsoleMaterialListBySupplier = outsoleMaterialList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();

            DataRow drQty = dt.NewRow();
            DataRow drDel = dt.NewRow();

            drQty["Title"] = "Order Qty";
            drDel["Title"] = "Delivery Qty";

            foreach (var sizeRun in sizeRunList)
            {
                var size = sizeRun.SizeNo;
                size = size.Contains(".") ? size.Replace(".", "@") : size;

                var outsoleMaterialBySize = outsoleMaterialListBySupplier.FirstOrDefault(f => f.SizeNo == sizeRun.SizeNo);
                int qtyDel = outsoleMaterialBySize.Quantity > 0 ? outsoleMaterialBySize.Quantity : 0;

                drQty[String.Format("{0}", size)] = sizeRun.Quantity;
                drDel[String.Format("{0}", size)] = qtyDel > 0 ? qtyDel.ToString() : "";
                if (qtyDel < sizeRun.Quantity)
                    drDel[String.Format("Foreground{0}", size)] = Brushes.Red;
            }

            int totalQtyOrder = sizeRunList.Sum(s => s.Quantity);
            int totalQtyDelivery = outsoleMaterialListBySupplier.Sum(s => s.Quantity);

            drQty["Total"] = totalQtyOrder;
            drDel["Total"] = totalQtyDelivery > 0 ? totalQtyDelivery.ToString() : "";
            if (totalQtyDelivery < totalQtyOrder)
                drDel["ForegroundTotal"] = Brushes.Red;

            dt.Rows.Add(drQty);
            dt.Rows.Add(drDel);

            dgDeliveryStatus.ItemsSource = dt.AsDataView();
            dgDeliveryStatus.Items.Refresh();

            // Binding Delivery Detail List
            txtSupplierDeliveryDetail.Text = String.Format("Delivery Detail of : {0}", supplierClicked.Name);

            var currentOutsoleMaterialCheckingListBySupplierClicked = currentOutsoleMaterialCheckingList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            LoadDataDetail(currentOutsoleMaterialCheckingListBySupplierClicked);

            dpCheckingDate.SelectedDate = DateTime.Now;
            allowPressKey = true;

            // HightLight
            HightSupplierClicked(supplierClicked.OutsoleSupplierId);

            oswhWorkingTimeList.Add(new OSWHWorkingTimeModel
            {
                WorkerId = workerId,
                ProductNo = poSearch,
                OutsoleSupplierId = supplierClicked.OutsoleSupplierId,
                StartingTime = DateTime.Now,
                CheckingDate = DateTime.Now
            });

            if (oswhWorkingTimeList.Count() > 0)
                timerCounterWT.Start();
        }

        private bool allowPressKey = false;
        string errorKey = "";
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // Press Supplier By Key: F1 .... F12
            if (e.Key.ToString().Contains("F"))
            {
                string supplierIdPressed = "";
                int supplierIdPressedInt = 0;
                supplierIdPressed = e.Key.ToString().Replace("F", "");
                Int32.TryParse(supplierIdPressed, out supplierIdPressedInt);

                if (supplierIdPressedInt <= 0 || supplierIdPressedInt > supplierListByPO.Count())
                    return;

                supplierClicked = supplierListByPO[supplierIdPressedInt - 1];

                if (supplierClicked == null)
                    return;
                LoadSupplierClicked();
            }
                      
            // Return
            if (allowPressKey == false || supplierClicked == null)
                return;
            // Give Mode

            var pressMode = PressMode.None;
            if (e.Key == Key.Add)
                pressMode = PressMode.Add;
            if (e.Key == Key.Multiply)
                pressMode = PressMode.Return;

            if (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9 && !_TypeOfMachine.ToUpper().Equals("TERMINAL"))
                pressMode = PressMode.Reject;
            if (Key.D0 <= e.Key && e.Key <= Key.D9 && _TypeOfMachine.ToUpper().Equals("TERMINAL"))
                pressMode = PressMode.Reject;

            if (!_TypeOfMachine.ToUpper().Equals("TERMINAL"))
                errorKey += e.Key.ToString().Replace("NumPad", "");
            else
                errorKey += e.Key.ToString().Replace("D", "");

            HighLightError("");

            var checkingDate = dpCheckingDate.SelectedDate.Value.Date;
            var currentOSMCheckListBySupp = currentOutsoleMaterialCheckingList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            var outsoleMaterialListBySupplier = outsoleMaterialList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();

            // HAS ERROR
            var errorPressed = errorList.FirstOrDefault(f => f.ErrorKey == errorKey);
            if (pressMode == PressMode.Reject && errorPressed != null)
            {
                HighLightError(errorKey);
                if (outsoleMaterialListBySupplier.Count() != 0)
                {
                    var window = new OutsoleMaterialWHCheckingAddQuantitySizeWindow(poSearch,
                                                                                    supplierClicked,
                                                                                    checkingDate,
                                                                                    errorPressed,
                                                                                    currentOSMCheckListBySupp,
                                                                                    outsoleMaterialListBySupplier,
                                                                                    workerId,
                                                                                    sizeRunList);
                    window.ShowDialog();

                    var Xmodel = window.outsoleMaterialCheckingUpdatedBySizeList.FirstOrDefault();
                    if (Xmodel != null)
                    {
                        currentOutsoleMaterialCheckingList.RemoveAll(r => r.SizeNo == Xmodel.SizeNo &&
                                                                          r.ErrorId == Xmodel.ErrorId &&
                                                                          r.CheckingDate == Xmodel.CheckingDate &&
                                                                          r.OutsoleSupplierId == Xmodel.OutsoleSupplierId);
                        currentOutsoleMaterialCheckingList.AddRange(window.outsoleMaterialCheckingUpdatedBySizeList);
                    }

                    // remove where error has qty = 0
                    currentOutsoleMaterialCheckingList.RemoveAll(r => r.Reject == 0 && r.ErrorId > 0);
                    var reloadList = currentOutsoleMaterialCheckingList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                    LoadDataDetail(reloadList);
                }

                errorKey = "";
            }
            else if (pressMode == PressMode.Reject && !string.IsNullOrEmpty(errorKey) && errorKey.Count() > 1)
            {
                if (!_TypeOfMachine.ToUpper().Equals("TERMINAL"))
                    errorKey = e.Key.ToString().Replace("NumPad", "");
                else
                    errorKey = e.Key.ToString().Replace("D", "");
            }

            // PASS
            if (pressMode == PressMode.Add && errorPressed != null)
            {
                HighLightError("Add");
                if (outsoleMaterialListBySupplier.Count() != 0)
                {
                    var window = new OutsoleMaterialWHCheckingAddQuantitySizeWindow(poSearch,
                                                                                    supplierClicked,
                                                                                    checkingDate,
                                                                                    errorPressed,
                                                                                    currentOSMCheckListBySupp,
                                                                                    outsoleMaterialListBySupplier,
                                                                                    workerId,
                                                                                    sizeRunList);
                    
                    window.ShowDialog();
                    var Xmodel = window.outsoleMaterialCheckingUpdatedBySizeList.FirstOrDefault();
                    if (Xmodel != null)
                    {
                        currentOutsoleMaterialCheckingList.RemoveAll(r => r.SizeNo              == Xmodel.SizeNo &&
                                                                          r.ErrorId             == Xmodel.ErrorId &&
                                                                          r.CheckingDate        == Xmodel.CheckingDate &&
                                                                          r.OutsoleSupplierId   == Xmodel.OutsoleSupplierId);

                        currentOutsoleMaterialCheckingList.AddRange(window.outsoleMaterialCheckingUpdatedBySizeList);
                    }
                    
                    // remove where qty = 0
                    currentOutsoleMaterialCheckingList.RemoveAll(r => r.Quantity == 0 && r.ErrorId == 0);
                    var reloadList = currentOutsoleMaterialCheckingList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                    LoadDataDetail(reloadList);
                }

                errorKey = "";
            }
            
            // RETURN REJECT
            if (pressMode == PressMode.Return && errorPressed != null)
            {
                HighLightError("Multiply");
                if (outsoleMaterialListBySupplier.Count() != 0)
                {
                    var window = new OutsoleMaterialWHCheckingAddQuantitySizeWindow(poSearch,
                                                                                    supplierClicked,
                                                                                    checkingDate,
                                                                                    errorPressed,
                                                                                    currentOSMCheckListBySupp,
                                                                                    outsoleMaterialListBySupplier,
                                                                                    workerId,
                                                                                    sizeRunList);

                    window.ShowDialog();
                    var Xmodel = window.outsoleMaterialCheckingUpdatedBySizeList.FirstOrDefault();
                    if (Xmodel != null)
                    {
                        currentOutsoleMaterialCheckingList.RemoveAll(r => r.SizeNo              == Xmodel.SizeNo &&
                                                                          r.ErrorId             == Xmodel.ErrorId &&
                                                                          r.CheckingDate        == Xmodel.CheckingDate &&
                                                                          r.OutsoleSupplierId   == Xmodel.OutsoleSupplierId);

                        currentOutsoleMaterialCheckingList.AddRange(window.outsoleMaterialCheckingUpdatedBySizeList);
                    }

                    // remove where return reject has qty = 0
                    currentOutsoleMaterialCheckingList.RemoveAll(r => r.ReturnReject == 0 && r.ErrorId == -1);
                    var reloadList = currentOutsoleMaterialCheckingList.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
                    LoadDataDetail(reloadList);
                }
                errorKey = "";
            }

            if (pressMode == PressMode.None)
                errorKey = "";
        }
        
        DataTable dtDeliveryDetail = new DataTable();
        private void LoadDataDetail(List<OutsoleMaterialCheckingModel> currentCheckingList)
        {
            dgSupplierDeliveryDetail.Columns.Clear();
            dgSupplierDeliveryDetail.ItemsSource = null;

            if (currentCheckingList.Count() == 0)
                return;

            // Create Column First.
            dtDeliveryDetail = new DataTable();
            DataGridTemplateColumn colDate = new DataGridTemplateColumn();
            dtDeliveryDetail.Columns.Add(String.Format("Date"), typeof(String));
            colDate.Header = String.Format("Date");
            colDate.Width = 120;
            DataTemplate tplDate = new DataTemplate();
            FrameworkElementFactory tblDate = new FrameworkElementFactory(typeof(TextBlock));
            tplDate.VisualTree = tblDate;
            tblDate.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Date")));
            tblDate.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblDate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colDate.CellTemplate = tplDate;
            colDate.SortMemberPath = "Date";
            colDate.ClipboardContentBinding = new Binding(String.Format("Date"));
            dgSupplierDeliveryDetail.Columns.Add(colDate);

            foreach (var sizeRun in sizeRunList)
            {
                var size = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                dtDeliveryDetail.Columns.Add(String.Format("{0}", size), typeof(String));
                dtDeliveryDetail.Columns.Add(String.Format("Foreground{0}", size), typeof(SolidColorBrush));
                dtDeliveryDetail.Columns.Add(String.Format("Background{0}", size), typeof(SolidColorBrush));
                dtDeliveryDetail.Columns.Add(String.Format("ToolTip{0}", size), typeof(String));

                DataGridTemplateColumn colSize = new DataGridTemplateColumn();
                colSize.Header = String.Format("{0}\n{1}", sizeRun.SizeNo, sizeRun.OutsoleSize);
                colSize.Width = columnWidth;
                DataTemplate tplSize = new DataTemplate();
                FrameworkElementFactory tblSize = new FrameworkElementFactory(typeof(TextBlock));
                tplSize.VisualTree = tblSize;

                tblSize.SetBinding(TextBlock.TextProperty, new Binding(String.Format("{0}", size)));
                tblSize.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblSize.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblSize.SetValue(TextBlock.ForegroundProperty, new Binding(String.Format("Foreground{0}", size)));
                tblSize.SetValue(TextBlock.BackgroundProperty, new Binding(String.Format("Background{0}", size)));
                tblSize.SetValue(TextBlock.ToolTipProperty, new Binding(String.Format("ToolTip{0}", size)));

                colSize.CellTemplate = tplSize;
                colSize.ClipboardContentBinding = new Binding(String.Format("{0}", size));
                dgSupplierDeliveryDetail.Columns.Add(colSize);
            }

            DataGridTemplateColumn colTotal = new DataGridTemplateColumn();
            dtDeliveryDetail.Columns.Add(String.Format("Total"), typeof(String));
            dtDeliveryDetail.Columns.Add(String.Format("ForegroundTotal"), typeof(SolidColorBrush));
            colTotal.Header = String.Format("Total");
            DataTemplate tplTotal = new DataTemplate();
            FrameworkElementFactory tblTotal = new FrameworkElementFactory(typeof(TextBlock));
            tplTotal.VisualTree = tblTotal;
            tblTotal.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Total")));
            tblTotal.SetBinding(TextBlock.ForegroundProperty, new Binding(String.Format("ForegroundTotal")));
            tblTotal.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblTotal.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colTotal.CellTemplate = tplTotal;
            colTotal.SortMemberPath = "Total";
            colTotal.ClipboardContentBinding = new Binding(String.Format("Total"));
            dgSupplierDeliveryDetail.Columns.Add(colTotal);

            // Binding Data.
            var dateCheckingList = currentCheckingList.Select(s => s.CheckingDate.Date).Distinct().ToList();
            if (dateCheckingList.Count() > 0)
                dateCheckingList = dateCheckingList.OrderBy(o => o).ToList();

            foreach (var dateCheck in dateCheckingList)
            {
                DataRow dr = dtDeliveryDetail.NewRow();
                dr["Date"] = String.Format("{0:dd/MM/yyyy}", dateCheck);

                var currentCheckListByDate = currentCheckingList.Where(w => w.CheckingDate == dateCheck).ToList();
                
                foreach (var sizeRun in sizeRunList)
                {
                    var sizeRunByOutsoleSizeDuplicateList_Size = sizeRunByOutsoleSizeDuplicateList.Where(s => s.OutsoleSize == sizeRun.OutsoleSize).ToList();
                    var indexOfOsSizeDuplicate = sizeRunByOutsoleSizeDuplicateList_Size.IndexOf(sizeRun);
                    var osSizeNotEmptyList = sizeRunList.Where(w => !String.IsNullOrEmpty(w.OutsoleSize)).ToList();
                    if (indexOfOsSizeDuplicate > 0 && osSizeNotEmptyList.Count() > 0)
                        continue;

                    List<String> displayDataList = new List<string>();
                    var size = sizeRun.SizeNo;
                    size = size.Contains(".") ? size.Replace(".", "@") : size;

                    string sizeToCompare = "";
                    if (!String.IsNullOrEmpty(sizeRun.OutsoleSize))
                        sizeToCompare = sizeRun.OutsoleSize;
                    else
                        sizeToCompare = sizeRun.SizeNo;

                    var currentCheckListBySize = currentCheckListByDate.Where(w => w.SizeNo == sizeToCompare).ToList();

                    int qtyCheckByDateBySize = currentCheckListBySize.Sum(s => s.Quantity);

                    List<string> qtyDisplayList = new List<string>();
                    string qtyDisplay = "";
                    if (qtyCheckByDateBySize > 0)
                    {
                        // only get WorkingCard for Quantity OK
                        //var workingcartLong = currentCheckingList.Where(w => w.CheckingDate == dateCheck && w.SizeNo == sizeToCompare && w.ErrorId == 0).Select(s => (long)(s.WorkingCard)).Distinct().ToList();
                        var workingCartList = currentCheckListBySize.Where(w => w.CheckingDate == dateCheck && w.SizeNo == sizeToCompare && w.WorkingCard != -999).Select(s => s.WorkingCard).Distinct().ToList();
                        if (workingCartList.Count() > 0)
                            workingCartList = workingCartList.OrderBy(o => o).ToList();

                        foreach (var workingCart in workingCartList)
                        {                            
                            var currentCheckingListByWorkingCart = currentCheckingList.Where(w =>   w.CheckingDate == dateCheck && 
                                                                                                    w.SizeNo == sizeToCompare && 
                                                                                                    w.WorkingCard == workingCart).ToList();

                            qtyDisplayList.Add(String.Format("({0}): {1}", workingCart, currentCheckingListByWorkingCart.Sum(s => s.Quantity)));
                        }

                        qtyDisplay = String.Join("\n", qtyDisplayList);
                        //dr[String.Format("{0}", size)] = qtyDisplay;
                        displayDataList.Add(qtyCheckByDateBySize.ToString());

                        dr[String.Format("{0}", size)] = String.Join("\n", displayDataList);
                    }

                    if (qtyCheckByDateBySize >= sizeRun.Quantity)
                        dr[String.Format("Foreground{0}", size)]    = Brushes.Blue;

                    // Reject is ErrorId != 0 and != -1 : ErrorId > 0
                    var rejectList = currentCheckListBySize.Where(w => w.ErrorId > 0).ToList();
                    if (rejectList.Count() > 0)
                    {
                        displayDataList.Add(String.Format("R:{0}", rejectList.Sum(s => s.Reject)));
                        dr[String.Format("Background{0}", size)]    = Brushes.Yellow;
                        //dr[String.Format("{0}", size)]              = String.Format("{0}R: {1}", String.IsNullOrEmpty(qtyDisplay) == false ? String.Format("{0}\n", qtyDisplay) : "", rejectCheckListByDateBySize.Sum(s => s.Reject));
                        //dr[String.Format("{0}", size)]              = String.Format("{0}R: {1}", qtyCheckByDateBySize > 0 ? String.Format("{0}\n", qtyCheckByDateBySize) : "", rejectCheckListByDateBySize.Sum(s => s.Reject));
                        dr[String.Format("{0}", size)]              = String.Join("\n", displayDataList);
                        dr[String.Format("Foreground{0}", size)]    = Brushes.Red;

                        var defectsList = rejectList.Select(w => w.ErrorId).ToList();
                        List<string> toolTips = new List<string>();
                        foreach (var defect in defectsList)
                        {
                            var errorName = errorList.FirstOrDefault(f => f.ErrorID == defect);
                            int rejectByError = rejectList.Where(w => w.ErrorId == defect).Sum(s => s.Reject);
                            toolTips.Add(String.Format("{0} : {1}", errorName.ErrorName, rejectByError));
                        }

                        dr[String.Format("ToolTip{0}", size)]       = String.Join("\n", toolTips);
                    }

                    // Return Reject
                    var returnRejectList = currentCheckListBySize.Where(w => w.ErrorId == -1).ToList();
                    if (returnRejectList.Count() > 0)
                    {
                        displayDataList.Add(String.Format("* {0}", returnRejectList.Sum(s => s.ReturnReject)));
                        dr[String.Format("{0}", size)]              = String.Join("\n", displayDataList);
                        dr[String.Format("Foreground{0}", size)]    = Brushes.Red;
                        dr[String.Format("Background{0}", size)]    = Brushes.Yellow;
                    }
                }

                int totalQtyCheckByDate = currentCheckListByDate.Sum(s => s.Quantity + s.ReturnReject);
                if (totalQtyCheckByDate > 0)
                    dr["Total"] = totalQtyCheckByDate;

                dtDeliveryDetail.Rows.Add(dr);
            }

            // Total row
            if (dateCheckingList.Count() > 1)
            {
                DataRow drTotal = dtDeliveryDetail.NewRow();
                drTotal["Date"] = "Total";
                foreach (var sizeRun in sizeRunList)
                {
                    var indexOfOsSizeDuplicate = sizeRunByOutsoleSizeDuplicateList.IndexOf(sizeRun);
                    if (indexOfOsSizeDuplicate > 0)
                        continue;

                    var size = sizeRun.SizeNo;
                    size = size.Contains(".") ? size.Replace(".", "@") : size;
                    string sizeToCompare = "";
                    if (!String.IsNullOrEmpty(sizeRun.OutsoleSize))
                        sizeToCompare = sizeRun.OutsoleSize;
                    else
                        sizeToCompare = sizeRun.SizeNo;

                    int totalQtyOKBySize = currentCheckingList.Where(w => w.SizeNo == sizeToCompare).Sum(s => s.Quantity + s.ReturnReject);

                    if (totalQtyOKBySize > 0)
                        drTotal[String.Format("{0}", size)] = totalQtyOKBySize;

                    drTotal[String.Format("Foreground{0}", size)] = Brushes.Blue;
                    if (totalQtyOKBySize < sizeRun.Quantity)
                        drTotal[String.Format("Foreground{0}", size)] = Brushes.Red;
                }
                int totalTotal = currentCheckingList.Sum(s => s.Quantity + s.ReturnReject);
                if (totalTotal > 0)
                    drTotal["Total"] = totalTotal;
                dtDeliveryDetail.Rows.Add(drTotal);
            }

            dgSupplierDeliveryDetail.ItemsSource = dtDeliveryDetail.AsDataView();
            dgSupplierDeliveryDetail.Items.Refresh();

            osMatWHCheckViewDetailList = currentCheckingList.ToList();
        }

        private void HighLightError(string borderName)
        {
            try
            {
                var childrenCount = VisualTreeHelper.GetChildrenCount(gridError);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(gridError, i);
                    if (child != null)
                    {
                        Border br = child as Border;
                        br.BorderBrush = Brushes.Gray;
                        br.BorderThickness = new Thickness(1, 1, 1, 1);
                        br.Background = Brushes.Transparent;

                        if (br.Name == string.Format("border{0}", borderName))
                            br.Background = Brushes.Yellow;
                        if (br.Name == string.Format("border{0}", borderName) && borderName.Equals("Add"))
                            br.Background = Brushes.Green;
                        if (br.Name == string.Format("border{0}", borderName) && borderName.Equals("Multiply"))
                            br.Background = Brushes.Tomato;
                    }
                }
            }
            catch { }
        }

        private void HightSupplierClicked(int supplierId)
        {
            try
            {
                HighLightError("");
                var childrenCount = VisualTreeHelper.GetChildrenCount(stkSuppliers);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(stkSuppliers, i);
                    if (child != null)
                    {
                        Button buttonClicked = child as Button;
                        var template = FindResource("ButtonSupplierTemplate") as ControlTemplate;
                        var templateClicked = FindResource("ButtonSupplierClickedTemplate") as ControlTemplate;
                        buttonClicked.Template = template;
                        if (buttonClicked.Name.Equals(String.Format("SupplierId{0}", supplierId)))
                            buttonClicked.Template = templateClicked;
                    }
                }
            }
            catch { }
        }
        
        private void dgDeliveryStatus_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
        
        private void dgSupplierDeliveryDetail_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dpCheckingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txtCheckDateTitle.Text = "Check Date";
            txtConfirmBy.Visibility = Visibility.Collapsed;
            txtConfirmBy.Password = "";
            chkbCheckingDate.IsChecked = false;
        }
       
        private void chkbCheckingDate_Checked(object sender, RoutedEventArgs e)
        {
            txtCheckDateTitle.Text = "Confirm Code";
            txtCheckDateTitle.Foreground = Brushes.Black;
            txtConfirmBy.Visibility = Visibility.Visible;
            txtConfirmBy.Focus();
            allowPressKey = false;
        }

        private void txtConfirmBy_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtCheckDateTitle.Foreground = Brushes.Red;
            if (txtConfirmBy.Password.Equals(definePrivate.WarehouseCode))
            {
                txtCheckDateTitle.Text = "Select a date";
                dpCheckingDate.IsEnabled = true;
                txtCheckDateTitle.Foreground = Brushes.Blue;
            }
            else
            {
                dpCheckingDate.IsEnabled = false;
                txtCheckDateTitle.Text = "Wrong Confirm Code !!!";
                txtCheckDateTitle.Foreground = Brushes.Red;
            }
        }

        private void chkbCheckingDate_Unchecked(object sender, RoutedEventArgs e)
        {
            txtCheckDateTitle.Text = "Check Date";
            txtCheckDateTitle.Foreground = Brushes.Black;
            txtConfirmBy.Visibility = Visibility.Collapsed;
            txtConfirmBy.Password = "";
            chkbCheckingDate.IsChecked = false;
            allowPressKey = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var insertList = oswhWorkingTimeList.Where(w => w.Pairs != 0).ToList();
            if (insertList.Count() > 0)
            {
                var totalHours = Math.Round(insertList.Sum(s => s.TotalHours) / 3600, 4, MidpointRounding.AwayFromZero);
                if (bwInsertCouterTime.IsBusy == false)
                {
                    MessageBox.Show(string.Format("Worker: {0}\nProductNo: {1}\nTotal Hours: {2} (hrs)", insertList.FirstOrDefault().ProductNo, poSearch, totalHours), "Working Time", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Cursor = Cursors.Wait;
                    bwInsertCouterTime.RunWorkerAsync(insertList);
                }
            }
            oswhWorkingTimeList.Clear();
        }

        private void btnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (osMatWHCheckViewDetailList.Count() <= 0 || dgSupplierDeliveryDetail.ItemsSource == null)
                return;
            var window = new OSMaterialWHCheckViewDetailWindow(osMatWHCheckViewDetailList, errorList, orderInformation, supplierClicked);
            window.Show();
        }

        private enum PressMode
        {
            Add,
            Reject,
            Return,
            None
        }
    }
}
