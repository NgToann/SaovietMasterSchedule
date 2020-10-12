using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.ViewModels;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleDeliveryStatusWindow.xaml
    /// </summary>
    public partial class OutsoleDeliveryStatusWindow : Window
    {
        BackgroundWorker threadLoad;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<SizeRunModel> sizeRunList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OrdersModel> orderList;
        DateTime dtDefault;
        DateTime etdStartSelect, etdEndSelect;
        List<OutsoleDeliveryStatusViewModel> outsoleDeliveryStatusViewList;
        List<SewingMasterModel> sewingMasterList;
        public OutsoleDeliveryStatusWindow()
        {
            InitializeComponent();
            threadLoad = new BackgroundWorker();
            threadLoad.WorkerSupportsCancellation = true;
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            sizeRunList = new List<SizeRunModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            orderList = new List<OrdersModel>();
            dtDefault = new DateTime(2000, 1, 1);
            outsoleDeliveryStatusViewList = new List<OutsoleDeliveryStatusViewModel>();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpETDStart.SelectedDate = DateTime.Now;
            dpETDEnd.SelectedDate = DateTime.Now;

            dpSupplierETDStart.SelectedDate = DateTime.Now;
            dpSupplierETDEnd.SelectedDate = DateTime.Now;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (threadLoad.IsBusy == false)
            {
                etdStartSelect = dpETDStart.SelectedDate.Value;
                etdEndSelect = dpETDEnd.SelectedDate.Value;
                outsoleDeliveryStatusViewList = new List<OutsoleDeliveryStatusViewModel>();

                btnLoad.IsEnabled = false;
                btnView.IsEnabled = false;
                dgMain.ItemsSource = null;
                this.Cursor = Cursors.Wait;
                threadLoad.RunWorkerAsync();
            }
        }

        void threadLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            //outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleRawMaterialList = OutsoleRawMaterialController.SelectFull(etdStartSelect, etdEndSelect);
            outsoleMaterialList = OutsoleMaterialController.SelectByOutsoleRawMaterialFull(etdStartSelect, etdEndSelect);
            sizeRunList = SizeRunController.SelectByOutsoleRawMaterialFull(etdStartSelect, etdEndSelect);
            outsoleSupplierList = OutsoleSuppliersController.Select();
            orderList = OrdersController.SelectByOutsoleRawMaterialFull(etdStartSelect, etdEndSelect);
            sewingMasterList = SewingMasterController.SelectFull(etdStartSelect, etdEndSelect);

            List<String> productNoList = outsoleRawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                OrdersModel order = orderList.FirstOrDefault(f => f.ProductNo == productNo);
                List<OutsoleRawMaterialModel> outsoleRawMaterialList_D1 = outsoleRawMaterialList.Where(o => o.ProductNo == productNo).ToList();
                List<SizeRunModel> sizeRunList_D1 = sizeRunList.Where(s => s.ProductNo == productNo).ToList();
                List<OutsoleMaterialModel> outsoleMaterialList_D1 = outsoleMaterialList.Where(o => o.ProductNo == productNo).ToList();
                var sewingMasterList_D1 = sewingMasterList.Where(w => w.ProductNo == productNo).ToList();
                foreach (OutsoleRawMaterialModel outsoleRawMaterial in outsoleRawMaterialList_D1)
                {
                    // 108a-2279
                    //bool isFull = OutsoleRawMaterialController.IsFull(sizeRunList_D1, new List<OutsoleRawMaterialModel>() { outsoleRawMaterial, }, outsoleMaterialList_D1);
                    if (
                        //isFull == false && 
                        outsoleRawMaterial.ETD.Date != dtDefault
                        //&& outsoleRawMaterial.ActualDate.Date == dtDefault
                        )
                    {
                        OutsoleDeliveryStatusViewModel outsoleDeliveryStatusView = new OutsoleDeliveryStatusViewModel();
                        outsoleDeliveryStatusView.ProductNo = productNo;
                        if (order != null)
                        {
                            outsoleDeliveryStatusView.Country = order.Country;
                            outsoleDeliveryStatusView.ShoeName = order.ShoeName;
                            outsoleDeliveryStatusView.ArticleNo = order.ArticleNo;
                            outsoleDeliveryStatusView.OutsoleCode = order.OutsoleCode;
                            outsoleDeliveryStatusView.Quantity = order.Quantity;
                            outsoleDeliveryStatusView.ETD = order.ETD;
                        }

                        OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId);
                        if (outsoleSupplier != null)
                        {
                            outsoleDeliveryStatusView.Supplier = outsoleSupplier.Name;
                        }

                        outsoleDeliveryStatusView.SupplierETD = outsoleRawMaterial.ETD;
                        //outsoleDeliveryStatusView.Actual = sizeRunList_D1.Sum(s => (s.Quantity - outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == s.SizeNo).Sum(o => (o.Quantity - o.QuantityReject)))).ToString();
                        
                        //int actualQty = sizeRunList_D1.Sum(s => (outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == s.SizeNo).Sum(o => (o.Quantity - o.QuantityReject))));
                        //int actualQty = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).Sum(s => s.Quantity - s.QuantityReject);
                        int actualQty = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).Sum(s => s.Quantity);
                        if (actualQty > 0)
                            outsoleDeliveryStatusView.ActualQuantity = actualQty.ToString();

                        int rejectQty = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).Sum(s => s.QuantityReject);
                        if (rejectQty > 0)
                            outsoleDeliveryStatusView.RejectQuantity = rejectQty.ToString();

                        outsoleDeliveryStatusView.IsFinished = true;
                        int balance = order.Quantity - actualQty + rejectQty;
                        if (balance > 0)
                        {
                            //outsoleDeliveryStatusView.Balance = balance.ToString();
                            outsoleDeliveryStatusView.IsFinished = false;
                        }

                        if (outsoleRawMaterial.ActualDate != dtDefault)
                        {
                            outsoleDeliveryStatusView.Actual = string.Format("{0:M/d}", outsoleRawMaterial.ActualDate);
                        }

                        outsoleDeliveryStatusView.SewingStartDate = dtDefault;
                        var sewingMasterModel = sewingMasterList_D1.FirstOrDefault();
                        if (sewingMasterModel != null)
                        {
                            outsoleDeliveryStatusView.SewingStartDate = sewingMasterModel.SewingStartDate;
                        }

                        outsoleDeliveryStatusViewList.Add(outsoleDeliveryStatusView);
                    }
                }
            }
        }

        void threadLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnLoad.IsEnabled = true;
            btnView.IsEnabled = true;
            this.Cursor = null;
        }

        private void dgMain_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            List<OutsoleDeliveryStatusViewModel> outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewList;
            
            string shoeName = txtArticleStyle.Text;
            if (string.IsNullOrEmpty(shoeName) == false)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.OutsoleCode.ToLower().Contains(shoeName.ToLower()) == true).ToList();
            }

            string supplier = txtSupplier.Text;
            if (string.IsNullOrEmpty(supplier) == false)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.Supplier.ToLower().Contains(supplier.ToLower()) == true).ToList();
            }

            if (chboSupplierETD.IsChecked == true)
            {
                DateTime etdStartSupplier = dpSupplierETDStart.SelectedDate.Value;
                DateTime etdEndSupplier = dpSupplierETDEnd.SelectedDate.Value;

                //outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => (etdStartSupplier.Year <= o.SupplierETD.Year &&
                //                                                                                      etdStartSupplier.Month <= o.SupplierETD.Month &&
                //                                                                                      etdStartSupplier.Day <= o.SupplierETD.Day) &&
                //                                                                                     (o.SupplierETD.Year <= etdEndSupplier.Year &&
                //                                                                                      o.SupplierETD.Month <= etdEndSupplier.Month &&
                //                                                                                      o.SupplierETD.Day <= etdEndSupplier.Day)).ToList();
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(w => etdStartSupplier.Date <= w.SupplierETD.Date &&
                                                                                                    w.SupplierETD.Date <= etdEndSupplier.Date).ToList();
            }

            if (chboFinished.IsChecked.Value == true || chboUnfinished.IsChecked.Value == true)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.IsFinished == chboFinished.IsChecked.Value || !o.IsFinished == chboUnfinished.IsChecked.Value).ToList();
            }
            else
            {
                outsoleDeliveryStatusViewFilterList = null;
            }
            dgMain.ItemsSource = null;
            dgMain.ItemsSource = outsoleDeliveryStatusViewFilterList;
        }
    }
}
