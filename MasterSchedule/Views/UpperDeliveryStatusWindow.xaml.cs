﻿using System;
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
    public partial class UpperDeliveryStatusWindow : Window
    {
        BackgroundWorker threadLoad;
        List<MaterialTypeModel> materialTypeList;
        List<OrdersModel> orderList;
        List<RawMaterialModel> rawMaterialList;
        DateTime dtDefault;
        DateTime etdStart, etdEnd;
        List<DeliveryStatusViewModel> deliveryStatusViewList;
        public UpperDeliveryStatusWindow()
        {
            InitializeComponent();
            threadLoad = new BackgroundWorker();
            threadLoad.WorkerSupportsCancellation = true;
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);

            materialTypeList = new List<MaterialTypeModel>();
            orderList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            dtDefault = new DateTime(2000, 1, 1);
            deliveryStatusViewList = new List<DeliveryStatusViewModel>();
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
                deliveryStatusViewList = new List<DeliveryStatusViewModel>();
                etdStart = dpETDStart.SelectedDate.Value;
                etdEnd = dpETDEnd.SelectedDate.Value;

                btnLoad.IsEnabled = false;
                btnView.IsEnabled = false;
                dgMain.ItemsSource = null;
                this.Cursor = Cursors.Wait;
                threadLoad.RunWorkerAsync();
            }
        }

        void threadLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            //materialTypeList = MaterialTypeController.Select();
            materialTypeList = MaterialTypeController.Select().Where(w => w.MaterialGroup.Contains("UPPER")).ToList();
            orderList = OrdersController.SelectFull(etdStart, etdEnd);
            rawMaterialList = RawMaterialController.SelectFull(etdStart, etdEnd);

            List<String> productNoList = rawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            var materialUpperIdList = materialTypeList.Select(s => s.MaterialTypeId).Distinct().ToList();

            foreach (string productNo in productNoList)
            {
                OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                //List<Int32> materialTypeIdList = rawMaterialList.Select(r => r.MaterialTypeId).Distinct().ToList();
                List<Int32> materialTypeIdList = rawMaterialList.Where(w => materialUpperIdList.Contains(w.MaterialTypeId)).Select(r => r.MaterialTypeId).Distinct().ToList();
                //Red!
                //materialTypeIdList.Remove(6);
                //materialTypeIdList.Remove(11);
                //materialTypeIdList.Remove(12);
                //materialTypeIdList.Remove(13);
                //materialTypeIdList.Remove(14);

                foreach (int materialTypeId in materialTypeIdList)
                {
                    RawMaterialModel rawMaterial = rawMaterialList.Where(r => r.ProductNo == productNo && r.MaterialTypeId == materialTypeId).FirstOrDefault();
                    if (rawMaterial != null && rawMaterial.ETD.Date != dtDefault
                        //&& rawMaterial.ActualDate.Date == dtDefault
                        )
                    {
                        DeliveryStatusViewModel deliveryStatusView = new DeliveryStatusViewModel();

                        deliveryStatusView.ProductNo = productNo;
                        if (order != null)
                        {
                            deliveryStatusView.Country = order.Country;
                            deliveryStatusView.ArticleNo = order.ArticleNo;
                            deliveryStatusView.ShoeName = order.ShoeName;
                            deliveryStatusView.Quantity = order.Quantity;
                            deliveryStatusView.ETD = order.ETD;
                        }
                        MaterialTypeModel materialType = materialTypeList.Where(m => m.MaterialTypeId == rawMaterial.MaterialTypeId).FirstOrDefault();
                        if (materialType != null)
                        {
                            deliveryStatusView.Supplier = materialType.Name;
                        }
                        deliveryStatusView.SupplierETD = rawMaterial.ETD;
                        if(rawMaterial.ActualDate != dtDefault)
                        {
                            deliveryStatusView.Actual = string.Format("{0:MM/dd/yyyy}", rawMaterial.ActualDate);
                            deliveryStatusView.IsFinished = true;
                        }
                        else
                        {
                            deliveryStatusView.Actual = rawMaterial.Remarks;
                            deliveryStatusView.IsFinished = false;
                        }                                            
                        deliveryStatusViewList.Add(deliveryStatusView);
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
            List<DeliveryStatusViewModel> outsoleDeliveryStatusViewFilterList = deliveryStatusViewList;
            
            string shoeName = txtArticleStyle.Text;
            if (string.IsNullOrEmpty(shoeName) == false)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.ShoeName.ToLower().Contains(shoeName.ToLower()) == true).ToList();
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
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => etdStartSupplier.Date <= o.SupplierETD.Date && o.SupplierETD.Date <= etdEndSupplier.Date).ToList();
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
