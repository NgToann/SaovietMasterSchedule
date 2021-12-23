using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using MasterSchedule.Views;
using MasterSchedule.Models;
using MasterSchedule.Helpers;
using System.ComponentModel;

using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
using System.Windows.Controls;

namespace MasterSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This project is not follow Object Oriented Program. Its difficult to maintenance and upgrade project. Read the code carefully.
    /// </summary>
    public partial class MainWindow : Window
    {
        AccountModel account;

        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OrdersModel> orderList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<SewingMasterModel> sewingMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<OutsoleMasterModel> outsoleMasterList;

        List<OutsoleRawMaterialModel> outsoleRawMaterialList;

        List<NoticeOutsoleWHInventoryViewModel> noticeOSWHInventoryDeliveryEarlyList;
        List<NoticeOutsoleWHInventoryViewModel> noticeOSWHInventoryNotDeliveryList;
        List<NoticeOutsoleWHInventoryViewModel> noticeOSWHInventoryRejectList;
        List<DelayShipmentViewModel> delayShipmentList;
        BackgroundWorker bwLoadDelivery;

        PopupOSWHDeliveryEarlyWindow window1;
        PopupNotYetDeliveryWindow window2;
        PopupOSWHRejectWindow window3;
        PopupDelayShipmentWindow window4;
        bool loaded = false;

        Point location1, location2, location3, location4;

        public MainWindow(AccountModel account)
        {
            this.account = account;
            outsoleMaterialList                 = new List<OutsoleMaterialModel>();
            outsoleReleaseMaterialList          = new List<OutsoleReleaseMaterialModel>();
            orderList                           = new List<OrdersModel>();
            outsoleSupplierList                 = new List<OutsoleSuppliersModel>();

            sewingMasterList                    = new List<SewingMasterModel>();
            assemblyMasterList                  = new List<AssemblyMasterModel>();
            outsoleMasterList                   = new List<OutsoleMasterModel>();

            outsoleOutputList                   = new List<OutsoleOutputModel>();
            assemblyReleaseList                 = new List<AssemblyReleaseModel>();
            outsoleRawMaterialList              = new List<OutsoleRawMaterialModel>();

            noticeOSWHInventoryDeliveryEarlyList    = new List<NoticeOutsoleWHInventoryViewModel>();
            noticeOSWHInventoryNotDeliveryList      = new List<NoticeOutsoleWHInventoryViewModel>();
            noticeOSWHInventoryRejectList           = new List<NoticeOutsoleWHInventoryViewModel>();
            delayShipmentList                       = new List<DelayShipmentViewModel>();

            bwLoadDelivery                       = new BackgroundWorker();
            bwLoadDelivery.DoWork               += new DoWorkEventHandler(bwLoadDelivery_DoWork);
            bwLoadDelivery.RunWorkerCompleted   += new RunWorkerCompletedEventHandler(bwLoadDelivery_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = String.Format("{0} - Version: {1} - User: {2}", this.Title, AssemblyHelper.Version(), account.FullName);
            if (account.UpperRMSchedule == true || account.OutsoleRMSchedule == true || account.CartonRMSchedule == true ||
                account.SewingMaster == true || account.CutPrepMaster == true ||
                account.OutsoleMaster == true || account.SockliningMaster == true ||
                account.AssemblyMaster == true || account.ViewOnly == true || account.ProductionMemo == true)
            {
                miRawMaterial.IsEnabled = true;
                miSearchMemo.IsEnabled = true;
            }
            if(account.OutsoleRMSchedule == true || account.ViewOnly == true)
            {
                miOutsoleReleaseMaterial.IsEnabled = true;
            }
            if (account.UpperRMSchedule == true || account.SewingMaster == true || account.CutPrepMaster == true || account.AssemblyMaster == true || account.ViewOnly == true)
            {
                miSewingMaster.IsEnabled = true;
                miUpperMaterialSchedule.IsEnabled = true;
            }
            if (account.OutsoleRMSchedule == true || account.OutsoleMaster == true || account.ViewOnly == true)
            {
                miOutsoleMaster.IsEnabled = true;
            }
            if (account.SockliningMaster == true || account.ViewOnly == true)
            {
                miSockliningMaster.IsEnabled = true;
            }
            if (account.CartonRMSchedule == true || account.AssemblyMaster == true || account.ViewOnly == true)
            {
                miAssemblyMaster.IsEnabled = true;
            }
            if (account.AssemblyMaster == true || account.ViewOnly == true)
            {
                miAssemblyRelease.IsEnabled = true;
            }
            if (account.ImportData == true)
            {
                miImport.IsEnabled = true;
            }
            if (account.ReviseData == true)
            {
                miRevise.IsEnabled = true;
            }  
            if (account.OffDay == true || account.ViewOnly == true)
            {
                miOffDay.IsEnabled = true;
            }
            if (account.ProductionMemo == true)
            {
                miInsertMemo.IsEnabled = true;
            }
            if (account.Notifications == true)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
                location1 = txtPopupPosition1.PointToScreen(new Point(0, 0));
                location2 = txtPopupPosition2.PointToScreen(new Point(0, 0));
                location3 = txtPopupPosition3.PointToScreen(new Point(0, 0));
                location4 = txtPopupPosition4.PointToScreen(new Point(0, 0));

                if (bwLoadDelivery.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;

                    //dgDeliveryEarly.ItemsSource = null;
                    //dgNotYetDelivery.ItemsSource = null;
                    //dgReject.ItemsSource = null;

                    //popupOSWHDeliveryEarly.IsOpen = false;
                    //popupOSWHNotDelivery.IsOpen = false;
                    //popupOSWHReject.IsOpen = false;
                    //popupDelayShipment.IsOpen = false;

                    bwLoadDelivery.RunWorkerAsync();
                }
            }
            if (account.OutsoleWH == true)
            {
                miOutsoleWH.IsEnabled = true;
            }
            if (account.UpperAccessories == true && account.MaterialPlan == true)
            {
                miImportUpperAccessoriesPlan.IsEnabled = true;
            }
        }
        
        // Notifications
        private void bwLoadDelivery_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleSupplierList         = OutsoleSuppliersController.Select();
            outsoleMaterialList         = OutsoleMaterialController.Select();
            outsoleReleaseMaterialList  = OutsoleReleaseMaterialController.SelectByOutsoleMaterial();
            outsoleOutputList           = OutsoleOutputController.SelectByAssemblyMaster();
            sewingMasterList            = SewingMasterController.Select();
            assemblyMasterList          = AssemblyMasterController.Select();
            outsoleMasterList           = OutsoleMasterController.Select();
            orderList                   = OrdersController.Select();
            outsoleRawMaterialList      = OutsoleRawMaterialController.Select();

            var outsoleCodeList = orderList.Select(o => o.OutsoleCode).Distinct().ToList();
            DateTime dtNow = DateTime.Now;
            DateTime dtDefault = new DateTime(2000, 1, 1);
            DateTime deliveryEFDDate = dtDefault;

            // Delay Shipment Notifications
            foreach (var order in orderList)
            {
                bool needToShow         = false;
                int assemblyBalance     = 0, sewingBalance = 0, outsoleBalance = 0;
                var assemblyMaster = assemblyMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                var sewingMaster = sewingMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                var outsoleMaster = outsoleMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (assemblyMaster == null || sewingMaster == null || outsoleMaster == null)
                {
                    continue;
                }

                if (assemblyMaster.AssemblyBalance != "")
                {
                    Int32.TryParse(assemblyMaster.AssemblyBalance, out assemblyBalance);
                }
                else
                {
                    assemblyBalance = order.Quantity;
                }

                if (sewingMaster.SewingBalance != "")
                {
                    Int32.TryParse(sewingMaster.SewingBalance, out sewingBalance);
                }
                else
                {
                    sewingBalance = order.Quantity;
                }

                if (outsoleMaster.OutsoleBalance != "")
                {
                    Int32.TryParse(outsoleMaster.OutsoleBalance, out outsoleBalance);
                }
                else
                {
                    outsoleBalance = order.Quantity;
                }

                if (dtNow.AddDays(5) < order.ETD)
                {
                    needToShow = false;
                }
                else
                {
                    if (assemblyBalance > 0 || sewingBalance > 0 || outsoleBalance > 0)
                    {
                        needToShow = true;
                    }
                    if (assemblyBalance > 0 && sewingBalance == 0 && outsoleBalance == 0)
                    {
                        needToShow = false;
                    }
                }

                if (needToShow == true)
                {
                    var delayShipment = new DelayShipmentViewModel();
                    delayShipment.Style = order.ShoeName;
                    delayShipment.ProductNo = order.ProductNo;
                    delayShipment.OrderCSD = order.ETD.AddDays(10);
                    if (assemblyBalance > 0)
                    {
                        delayShipment.AssemblyBalance = assemblyBalance;
                    }
                    if (sewingBalance > 0)
                    {
                        delayShipment.SewingBalance = sewingBalance;
                    }
                    if (outsoleBalance > 0)
                    {
                        delayShipment.OutsoleBalance = outsoleBalance;
                    }
                    if (delayShipment.OrderCSD < DateTime.Now.Date)
                        delayShipmentList.Add(delayShipment);
                }
            }

            // OSWH Notifications
            foreach (string outsoleCode in outsoleCodeList)
            {
                var productNoList = orderList.Where(o => o.OutsoleCode == outsoleCode && o.IsEnable == true).Select(o => o.ProductNo).Distinct().ToList();
                foreach (var productNo in productNoList)
                {
                    var sewingMasterModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                    var orderModel = orderList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                    if (sewingMasterModel == null || orderModel == null)
                    {
                        continue;
                    }
                    var outsoleMaterialList_D1 = outsoleMaterialList.Where(w => w.ProductNo == productNo).ToList();
                    var supplierIdList = outsoleMaterialList_D1.Select(s => s.OutsoleSupplierId).Distinct().ToList();

                    foreach (var supplierId in supplierIdList)
                    {
                        int quantityDelivery = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == supplierId).Sum(s => s.Quantity) - outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == supplierId).Sum(s => s.QuantityReject);
                        int quantityDelivery1 = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == supplierId).Sum(s => s.Quantity - s.QuantityReject);
                        var outsoleMaterialDelivery = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == supplierId).FirstOrDefault();
                        if (quantityDelivery > 0)
                        {
                            var outsoleRawMaterialModel = outsoleRawMaterialList.Where(w => w.ProductNo == productNo && w.OutsoleSupplierId == supplierId).FirstOrDefault();
                            if (outsoleRawMaterialModel != null)
                            {
                                deliveryEFDDate = outsoleRawMaterialModel.ETD;
                            }
                            if (deliveryEFDDate != dtDefault && deliveryEFDDate > dtNow.AddDays(15) && sewingMasterModel.SewingStartDate > dtNow.AddDays(25))
                            {
                                var deliveryEarly = new NoticeOutsoleWHInventoryViewModel()
                                {
                                    Style = orderModel.ShoeName,
                                    ProductNo = productNo,
                                    QuantityDelivery = quantityDelivery,
                                    Supplier = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierId).Select(s => s.Name).FirstOrDefault(),
                                    OutsoleCode = outsoleCode,
                                    DeliveryEFDDate = deliveryEFDDate,
                                    SewingStartDate = sewingMasterModel.SewingStartDate,
                                };
                                noticeOSWHInventoryDeliveryEarlyList.Add(deliveryEarly);
                            }
                        }

                        var outsoleRawMaterialLateModel = outsoleRawMaterialList.Where(w => w.ETD <= dtNow && w.ProductNo == productNo && w.OutsoleSupplierId == supplierId).FirstOrDefault();
                        if (outsoleRawMaterialLateModel != null)
                        {
                            int quantityOrder = orderModel.Quantity;
                            if (quantityOrder - quantityDelivery > 0)
                            {
                                var notDelivery = new NoticeOutsoleWHInventoryViewModel()
                                {
                                    Style = orderModel.ShoeName,
                                    ProductNo = productNo,
                                    QuantityNotDelivery = quantityOrder - quantityDelivery,
                                    Supplier = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierId).Select(s => s.Name).FirstOrDefault(),
                                    OutsoleCode = outsoleCode,
                                    DeliveryEFDDate = outsoleRawMaterialLateModel.ETD,
                                    OrderCSD = orderModel.ETD.AddDays(10)
                                };
                                noticeOSWHInventoryNotDeliveryList.Add(notDelivery);
                            }
                        }

                        int quantityReject = outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == supplierId).Sum(s => s.QuantityReject);
                        var outsoleMaterialReject= outsoleMaterialList_D1.Where(w => w.OutsoleSupplierId == supplierId).FirstOrDefault(); // allways not null
                        var outsoleRawMaterialReject = outsoleRawMaterialList.Where(w => w.ETD <= dtNow && w.ProductNo == productNo && w.OutsoleSupplierId == supplierId).FirstOrDefault();
                        if (quantityReject > 0 && outsoleMaterialReject.ModifiedTimeReject < dtNow.AddDays(-2) && outsoleRawMaterialReject != null)
                        {
                            var reject = new NoticeOutsoleWHInventoryViewModel()
                            {
                                Style = orderModel.ShoeName,
                                ProductNo = productNo,
                                OrderEFD = orderModel.ETD,
                                QuantityReject = quantityReject,
                                Supplier = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierId).Select(s => s.Name).FirstOrDefault(),
                                OutsoleCode = outsoleCode,
                                DeliveryEFDDate = outsoleRawMaterialReject.ETD
                            };

                            noticeOSWHInventoryRejectList.Add(reject);
                        }
                    }
                }
            }
        }
        private void bwLoadDelivery_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (noticeOSWHInventoryDeliveryEarlyList.Count > 0)
            //{
            //    dgDeliveryEarly.ItemsSource = noticeOSWHInventoryDeliveryEarlyList;
            //    popupOSWHDeliveryEarly.IsOpen = true;
            //}

            //if (noticeOSWHInventoryNotDeliveryList.Count > 0)
            //{
            //    dgNotYetDelivery.ItemsSource = noticeOSWHInventoryNotDeliveryList;
            //    popupOSWHNotDelivery.IsOpen = true;
            //}

            //if (noticeOSWHInventoryRejectList.Count > 0)
            //{
            //    dgReject.ItemsSource = noticeOSWHInventoryRejectList;
            //    popupOSWHReject.IsOpen = true;
            //}

            //if (delayShipmentList.Count > 0)
            //{
            //    dgDelayShipment.ItemsSource = delayShipmentList;
            //    popupDelayShipment.IsOpen = true;
            //}

            //noticeOSWHInventoryDeliveryEarlyTranferList = noticeOSWHInventoryDeliveryEarlyList;

            //window1 = new PopupOSWHDeliveryEarlyWindow(noticeOSWHInventoryDeliveryEarlyList);
            //window2 = new PopupNotYetDeliveryWindow(noticeOSWHInventoryNotDeliveryList);
            //window3 = new PopupOSWHRejectWindow(noticeOSWHInventoryRejectList);
            //window4 = new PopupDelayShipmentWindow(delayShipmentList);

            ShowPopup(location1, location2, location3, location4);
            

            

            loaded = true;
            this.Cursor = null;
        }

        private void miRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            RawMaterialWindow window = new RawMaterialWindow(account);
            window.Show();
        }

        private void miDelayReport_Click(object sender, RoutedEventArgs e)
        {
            UpperDelayReportWindow window = new UpperDelayReportWindow();
            window.Show();
        }

        private void miImportOrders_Click(object sender, RoutedEventArgs e)
        {
            ImportOrdersWindow window = new ImportOrdersWindow(account);
            window.ShowDialog();
        }

        private void miInportSizeRun_Click(object sender, RoutedEventArgs e)
        {
            ImportSizeRunWindow window = new ImportSizeRunWindow();
            //ImportSizeRunNewWindow window = new ImportSizeRunNewWindow(account);
            window.ShowDialog();
        }

        private void miDeliveryReport_Click(object sender, RoutedEventArgs e)
        {
            UpperDeliveryReportWindow window = new UpperDeliveryReportWindow();
            window.Show();
        }

        private void miUpdateOrders_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrdersWindow window = new UpdateOrdersWindow(account);
            window.Show();
        }

        private void miOutsoleDelayReport_Click(object sender, RoutedEventArgs e)
        {
            OutsoleDelayReportWindow window = new OutsoleDelayReportWindow();
            window.Show();
        }

        private void miOutsoleDeliveryReport_Click(object sender, RoutedEventArgs e)
        {
            OutsoleDeliveryReportWindow window = new OutsoleDeliveryReportWindow();
            window.Show();
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
            this.Close();
        }

        private void miImportOutsoleRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            ImportOutsoleRawMaterialWindow window = new ImportOutsoleRawMaterialWindow();
            window.Show();
        }

        private void miOutsoleReleaseMaterial_Click(object sender, RoutedEventArgs e)
        {
            OutsoleReleaseMaterialWindow window = new OutsoleReleaseMaterialWindow(account, ERelease.ReleaseToOutsole);
            window.Show();
        }

        private void miOutsoleWHInventory_Click(object sender, RoutedEventArgs e)
        {
            OutsoleWHInventoryWindow window = new OutsoleWHInventoryWindow();
            window.Show();
        }

        private void miOutsoleWHDelivery_Click(object sender, RoutedEventArgs e)
        {
            //OutsoleWHDeliveryWindow window = new OutsoleWHDeliveryWindow();
            OutsoleWHDeliveryWindow_1 window = new OutsoleWHDeliveryWindow_1();
            window.Show();
        }

        private void miUpdateOutsoleReleaseMaterial_Click(object sender, RoutedEventArgs e)
        {
            SelectOutsoleReleaseMaterialWindow window = new SelectOutsoleReleaseMaterialWindow(account, ERelease.ReleaseToOutsole);
            window.Show();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            string about = "*Software: Master Schedule (Only for Saoviet Corporation)\n"
                + String.Format("*Version: {0}\n", AssemblyHelper.Version())
                + "*Created by: Mr.Denis Sy & Mr.Vũ\n"
                + "*Updated by: NgToann\n"
                + "*Contact:\n"
                + " -Phone: 0973.148.429\n"
                + " -Skype: vuvd_it";
            MessageBox.Show(about, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void miOffDay_Click(object sender, RoutedEventArgs e)
        {
            InsertOffDayWindow window = new InsertOffDayWindow(account);
            window.Show();
        }

        private void miSewingMaster_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Testing...", this.Title, MessageBoxButton.OK, MessageBoxImage.Stop);
            //return;
            SewingMasterWindow window = new SewingMasterWindow(account);
            window.Show();
        }

        private void miOutsoleMaterialReject_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMaterialRejectReportWindow window = new OutsoleMaterialRejectReportWindow();
            window.Show();
        }

        private void miOutsoleMaster_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMasterWindow window = new OutsoleMasterWindow(account);
            window.Show();

        }

        private void miSockliningMaster_Click(object sender, RoutedEventArgs e)
        {
            SockliningMasterWindow window = new SockliningMasterWindow(account);
            window.Show();
        }

        private void miAssemblyMaster_Click(object sender, RoutedEventArgs e)
        {
            AssemblyMasterWindow window = new AssemblyMasterWindow(account);
            window.Show();
        }

        private void miSewingMasterReport_Click(object sender, RoutedEventArgs e)
        {
            SewingMasterFilterWindow window = new SewingMasterFilterWindow();
            window.Show();
        }

        private void miCutprepMasterReport_Click(object sender, RoutedEventArgs e)
        {
            CutprepMasterFilterWindow window = new CutprepMasterFilterWindow();
            window.Show();
        }

        private void miOutsoleMasterReport_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMasterFilterWindow window = new OutsoleMasterFilterWindow();
            window.Show();
        }

        private void miAssemblyMasterReport_Click(object sender, RoutedEventArgs e)
        {
            AssemblyMasterFilterWindow window = new AssemblyMasterFilterWindow();
            window.Show();
        }

        private void miSockliningMasterReport_Click(object sender, RoutedEventArgs e)
        {
            SockliningMasterFilterWindow window = new SockliningMasterFilterWindow();
            window.Show();
        }

        private void miAssemblyRelease_Click(object sender, RoutedEventArgs e)
        {
            AssemblyReleaseWindow window = new AssemblyReleaseWindow(account);
            window.Show();
        }

        private void miUpdateAssemblyRelease_Click(object sender, RoutedEventArgs e)
        {
            SelectAssemblyReleaseWindow window = new SelectAssemblyReleaseWindow(account);
            window.Show();
        }

        private void miUpperWHInventory_Click(object sender, RoutedEventArgs e)
        {
            UpperWHInventoryWindow window = new UpperWHInventoryWindow();
            window.Show();
        }

        private void miScheduleDelaySewing_Click(object sender, RoutedEventArgs e)
        {
            SewingScheduleDelayReportWindow window = new SewingScheduleDelayReportWindow();
            window.Show();
        }

        private void miScheduleDelayAssembly_Click(object sender, RoutedEventArgs e)
        {
            AssemblyScheduleDelayReportWindow window = new AssemblyScheduleDelayReportWindow();
            window.Show();
        }

        private void miDeliveryStatus_Click(object sender, RoutedEventArgs e)
        {
            UpperDeliveryStatusWindow window = new UpperDeliveryStatusWindow();
            window.Show();
        }

        private void miOutsoleDeliveryStatus_Click(object sender, RoutedEventArgs e)
        {
            OutsoleDeliveryStatusWindow window = new OutsoleDeliveryStatusWindow();
            window.Show();
        }

        private void miCompletionStatus_Click(object sender, RoutedEventArgs e)
        {
            CompletionStatusWindow window = new CompletionStatusWindow();
            window.Show();
        }

        private void miInsertMemo_Click(object sender, RoutedEventArgs e)
        {
            ProductionMemoWindow window = new ProductionMemoWindow();
            window.Show();
        }

        private void miSearchMemo_Click(object sender, RoutedEventArgs e)
        {
            SearchProductionMemoWindow window = new SearchProductionMemoWindow(account);
            window.Show();
        }

        private void miChartSchedule_Click(object sender, RoutedEventArgs e)
        {
            ChartScheduleWindow window = new ChartScheduleWindow();
            window.Show();
        }

        //private void miImportMachineRequirement_Click(object sender, RoutedEventArgs e)
        //{
        //    ImportMachineRequirementWindow window = new ImportMachineRequirementWindow();
        //    window.Show();
        //}

        private void miMachineRequirementScheduleChart_Click(object sender, RoutedEventArgs e)
        {
            MachineRequirementScheduleChartWindow window = new MachineRequirementScheduleChartWindow();
            window.Show();
        }

        //private void miUpdateMachineRequirement_Click(object sender, RoutedEventArgs e)
        //{
        //    UpdateMachineRequirementWindow window = new UpdateMachineRequirementWindow();
        //    window.Show();
        //}

        //private void miImportAvailableMachine_Click(object sender, RoutedEventArgs e)
        //{
        //    ImportAvailableMachineWindow window = new ImportAvailableMachineWindow();
        //    window.Show();
        //}

        //private void miUpdateAvailableMachine_Click(object sender, RoutedEventArgs e)
        //{
        //    UpdateAvailableMachineWindow window = new UpdateAvailableMachineWindow();
        //    window.Show();
        //}

        private void btnClosePopupOSWHDeliveryBefore_Click(object sender, RoutedEventArgs e)
        {
            //popupOSWHDeliveryEarly.IsOpen = false;
        }

        private void btnClosePopupOSWHNotDelivery_Click(object sender, RoutedEventArgs e)
        {
            //popupOSWHNotDelivery.IsOpen = false;
        }

        private void miImportUpperComponent_Click(object sender, RoutedEventArgs e)
        {
            ImportUpperComponentEFDWindow window = new ImportUpperComponentEFDWindow();
            window.Show();
        }

        private void miUpperComponentInventory_Click(object sender, RoutedEventArgs e)
        {
            UpperComponentWHInventoryWindow window = new UpperComponentWHInventoryWindow();
            window.Show();
        }

        private void miUpperComponentDelivery_Click(object sender, RoutedEventArgs e)
        {
            UpperComponentWHDeliveryWindow window = new UpperComponentWHDeliveryWindow();
            window.Show();
        }

        string modeViewStatistics = "";
        string cutAB = "";
        private void miPerSection_Click(object sender, RoutedEventArgs e)
        {
            modeViewStatistics = "1";
            LeadTimePerSectionWindow window = new LeadTimePerSectionWindow(modeViewStatistics, cutAB);
            window.Show();
        }
        private void miCutA_Click(object sender, RoutedEventArgs e)
        {
            modeViewStatistics = "2";
            cutAB = "Cut A";
            LeadTimePerSectionWindow window = new LeadTimePerSectionWindow(modeViewStatistics, cutAB);
            window.Show();
        }

        private void miCutB_Click(object sender, RoutedEventArgs e)
        {
            modeViewStatistics = "2";
            cutAB = "Cut B";
            LeadTimePerSectionWindow window = new LeadTimePerSectionWindow(modeViewStatistics, cutAB);
            window.Show();
        }

        string modePerStyle = "";
        private void miPerPatternNo_Click(object sender, RoutedEventArgs e)
        {
            modePerStyle = "PM";
            LeadTimePerStyleWindow window = new LeadTimePerStyleWindow(modePerStyle);
            window.Show();
        }

        private void miPerArticleNo_Click(object sender, RoutedEventArgs e)
        {
            modePerStyle = "Article";
            LeadTimePerStyleWindow window = new LeadTimePerStyleWindow(modePerStyle);
            window.Show();
        }

        private void btnClosePopupOSReject_Click(object sender, RoutedEventArgs e)
        {
            //popupOSWHReject.IsOpen = false;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClosePopupDelayShipment_Click(object sender, RoutedEventArgs e)
        {
            //popupDelayShipment.IsOpen = false;
        }

        private void miOutsoleDeliveryVariance_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMaterialDetailVarianceReportWindow window = new OutsoleMaterialDetailVarianceReportWindow();
            window.Show();
        }

        private void miInputOutsoleMaterialDetail_Click(object sender, RoutedEventArgs e)
        {
            OutsoleInputMaterialDetailWindow window = new OutsoleInputMaterialDetailWindow(account);
            window.Show();
        }

        private void miInputOutsoleWHFG_Click(object sender, RoutedEventArgs e)
        {
            OutsoleWHFinishGoodsWindow window = new OutsoleWHFinishGoodsWindow();
            window.Show();
        }

        private void miOutsoleAccumulating_Click(object sender, RoutedEventArgs e)
        {
            OutsoleWHAccumulatingReportWindow window = new OutsoleWHAccumulatingReportWindow();
            window.Show();
        }

        private void miImportInsockRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            ImportInsockRawMaterialWindow window = new ImportInsockRawMaterialWindow();
            window.Show();
        }

        private void miInsockInventory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miInsockDelivery_Click(object sender, RoutedEventArgs e)
        {
            InsockDeliveryWindow window = new InsockDeliveryWindow();
            window.Show();
        }

        private void miOutsoleOutputBalance_Click(object sender, RoutedEventArgs e)
        {
            OutsoleOutputBalanceWindow window = new OutsoleOutputBalanceWindow();
            window.Show();
        }

        private void miReleaseMaterialToWHInspection_Click(object sender, RoutedEventArgs e)
        {
            OutsoleReleaseMaterialWindow window = new OutsoleReleaseMaterialWindow(account, ERelease.ReleaseToWHInspection);
            window.Show();
        }

        private void miReviseReleaseToWHInspection_Click(object sender, RoutedEventArgs e)
        {
            SelectOutsoleReleaseMaterialWindow window = new SelectOutsoleReleaseMaterialWindow(account, ERelease.ReleaseToWHInspection);
            window.Show();
        }

        private void miImportMachine_Click(object sender, RoutedEventArgs e)
        {
            ImportMachineWindow window = new ImportMachineWindow();
            window.Show();
        }

        private void miReviseMachine_Click(object sender, RoutedEventArgs e)
        {
            //UpdateMachineWindow window = new UpdateMachineWindow();
            //window.Show();
        }

        private void miImportMachineRequest_Click(object sender, RoutedEventArgs e)
        {
            ImportMachineRequestWindow window = new ImportMachineRequestWindow();
            window.Show();
        }

        private void ClosePopup()
        {
            if (account.Notifications == true)
            {
                window1.Close();
                window2.Close();
                window3.Close();
                window4.Close();
            }
        }

        private void HidePopup()
        {
            if (account.Notifications == true)
            {
                window1.Hide();
                window2.Hide();
                window3.Hide();
                window4.Hide();
            }
        }

        private void ShowPopup(Point point1, Point point2, Point point3, Point point4)
        {
            window1 = new PopupOSWHDeliveryEarlyWindow(noticeOSWHInventoryDeliveryEarlyList);
            window2 = new PopupNotYetDeliveryWindow(noticeOSWHInventoryNotDeliveryList);
            window3 = new PopupOSWHRejectWindow(noticeOSWHInventoryRejectList);
            window4 = new PopupDelayShipmentWindow(delayShipmentList);

            window1.Left = point1.X;
            window1.Top = point1.Y;
            window1.Width = point2.X - point1.X;
            window1.Height = point4.Y - point1.Y;
            window1.ShowInTaskbar = false;
            window1.Topmost = true;
            window1.Show();

            window2.Left = point2.X;
            window2.Top = point2.Y;
            window2.Width = point2.X - point1.X;
            window2.Height = point4.Y - point1.Y;
            window2.ShowInTaskbar = false;
            window2.Topmost = true;
            window2.Show();

            window3.Left = point1.X;
            window3.Top = point3.Y;
            window3.Width = point2.X - point1.X;
            window3.Height = point4.Y - point1.Y;
            window3.ShowInTaskbar = false;
            window3.Topmost = true;
            window3.Show();

            window4.Left = point4.X;
            window4.Top = point3.Y;
            window4.Width = point2.X - point1.X;
            window4.Height = point4.Y - point1.Y;
            window4.ShowInTaskbar = false;
            window4.Topmost = true;
            window4.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ClosePopup();
        }

        private void miSewingSummary_Click(object sender, RoutedEventArgs e)
        {
            SummarySewingReportWindow window = new SummarySewingReportWindow();
            window.Show();
        }

        private void miOutsoleWH_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMaterialWHCheckingWindow window = new OutsoleMaterialWHCheckingWindow();
            window.Show();
        }

        private void miOutsoleMaterialWHChecking_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMaterialWHCheckingReportWindow window = new OutsoleMaterialWHCheckingReportWindow();
            window.Show();
        }

        private void miOutsoleMaterialWHCheckingDetail_Click(object sender, RoutedEventArgs e)
        {
            OutsoleMaterialWHCheckingDetailReportWindow window = new OutsoleMaterialWHCheckingDetailReportWindow();
            window.Show();
        }

        private void miPrintSizeRun_Click(object sender, RoutedEventArgs e)
        {
            PrintSizeRunWindow window = new PrintSizeRunWindow("");
            window.Show();
        }

        private void miInportSizeRunAndOrders_Click(object sender, RoutedEventArgs e)
        {
            ImportSizeRunNewWindow window = new ImportSizeRunNewWindow(account);
            window.Show();
        }

        private void miImportUpperAccessoriesPlan_Click(object sender, RoutedEventArgs e)
        {
            ImportUpperAccessoriesPlanWindow window = new ImportUpperAccessoriesPlanWindow();
            window.Show();
        }

        private void miUpperAccessories_Click(object sender, RoutedEventArgs e)
        {
            UpperAccessoriesReportWindow window = new UpperAccessoriesReportWindow();
            window.Show();
        }

        private void miImportLaminationMaterial_Click(object sender, RoutedEventArgs e)
        {
            ImportLaminationMaterialWindow window = new ImportLaminationMaterialWindow();
            window.Show();
        }

        private void miOSPaintShop_Click(object sender, RoutedEventArgs e)
        {
            OSMPaintShopWindow window = new OSMPaintShopWindow();
            window.Show();
        }

        private void miMasterChart_Click(object sender, RoutedEventArgs e)
        {
            MasterStatisticsWindow window = new MasterStatisticsWindow();
            window.Show();
        }

        private void miConfirmOSWorkingCart_Click(object sender, RoutedEventArgs e)
        {
            ConfirmOutsoleMaterialWorkingCartWindow window = new ConfirmOutsoleMaterialWorkingCartWindow(account);
            window.Show();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized && loaded == true)
            {
                HidePopup();
            }
            if (this.WindowState == System.Windows.WindowState.Maximized && loaded == true)
            {
                ShowPopup(location1, location2, location3, location4);
            }
            if (this.WindowState == System.Windows.WindowState.Normal && loaded == true)
            {
                HidePopup();
            }
        }

        private void miLog_Click(object sender, RoutedEventArgs e)
        {
            string version = "Update History\n\n" +
                "1.1.6.1: " + "Update: Report " + "List Of Delay Upper." + "\n" +
                "1.1.6.2: " + "Update: Schedule Chart " + "Create Machine Requirement Chart.\n" +
                "1.1.6.3: " + "Update: Machine Requirement Chart, Schedule Chart, Available Machine.\n" +
                "1.1.6.4: " + "Update: Color for Sewing Schedule, WH Master File, ChartSchedule, MachineRequirementSchedule.\n" +
                "1.1.6.5: " + "Update: Machine requirement chart, Sort Sewingline, Assemblyline Calulate total , Get data for worker available.\n" +
                "1.1.6.6: " + "Update: Fix error import excel file.\n" +
                "1.1.6.7: " + "Update: Outsole ETD.\n" +
                "1.1.6.8: " + "Update: OS'MaterialArrival (SewingCutprep, Assembly, Outsole)Master.\n" +
                "1.1.6.9: " + "Update: AssyStartDate, Color(CartonETD, CartonActualDate)WHMasterFile.\n" +
                "1.1.7.0: " + "Update: Sort EFD,CartonETD,CartonActualDate.\n" +
                "1.1.7.5: " + "Update: Create Outsole WH Delivery.\n" +
                "1.1.7.6: " + "Update: Revise OSWHInventory, Create Notice.\n" +
                "1.1.7.7: " + "Update: Revise OS Delivery Status, Add LaserCut, HuasenCut(S).\n" +
                "1.1.7.8: " + "Update: Revise Color UpperMatsArrival, Import Orders.\n" +
                "1.1.8.0: " + "Update: Add LeadTime, Upper Component WH, ActualDate(sewing,outsole), Add SewingPrep,CutBStartDate.\n" +
                "1.1.8.1: " + "Update: OS Delivery Color.\n" +
                "1.1.8.2: " + "Update: Reject Notice.\n" +
                "1.1.8.3: " + "Update: Chart Schedule Tooltip.\n" +
                "1.1.8.6: " + "Update: Chart Schedule Yellow(0->7).\n" +
                "1.1.9.1: " + "Update: Outsole, UpperComponent Reject, Remove PO disable on notice.\n" +
                "1.1.9.2: " + "Update: Simulation Mode.\n" +
                "1.1.9.3: " + "Update: Delay Notification.\n" +
                "1.1.9.5: " + "Update: Outsole material detail Revise.\n" +
                "1.1.9.7: " + "Update: Outsole WH Accumulating Report.\n" +
                "1.1.9.8: " + "Update: WH Insock, Revise Input OS Material Reject.\n" +
                "1.1.9.9: " + "Update: CutAStartDate(Before Sewing 10 days), Add OutsoleLine(Assembly).\n" +
                "1.2.0.0: " + "Update: Order(OutsoleSize, MidsoleSize).\n" +
                "1.2.0.2: " + "Update: Outsole WH Accumulating, Export Excel (OS Delivery Detail), CutAStartDate(Before Sewing 14 days), Color.\n" +
                "1.2.0.4: " + "Update: Import SizeRun (OutsoleSize, MidsoleSize), CutAStartDate(Before Sewing 10 days).\n" +
                "1.2.0.5: " + "Update: Input OutsoleMaterial (Add Rack), Add Line (Print Report).\n" +
                "1.2.0.6: " + "Update: Report: OutsoleOutputBalance, WH Accumulating Sheet (Reject Detail). Sort OrderSize (List of Delay Outsole).\n" +
                "1.2.0.8: " + "Update: Report (Outsole Output Balance), Remarks(WH).\n" +
                "1.2.1.0: " + "Update: Input Reject Assembly (OutsoleMaterial).\n" +
                "1.2.1.2: " + "Update: Reload (WH Master File).\n" +
                "1.2.1.4: " + "Update: Balance(OutsoleInputMaterial), Search with EFD Range.\n" +
                "1.2.1.5: " + "Update: Reject(OutsoleDeliveryStatus).\n" +
                "1.2.1.7: " + "Update: Reject, Balance (OutsoleInputMaterial).\n" +
                "1.2.1.8: " + "Update: Add, Revise, Report (Release O/S Material to WH Inspection), Add Article(Outsole Delay Report), Add O/S Code (Completion PO).\n" +
                "1.2.1.9: " + "Update: AddBlankRow(OSMaterialReleaseToWHInspectionReport).\n" +
                "1.2.2.0: " + "Update: ReviseNotifications (MainWindow).\n" +
                "1.2.2.1: " + "Update: Add Size 16, 17 (Import SizeRun), Add OutsoleSize header row (OSWHDeliveryDetail).\n" +
                "1.2.2.2: " + "Update: Show Only Balance (OSWHDeliveryDetail Window).\n" +
                "1.2.2.3: " + "Create: Outsole Material Balance Report (OutsoleMaterialReject/Balance).\n" +
                "1.2.2.4: " + "Update: SizeNo (OutsoleMaterialWHRackposition), OSWHDelivery Header.\n" +
                "1.2.2.5: " + "Update: ActualQuantity(OSDeliveryStatus).\n" +
                "1.2.2.6: " + "Update: Add,Delete,Edit(RackNumber) OSInputMaterialWindow.\n" +
                "1.2.2.7: " + "Update: OutsoleMaterialRackPosition (WHMasterFile).\n" +
                "1.2.2.8: " + "Update: InputRackNumber, OutsoleMaterialReport.\n" +
                "1.2.3.0: " + "Update: OutsoleSize(OutsoleMaterialReport).\n" +
                "1.2.3.1: " + "Update: Put OrderSize, OutsoleSize(OutsoleMaterialReport).\n" +
                "1.2.3.2: " + "Update: Input Socklining Balance (Socklining Master)\n" +
                "1.2.3.3: " + "Update: Add Column: CutBStart, AtomCutAB, LaserCutAB, HuasenCutAB(Cutprep) - 'SV'. Add PatternNo (Outsole) - 'TL'.\n" +
                "1.2.3.4: " + "Update: OSMaterialArrival (Print Outsole - 'TL').\n" +
                "1.2.3.5: " + "Update: Add CutA Column (PO Completion) - 'SV'\n" +
                "1.2.3.6: " + "Update: Delay Shipment - 'SV', OSWHInventory By O/SLine - 'TL'\n" +
                "1.2.3.7: " + "Update: Reject Assembly / Stockfit (OSMaterialReport) - 'TL'\n" +
                "1.2.3.8: " + "Update: CutAActualStartDate, CutBActualStartDate (SewingMaster) column - 'SV'\n" +
                "1.2.4.0: " + "Update: OSWHDelivery(Detail)Report - 'SV'\n" +
                "1.2.4.1: " + "Update: OSWHBalance (O/S Master) - 'SV'\n" +
                "1.2.4.2: " + "Update: Saving Data - 'DL'\n" +
                "1.2.4.4: " + "Update: Import (Orders, SizeRun), Multiple 'SV-DL-TL'\n" +
                "1.2.4.5: " + "Update: SizeList ExportExcel, Column Reject (OSWHDeliveryDetail) 'TL'\n" +
                "1.2.4.6: " + "Update: OSWHDelivery(Wrong Supplier) 'TL'\n" +
                "1.2.4.8: " + "Update: Improve Loading Data Speed (WH Master File) 'SV'\n" +
                "1.2.5.0: " + "Update: Improve (Sewing Master File) 'SV'\n" +
                "1.2.5.2: " + "Update: List of Delivery Outsole 'SV'\n" +
                "1.2.5.3: " + "Update: Print Sewing Summary Report 'SV'\n" +
                "1.2.5.4: " + "Update: OSM WH Check 'SV'\n" +
                "1.2.5.6: " + "Update: OSM WH Check (Working Cart, WorkerId, Report OSMWHCheck Detail) 'SV'\n" +
                "1.2.5.7: " + "Update: OSM WH Check (Input qty follow o/s size). 'SV'\n" +
                "1.2.5.9: " + "Update: WH Check Current (OSMaster), POStatus Report, Reviser Incentive Report (OSMWHCheck) 'SV'\n" +
                "1.2.6.2: " + "Update: Input Return Reject, Report (OSWHMatsCheck), SewingSummaryReport 'SV'\n" +
                "1.2.6.3: " + "Update: Remove Input Reject (OSMaterial), OSSize Duplicate Error. 'SV'\n" +
                "1.2.6.5: " + "Update: Release Painting (Input OSMaterial WHMasterFile), ReleasePainting col DeliveryDetailReport 'SV'.\n" +
                "1.2.6.7: " + "Update: Sync OutsoleMaterialArrivalDate 'SV'.\n" +
                "1.2.6.9: " + "Update: SewingStart > CutAStart 18days, Speed Outsole (Inventory, Delivery) Reports 'SV'.\n" +
                "1.2.7.0: " + "Update: Add ComelzCutAB Column, Custom CutprepPrintReport. 'SV'.\n" +
                "1.2.7.3: " + "Update: Counter WorkingTime OSMWHChecking, Speed Report OSMWH, OSMWHChecking View Detail 'SV', col OSMatsArrival UpperDetailReport; Keys,Input Reject 'TL'.\n" +
                "1.2.7.4: " + "Update: PO OsMasterFile Empty, Reviser PO 'SV'.\n" +
                "1.2.7.8: " + "Update: Print Size Run Report, Add Import SizeRunAndOrders 'SV'.\n" +
                "1.2.7.9: " + "Update: Add LastSize (SizeRun) 'SV'\n" +
                "1.2.8.1: " + "Update: Combine Reject O/SWHMasterFile and O/SMaterial Check 'SV', Hidden OSSize.\n" +
                "1.2.8.2: " + "Update: Add OSMaterialWHCheck Inventory Report 'SV'.\n" +
                "1.2.8.3: " + "Update: Add Input Inspect Upper Accessories, Import Excel Data, Revise Supplier, Create Report, Report Detail 'SV.'\n" +
                "1.2.8.4: " + "Update: Import WHLamination Material, Report(OSWHMaterialCheckDetail) 'SV'.\n" +
                "1.2.8.5: " + "Update: Input Upper Accessories Delivery 'SV'.\n" +
                "1.2.8.6: " + "Update: Upper Accessories Reports 'SV'\n" +
                "1.2.8.8: " + "Update: Display OS/Size OSDeliveryReport 'SV'\n" +
                "1.2.9.1: " + "Update: SewingMaster report directly 'SV'\n" +
                "1.2.9.2: " + "Update: Import lamination excel file from newshoes report 'SV'\n" +
                "1.2.9.3: " + "Update: Improve WindowLoading Speed 'SV'\n" +
                "1.2.9.5: " + "Update: OS WH Material Report FromTo, OS Painting Report 'SV'\n" +
                "1.2.9.8: " + "Update: Improve saving data speed (sewing, outsole, assembly, socklining) 'SV', Separate SamplePO 'TL'.\n" +
                "1.2.9.9: " + "Update: Add ShipDate Column 'SV'.\n" +
                "1.3.0.2: " + "Update: Add Material Arrival Report Weekly 'SV'\n" +
                "1.3.0.4: " + "Update: Add Borrow OSMaterial, Send report to supervisors by email, Revise OSMaterialArrival Report, Create multi threads while load data from db. 'SV'";
            //MessageBox.Show(version, string.Format("Current Version: {0}", AssemblyHelper.Version()), MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateHistoryWindow window = new UpdateHistoryWindow(version, AssemblyHelper.Version());
            window.Show();
        }

        private void miUpperMaterialSchedule_Click(object sender, RoutedEventArgs e)
        {
            ImportUpperMaterialScheduleWindow window = new ImportUpperMaterialScheduleWindow();
            window.Show();
        }
    }
}
