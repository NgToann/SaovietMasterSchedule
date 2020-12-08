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
            this.poSearch                       = poSearch;
            this.supplierClicked                = supplierClicked;
            this.checkingDate                   = checkingDate;
            this.errorPressed                   = errorPressed;
            this.currentOSMCheckListBySupp      = currentOSMCheckListBySupp;
            this.outsoleMaterialListBySupplier  = outsoleMaterialListBySupplier;
            this.workerId                       = workerId;
            this.sizeRunList                    = sizeRunList;

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
            this.Cursor             = null;
            tblProductNo.Text       = poSearch;
            tblSupplier.Text        = supplierClicked.Name;
            tblCheckingDate.Text    = String.Format("{0:dd/MM/yyyy}", checkingDate);
            tblWorkerId.Text        = workerId;
            brGroupInfor.Background = Brushes.Green;

            tblWorkingCard.Visibility = Visibility.Visible;
            txtWorkingCard.Visibility = Visibility.Visible;

            // Case Reject
            if (errorPressed.ErrorID != 0)
            {
                // Case Input Reject
                if (errorPressed.ErrorID != -1)
                {
                    tblDefectTitle.Text         = "Defect: ";
                    tblDefect.Text              = String.Format("{0} - {1}", errorPressed.ErrorName, errorPressed.ErrorVietNamese);                    
                    tblQtyOrQtyReject.Text      = "Reject";
                    brGroupInfor.Background     = Brushes.Yellow;
                    tblGroupInfor.Text          = "    Input Size and Reject    ";
                    modeReject                  = true;
                }
                // Case Return Reject
                else
                {
                    tblQtyOrQtyReject.Text      = "Qty Return";
                    brGroupInfor.Background     = Brushes.Tomato;
                    tblGroupInfor.Text          = "    Input Size and Quantity Return    ";
                    modeReturnReject            = true;
                }
                tblWorkingCard.Visibility   = Visibility.Collapsed;
                txtWorkingCard.Visibility   = Visibility.Collapsed;
                this.Width                  = 450;
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
            if(e.Key == Key.Enter || e.Key == Key.Tab)
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
            var orderSizeList   = sizeRunList.Select(s => s.SizeNo).ToList();

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
            var sizeRunByOrderSize  = sizeRunList.FirstOrDefault(f => f.SizeNo == sizeNoInputted);

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
                    InsertModel(-1);
            }
        }

        private bool CheckQuantityInput()
        {            
            // Check Incorrect
            Int32.TryParse(txtQuantity.Text.Trim().ToString(), out qtyInputted);
            if (qtyInputted == 0)
            {
                MessageBox.Show(String.Format("Quantity: {0} incorrect or greater than quantity delivery !", txtQuantity.Text.Trim().ToString()), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return false;
            }
            var totalQtyCheckCurrentBySize      = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.Quantity);
            var totalRejectCurrentBySize        = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.Reject);
            var totalReturnRejectCurrentBySize  = currentOSMCheckListBySupp.Where(w => w.SizeNo == sizeNoInputted).Sum(s => s.ReturnReject);
            
            // Case Reject Or Input Qty
            if (modeReturnReject == false)
            {
                if ((modeReject == false && totalQtyCheckCurrentBySize + qtyInputted > qtyDeliveryBySize) ||
                    (modeReject == true && totalRejectCurrentBySize + qtyInputted > qtyDeliveryBySize))
                {
                    MessageBox.Show(String.Format("Quantity: {0} incorrect or greater than quantity delivery !", qtyInputted), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtQuantity.Focus();
                    txtQuantity.SelectAll();
                    return false;
                }
            }
            
            // Case Return Reject
            if (modeReturnReject == true && totalReturnRejectCurrentBySize + qtyInputted > totalRejectCurrentBySize)
            {
                MessageBox.Show(String.Format("Quantity Return: {0} excess quantity reject {1} !", qtyInputted, totalRejectCurrentBySize), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return false;
            }

            // Excess Quantity Delivery
            if (modeReturnReject == true && totalQtyCheckCurrentBySize + qtyInputted > qtyDeliveryBySize)
            {
                MessageBox.Show(String.Format("Quantity (Return + Check): {0}\nexcess quantity delivery {1} !", totalQtyCheckCurrentBySize + qtyInputted, qtyDeliveryBySize), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show(String.Format("Working Cart(Index No): {0} incorrect !", txtWorkingCard.Text.Trim().ToString()), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    txtWorkingCard.Focus();
                    txtWorkingCard.SelectAll();
                    return;
                }

                InsertModel(workingCardInputted);
            }
        }
        
        private void InsertModel (int workingCart)
        {
            int qty = 0, reject = 0, returnReject = 0;
            if (modeReturnReject == true)
                returnReject = qtyInputted;
            else
            {
                if (modeReject == true)
                    reject = qtyInputted;
                else
                    qty = qtyInputted;
            }

            var osmCheckCurrent = new OutsoleMaterialCheckingModel
            {
                ProductNo               = poSearch,
                WorkerId                = workerId,
                CheckingDate            = checkingDate,
                OutsoleSupplierId       = supplierClicked.OutsoleSupplierId,
                SizeNo                  = sizeNoInputted,
                Quantity                = qty,
                Reject                  = reject,
                ReturnReject            = returnReject,
                ErrorId                 = errorPressed.ErrorID,
                WorkingCard             = workingCart,
                UpdateReject            = modeReject,
                UpdateQuantity          = !modeReject,
                UpdateReturnReject      = modeReturnReject
            };

            if (bwInsert.IsBusy == false)
            {
                var tempValidateList = currentOSMCheckListBySupp.ToList();
                tempValidateList.Add(osmCheckCurrent);
                
                // Case Input or Reject
                if (modeReject == false && modeReturnReject == false)
                {
                    osmCheckCurrent.Quantity = tempValidateList.Where(w => w.SizeNo         == osmCheckCurrent.SizeNo &&
                                                                            w.CheckingDate  == osmCheckCurrent.CheckingDate &&
                                                                            w.WorkingCard   == osmCheckCurrent.WorkingCard).Sum(s => s.Quantity);
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
                    osmCheckCurrent.Reject = tempValidateList.Where(w =>    w.SizeNo        == osmCheckCurrent.SizeNo &&
                                                                            w.CheckingDate  == osmCheckCurrent.CheckingDate &&
                                                                            w.WorkingCard   == osmCheckCurrent.WorkingCard &&
                                                                            w.ErrorId       == osmCheckCurrent.ErrorId).Sum(s => s.Reject);
                    if (MessageBox.Show(string.Format("Confirm add(update) record ?\n Size     : {0}\n Qty      : {1}\n Reject  : {2} - {3}", osmCheckCurrent.SizeNo, osmCheckCurrent.Reject, errorPressed.ErrorName, errorPressed.ErrorVietNamese), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                    {
                        txtSizeNo.Focus();
                        txtSizeNo.SelectAll();
                        return;
                    }
                    if (osmCheckCurrent.Reject < 0)
                    {
                        MessageBox.Show("Reject cannot be < 0 !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();
                        return;
                    }
                }

                // Case Return Reject
                if (modeReturnReject == true)
                {
                    osmCheckCurrent.ReturnReject = tempValidateList.Where(w => w.SizeNo     == osmCheckCurrent.SizeNo &&
                                                                            w.CheckingDate  == osmCheckCurrent.CheckingDate &&
                                                                            w.WorkingCard   == osmCheckCurrent.WorkingCard).Sum(s => s.ReturnReject);
                    if (MessageBox.Show(string.Format("Confirm add(update) record ?\n Size  : {0}\n Quantity Return   : {1}", osmCheckCurrent.SizeNo, osmCheckCurrent.ReturnReject), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                    {
                        txtSizeNo.Focus();
                        txtSizeNo.SelectAll();
                        return;
                    }

                    if (osmCheckCurrent.ReturnReject < 0)
                    {
                        MessageBox.Show("Return Reject cannot be < 0 !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();
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
                currentOSMCheckListBySupp.RemoveAll(r => r.SizeNo       == osmCheckCurrent.SizeNo &&
                                                         r.CheckingDate == osmCheckCurrent.CheckingDate &&
                                                         r.WorkingCard  == osmCheckCurrent.WorkingCard);

                currentOSMCheckListBySupp.Add(osmCheckCurrent);
                if (modeReject == false && modeReturnReject == false)
                    osmCheckCurrent.Quantity = currentOSMCheckListBySupp.Where(w => w.SizeNo        == osmCheckCurrent.SizeNo &&
                                                                                    w.CheckingDate  == osmCheckCurrent.CheckingDate).Sum(s => s.Quantity);
                else if (modeReject == true && modeReturnReject == false)
                    osmCheckCurrent.Reject = currentOSMCheckListBySupp.Where(w => w.SizeNo          == osmCheckCurrent.SizeNo &&
                                                                                    w.CheckingDate  == osmCheckCurrent.CheckingDate).Sum(s => s.Reject);
                if (modeReturnReject == true)
                    osmCheckCurrent.ReturnReject = currentOSMCheckListBySupp.Where(w => w.SizeNo    == osmCheckCurrent.SizeNo &&
                                                                                    w.CheckingDate  == osmCheckCurrent.CheckingDate).Sum(s => s.ReturnReject);
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
    }
}
