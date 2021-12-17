using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using MasterSchedule.Controllers;
using MasterSchedule.Models;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for BorrowOutsoleWHMaterialWindow.xaml
    /// </summary>
    public partial class BorrowOutsoleWHMaterialWindow : Window
    {
        OutsoleSuppliersModel supplier;
        List<OutsoleMaterialCheckingModel> oswhCheckList;
        List<OutsoleMaterialCheckingModel> oswhCheckBySupplier;
        List<OSMaterialBorrowModel> currentBorrowedList;
        private List<OrdersModel> orderList;
        public List<OutsoleMaterialCheckingModel> oswhAfterBorrow;
        //public List<OSMaterialBorrowModel> oswhBorrowedList;
        private string sizeBorrow = "";
        private string poTransfer = "";
        private int osCheckingId = 0;
        public BorrowOutsoleWHMaterialWindow(List<OutsoleMaterialCheckingModel> oswhCheckList, OutsoleSuppliersModel supplier, List<OutsoleMaterialCheckingModel> oswhCheckBySupplier)
        {
            this.supplier = supplier;
            this.oswhCheckList = oswhCheckList;
            this.oswhCheckBySupplier = oswhCheckBySupplier;

            oswhAfterBorrow = new List<OutsoleMaterialCheckingModel>();
            //oswhBorrowedList = new List<OSMaterialBorrowModel>();
            currentBorrowedList = new List<OSMaterialBorrowModel>();

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var header = lblHeader.Text;
            header = string.Format(header, oswhCheckList.FirstOrDefault().SizeNo, supplier.Name);
            lblHeader.Text = header;

            sizeBorrow = oswhCheckList.FirstOrDefault().SizeNo;
            poTransfer = oswhCheckList.FirstOrDefault().ProductNo;
            osCheckingId = oswhCheckList.FirstOrDefault().OSCheckingId;

            try
            {
                currentBorrowedList = OSMaterialBorrowController.GetByOSCheckingId(osCheckingId).Where(w => !string.IsNullOrEmpty(w.ProductNoBorrow)).ToList();
                orderList = OrdersController.Select();
                var poList = currentBorrowedList.Select(s => s.ProductNoBorrow).Distinct().ToList();
                if (poList.Count() > 1)
                {
                    stkPOBorrow.Visibility = Visibility.Visible;
                    stkPOBorrow.Margin = new Thickness(0, 5, 0, 0);
                    int nameOfCheckBox = 0;
                    foreach (var po in poList)
                    {
                        var borowedByPO = currentBorrowedList.Where(w => w.ProductNoBorrow == po).ToList();

                        var checkBox = new CheckBox();
                        checkBox.Content = $"Chọn: {po}";
                        checkBox.Name = $"chk{nameOfCheckBox}";
                        checkBox.Margin = new Thickness(0, 0, 15, 0);
                        checkBox.FontSize = 19;
                        checkBox.FontStyle = FontStyles.Italic;
                        checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                        checkBox.Tag = borowedByPO;
                        checkBox.Cursor = Cursors.Hand;

                        //var checkBox = new CheckBox()
                        //{
                        //    Content = $"Chọn {po}",
                        //    Name = $"chk{po}",
                        //    Margin = new Thickness(0, 10, 0, 0),
                        //    VerticalContentAlignment = VerticalAlignment.Center,
                        //    Tag = borowedByPO,
                        //};

                        checkBox.Checked += CheckBox_Checked;
                        stkPOBorrow.Children.Add(checkBox);
                        nameOfCheckBox++;
                    }
                }
                else if (poList.Count() == 1)
                {
                    txtProductNo.Text   = poList.FirstOrDefault();
                    txtQuantity.Text    = currentBorrowedList.Sum(s => s.QuantityBorrow).ToString();
                    displayArticleNo(poList.FirstOrDefault(), orderList);
                }
                else
                {
                    txtProductNo.Focus();
                    btnReturn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.InnerException.Message}", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var checkBoxChecked = sender as CheckBox;
                var childrenCount = VisualTreeHelper.GetChildrenCount(stkPOBorrow);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(stkPOBorrow, i);
                    if (child != null)
                    {
                        CheckBox chkChild = child as CheckBox;
                        if (checkBoxChecked.Name != chkChild.Name)
                            chkChild.IsChecked = false;
                    }
                }
                var borrowedList = checkBoxChecked.Tag as List<OSMaterialBorrowModel>;
                txtQuantity.Text = borrowedList.Sum(s => s.QuantityBorrow).ToString();
                txtProductNo.Text = borrowedList.FirstOrDefault().ProductNoBorrow;
                displayArticleNo(borrowedList.FirstOrDefault().ProductNoBorrow, orderList);
                txtQuantity.Focus();
            }
            catch { }
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            var po = oswhCheckList.FirstOrDefault().ProductNo;
            var poInput = txtProductNo.Text.Trim().ToUpper().ToString();

            int qtyInput = 0;
            string qtyInputString = txtQuantity.Text.Trim().ToUpper().ToString();
            Int32.TryParse(qtyInputString, out qtyInput);

            if (string.IsNullOrEmpty(poInput))
            {
                MessageBox.Show(string.Format("PO {0} is invalid !\nĐơn hàng không hợp lệ!", poInput), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }

            if (po.ToUpper().ToString() == poInput)
            {
                MessageBox.Show(string.Format("PO {0} duplicated !\nĐơn hàng bị trùng!", poInput), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }

            var osCheckingByPOList = new List<OutsoleMaterialCheckingModel>();
            var osMaterialBorrowed = new List<OSMaterialBorrowModel>();
            try
            {
                if (qtyInput == 0)
                {
                    goto TheEnd;
                }

                osCheckingByPOList          = OutsoleMaterialCheckingController.SelectByPO(poInput).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                if (osCheckingByPOList.Count() == 0)
                {
                    MessageBox.Show(string.Format("PO {0} is invalid !\nĐơn hàng không hợp lệ!", poInput), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtProductNo.Focus();
                    return;
                }
                //osCheckingBorrowedByPOList  = OutsoleMaterialCheckingController.SelectByPOBorrow(poInput).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                osMaterialBorrowed = OSMaterialBorrowController.GetByPO(poInput).Where(w => w.OSCheckingId == osCheckingId).ToList();
                var qtyBorrowed     = osMaterialBorrowed.Sum(s => s.QuantityBorrow);
                var qtyCanBorrow    = osCheckingByPOList.Where(w => w.SizeNo == sizeBorrow).Sum(s => s.Quantity + s.ReturnReject);
                var totalReject     = oswhCheckBySupplier.Where(w => w.SizeNo == sizeBorrow).Sum(s => s.Reject);
                var qtyCanInput     = qtyCanBorrow - qtyBorrowed;

                if (qtyInput + qtyBorrowed > totalReject)
                {
                    goto TheEnd;
                }
                if (qtyInput > qtyCanInput)
                {
                    goto TheEnd;
                }

                if (MessageBox.Show($"Confirm borrow {qtyInput} pairs size: #{sizeBorrow} from PO: {poInput} ?\nXác nhận mượn Outsole Material", this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    txtQuantity.Focus();
                    return;
                }

                // Update Record DB
                var borrowItem = new OSMaterialBorrowModel
                {
                    OSCheckingId = osCheckingId,
                    ProductNoBorrow = poInput,
                    QuantityBorrow = qtyInput
                };
                OSMaterialBorrowController.Insert(borrowItem);
                //oswhBorrowedList = OSMaterialBorrowController.GetByOSCheckingId(osCheckingId);
                oswhAfterBorrow = OutsoleMaterialCheckingController.SelectByPO_1(poTransfer).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                
                this.Close();
                return;
            TheEnd:
                {
                    MessageBox.Show(string.Format("Quantity is invalid !\nSố lượng không hợp lệ!"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtQuantity.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.InnerException.Message}", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var poInput = txtProductNo.Text.Trim().ToUpper().ToString();
            int qtyReturn = 0;
            string qtyInputString = txtQuantity.Text.Trim().ToUpper().ToString();
            Int32.TryParse(qtyInputString, out qtyReturn);

            if (qtyReturn == 0)
            {
                MessageBox.Show(string.Format("Quantity is invalid !\nSố lượng không hợp lệ!"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtQuantity.Focus();
                return;
            }

            if (MessageBox.Show($"Confirm return {qtyReturn} pairs size: #{sizeBorrow} to PO: {poInput} ?\nXác nhận trả Outsole Material ?", this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                txtQuantity.Focus();
                return;
            }

            try
            {
                var updateRecords = currentBorrowedList.Where(w => w.ProductNoBorrow == poInput).ToList();                
                foreach (var updateRecord in updateRecords)
                {
                    int qtyBorrowBeforeUpdate = updateRecord.QuantityBorrow;
                    if (qtyReturn > 0)
                    {
                        var qtyAfterReturn = updateRecord.QuantityBorrow - qtyReturn;
                        updateRecord.QuantityBorrow = qtyAfterReturn < 0 ? 0 : qtyAfterReturn;
                        if (updateRecord.QuantityBorrow == 0)
                            updateRecord.ProductNoBorrow = "";
                        OSMaterialBorrowController.Update(updateRecord);
                    }
                    qtyReturn -= qtyBorrowBeforeUpdate;
                }

                //oswhBorrowedList = OSMaterialBorrowController.GetByOSCheckingId(osCheckingId);

                // Update DB
                //foreach (var item in oswhCheckList)
                //{
                //    int qtyAfterReturn = item.QuantityBorrow - qtyReturn;
                //    item.QuantityBorrow = qtyAfterReturn < 0 ? 0 : qtyAfterReturn;
                //    if (item.QuantityBorrow == 0)
                //        item.ProductNoBorrow = "";
                //    OutsoleMaterialCheckingController.updateBorrow(item);
                //}
                oswhAfterBorrow = OutsoleMaterialCheckingController.SelectByPO_1(poTransfer).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.InnerException.Message}", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }
        }

        private void txtProductNo_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //txtProductNo.SelectAll();
        }
        private void txtProductNo_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //txtProductNo.SelectAll();
        }
        private void txtQuantity_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txtQuantity.SelectAll();
        }
        private void txtQuantity_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtQuantity.SelectAll();
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        
        private void txtProductNo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtQuantity.Focus();
            }
        }

        private void txtQuantity_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (btnReturn.IsEnabled)
                {
                    btnReturn.IsDefault = true;
                }
                else
                    btnBorrow.IsDefault = true;
            }
        }

        private void txtProductNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var inputWhat = txtProductNo.Text.Trim().ToUpper().ToString();
            displayArticleNo(inputWhat, orderList);
        }
        private void displayArticleNo (string po, List<OrdersModel> orderList)
        {
            lblArticleNo.Text = "";
            var orderByPO = orderList.Where(w => w.ProductNo.ToUpper().Equals(po)).FirstOrDefault();
            if (orderByPO != null)
            {
                lblArticleNo.Text = $"ArticleNo (Kiểu giày): {orderByPO.ArticleNo}";
            }
        }
    }
}
