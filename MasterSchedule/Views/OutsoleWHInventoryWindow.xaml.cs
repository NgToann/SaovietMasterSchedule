using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class OutsoleWHInventoryWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OrdersModel> orderList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SizeRunModel> sizeRunList;
        List<OutsoleWHInventoryViewModel> outsoleWHInventoryViewList;
        bool viewByOutsoleLine = false;

        List<ReportOutsoleInventoryModel> inventoryByOSCodeList;
        List<ReportOutsoleInventoryModel> inventoryByOSLineList;

        public OutsoleWHInventoryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
            orderList = new List<OrdersModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();

            outsoleOutputList = new List<OutsoleOutputModel>();
            assemblyReleaseList = new List<AssemblyReleaseModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();

            outsoleWHInventoryViewList = new List<OutsoleWHInventoryViewModel>();
            sizeRunList = new List<SizeRunModel>();

            inventoryByOSCodeList = new List<ReportOutsoleInventoryModel>();
            inventoryByOSLineList = new List<ReportOutsoleInventoryModel>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialList = OutsoleMaterialController.Select();
            outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectByOutsoleMaterial();
            outsoleOutputList = OutsoleOutputController.SelectByAssemblyMaster();
            outsoleMasterList = OutsoleMasterController.Select();
            orderList = OrdersController.Select();
            outsoleSupplierList = OutsoleSuppliersController.Select();
            assemblyReleaseList = AssemblyReleaseController.SelectByAssemblyMaster();
            sizeRunList = SizeRunController.SelectIsEnable();

            inventoryByOSCodeList = ReportController.InventoryByOutsoleCode();
            inventoryByOSLineList = ReportController.InventoryByOutsoleLine();

            foreach (var inven in inventoryByOSCodeList)
            {
                inven.ProductNoList = orderList.Where(w => w.OutsoleCode == inven.OutsoleCode).Select(s => s.ProductNo).Distinct().ToList();
            }
            foreach (var inven in inventoryByOSLineList)
            {
                inven.ProductNoList = outsoleMasterList.Where(w => w.OutsoleLine == inven.OutsoleLine).Select(s => s.ProductNo).Distinct().ToList();
                inven.OutsoleCode   = inven.OutsoleLine;
            }
        }

        bool canReload = false;
        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Default : OutsoleCode
            dgInventory.ItemsSource = inventoryByOSCodeList;
            dgInventory.Items.Refresh();

            lblTotalQTy.Text        = inventoryByOSCodeList.Sum(o => o.Quantity).ToString();
            lblMatching.Text        = inventoryByOSCodeList.Sum(o => o.Matching).ToString();
            lblFinishedOutsole.Text = inventoryByOSCodeList.Sum(o => o.FinishedOutsoleQuantity).ToString();

            this.Cursor = null;
            canReload = true;
        }

        private void radByOutsoleLine_Checked(object sender, RoutedEventArgs e)
        {
            if (canReload == true)
            {
                lblTotalQTy.Text = "";
                lblMatching.Text = "";
                lblFinishedOutsole.Text = "";

                viewByOutsoleLine = true;

                // groupby outsoleLine
                Column1.Header = "Outsole Line";
                dgInventory.ItemsSource = inventoryByOSLineList;
                dgInventory.Items.Refresh();

                lblTotalQTy.Text        = inventoryByOSLineList.Sum(o => o.Quantity).ToString();
                lblMatching.Text        = inventoryByOSLineList.Sum(o => o.Matching).ToString();
                lblFinishedOutsole.Text = inventoryByOSLineList.Sum(o => o.FinishedOutsoleQuantity).ToString();
            }
        }

        private void radByOutsoleCode_Checked(object sender, RoutedEventArgs e)
        {
            if (canReload == true)
            {
                lblTotalQTy.Text = "";
                lblMatching.Text = "";
                lblFinishedOutsole.Text = "";

                viewByOutsoleLine = false;
                // groupby outsoleCode
                Column1.Header = "OS/Code";

                dgInventory.ItemsSource = inventoryByOSCodeList;
                dgInventory.Items.Refresh();
                lblTotalQTy.Text        = inventoryByOSCodeList.Sum(o => o.Quantity).ToString();
                lblMatching.Text        = inventoryByOSCodeList.Sum(o => o.Matching).ToString();
                lblFinishedOutsole.Text = inventoryByOSCodeList.Sum(o => o.FinishedOutsoleQuantity).ToString();
            }
        }

        private void dgInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var outsoleWHInventoryView = (OutsoleWHInventoryViewModel)dgInventory.CurrentItem;
            var reportView = (ReportOutsoleInventoryModel)dgInventory.CurrentItem;
            if (reportView != null)
            {
                var productNoList = reportView.ProductNoList;
                var supplierList = outsoleMaterialList.Where(o => productNoList.Contains(o.ProductNo)).Select(o => o.OutsoleSupplierId).Distinct().ToList();
                var window = new OutsoleWHInventoryDetailWindow(
                    //outsoleWHInventoryView
                    productNoList,
                    orderList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    outsoleReleaseMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    outsoleSupplierList.Where(o => supplierList.Contains(o.OutsoleSupplierId) == true).ToList(),
                    outsoleMaterialList.Where(w => productNoList.Contains(w.ProductNo)).ToList(),
                    assemblyReleaseList.Where(w => productNoList.Contains(w.ProductNo)).ToList(),
                    outsoleOutputList.Where(w => productNoList.Contains(w.ProductNo)).ToList(),
                    viewByOutsoleLine
                    );
                window.Title = String.Format("{0} for {1}", window.Title, reportView.OutsoleCode);
                window.Show();
            }
        }
    }
}
