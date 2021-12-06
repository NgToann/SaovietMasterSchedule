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
    /// Interaction logic for BorrowOutsoleWHMaterialWindow.xaml
    /// </summary>
    public partial class BorrowOutsoleWHMaterialWindow : Window
    {
        OutsoleSuppliersModel supplier;
        List<OutsoleMaterialCheckingModel> oswhCheckList;
        List<OutsoleMaterialCheckingModel> oswhCheckBySupplier;
        public BorrowOutsoleWHMaterialWindow(List<OutsoleMaterialCheckingModel> oswhCheckList, OutsoleSuppliersModel supplier, List<OutsoleMaterialCheckingModel> oswhCheckBySupplier)
        {
            this.supplier = supplier;
            this.oswhCheckList = oswhCheckList;
            this.oswhCheckBySupplier = oswhCheckBySupplier;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var header = lblHeader.Text;
            header = string.Format(header, oswhCheckList.FirstOrDefault().SizeNo, supplier.Name);
            lblHeader.Text = header;

            var qtyBorrowed = oswhCheckList.Sum(s => s.QuantityBorrow);
            if (qtyBorrowed > 0)
            {
                txtQuantity.Focus();
                txtProductNo.Text = oswhCheckList.FirstOrDefault().ProductNoBorrow;
                txtQuantity.Text = qtyBorrowed.ToString();
            }
            else
            {
                txtProductNo.Focus();
                btnReturn.IsEnabled = false;
            }
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            var po      = oswhCheckList.FirstOrDefault().ProductNo;
            var poInput = txtProductNo.Text.Trim().ToUpper().ToString();

            int qtyInput = 0;
            string qtyInputString = txtQuantity.Text.Trim().ToUpper().ToString();
            Int32.TryParse(qtyInputString, out qtyInput);

            if (string.IsNullOrEmpty(poInput))
            {
                MessageBox.Show(string.Format("PO: {0} is invalid !\nĐơn hàng không hợp lệ!", poInput), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }

            if (po.ToUpper().ToString() == poInput)
            {
                MessageBox.Show(string.Format("PO: {0} duplicated !\nĐơn hàng bị trùng!", poInput), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }
            List<OutsoleMaterialCheckingModel> osCheckingByPOList = new List<OutsoleMaterialCheckingModel>();
            List<OutsoleMaterialCheckingModel> osCheckingBorrowedByPOList = new List<OutsoleMaterialCheckingModel>();
            try
            {
                var sizeBorrow              = oswhCheckList.FirstOrDefault().SizeNo;
                osCheckingByPOList          = OutsoleMaterialCheckingController.SelectByPO(poInput).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                osCheckingBorrowedByPOList  = OutsoleMaterialCheckingController.SelectByPOBorrow(poInput).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
                
                if (osCheckingByPOList.Count() == 0)
                {
                    MessageBox.Show(string.Format("PO: {0} is invalid !\nĐơn hàng không hợp lệ!", poInput), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtProductNo.Focus();
                    return;
                }
                if (qtyInput == 0)
                {
                    MessageBox.Show(string.Format("Quantity is invalid !\nSố lượng không hợp lệ!"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtQuantity.Focus();
                    return;
                }

                var qtyBorrowed     = osCheckingBorrowedByPOList.Where(w => w.SizeNo == sizeBorrow).Sum(s => s.QuantityBorrow);
                var qtyCanBorrow    = osCheckingByPOList.Where(w => w.SizeNo == sizeBorrow).Sum(s => s.Quantity);
                var totalReject     = oswhCheckBySupplier.Where(w => w.SizeNo == sizeBorrow).Sum(s => s.Reject);
                var qtyCanInput     = qtyCanBorrow - qtyBorrowed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}", ex.InnerException.Message), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtProductNo.Focus();
                return;
            }

            this.Close();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Confirm return {} pairs size: #{} to PO: {}"), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
        }

        private void txtProductNo_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txtProductNo.SelectAll();
        }
        private void txtQuantity_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txtQuantity.SelectAll();
        }
        private void txtProductNo_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtProductNo.SelectAll();
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
    }
}
