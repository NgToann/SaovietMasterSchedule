using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Data;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ConfirmOutsoleMaterialWorkingCartWindow.xaml
    /// </summary>
    public partial class ConfirmOutsoleMaterialWorkingCartWindow : Window
    {
        BackgroundWorker bwSearch;
        List<OutsoleMaterialConfirmWorkingCartModel> osmConfirmList;
        List<OutsoleMaterialConfirmWorkingCartModel> releaseSelectedList;
        private LinearGradientBrush bgNormal;
        private LinearGradientBrush bgSelected;
        private LinearGradientBrush bgReleased;
        private LinearGradientBrush bgConfirmSelected;
        private LinearGradientBrush resourceBorderBackground;
        private Thickness resourceBorderThickness;

        private CornerRadius radius;
        List<string> productNoList;
        AccountModel account;
        DispatcherTimer dispatcherTimer;
        List<SizeRunModel> sizeRunList;

        public ConfirmOutsoleMaterialWorkingCartWindow(AccountModel account)
        {
            this.account = account;
            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += BwSearch_DoWork;
            bwSearch.RunWorkerCompleted += BwSearch_RunWorkerCompleted;

            osmConfirmList = new List<OutsoleMaterialConfirmWorkingCartModel>();
            releaseSelectedList = new List<OutsoleMaterialConfirmWorkingCartModel>();
            productNoList = new List<string>();
            sizeRunList = new List<SizeRunModel>();

            bgNormal = (LinearGradientBrush)TryFindResource("radBgNormal");
            bgSelected = (LinearGradientBrush)TryFindResource("radBgSelected");
            bgReleased = (LinearGradientBrush)TryFindResource("bgReleased");
            bgConfirmSelected = (LinearGradientBrush)TryFindResource("bgConfirmSelected");

            radius = (CornerRadius)TryFindResource("radiusType2");

            resourceBorderBackground = (LinearGradientBrush)TryFindResource("BorderBackgroundConfirmOSMatsCart");
            resourceBorderThickness = (Thickness)TryFindResource("thicknessType1");

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            InitializeComponent();
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = string.Format("User: {0} ---- {1:dd/MM/yyyy HH:mm:ss}", account.FullName, DateTime.Now);
        }
        private void clearValue()
        {
            if (this.IsLoaded)
            {
                productNoList.Clear();
                osmConfirmList.Clear();
                releaseSelectedList.Clear();

                wrProductNoList.Children.Clear();
                wrpSizeNoList.Children.Clear();
                gridInfo.DataContext = null;
                dgReleaseDetail.Columns.Clear();
                dgReleaseDetail.ItemsSource = null;

                btnConfirm.Tag = null;
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            clearValue();

            string searchWhat = txtIndexNo.Text.Trim().ToUpper().ToString();
            int indexNo = 0;
            Int32.TryParse(searchWhat, out indexNo);
            if (indexNo == 0 && doMode == DoMode.Confirm)
            {
                MessageBox.Show("IndexNo is invalid !\nIndexNo không hợp lệ !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtIndexNo.SelectAll();
                txtIndexNo.Focus();
                return;
            }

            if (!bwSearch.IsBusy)
            {
                btnSearch.IsDefault = false;
                btnSearch.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwSearch.RunWorkerAsync(searchWhat);
            }
        }
        private void BwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            var searchWhat = e.Argument as String;
            try
            {
                if (doMode == DoMode.Confirm)
                    osmConfirmList = OutsoleMaterialController.GetOSConfirmByIndexNo(int.Parse(searchWhat));
                else if (doMode == DoMode.Release)
                {
                    osmConfirmList = OutsoleMaterialController.GetOSConfirmByPO(searchWhat);
                    sizeRunList = SizeRunController.Select(searchWhat);
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }
        private void BwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (doMode == DoMode.Confirm)
                loadProductNo(osmConfirmList);
            else if (doMode == DoMode.Release)
                loadWorkingCart(osmConfirmList);
            if (osmConfirmList.Count() == 0)
            {
                MessageBox.Show("Not Found !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.Cursor = null;
            txtIndexNo.SelectAll();
            txtIndexNo.Focus();
            btnSearch.IsEnabled = true;
            btnSearch.IsDefault = true;
        }

        private void loadProductNo(List<OutsoleMaterialConfirmWorkingCartModel> osmConfirmList)
        {
            productNoList = osmConfirmList.Select(s => s.ProductNo).Distinct().ToList();
            productNoList.OrderBy(o => o);

            foreach (var po in productNoList)
            {
                var osConfirmByPO = osmConfirmList.Where(w => w.ProductNo == po).ToList();
                RadioButton radPO = new RadioButton
                {
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(5, 5, 5, 5),
                    GroupName = "T",
                    Tag = osConfirmByPO,
                    Content = new Border
                    {
                        CornerRadius = new CornerRadius(3, 3, 3, 3),
                        Padding = new Thickness(10, 4, 10, 4),
                        BorderBrush = (LinearGradientBrush)resourceBorderBackground,
                        BorderThickness = (Thickness)resourceBorderThickness,
                        Background = osConfirmByPO.FirstOrDefault().IsConfirm ? bgSelected : bgNormal,
                        Child = new TextBlock
                        {
                            Text = po
                        }
                    }
                };
                if (productNoList.Count() == 1)
                    radPO.IsChecked = true;
                radPO.Checked += RadPO_Checked;
                wrProductNoList.Children.Add(radPO);
            }
        }
        private void loadWorkingCart(List<OutsoleMaterialConfirmWorkingCartModel> osmConfirmList)
        {
            wrProductNoList.Tag = null;
            wrProductNoList.Children.Clear();
            var workingCartList = osmConfirmList.Where(w => w.WorkingCard > 0).Select(s => s.WorkingCard).Distinct().ToList();
            if (workingCartList.Count() > 1)
                workingCartList = workingCartList.OrderBy(o => o).ToList();
            foreach (var wcart in workingCartList)
            {
                var osConfirmByCart = osmConfirmList.Where(w => w.WorkingCard == wcart).ToList();

                CheckBox chkWorkingCart = new CheckBox
                {
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(5, 5, 5, 5),
                    Tag = osConfirmByCart,
                    Content = new Border
                    {
                        CornerRadius = new CornerRadius(2),
                        Padding = new Thickness(10),
                        BorderBrush = (LinearGradientBrush)resourceBorderBackground,
                        BorderThickness = (Thickness)resourceBorderThickness,
                        Background = osConfirmByCart.FirstOrDefault().IsRelease ? bgReleased :
                        osConfirmByCart.FirstOrDefault().IsConfirm ? bgSelected : bgNormal,
                        Child = new TextBlock
                        {
                            Text = wcart.ToString(),
                            FontSize = 20,
                        }
                    }
                };

                wrProductNoList.Children.Add(chkWorkingCart);
                chkWorkingCart.Checked += ChkWorkingCart_Checked;
                chkWorkingCart.Unchecked += ChkWorkingCart_Unchecked; ;
            }
            wrProductNoList.Tag = osmConfirmList;
            loadReleaseDetail(osmConfirmList, osmConfirmList.Where(w => w.IsRelease == true).ToList());
        }
        private void ChkWorkingCart_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isX == true)
                {
                    isX = false;
                    return;
                }

                var chkUncheck = sender as CheckBox;
                var osConfirmByCart = chkUncheck.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                osConfirmByCart.ForEach(f => f.IsRelease = false);
                chkUncheck.Tag = osConfirmByCart;
                releaseSelectedList.RemoveAll(r => osConfirmByCart.Select(s => s.OSCheckingID).Distinct().Contains(r.OSCheckingID));

                Border brChild = chkUncheck.Content as Border;
                brChild.Background = bgNormal;
                if (osConfirmByCart.FirstOrDefault().IsConfirm)
                    brChild.Background = bgSelected;
                if (osConfirmByCart.FirstOrDefault().IsRelease)
                    brChild.Background = bgReleased;

                var confirmByPOList = wrProductNoList.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                loadReleaseDetail(confirmByPOList, releaseSelectedList);
                btnConfirm.Tag = new object[] { confirmByPOList, releaseSelectedList };
            }
            catch { }
        }

        private bool isX = false;
        private void ChkWorkingCart_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var chkCheck = sender as CheckBox;
                var osConfirmByCart = chkCheck.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                var childrenCount = VisualTreeHelper.GetChildrenCount(wrProductNoList);
                var workingCartInfo = osConfirmByCart.FirstOrDefault();
                isX = false;
                if (!osConfirmByCart.FirstOrDefault().IsConfirm)
                {
                    MessageBox.Show(String.Format("This cart: {0} not yet confirm !\nThùng hàng chưa được xác nhận !", workingCartInfo.WorkingCard), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    isX = true;
                    chkCheck.IsChecked = false;
                    return;
                }
                if (osConfirmByCart.FirstOrDefault().IsRelease)
                {
                    MessageBox.Show(String.Format("This cart: {0} has been released !\nThùng hàng đã released !", workingCartInfo.WorkingCard), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    isX = true;
                    chkCheck.IsChecked = false;
                    return;
                }

                osConfirmByCart.ForEach(f => f.IsRelease = true);
                osConfirmByCart.ForEach(f => f.ReleasedTime = DateTime.Now);

                // Highlight
                Border brChild = chkCheck.Content as Border;
                brChild.Background = bgReleased;
                releaseSelectedList.AddRange(osConfirmByCart);
                var confirmByPOList = wrProductNoList.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                loadReleaseDetail(confirmByPOList, releaseSelectedList);
                btnConfirm.Tag = new object[] { confirmByPOList, releaseSelectedList };
                canPressPlus = true;
            }
            catch { }
        }
        private void loadSizeNoList(List<OutsoleMaterialConfirmWorkingCartModel> osmConfirmListByPO)
        {
            int fontSize = 20;
            var sizeNoList = osmConfirmListByPO.Select(s => s.SizeNo).Distinct().ToList();
            var regex = new Regex("[a-z]|[A-Z]");
            sizeNoList = sizeNoList.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

            wrpSizeNoList.Children.Clear();
            gridInfo.DataContext = null;
            var workingCartInfo = osmConfirmListByPO.FirstOrDefault();
            gridInfo.DataContext = new
            {
                ProductNo = workingCartInfo.ProductNo,
                Name = workingCartInfo.Name,
                ArticleNo = workingCartInfo.ArticleNo,
                OutsoleCode = workingCartInfo.OutsoleCode,
                OutsoleLine = workingCartInfo.OutsoleLine,
                OutsoleStartDate = String.Format("{0: dd-MM-yyyy}", workingCartInfo.OutsoleStartDate),
                WorkingCart = workingCartInfo.IsConfirm ? String.Format("IndexNo: {0}  (Confirmed - Đã xác nhận)", workingCartInfo.WorkingCard) :
                                                            String.Format("IndexNo: {0}", workingCartInfo.WorkingCard)
            };
            var bgDisplay = workingCartInfo.IsConfirm ?
                (LinearGradientBrush)TryFindResource("bgConfirm") :
                (LinearGradientBrush)TryFindResource("bgNotConfirm");

            foreach (var sizeNo in sizeNoList)
            {
                StackPanel stk = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(4, 10, 4, 4)
                };
                var qtyPerSize = osmConfirmListByPO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity);
                
                stk.Children.Add(new Border
                {
                    CornerRadius = radius,
                    Background = bgDisplay,
                    Padding = new Thickness(10, 5, 10, 5),
                    Child = new TextBlock
                    {
                        Text = string.Format("#{0}", sizeNo),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = fontSize
                    }
                });
                stk.Children.Add(new Border
                {
                    CornerRadius = radius,
                    Background = bgDisplay,
                    Margin = new Thickness(0, 10, 0, 0),
                    Padding = new Thickness(10, 5, 10, 5),
                    Child = new TextBlock
                    {
                        Text = qtyPerSize.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = fontSize
                    }
                });
                wrpSizeNoList.Children.Add(stk);
            }

            if (sizeNoList.Count() > 1)
            {
                StackPanel stk = new StackPanel();
                stk.VerticalAlignment = VerticalAlignment.Center;
                stk.Children.Add(new TextBlock
                {
                    Text = "Total",
                    FontSize = fontSize,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    
                });
                stk.Children.Add(new TextBlock
                {
                    Text = osmConfirmListByPO.Sum(s => s.Quantity).ToString(),
                    FontSize = fontSize,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                wrpSizeNoList.Children.Add(new Border
                {
                    CornerRadius = radius,
                    Background = bgDisplay,
                    Margin = new Thickness(4, 10, 4, 4),
                    Padding = new Thickness(10, 5, 10, 5),
                    Child = stk
                });
            }

            wrpSizeNoList.ContextMenu = null;
            
            ContextMenu ctMenu = new ContextMenu();
            MenuItem miCancel = new MenuItem();
            miCancel.Foreground = Brushes.Red;
            miCancel.Header = String.Format("Cancel (Hủy xác nhận) Cart: {0}; PO :{1}", workingCartInfo.WorkingCard, workingCartInfo.ProductNo);
            miCancel.Tag = osmConfirmListByPO;
            miCancel.Click += MiCancel_Click;
            ctMenu.Items.Add(miCancel);
            if (workingCartInfo.IsConfirm)
                wrpSizeNoList.ContextMenu = ctMenu;
        }
        private void loadReleaseDetail(List<OutsoleMaterialConfirmWorkingCartModel> confirmListByPO,
                                        List<OutsoleMaterialConfirmWorkingCartModel> releaseList)
        {
            var workingCartInfo = confirmListByPO.FirstOrDefault();
            if (workingCartInfo != null)
                gridInfo.DataContext = new
                {
                    ProductNo = workingCartInfo.ProductNo,
                    Name = workingCartInfo.Name,
                    ArticleNo = workingCartInfo.ArticleNo,
                    OutsoleCode = workingCartInfo.OutsoleCode,
                    OutsoleLine = workingCartInfo.OutsoleLine,
                    OutsoleStartDate = String.Format("{0: dd-MM-yyyy}", workingCartInfo.OutsoleStartDate),
                    WorkingCart = "Release Detail"
                };

            dgReleaseDetail.Columns.Clear();
            var dt = new DataTable();

            DataGridTemplateColumn colTitle = new DataGridTemplateColumn();
            dt.Columns.Add(String.Format("ReleaseDate"), typeof(String));
            colTitle.Header = String.Format("ReleaseDate");
            DataTemplate tplTitle = new DataTemplate();
            FrameworkElementFactory tblTitle = new FrameworkElementFactory(typeof(TextBlock));
            tplTitle.VisualTree = tblTitle;
            tblTitle.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ReleaseDate")));
            tblTitle.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblTitle.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colTitle.CellTemplate = tplTitle;
            colTitle.SortMemberPath = "ReleaseDate";
            colTitle.ClipboardContentBinding = new Binding(String.Format("ReleaseDate"));
            dgReleaseDetail.Columns.Add(colTitle);

            DataGridTemplateColumn colIndexNo = new DataGridTemplateColumn();
            dt.Columns.Add(String.Format("IndexNo"), typeof(String));
            colIndexNo.Header = String.Format("IndexNo");
            DataTemplate tplIndexNo = new DataTemplate();
            FrameworkElementFactory tblIndexNo = new FrameworkElementFactory(typeof(TextBlock));
            tplIndexNo.VisualTree = tblIndexNo;
            tblIndexNo.SetBinding(TextBlock.TextProperty, new Binding(String.Format("IndexNo")));
            tblIndexNo.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblIndexNo.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colIndexNo.CellTemplate = tplIndexNo;
            colIndexNo.SortMemberPath = "IndexNo";
            colIndexNo.ClipboardContentBinding = new Binding(String.Format("IndexNo"));
            dgReleaseDetail.Columns.Add(colIndexNo);

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(String));
                DataGridTemplateColumn colSize = new DataGridTemplateColumn();
                colSize.Header = string.Format("{0}\n{1}\n{2}", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.Quantity);
                colSize.MinWidth = 45;
                DataTemplate tplSize = new DataTemplate();
                FrameworkElementFactory tblSize = new FrameworkElementFactory(typeof(TextBlock));
                tplSize.VisualTree = tblSize;

                tblSize.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", i)));
                tblSize.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblSize.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                colSize.CellTemplate = tplSize;
                colSize.ClipboardContentBinding = new Binding(String.Format("Column{0}", i));
                dgReleaseDetail.Columns.Add(colSize);
            }

            DataGridTemplateColumn colTotalRelease = new DataGridTemplateColumn();
            dt.Columns.Add(String.Format("TotalRelease"), typeof(String));
            colTotalRelease.Header = String.Format("Total\n\n{0}", sizeRunList.Sum(s => s.Quantity));
            DataTemplate tplTotalRelease = new DataTemplate();
            FrameworkElementFactory tblTotalRelease = new FrameworkElementFactory(typeof(TextBlock));
            tplTotalRelease.VisualTree = tblTotalRelease;
            tblTotalRelease.SetBinding(TextBlock.TextProperty, new Binding(String.Format("TotalRelease")));
            tblTotalRelease.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            tblTotalRelease.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            colTotalRelease.CellTemplate = tplTotalRelease;
            colTotalRelease.SortMemberPath = "TotalRelease";
            colTotalRelease.ClipboardContentBinding = new Binding(String.Format("TotalRelease"));
            dgReleaseDetail.Columns.Add(colTotalRelease);

            // Bindind data
            foreach (var cfItem in confirmListByPO)
            {
                var checkReleased = releaseList.FirstOrDefault(f => f.OSCheckingID == cfItem.OSCheckingID);
                if (checkReleased != null)
                {
                    cfItem.IsRelease = true;
                    cfItem.ReleasedTime = checkReleased.ReleasedTime;
                }
            }
            var releasedList = confirmListByPO.Where(w => w.IsRelease == true).ToList();
            var dateList = releasedList.Select(s => s.ReleasedTime?.Date).Distinct().ToList();
            if (dateList.Count() > 0)
                dateList = dateList.OrderBy(o => o).ToList();

            foreach (var date in dateList)
            {
                var releaseByDate = releasedList.Where(w => w.ReleasedTime?.Date == date).ToList();
                var displayFirstDate = false;
                var cartList = releaseByDate.OrderBy(o => o.ReleasedTime).Select(s => s.WorkingCard).Distinct().ToList();
                foreach (var cart in cartList)
                {
                    DataRow dr = dt.NewRow();
                    var releaseByCart = releaseByDate.Where(w => w.WorkingCard == cart).ToList();
                    if (!displayFirstDate)
                        dr["ReleaseDate"] = String.Format("{0:dd/MM/yyyy}", date);
                    dr["IndexNo"] = cart.ToString();

                    var sizeList = releaseByCart.Select(s => s.SizeNo).Distinct().ToList();
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        var sizeRun = sizeRunList[i];
                        var sizeCompare = sizeRun.OutsoleSize != "" ? sizeRun.OutsoleSize : sizeRun.SizeNo;
                        var releaseBySize = releaseByCart.FirstOrDefault(f => f.SizeNo == sizeCompare);
                        if (releaseBySize != null)
                            dr[String.Format("Column{0}", i)] = releaseBySize.Quantity.ToString();

                        var sizeDouble = sizeRunList.Where(w => w.OutsoleSize == sizeCompare).ToList();
                        if (sizeDouble.Count() > 1)
                            i = i + 1;
                    }
                    if (releaseByCart.Count() > 0)
                        dr["TotalRelease"] = releaseByCart.Sum(s => s.Quantity).ToString();

                    displayFirstDate = true;
                    dt.Rows.Add(dr);
                }
            }

            dgReleaseDetail.ItemsSource = dt.AsDataView();
        }
        private void MiCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Confirm Cancel ?\nBạn có chắc muốn hủy ?"), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            try
            {
                var confirmListByPO = btnConfirm.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                if (confirmListByPO == null)
                    return;
                confirmListByPO.ForEach(f => f.IsConfirm = false);
                foreach (var model in confirmListByPO)
                {
                    OutsoleMaterialController.UpdateOSMaterial(model, 1);
                }
                loadSizeNoList(confirmListByPO);
                txtIndexNo.Focus();
                txtIndexNo.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }       
        
        private bool canPressPlus = false;
        private void RadPO_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var radSelected = sender as RadioButton;
                var osConfirmByPO = radSelected.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                var poSelected = osConfirmByPO.FirstOrDefault().ProductNo;
                var childrenCount = VisualTreeHelper.GetChildrenCount(wrProductNoList);

                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(wrProductNoList, i);
                    if (child != null)
                    {
                        RadioButton radChild = child as RadioButton;
                        var checkIsConfirm = radChild.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                        Border brChild = radChild.Content as Border;
                        brChild.Background = bgNormal;
                        if (radChild.IsChecked == true)
                            brChild.Background = bgConfirmSelected;
                        if (checkIsConfirm.FirstOrDefault().IsConfirm == true)
                            brChild.Background = bgSelected;
                    }
                }
                loadSizeNoList(osmConfirmList.Where(w => w.ProductNo.Equals(poSelected)).ToList());

                btnConfirm.Tag = osmConfirmList.Where(w => w.ProductNo.Equals(poSelected)).ToList();

                canPressPlus = true;
            }
            catch { }
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Confirm();
        }
        private void Confirm()
        {
            try
            {
                if (doMode == DoMode.Confirm)
                {
                    var confirmList = btnConfirm.Tag as List<OutsoleMaterialConfirmWorkingCartModel>;
                    if (confirmList == null || confirmList.Count() == 0)
                        return;
                    confirmList.ForEach(f => f.IsConfirm = true);
                    foreach (var model in confirmList)
                    {
                        OutsoleMaterialController.UpdateOSMaterial(model, 1);
                    }
                    MessageBox.Show("Confirmed !\nĐã xác nhận !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    loadSizeNoList(confirmList);
                    loadReadyToRelease(confirmList);
                }
                else if (doMode == DoMode.Release)
                {
                    var obj = btnConfirm.Tag as object[];
                    if (obj == null)
                        return;

                    var confirmByPOList = obj[0] as List<OutsoleMaterialConfirmWorkingCartModel>;
                    var releaseSelectedList = obj[1] as List<OutsoleMaterialConfirmWorkingCartModel>;

                    if (releaseSelectedList.Count() == 0)
                        return;

                    foreach (var cf in confirmByPOList)
                    {
                        var rlItem = releaseSelectedList.FirstOrDefault(f => f.OSCheckingID == cf.OSCheckingID);
                        if (rlItem != null)
                        {
                            cf.IsRelease = true;
                            cf.ReleasedTime = rlItem.ReleasedTime;
                        }
                    }
                    foreach (var updateModel in releaseSelectedList)
                    {
                        OutsoleMaterialController.UpdateOSMaterial(updateModel, 2);
                    }

                    MessageBox.Show("Released !\nĐã phát hàng !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    loadWorkingCart(confirmByPOList);
                    releaseSelectedList.Clear();
                }
                txtIndexNo.Focus();
                txtIndexNo.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void loadReadyToRelease(List<OutsoleMaterialConfirmWorkingCartModel> confirmListByPO)
        {
            var textDisplay = String.Format("{0}-{1}", confirmListByPO.FirstOrDefault().ProductNo, confirmListByPO.FirstOrDefault().WorkingCard);
            Border br = new Border
            {
                CornerRadius = radius,
                Background = bgNormal,
                Margin = new Thickness(4, 10, 4, 4),
                Padding = new Thickness(10, 5, 10, 5),
                Child = new TextBlock { Text = textDisplay }
            };
            wrpReadyToRelease.Children.Add(br);
        }
        private void txtIndexNo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            btnSearch.IsEnabled = true;
            btnSearch.IsDefault = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIndexNo.Focus();
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add && canPressPlus)
            {
                Confirm();
                canPressPlus = false;
            }
        }

        private DoMode doMode = DoMode.Confirm;
        private void radConfirmMode_Checked(object sender, RoutedEventArgs e)
        {
            doMode = DoMode.Confirm;
            grMain.DataContext = new
            {
                InputTitle = "INPUT INDEX.NO ( NHẬP MÃ THÙNG )",
                GroupPOTitle = "ProductNo ( Đơn hàng )",
                ButtonConfirmTitle = "Confirm (Xác Nhận)",
                LabelHintTitle = "Press key Add to confirm (Bấm phím + để xác nhận)"
            };
            //grbReadyToRelease.Visibility = Visibility.Visible;
            if (dgReleaseDetail != null)
                dgReleaseDetail.Visibility = Visibility.Collapsed;
            txtIndexNo.SelectAll();
            txtIndexNo.Focus();
            clearValue();
        }

        private void radReleaseMode_Checked(object sender, RoutedEventArgs e)
        {
            doMode = DoMode.Release;
            grMain.DataContext = new
            {
                InputTitle = "INPUT PRODUCT.NO ( NHẬP ĐƠN HÀNG )",
                GroupPOTitle = "Working Cart ( Thùng hàng )",
                ButtonConfirmTitle = "Release to outsole",
                LabelHintTitle = "Press key Add to release (Bấm phím + để phát hàng)"
            };

            grbReadyToRelease.Visibility = Visibility.Collapsed;
            if (dgReleaseDetail != null)
                dgReleaseDetail.Visibility = Visibility.Visible;
            txtIndexNo.SelectAll();
            txtIndexNo.Focus();
            clearValue();
        }

        private enum DoMode
        {
            Confirm, Release
        }
    }
}
