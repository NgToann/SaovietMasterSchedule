using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using MasterSchedule.Controllers;
using MasterSchedule.Models;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleMaterialWHCheckingAddQuantitySizeWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialWHCheckingAddQuantitySizeWindow : Window
    {
        string poSearch;
        BackgroundWorker bwInsert;
        BackgroundWorker bwLoad;

        OutsoleSuppliersModel supplierClicked;
        DateTime checkingDate;
        ErrorModel errorPressed;

        List<OutsoleMaterialCheckingModel> currentOSMCheckListBySupp;
        List<OutsoleMaterialModel> outsoleMaterialListBySupplier;

        private bool modeReject = false;
        private bool modeReturnReject = false;
        public List<OutsoleMaterialCheckingModel> outsoleMaterialCheckingUpdatedBySizeList;
        string workerId;
        List<SizeRunModel> sizeRunList;
        public OutsoleMaterialWHCheckingAddQuantitySizeWindow(string poSearch,
                                                                OutsoleSuppliersModel supplierClicked,
                                                                DateTime checkingDate,
                                                                ErrorModel errorPressed,
                                                                List<OutsoleMaterialCheckingModel> currentOSMCheckListBySupp,
                                                                List<OutsoleMaterialModel> outsoleMaterialListBySupplier,
                                                                string workerId,
                                                                List<SizeRunModel> sizeRunList)
        {
            this.poSearch = poSearch;
            this.supplierClicked = supplierClicked;
            this.checkingDate = checkingDate;
            this.errorPressed = errorPressed;
            this.currentOSMCheckListBySupp = currentOSMCheckListBySupp;
            this.outsoleMaterialListBySupplier = outsoleMaterialListBySupplier;
            this.workerId = workerId;
            this.sizeRunList = sizeRunList;

            outsoleMaterialCheckingUpdatedBySizeList = new List<OutsoleMaterialCheckingModel>();

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += BwInsert_DoWork;
            bwInsert.RunWorkerCompleted += BwInsert_RunWorkerCompleted;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

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
            try
            {
                currentOSMCheckListBySupp = OutsoleMaterialCheckingController.SelectByPO(poSearch).Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            }
            catch
            {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show("An error occured when execute data !\nPlease try again !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            tblProductNo.Text = poSearch;
            tblSupplier.Text = supplierClicked.Name;
            tblCheckingDate.Text = String.Format("{0:dd/MM/yyyy}", checkingDate);
            tblWorkerId.Text = workerId;
            brGroupInfor.Background = Brushes.Green;

            tblWorkingCard.Visibility = Visibility.Visible;
            txtWorkingCard.Visibility = Visibility.Visible;

            // Case Input Reject
            if (errorPressed.ErrorID > 0)
            {
                tblDefectTitle.Text = "Defect: (Lỗi)";
                tblDefect.Text = String.Format("{0} - {1}", errorPressed.ErrorName, errorPressed.ErrorVietNamese);
                tblQtyOrQtyReject.Text = "Reject (Hàng phế)";
                brGroupInfor.Background = Brushes.Yellow;
                tblGroupInfor.Text = "Input Size and Reject (Nhập hàng phế)";
                modeReject = true;

                this.Width = 450;

                tblWorkingCard.Visibility = Visibility.Collapsed;
                txtWorkingCard.Visibility = Visibility.Collapsed;
                tblReturnRemark.Visibility = Visibility.Collapsed;
                txtReturnRemark.Visibility = Visibility.Collapsed;
                txtWorkingCard.IsEnabled = false;
                txtReturnRemark.IsEnabled = false;
            }

            // Case Return Reject
            if (errorPressed.ErrorID < 0)
            {
                tblQtyOrQtyReject.Text = "Qty OK\n(Hàng bù OK)";
                brGroupInfor.Background = Brushes.Tomato;
                tblGroupInfor.Text = "Input Size and Quantity Return (Nhập hàng bù)";
                modeReturnReject = true;

                //brReturnRemark.Visibility = Visibility.Visible;
                this.Width = 480;
                //this.Height = 330;

                tblWorkingCard.Visibility = Visibility.Collapsed;
                txtWorkingCard.Visibility = Visibility.Collapsed;
                txtWorkingCard.IsEnabled = false;
                txtReturnRemark.Visibility = Visibility.Visible;
                tblReturnRemark.Visibility = Visibility.Visible;
            }

            txtSizeNo.Focus();
            txtSizeNo.SelectAll();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        string sizeNoInputted = "";
        private void txtSizeNo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (CheckSizeInput() == false)
                    return;

                txtQuantity.Focus();
                txtQuantity.SelectAll();
            }
        }

        private int qtyDeliveryBySize = 0;

        private bool CheckSizeInput()
        {
            sizeNoInputted = txtSizeNo.Text.Trim().ToUpper().ToString();
            if (String.IsNullOrEmpty(sizeNoInputted))
            {
                MessageBox.Show(String.Format("Size Empty !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtSizeNo.Focus();
                txtSizeNo.SelectAll();
                return false;
            }

            var outsoleSizeList = sizeRunList.Where(w => !String.IsNullOrEmpty(w.OutsoleSize)).Select(s => s.OutsoleSize).ToList();
            var orderSizeList = sizeRunList.Select(s => s.SizeNo).ToList();

            var outsoleSizeBySizeInputted = outsoleSizeList.FirstOrDefault(f => f == sizeNoInputted);
            if (outsoleSizeList.Count() == orderSizeList.Count() &&
                outsoleSizeBySizeInputted == null)
            {
                MessageBox.Show(String.Format("Size: {0} does not exist in OutsoleSizeList !", sizeNoInputted), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtSizeNo.Focus();
                txtSizeNo.SelectAll();
                return false;
            }

            string sizeToCompare = "";
            var sizeRunByOutsoleSize = sizeRunList.FirstOrDefault(f => f.OutsoleSize == sizeNoInputted);
            var sizeRunByOrderSize = sizeRunList.FirstOrDefault(f => f.SizeNo == sizeNoInputted);

            if (sizeRunByOutsoleSize != null)
                sizeToCompare = sizeRunByOutsoleSize.SizeNo;
            else if (sizeRunByOrderSize != null)
                sizeToCompare = sizeRunByOrderSize.SizeNo;

            qtyDeliveryBySize = outsoleMaterialListBySupplier.Where(w => w.SizeNo == sizeToCompare).Sum(s => s.Quantity);

            var osSizeList = sizeRunList.Where(w => w.OutsoleSize == sizeNoInputted).ToList();
            if (osSizeList.Count() > 1)
            {
                qtyDeliveryBySize = 0;
                foreach (var osSize in osSizeList)
                {
                    qtyDeliveryBySize += outsoleMaterialListBySupplier.Where(w => w.SizeNo == osSize.SizeNo).Sum(s => s.Quantity);
                }
            }

            if (qtyDeliveryBySize == 0)
            {
                MessageBox.Show(String.Format("Size: {0} incorrect or not yet delivery !", sizeNoInputted), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtSizeNo.Focus();
                txtSizeNo.SelectAll();
                return false;
            }
            else
                return true;
        }

        int qtyInputted = 0;
        private void txtQuantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (CheckSizeInput() == false || CheckQuantityInput() == false)
                    return;

                // Case Input or Reject
                if (modeReturnReject == false)
                {
                    if (modeReject == false)
                    {
                        txtWorkingCard.Focus();
                        txtWorkingCard.SelectAll();
                    }
                    else
                        InsertModel(-999);
                }
                // Case Return Reject
                if (modeReturnReject == true)
                {
                    txtReturnRemark.Focus();
                    txtReturnRemark.SelectAll();
                    //InsertModel(-1);
                }
            }
        }

        private bool CheckQuantityInput()
        {
            // Check Incorrect
            Int32.TryParse(txtQuantity.Text.Trim().ToString(), out qtyInputted);
            if (modeReturnReject == false && qtyInputted == 0)
            {
                MessageBox.Show(String.Format("Quantity: {0} Incorrect !\nSố lượng: {1} không đúng !", txtQuantity.Text, txtQuantity.Text), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return false;
            }
            var totalQtyCheckCurrentBySize = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.Quantity);
            var totalRejectCurrentBySize = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.Reject);
            var totalReturnRejectCurrentBySize = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.ReturnReject);

            // Case Reject Or Input Qty
            if (modeReturnReject == false)
            {
                if ((modeReject == false && totalQtyCheckCurrentBySize + qtyInputted > qtyDeliveryBySize) ||
                    (modeReject == true && totalRejectCurrentBySize + qtyInputted > qtyDeliveryBySize))
                {
                    MessageBox.Show(String.Format("Quantity: {0} incorrect or greater than quantity delivery !\nSố lượng: {1} không đúng hoặc lớn hơn hàng về", qtyInputted, qtyInputted), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtQuantity.Focus();
                    txtQuantity.SelectAll();
                    return false;
                }
            }

            // Case Return Reject
            if (modeReturnReject == true && totalReturnRejectCurrentBySize + qtyInputted > totalRejectCurrentBySize)
            {
                MessageBox.Show(String.Format("Quantity Return: {0} excess quantity reject {1} !\nHàng bù: {2} lớn hơn số lượng hàng phế {3}", qtyInputted, totalRejectCurrentBySize, qtyInputted, totalRejectCurrentBySize), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return false;
            }

            // Excess Quantity Delivery
            if (modeReturnReject == true && totalQtyCheckCurrentBySize + qtyInputted > qtyDeliveryBySize)
            {
                MessageBox.Show(String.Format("Quantity (Return + Check): {0} excess quantity delivery {1} !\nTổng hàng đã OK (Hàng bù + Hàng OK): {2} lớn hơn số lượng hàng về {3}",
                                                totalQtyCheckCurrentBySize + qtyInputted, qtyDeliveryBySize,
                                                totalQtyCheckCurrentBySize + qtyInputted, qtyDeliveryBySize), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return false;
            }
            return true;
        }

        private void txtWorkingCard_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (CheckSizeInput() == false)
                    return;
                if (CheckQuantityInput() == false)
                    return;

                int workingCardInputted = 0;
                Int32.TryParse(txtWorkingCard.Text.Trim().ToString(), out workingCardInputted);
                if (workingCardInputted <= 0)
                {
                    MessageBox.Show(String.Format("Working Cart(Index No): {0} incorrect !\nSố thùng (IndexNo): {1} không đúng !",
                        txtWorkingCard.Text, txtWorkingCard.Text), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtWorkingCard.Focus();
                    txtWorkingCard.SelectAll();
                    return;
                }

                InsertModel(workingCardInputted);
            }
        }

        private void InsertModel(int workingCart)
        {
            int qty = 0, reject = 0, returnReject = 0, returnRemark = 0;
            if (modeReturnReject == true)
            {
                returnReject = qtyInputted;
                Int32.TryParse(txtReturnRemark.Text.Trim().ToString(), out returnRemark);
            }
            else
            {
                if (modeReject == true)
                    reject = qtyInputted;
                else
                    qty = qtyInputted;
            }

            var osmCheckCurrent = new OutsoleMaterialCheckingModel
            {
                ProductNo = poSearch,
                WorkerId = workerId,
                CheckingDate = checkingDate,
                OutsoleSupplierId = supplierClicked.OutsoleSupplierId,
                SizeNo = sizeNoInputted,
                Quantity = qty,
                Reject = reject,
                ReturnReject = returnReject,
                ReturnRemark = returnRemark,
                ErrorId = errorPressed.ErrorID,
                WorkingCard = workingCart,
                UpdateReject = modeReject,
                UpdateQuantity = !modeReject,
                UpdateReturnReject = modeReturnReject
            };

            if (bwInsert.IsBusy == false)
            {
                var tempValidateList = currentOSMCheckListBySupp.ToList();
                tempValidateList.Add(osmCheckCurrent);

                // Case Input or Reject
                if (modeReject == false && modeReturnReject == false)
                {
                    osmCheckCurrent.Quantity = tempValidateList.Where(w => w.SizeNo == osmCheckCurrent.SizeNo &&
                                                                            w.CheckingDate == osmCheckCurrent.CheckingDate &&
                                                                            w.WorkingCard == osmCheckCurrent.WorkingCard).Sum(s => s.Quantity);
                    if (MessageBox.Show(string.Format("Confirm add(update) record ?\n Size  : {0}\n Qty   : {1}\n Working Cart(Index No): {2}", osmCheckCurrent.SizeNo, osmCheckCurrent.Quantity, osmCheckCurrent.WorkingCard), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                    {
                        txtSizeNo.Focus();
                        txtSizeNo.SelectAll();
                        return;
                    }
                    if (osmCheckCurrent.Quantity < 0)
                    {
                        MessageBox.Show("Quantity cannot be < 0 !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();
                        return;
                    }
                }
                else if (modeReject == true && modeReturnReject == false)
                {
                    osmCheckCurrent.Reject = tempValidateList.Where(w => w.SizeNo == osmCheckCurrent.SizeNo &&
                                                                            w.CheckingDate == osmCheckCurrent.CheckingDate &&
                                                                            w.WorkingCard == osmCheckCurrent.WorkingCard &&
                                                                            w.ErrorId == osmCheckCurrent.ErrorId).Sum(s => s.Reject);
                    if (MessageBox.Show(string.Format("Confirm add(update) record ?\nSize     : {0}\nQty      : {1}\nReject  : {2} - {3}", osmCheckCurrent.SizeNo, osmCheckCurrent.Reject, errorPressed.ErrorName, errorPressed.ErrorVietNamese), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                    {
                        txtSizeNo.Focus();
                        txtSizeNo.SelectAll();
                        return;
                    }
                    if (osmCheckCurrent.Reject < 0)
                    {
                        MessageBox.Show("Reject cannot be < 0 !\nHàng phế < 0 !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();
                        return;
                    }
                }

                // Case Return Reject
                if (modeReturnReject == true)
                {
                    osmCheckCurrent.ReturnReject = tempValidateList.Where(w => w.SizeNo == osmCheckCurrent.SizeNo &&
                                                                            w.CheckingDate == osmCheckCurrent.CheckingDate &&
                                                                            w.WorkingCard == osmCheckCurrent.WorkingCard).Sum(s => s.ReturnReject);
                    if (osmCheckCurrent.ReturnReject < 0)
                    {
                        MessageBox.Show("Return Reject cannot be < 0 !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();
                        return;
                    }

                    if (MessageBox.Show(string.Format("Confirm add (update) record ?\nSize                   : {0}\nQtyReturn OK  : {1}\nQtyRemark       : {2}",
                                                        osmCheckCurrent.SizeNo,
                                                        osmCheckCurrent.ReturnReject,
                                                        osmCheckCurrent.ReturnRemark), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                    {
                        txtSizeNo.Focus();
                        txtSizeNo.SelectAll();
                        return;
                    }
                }

                this.Cursor = Cursors.Wait;
                txtQuantity.IsEnabled = false;
                txtSizeNo.IsEnabled = false;

                currentOSMCheckListBySupp.Add(osmCheckCurrent);
                object[] par = new object[] { osmCheckCurrent, currentOSMCheckListBySupp };
                bwInsert.RunWorkerAsync(par);
            }
        }

        private void BwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            var par = e.Argument as object[];
            var osmCheckCurrent = par[0] as OutsoleMaterialCheckingModel;
            var currentOSMCheckListBySupp = par[1] as List<OutsoleMaterialCheckingModel>;
            try
            {
                OutsoleMaterialCheckingController.Insert(osmCheckCurrent);
                // Update Reject to WHMasterFile.
                var osMaterialCheckByPO = OutsoleMaterialCheckingController.SelectByPOSumBySize(osmCheckCurrent.ProductNo).ToList();
                foreach (var sizeRun in sizeRunList)
                {
                    var osMaterialCheckByPOBySupplier = osMaterialCheckByPO.Where(w => w.OutsoleSupplierId == osmCheckCurrent.OutsoleSupplierId).ToList();

                    var sizeCompare = String.IsNullOrEmpty(sizeRun.OutsoleSize) == false ? sizeRun.OutsoleSize : sizeRun.SizeNo;
                    var osMatCheckBySize = osMaterialCheckByPOBySupplier.FirstOrDefault(f => f.SizeNo == sizeCompare);

                    int rejectUpdate = 0;
                    if (osMatCheckBySize != null)
                        rejectUpdate = osMatCheckBySize.Reject - osMatCheckBySize.ReturnReject > 0 ? osMatCheckBySize.Reject - osMatCheckBySize.ReturnReject : 0;
                    var osMaterialUpdate = new OutsoleMaterialModel()
                    {
                        ProductNo = osmCheckCurrent.ProductNo,
                        OutsoleSupplierId = osmCheckCurrent.OutsoleSupplierId,
                        SizeNo = sizeRun.SizeNo,
                        QuantityReject = rejectUpdate
                    };
                    OutsoleMaterialController.UpdateByOSCheck(osMaterialUpdate);
                }

                // Tranfer to mainwindow sum order by size
                // Update current list
                currentOSMCheckListBySupp.RemoveAll(r => r.SizeNo == osmCheckCurrent.SizeNo &&
                                                         r.CheckingDate == osmCheckCurrent.CheckingDate &&
                                                         r.WorkingCard == osmCheckCurrent.WorkingCard);

                currentOSMCheckListBySupp.Add(osmCheckCurrent);
                if (modeReject == false && modeReturnReject == false)
                    osmCheckCurrent.Quantity = currentOSMCheckListBySupp.Where(w => w.SizeNo == osmCheckCurrent.SizeNo &&
                                                                                    w.CheckingDate == osmCheckCurrent.CheckingDate).Sum(s => s.Quantity);
                else if (modeReject == true && modeReturnReject == false)
                    osmCheckCurrent.Reject = currentOSMCheckListBySupp.Where(w => w.SizeNo == osmCheckCurrent.SizeNo &&
                                                                                    w.CheckingDate == osmCheckCurrent.CheckingDate).Sum(s => s.Reject);
                if (modeReturnReject == true)
                    osmCheckCurrent.ReturnReject = currentOSMCheckListBySupp.Where(w => w.SizeNo == osmCheckCurrent.SizeNo &&
                                                                                    w.CheckingDate == osmCheckCurrent.CheckingDate).Sum(s => s.ReturnReject);
                outsoleMaterialCheckingUpdatedBySizeList.Add(osmCheckCurrent);
            }
            catch
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show("An error occured when execute data !\nPlease try again !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void BwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            this.Close();
        }

        private void txtReturnRemark_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrEmpty(sizeNoInputted))
            {
                MessageBox.Show(String.Format("SizeNo is empty !\nChưa nhập SizeNo !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtSizeNo.Focus();
                txtSizeNo.SelectAll();
                return;
            }
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                var rejectCurrent = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.Reject);
                var returnCurrent = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.ReturnReject);
                int qtyRemark = 0;
                Int32.TryParse(txtReturnRemark.Text.Trim().ToString(), out qtyRemark);

                int qtyReturn = 0;
                Int32.TryParse(txtQuantity.Text.Trim().ToString(), out qtyReturn);
                if (qtyRemark < 0)
                {
                    MessageBox.Show(String.Format("Remark: {0} incorrect !\nSố lượng Remark {1} không đúng", txtWorkingCard.Text, txtWorkingCard.Text), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtWorkingCard.Focus();
                    txtWorkingCard.SelectAll();
                    return;
                }

                if (qtyRemark + qtyReturn + returnCurrent > rejectCurrent)
                {
                    MessageBox.Show(String.Format("Remark + Return = {0} can not be > Reject {1}!\nHàng bù + Hàng remark = {2} > Tổng hàng phế: {3} !",
                        qtyRemark + qtyReturn + returnCurrent,
                        rejectCurrent,
                        qtyRemark + qtyReturn + returnCurrent,
                        rejectCurrent),
                        this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtReturnRemark.Focus();
                    txtReturnRemark.SelectAll();
                    return;
                }
                InsertModel(-1);
            }
        }
    }
}
