using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for RawMaterialWindow.xaml
    /// </summary>
    public partial class RawMaterialWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoad;
        BackgroundWorker bwReload;
        List<RawMaterialViewModel> rawMaterialViewList;
        List<RawMaterialViewModel> rawMaterialViewToImportList;
        BackgroundWorker bwImport;
        DateTime dtDefault;
        DateTime dtNothing;
        public ObservableCollection<RawMaterialViewModel> rawMaterialViewSearchedList;
        List<RawMaterialViewModel> rawMaterialViewToRemoveList;
        BackgroundWorker bwRemoveOrder;
        List<RawMaterialCellChangedModel> rawMaterialCellChangedList;
        List<String> orderExtraChangedList;
        RawMaterialSearchBoxWindow searchBox;
        List<ProductionMemoModel> productionMemoList;

        List<RawMaterialViewModel> rawMaterialViewReloadList;
        List<RawMaterialViewModelNew> rawMaterialViewModelNewList;
        List<RejectModel> rejectUpperAccessoriesList;
        private PrivateDefineModel def;

        List<OrdersModel> ordersList;
        List<RawMaterialModel> rawMaterialList;
        List<OrderExtraModel> orderExtraList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        public RawMaterialWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;

            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwReload = new BackgroundWorker();
            bwReload.DoWork +=new DoWorkEventHandler(bwReload_DoWork);
            bwReload.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwReload_RunWorkerCompleted);

            rawMaterialViewList = new List<RawMaterialViewModel>();
            rawMaterialViewToImportList = new List<RawMaterialViewModel>();

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);

            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>();
            rawMaterialViewToRemoveList = new List<RawMaterialViewModel>();

            bwRemoveOrder = new BackgroundWorker();
            bwRemoveOrder.DoWork += new DoWorkEventHandler(bwRemoveOrder_DoWork);
            bwRemoveOrder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRemoveOrder_RunWorkerCompleted);

            rawMaterialCellChangedList = new List<RawMaterialCellChangedModel>();
            orderExtraChangedList = new List<String>();
            searchBox = new RawMaterialSearchBoxWindow();
            productionMemoList = new List<ProductionMemoModel>();

            rawMaterialViewReloadList = new List<RawMaterialViewModel>();
            rawMaterialViewModelNewList = new List<RawMaterialViewModelNew>();
            rejectUpperAccessoriesList = new List<RejectModel>();
            def = new PrivateDefineModel();

            ordersList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            orderExtraList = new List<OrderExtraModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            def = PrivateDefineController.GetDefine();
            if (!string.IsNullOrEmpty(def.Factory) && !def.Factory.Equals("THIENLOC"))
            {
                var productNoListWithAccount = OrdersController.Select().Where(w => account.TypeOfShoes != -1 ? w.TypeOfShoes == account.TypeOfShoes : w.TypeOfShoes != -16111992).Select(s => s.ProductNo).ToList();
                productionMemoList = ProductionMemoController.Select().Where(w => productNoListWithAccount.Contains(w.ProductionNumbers)).ToList();
                rawMaterialViewModelNewList = RawMaterialController.Select_1().Where(w => productNoListWithAccount.Contains(w.ProductNo)).ToList();
                rejectUpperAccessoriesList = RejectController.GetRejectUpperAccessories();
                int index = 1;
                foreach (var x in rawMaterialViewModelNewList)
                {
                    Dispatcher.Invoke(new Action(() => {
                        lblStatus.Text = String.Format("Creating {0}/{1} rows ...", index, rawMaterialViewModelNewList.Count());
                    }));
                    rawMaterialViewList.Add(ConvertX(x));
                    index++;
                }
            }
            else
            {
                ordersList = OrdersController.Select();
                rawMaterialList = RawMaterialController.Select();
                orderExtraList = OrderExtraController.Select();
                sewingMasterList = SewingMasterController.SelectCutAStartDate();
                productionMemoList = ProductionMemoController.Select();
                outsoleRawMaterialList = OutsoleRawMaterialController.Select();
                outsoleMaterialList = OutsoleMaterialController.Select();
                assemblyMasterList = AssemblyMasterController.Select();
                outsoleMasterList = OutsoleMasterController.Select();

                int index = 1;
                foreach (OrdersModel orders in ordersList)
                {
                    Dispatcher.Invoke(new Action(() => {
                        lblStatus.Text = String.Format("Creating {0}/{1} rows ...", index, ordersList.Count());
                    }));
                    RawMaterialViewModel rawMaterialView = Convert(orders);
                    {
                        rawMaterialViewList.Add(rawMaterialView);
                    }
                    index++;
                }
            }

        }
        
        private RawMaterialViewModel Convert(OrdersModel orders)
        {
            var rawMaterialView = new RawMaterialViewModel();
            rawMaterialView.ProductNo = orders.ProductNo;
            rawMaterialView.ProductNoBackground = Brushes.Transparent;
            rawMaterialView.Country = orders.Country;
            rawMaterialView.ShoeName = orders.ShoeName;
            rawMaterialView.ArticleNo = orders.ArticleNo;
            rawMaterialView.PatternNo = orders.PatternNo;
            rawMaterialView.OutsoleCode = orders.OutsoleCode;
            rawMaterialView.Quantity = orders.Quantity;
            rawMaterialView.ETD = orders.ETD;//String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", orders.ETD);

            rawMaterialView.CutAStartDate = dtDefault;
            var sewingMaster = sewingMasterList.Where(s => s.ProductNo == orders.ProductNo).FirstOrDefault();
            if (sewingMaster != null)
            {
                rawMaterialView.CutAStartDate = sewingMaster.CutAStartDate;
                // addtion code wrong
                //rawMaterialView.Sewing_StartDate = sewingMaster.SewingStartDate;
            }
            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            var materialArrivalUpper = MaterialArrival(orders.ProductNo, materialIdUpperArray);
            DateTime upperMatsArrivalOrginal = dtDefault;
            if (materialArrivalUpper != null)
            {
                upperMatsArrivalOrginal = materialArrivalUpper.Date;
            }
            rawMaterialView.CutAStartDateForeground = Brushes.Black;
            if (rawMaterialView.CutAStartDate < upperMatsArrivalOrginal)
            {
                rawMaterialView.CutAStartDateForeground = Brushes.Red;
            }
            else
            {
                if (materialArrivalUpper != null)
                {
                    // addtion code. same with cutastartdate in sewing master file
                    //int range = TimeHelper.CalculateDate(materialArrivalUpper.Date, rawMaterialView.CutAStartDate);
                    int range = (Int32)((rawMaterialView.CutAStartDate - materialArrivalUpper.Date).TotalDays);
                    if (0 <= range && range <= 3)
                    {
                        rawMaterialView.CutAStartDateForeground = Brushes.Orange;
                    }

                    else
                    {
                        if (rawMaterialView.CutAStartDateForeground == Brushes.Orange)
                        {
                            rawMaterialView.CutAStartDateForeground = Brushes.Orange;
                        }
                        else
                        {
                            rawMaterialView.CutAStartDateForeground = materialArrivalUpper.Foreground;
                        }
                    }
                }
            }

            // Assembly startDate
            var assyMaster = assemblyMasterList.Where(w => w.ProductNo == orders.ProductNo).FirstOrDefault();
            var outsoleMaster = outsoleMasterList.Where(w => w.ProductNo == orders.ProductNo).FirstOrDefault();
            if (assyMaster != null && sewingMaster != null && outsoleMaster != null)
            {
                rawMaterialView.AssemblyStartDate = assyMaster.AssemblyStartDate;
                rawMaterialView.AssemblyStartDateForeground = Brushes.Black;
                if (rawMaterialView.AssemblyStartDate < new DateTime(Math.Max(outsoleMaster.OutsoleFinishDate.Ticks, sewingMaster.SewingFinishDate.Ticks)))
                {
                    rawMaterialView.AssemblyStartDateForeground = Brushes.Red;
                }
            }

            string memoId = "";
            var productionMemoByProductionNumberList = productionMemoList.Where(p => p.ProductionNumbers.Contains(orders.ProductNo) == true).ToList();
            for (int p = 0; p <= productionMemoByProductionNumberList.Count - 1; p++)
            {
                var productionMemo = productionMemoByProductionNumberList[p];
                memoId += productionMemo.MemoId;
                if (p < productionMemoByProductionNumberList.Count - 1)
                {
                    memoId += "\n";
                }
            }
            rawMaterialView.MemoId = memoId;
            //LAMINATION 1
            var rawMaterialList_D1 = rawMaterialList.Where(r => r.ProductNo == orders.ProductNo).ToList();

            var lamination = rawMaterialList_D1.Where(r => r.MaterialTypeId == 1).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (lamination != null)
            {
                rawMaterialView.LAMINATION_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", lamination.ETD);
                if (lamination.ETD.Date == dtDefault)
                {
                    rawMaterialView.LAMINATION_ETD = "";
                }
                rawMaterialView.LAMINATION_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", lamination.ActualDate);
                if (lamination.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.LAMINATION_ActualDate = "";
                }
                rawMaterialView.LAMINATION_Remarks = lamination.Remarks;
            }

            //TAIWAN 10
            var taiwan = rawMaterialList_D1.Where(r => r.MaterialTypeId == 10).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (taiwan != null)
            {
                rawMaterialView.TAIWAN_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", taiwan.ETD);
                if (taiwan.ETD.Date == dtDefault)
                {
                    rawMaterialView.TAIWAN_ETD = "";
                }
                rawMaterialView.TAIWAN_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", taiwan.ActualDate);
                if (taiwan.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.TAIWAN_ActualDate = "";
                }
                rawMaterialView.TAIWAN_Remarks = taiwan.Remarks;
            }

            //CUTTING 2
            var cutting = rawMaterialList_D1.Where(r => r.MaterialTypeId == 2).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (cutting != null)
            {
                rawMaterialView.CUTTING_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", cutting.ETD);
                if (cutting.ETD.Date == dtDefault)
                {
                    rawMaterialView.CUTTING_ETD = "";
                }
                rawMaterialView.CUTTING_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", cutting.ActualDate);
                if (cutting.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.CUTTING_ActualDate = "";
                }
                rawMaterialView.CUTTING_Remarks = cutting.Remarks;
            }

            //LEATHER 3
            var leather = rawMaterialList_D1.Where(r => r.MaterialTypeId == 3).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (leather != null)
            {
                rawMaterialView.LEATHER_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", leather.ETD);
                if (leather.ETD.Date == dtDefault)
                {
                    rawMaterialView.LEATHER_ETD = "";
                }
                rawMaterialView.LEATHER_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", leather.ActualDate);
                if (leather.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.LEATHER_ActualDate = "";
                }
                rawMaterialView.LEATHER_Remarks = leather.Remarks;
            }

            //SEMI-PROCESS 4
            var semiprocess = rawMaterialList_D1.Where(r => r.MaterialTypeId == 4).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (semiprocess != null)
            {
                rawMaterialView.SEMIPROCESS_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", semiprocess.ETD);
                if (semiprocess.ETD.Date == dtDefault)
                {
                    rawMaterialView.SEMIPROCESS_ETD = "";
                }
                rawMaterialView.SEMIPROCESS_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", semiprocess.ActualDate);
                if (semiprocess.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.SEMIPROCESS_ActualDate = "";
                }
                rawMaterialView.SEMIPROCESS_Remarks = semiprocess.Remarks;
            }

            //SEWING 5
            var sewing = rawMaterialList_D1.Where(r => r.MaterialTypeId == 5).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (sewing != null)
            {
                rawMaterialView.SEWING_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewing.ETD);
                if (sewing.ETD.Date == dtDefault)
                {
                    rawMaterialView.SEWING_ETD = "";
                }
                rawMaterialView.SEWING_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewing.ActualDate);
                if (sewing.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.SEWING_ActualDate = "";
                }
                rawMaterialView.SEWING_Remarks = sewing.Remarks;
            }

            //OUTSOLE 6

            #region OLD CODE
            //RawMaterialModel outsole = new RawMaterialModel();
            //outsole = rawMaterialList_D1.Where(r => r.MaterialTypeId == 6).OrderBy(r => r.ModifiedTime).LastOrDefault();

            //var outsoleMaterialQuantityZero = new List<OutsoleMaterialModel>();
            //outsoleMaterialQuantityZero = outsoleMaterialList.Where(w => w.Quantity == 0 && w.ProductNo == orders.ProductNo).ToList();
            //var outsoleSupplierIdList = new List<int>();
            //outsoleSupplierIdList = outsoleMaterialQuantityZero.Select(s => s.OutsoleSupplierId).Distinct().ToList();
            //var outsoleRawMaterialNotSupplied = new List<OutsoleRawMaterialModel>();
            //foreach (int supplierId in outsoleSupplierIdList)
            //{
            //    var outsoleRawMaterialBySupplierId = new OutsoleRawMaterialModel();
            //    outsoleRawMaterialBySupplierId = outsoleRawMaterialList.Where(w => w.ProductNo == orders.ProductNo && w.OutsoleSupplierId == supplierId).FirstOrDefault();
            //    outsoleRawMaterialNotSupplied.Add(outsoleRawMaterialBySupplierId);
            //}

            //if (outsoleRawMaterialNotSupplied.Count > 0)
            //{
            //    var outsoleRaw = outsoleRawMaterialNotSupplied.OrderBy(o => o.ETD).LastOrDefault();
            //    var outsoleRemarks = rawMaterialList_D1.Where(r => r.MaterialTypeId == 6).OrderBy(r => r.ModifiedTime).LastOrDefault();
            //    rawMaterialView.OUTSOLE_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", outsoleRaw.ETD);
            //    rawMaterialView.OUTSOLE_ActualDate = "";
            //    rawMaterialView.OUTSOLE_Remarks = outsoleRemarks.Remarks;
            //}
            //else
            //{
            //    var outsole = rawMaterialList_D1.Where(r => r.MaterialTypeId == 6).OrderBy(r => r.ModifiedTime).LastOrDefault();
            //    if (outsole != null)
            //    {
            //        rawMaterialView.OUTSOLE_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", outsole.ETD);
            //        if (outsole.ETD.Date == dtDefault)
            //        {
            //            rawMaterialView.OUTSOLE_ETD = "";
            //        }
            //        rawMaterialView.OUTSOLE_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", outsole.ActualDate);
            //        if (outsole.ActualDate.Date == dtDefault)
            //        {
            //            rawMaterialView.OUTSOLE_ActualDate = "";
            //        }
            //        rawMaterialView.OUTSOLE_Remarks = outsole.Remarks;
            //    }
            //}
            #endregion

            // Load ETD,ActualDate from OutsoleRawMaterial
            var outsoleRawMaterial_PO = outsoleRawMaterialList.Where(w => w.ProductNo == orders.ProductNo).ToList();
            if (outsoleRawMaterial_PO.Count > 0)
            {
                rawMaterialView.OUTSOLE_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}",
                                                            outsoleRawMaterial_PO.OrderBy(o => o.ETD).LastOrDefault().ETD);
                var actualDateList = outsoleRawMaterial_PO.Select(s => s.ActualDate).ToList();
                if (!actualDateList.Contains(dtDefault))
                {
                    rawMaterialView.OUTSOLE_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}",
                                                                       outsoleRawMaterial_PO.OrderBy(o => o.ActualDate).LastOrDefault().ActualDate);
                }
            }

            // Load Outsole_Remarks from OutsoleMaterial
            var outsoleMaterial_PO = outsoleMaterialList.Where(w => w.ProductNo == orders.ProductNo).ToList();
            if (outsoleMaterial_PO.Sum(s => s.Quantity) > 0)
            {
                var supplierIdList = outsoleMaterial_PO.Select(s => s.OutsoleSupplierId).Distinct().ToList();
                var balanceList = new List<Int32>();
                foreach (var supplierId in supplierIdList)
                {
                    int quantity_SupplierId = outsoleMaterial_PO.Where(w => w.OutsoleSupplierId == supplierId).Sum(s => s.Quantity);
                    int reject_SupplierId = outsoleMaterial_PO.Where(w => w.OutsoleSupplierId == supplierId).Sum(s => s.QuantityReject);
                    int balance;
                    if (quantity_SupplierId < orders.Quantity)
                        balance = orders.Quantity - quantity_SupplierId + reject_SupplierId;
                    else
                        balance = reject_SupplierId;
                    balanceList.Add(balance);
                }
                if (balanceList.Max() > 0)
                {
                    rawMaterialView.OUTSOLE_Remarks = balanceList.Max().ToString();
                    rawMaterialView.OUTSOLE_ActualDate = "";
                }

                // Highlight Po has RejectAssembly
                rawMaterialView.OUTSOLE_ActualDate_BACKGROUND = Brushes.Transparent;
                var totalRejectAssembly = outsoleMaterial_PO.Sum(s => s.RejectAssembly);
                if (totalRejectAssembly > 0)
                    rawMaterialView.OUTSOLE_ActualDate_BACKGROUND = Brushes.Yellow;
            }

            // UPPER COMPONENT MATERIAL 12
            var upperComponentMaterial = rawMaterialList_D1.Where(w => w.MaterialTypeId == 12).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (upperComponentMaterial != null)
            {
                if (upperComponentMaterial.ETD.Date != dtDefault)
                    rawMaterialView.UPPERCOMPONENT_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", upperComponentMaterial.ETD);
                if (upperComponentMaterial.ActualDate.Date != dtDefault)
                    rawMaterialView.UPPERCOMPONENT_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", upperComponentMaterial.ActualDate);

                rawMaterialView.UPPERCOMPONENT_Remarks = upperComponentMaterial.Remarks;
            }

            // INSOCK 13
            var insockRawMaterial = rawMaterialList_D1.Where(w => w.MaterialTypeId == 13).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (insockRawMaterial != null)
            {
                rawMaterialView.INSOCK_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", insockRawMaterial.ETD);
                if (insockRawMaterial.ETD.Date == dtDefault)
                {
                    rawMaterialView.INSOCK_ETD = "";
                }
                rawMaterialView.INSOCK_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", insockRawMaterial.ActualDate);
                if (insockRawMaterial.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.INSOCK_ActualDate = "";
                }
                rawMaterialView.INSOCK_Remarks = insockRawMaterial.Remarks;
            }


            //SECURITY LABEL 7
            var securityLabel = rawMaterialList_D1.Where(r => r.MaterialTypeId == 7).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (securityLabel != null)
            {
                rawMaterialView.SECURITYLABEL_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", securityLabel.ETD);
                if (securityLabel.ETD.Date == dtDefault)
                {
                    rawMaterialView.SECURITYLABEL_ETD = "";
                }
                rawMaterialView.SECURITYLABEL_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", securityLabel.ActualDate);
                if (securityLabel.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.SECURITYLABEL_ActualDate = "";
                }
                rawMaterialView.SECURITYLABEL_Remarks = securityLabel.Remarks;
            }

            //ASSEMBLY 8
            var assembly = rawMaterialList_D1.Where(r => r.MaterialTypeId == 8).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (assembly != null)
            {
                rawMaterialView.ASSEMBLY_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", assembly.ETD);
                if (assembly.ETD.Date == dtDefault)
                {
                    rawMaterialView.ASSEMBLY_ETD = "";
                }
                rawMaterialView.ASSEMBLY_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", assembly.ActualDate);
                if (assembly.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.ASSEMBLY_ActualDate = "";
                }
                rawMaterialView.ASSEMBLY_Remarks = assembly.Remarks;
            }

            //SOCK LINING 9
            var sockLining = rawMaterialList_D1.Where(r => r.MaterialTypeId == 9).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (sockLining != null)
            {
                rawMaterialView.SOCKLINING_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sockLining.ETD);
                if (sockLining.ETD.Date == dtDefault)
                {
                    rawMaterialView.SOCKLINING_ETD = "";
                }
                rawMaterialView.SOCKLINING_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sockLining.ActualDate);
                if (sockLining.ActualDate.Date == dtDefault)
                {
                    rawMaterialView.SOCKLINING_ActualDate = "";
                }
                rawMaterialView.SOCKLINING_Remarks = sockLining.Remarks;
            }

            //CARTON 11
            var assyGetStartDate = new AssemblyMasterModel();
            assyGetStartDate = assemblyMasterList.Where(w => w.ProductNo == orders.ProductNo).FirstOrDefault();
            DateTime assyStartDate = dtDefault;
            if (assyGetStartDate != null)
            {
                if (assyGetStartDate.AssemblyStartDate != dtDefault)
                {
                    assyStartDate = assyGetStartDate.AssemblyStartDate;
                }
            }

            var carton = rawMaterialList_D1.Where(r => r.MaterialTypeId == 11).OrderBy(r => r.ModifiedTime).LastOrDefault();
            if (carton != null)
            {
                int dateRange = 0;
                rawMaterialView.CARTON_ETD = "";
                rawMaterialView.CARTON_ActualDate = "";
                rawMaterialView.CARTON_Remarks = carton.Remarks;

                rawMaterialView.CARTON_ActualDate_Sort = carton.ActualDate;
                rawMaterialView.CARTON_ETD_Sort = carton.ETD;
                if (carton.ETD != dtDefault)
                {
                    rawMaterialView.CARTON_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", carton.ETD);
                    if (assyStartDate != dtDefault)
                    {
                        //dateRange = TimeHelper.CalculateDate(carton.ETD, assyStartDate);
                        dateRange = (Int32)((assyStartDate - carton.ETD).TotalDays);
                    }
                }

                if (carton.ActualDate != dtDefault)
                {
                    rawMaterialView.CARTON_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", carton.ActualDate);
                    if (assyStartDate != dtDefault)
                    {
                        //dateRange = TimeHelper.CalculateDate(carton.ActualDate, assyStartDate);
                        dateRange = (Int32)((assyStartDate - carton.ActualDate).TotalDays);
                    }
                }

                // Hightlight
                rawMaterialView.CARTON_ETD_Background = Brushes.Transparent;
                rawMaterialView.CARTON_ActualDate_Background = Brushes.Transparent;
                if (dateRange <= 2)
                {
                    if (String.IsNullOrEmpty(rawMaterialView.CARTON_ActualDate) == false)
                        rawMaterialView.CARTON_ActualDate_Background = Brushes.Red;
                    if (String.IsNullOrEmpty(rawMaterialView.CARTON_ETD) == false)
                        rawMaterialView.CARTON_ETD_Background = Brushes.Red;
                }

                if (dateRange > 14)
                {
                    if (String.IsNullOrEmpty(rawMaterialView.CARTON_ActualDate) == false)
                        rawMaterialView.CARTON_ActualDate_Background = Brushes.Yellow;
                    if (String.IsNullOrEmpty(rawMaterialView.CARTON_ETD) == false)
                        rawMaterialView.CARTON_ETD_Background = Brushes.Yellow;
                }
            }


            var orderExtra = orderExtraList.Where(o => o.ProductNo == orders.ProductNo).FirstOrDefault();
            if (orderExtra != null)
            {
                DateTime loadingDate = dtDefault;
                if (DateTime.TryParse(orderExtra.LoadingDate, out loadingDate) == true)
                {
                    rawMaterialView.LoadingDate = string.Format(new CultureInfo("en-US"), "{0:dd-MMM}", loadingDate);
                }
                else
                {
                    rawMaterialView.LoadingDate = "";
                }
            }
            return rawMaterialView;
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            var rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            rawMaterialTypeList.RemoveAll(r => r.ETD.Date == dtDefault.Date);
            var materialArrivalView = new MaterialArrivalViewModel();
            if (rawMaterialTypeList.Select(r => r.ActualDate).Count() > 0 && rawMaterialTypeList.Select(r => r.ActualDate.Date).Contains(dtDefault.Date) == false)
            {
                materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ActualDate).Max();
                materialArrivalView.Foreground = Brushes.Blue;
                materialArrivalView.Background = Brushes.Transparent;
            }
            else
            {
                if (rawMaterialTypeList.Select(r => r.ETD).Count() > 0 && rawMaterialTypeList.Where(r => r.ETD.Date != dtDefault.Date).Count() > 0)
                {
                    materialArrivalView.Date = rawMaterialTypeList.Where(r => r.ActualDate.Date == dtDefault.Date).Select(r => r.ETD).Max();
                    materialArrivalView.Foreground = Brushes.Black;
                    materialArrivalView.Background = Brushes.Transparent;
                    if (materialArrivalView.Date < DateTime.Now.Date)
                    {
                        materialArrivalView.Background = Brushes.Red;
                    }
                    else
                    {
                        if (rawMaterialTypeList.Where(r => String.IsNullOrEmpty(r.Remarks) == false).Count() > 0)
                        {
                            materialArrivalView.Background = Brushes.Yellow;
                        }
                    }
                }
                else
                {
                    materialArrivalView = null;
                }
            }
            return materialArrivalView;
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewList);
            dgRawMaterial.ItemsSource = rawMaterialViewSearchedList;
            lblStatus.Visibility = Visibility.Collapsed;
            btnSave.IsEnabled = true;
            btnReload.IsEnabled = true;
            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.UpperRMSchedule == true)
            {
                Column8.IsReadOnly = false;
                Column9.IsReadOnly = false;
                Column10.IsReadOnly = false;

                Column10_1.IsReadOnly = false;
                Column10_2.IsReadOnly = false;
                Column10_3.IsReadOnly = false;

                Column11.IsReadOnly = false;
                Column12.IsReadOnly = false;
                Column13.IsReadOnly = false;

                Column14.IsReadOnly = false;
                Column15.IsReadOnly = false;
                Column16.IsReadOnly = false;

                Column17.IsReadOnly = false;
                Column18.IsReadOnly = false;
                Column19.IsReadOnly = false;

                Column20.IsReadOnly = false;
                Column21.IsReadOnly = false;
                Column22.IsReadOnly = false;

                Column26.IsReadOnly = false;
                Column27.IsReadOnly = false;
                Column28.IsReadOnly = false;

                Column29.IsReadOnly = false;
                Column30.IsReadOnly = false;
                Column31.IsReadOnly = false;

                Column32.IsReadOnly = false;
                Column33.IsReadOnly = false;
                Column34.IsReadOnly = false;
            }
            if (account.UpperComponentRMSchedule == true)
            {
                //Upper Component Material
                //Column25_1.IsReadOnly = false;
                //Column25_2.IsReadOnly = false;
                //Column25_3.IsReadOnly = false;
            }

            //if (account.UpperAccessories == true)
            //{
            //    Column25_1.IsReadOnly = true;
            //    Column25_2.IsReadOnly = true;
            //}

            if (account.CartonRMSchedule == true)
            {
                Column35.IsReadOnly = false;
                Column36.IsReadOnly = false;
                Column37.IsReadOnly = false;

                Column38.IsReadOnly = false;
            }

            // LAMINATION
            Column8.SetValue(TagProperty, 1);
            Column9.SetValue(TagProperty, 1);
            Column10.SetValue(TagProperty, 1);

            // TAIWAN
            Column10_1.SetValue(TagProperty, 10);
            Column10_2.SetValue(TagProperty, 10);
            Column10_3.SetValue(TagProperty, 10);

            // CUTTING
            Column11.SetValue(TagProperty, 2);
            Column12.SetValue(TagProperty, 2);
            Column13.SetValue(TagProperty, 2);

            // LEATHER
            Column14.SetValue(TagProperty, 3);
            Column15.SetValue(TagProperty, 3);
            Column16.SetValue(TagProperty, 3);

            // SEMI-PROCESS
            Column17.SetValue(TagProperty, 4);
            Column18.SetValue(TagProperty, 4);
            Column19.SetValue(TagProperty, 4);

            // SEWING
            Column20.SetValue(TagProperty, 5);
            Column21.SetValue(TagProperty, 5);
            Column22.SetValue(TagProperty, 5);

            // OUTSOLE
            Column23.SetValue(TagProperty, 6);
            Column24.SetValue(TagProperty, 6);
            Column25.SetValue(TagProperty, 6);

            // SECURITY LABEL
            Column26.SetValue(TagProperty, 7);
            Column27.SetValue(TagProperty, 7);
            Column28.SetValue(TagProperty, 7);

            // ASSEMBLY
            Column29.SetValue(TagProperty, 8);
            Column30.SetValue(TagProperty, 8);
            Column31.SetValue(TagProperty, 8);

            // SOCKLINING
            Column32.SetValue(TagProperty, 9);
            Column33.SetValue(TagProperty, 9);
            Column34.SetValue(TagProperty, 9);

            // CARTON
            Column35.SetValue(TagProperty, 11);
            Column36.SetValue(TagProperty, 11);
            Column37.SetValue(TagProperty, 11);

            // UPPER COMPONENT
            Column25_1.SetValue(TagProperty, 12);
            Column25_2.SetValue(TagProperty, 12);
            Column25_3.SetValue(TagProperty, 12);

            // INSOCK
            Column25_4.SetValue(TagProperty, 13);
            Column25_5.SetValue(TagProperty, 13);
            Column25_6.SetValue(TagProperty, 13);

            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private RawMaterialViewModel ConvertX( RawMaterialViewModelNew rawNew)
        {
            var rawMaterialView = new RawMaterialViewModel();
            rawMaterialView.ProductNo = rawNew.ProductNo;
            rawMaterialView.ProductNoBackground = Brushes.Transparent;
            rawMaterialView.Country = rawNew.Country;
            rawMaterialView.ShoeName = rawNew.ShoeName;
            rawMaterialView.ArticleNo = rawNew.ArticleNo;
            rawMaterialView.PatternNo = rawNew.PatternNo;
            rawMaterialView.OutsoleCode = rawNew.OutsoleCode;
            rawMaterialView.Quantity = rawNew.Quantity;
            rawMaterialView.ETD = rawNew.ETD;
            rawMaterialView.CutAStartDate = rawNew.CutAStartDate;
            rawMaterialView.AssemblyStartDate = rawNew.AssemblyStartDate;
            
            // Highlight CutAStartDate Column
            // da giao hang
            if (rawNew.MatterialArrival != dtDefault)
            {
                rawMaterialView.CutAStartDateForeground = Brushes.Blue;
                var range = (rawNew.CutAStartDate - rawNew.MatterialArrival).TotalDays;
                if (range <= 3 && range >= 0)
                    rawMaterialView.CutAStartDateForeground = Brushes.Orange;
                if (rawNew.CutAStartDate < rawNew.MatterialArrival)
                    rawMaterialView.CutAStartDateForeground = Brushes.Red;
            }
            // chua giao hang
            else
            {
                rawMaterialView.CutAStartDateForeground = Brushes.Black;
                if (rawNew.MatterialETD != dtDefault)
                {
                    if (rawNew.MatterialETD < DateTime.Now.Date)
                        rawMaterialView.CutAStartDateForeground = Brushes.Red;
                    else
                    {
                        // chua giao hang, co remarks
                        if (String.IsNullOrEmpty(rawNew.MatterialRemarks) == false)
                            rawMaterialView.CutAStartDateForeground = Brushes.Yellow;
                    }
                }
            }

            // Highlight AssemblyStartDate Column
            rawMaterialView.AssemblyStartDateForeground = Brushes.Black;
            if (rawNew.AssemblyStartDate < new DateTime(Math.Max(rawNew.OutsoleFinishDate.Ticks, rawNew.SewingFinishDate.Ticks)))
            {
                rawMaterialView.AssemblyStartDateForeground = Brushes.Red;
            }

            // Highlight Carton Column
            rawMaterialView.CARTON_ETD_Background           = Brushes.Transparent;
            rawMaterialView.CARTON_ActualDate_Background    = Brushes.Transparent;
            double x = 0;
            if (rawNew.Carton_ETD_Sort != dtDefault)
                x = (rawNew.AssemblyStartDate - rawNew.Carton_ETD_Sort).TotalDays;            
            if (rawNew.Carton_ActualDate_Sort != dtDefault)
                x = (rawNew.AssemblyStartDate - rawNew.Carton_ActualDate_Sort).TotalDays;
            
            if (x <= 2)
            {
                if (rawNew.Carton_ETD_Sort != dtDefault)
                    rawMaterialView.CARTON_ETD_Background = Brushes.Red;
                if (rawNew.Carton_ActualDate_Sort != dtDefault)
                    rawMaterialView.CARTON_ActualDate_Background = Brushes.Red;
            }
            if (x > 14)
            {
                if (rawNew.Carton_ETD_Sort != dtDefault)
                    rawMaterialView.CARTON_ETD_Background = Brushes.Yellow;
                if (rawNew.Carton_ActualDate_Sort != dtDefault)
                    rawMaterialView.CARTON_ActualDate_Background = Brushes.Yellow;
            }

            // Highlight Outsole Column
            if (String.IsNullOrEmpty(rawNew.OUTSOLE_AssemblyReject) == false)
                rawMaterialView.OUTSOLE_ActualDate_BACKGROUND = Brushes.Yellow;

            var memoByPOList = productionMemoList.Where(p => p.ProductionNumbers.Contains(rawNew.ProductNo) == true).Select(s => s.MemoId).ToList();

            rawMaterialView.MemoId = memoByPOList.Count() > 0 ? String.Join("\n", memoByPOList) : "";
            // LAMINATION 1
            rawMaterialView.LAMINATION_ETD = rawNew.LAMINATION_ETD;
            rawMaterialView.LAMINATION_ActualDate = rawNew.LAMINATION_ActualDate;
            rawMaterialView.LAMINATION_Remarks = rawNew.LAMINATION_Remarks;

            // TAIWAN 10
            rawMaterialView.TAIWAN_ETD = rawNew.TAIWAN_ETD;
            rawMaterialView.TAIWAN_ActualDate = rawNew.TAIWAN_ActualDate;
            rawMaterialView.TAIWAN_Remarks = rawNew.TAIWAN_Remarks;

            // CUTTING 2
            rawMaterialView.CUTTING_ETD = rawNew.CUTTING_ETD;
            rawMaterialView.CUTTING_ActualDate = rawNew.CUTTING_ActualDate;
            rawMaterialView.CUTTING_Remarks = rawNew.CUTTING_Remarks;

            // LEATHER 3
            rawMaterialView.LEATHER_ETD = rawNew.LEATHER_ETD;
            rawMaterialView.LEATHER_ActualDate = rawNew.LEATHER_ActualDate;
            rawMaterialView.LEATHER_Remarks = rawNew.LEATHER_Remarks;

            // SEMIPROCESS 4
            rawMaterialView.SEMIPROCESS_ETD = rawNew.SEMIPROCESS_ETD;
            rawMaterialView.SEMIPROCESS_ActualDate = rawNew.SEMIPROCESS_ActualDate;
            rawMaterialView.SEMIPROCESS_Remarks = rawNew.SEMIPROCESS_Remarks;

            // SEWING 4
            rawMaterialView.SEWING_ETD = rawNew.SEWING_ETD;
            rawMaterialView.SEWING_ActualDate = rawNew.SEWING_ActualDate;
            rawMaterialView.SEWING_Remarks = rawNew.SEWING_Remarks;

            // OUTSOLE 6
            rawMaterialView.OUTSOLE_ETD = rawNew.OUTSOLE_ETD;
            rawMaterialView.OUTSOLE_ActualDate = rawNew.OUTSOLE_ActualDate;
            rawMaterialView.OUTSOLE_Remarks = rawNew.OUTSOLE_Remarks;

            // UPPER COMPONENT 12
            rawMaterialView.UPPERCOMPONENT_ETD = rawNew.UPPERCOMPONENT_ETD;
            rawMaterialView.UPPERCOMPONENT_ActualDate = rawNew.UPPERCOMPONENT_ActualDate;
            rawMaterialView.UPPERCOMPONENT_Remarks = rawNew.UPPERCOMPONENT_Remarks;

            // INSOCK 13
            rawMaterialView.INSOCK_ETD = rawNew.INSOCK_ETD;
            rawMaterialView.INSOCK_ActualDate = rawNew.INSOCK_ActualDate;
            rawMaterialView.INSOCK_Remarks = rawNew.INSOCK_Remarks;

            // SECURITY 7
            rawMaterialView.SECURITYLABEL_ETD = rawNew.SECURITYLABEL_ETD;
            rawMaterialView.SECURITYLABEL_ActualDate = rawNew.SECURITYLABEL_ActualDate;
            rawMaterialView.SECURITYLABEL_Remarks = rawNew.SECURITYLABEL_Remarks;

            // ASSEMBLY 8
            rawMaterialView.ASSEMBLY_ETD = rawNew.ASSEMBLY_ETD;
            rawMaterialView.ASSEMBLY_ActualDate = rawNew.ASSEMBLY_ActualDate;
            rawMaterialView.ASSEMBLY_Remarks = rawNew.ASSEMBLY_Remarks;

            // SOCKLINING 8
            rawMaterialView.SOCKLINING_ETD = rawNew.SOCKLINING_ETD;
            rawMaterialView.SOCKLINING_ActualDate = rawNew.SOCKLINING_ActualDate;
            rawMaterialView.SOCKLINING_Remarks = rawNew.SOCKLINING_Remarks;

            // CARTON 11
            rawMaterialView.CARTON_ETD = rawNew.CARTON_ETD;
            rawMaterialView.CARTON_ActualDate = rawNew.CARTON_ActualDate;
            rawMaterialView.CARTON_Remarks = rawNew.CARTON_Remarks;
            rawMaterialView.CARTON_ETD_Sort = rawNew.Carton_ETD_Sort;
            rawMaterialView.CARTON_ActualDate_Sort = rawNew.Carton_ActualDate_Sort;

            rawMaterialView.UpperAccessories_ETD        = rawNew.UpperAccessories_ETD;
            rawMaterialView.UpperAccessories_ActualDate = rawNew.UpperAccessories_ActualDate;
            rawMaterialView.UpperAccessories_Remarks    = rawNew.UpperAccessories_Remarks;
            rawMaterialView.UpperAccessories_ActualDeliveryDate = rawNew.UpperAccessories_ActualDeliveryDate;

            rawMaterialView.LoadingDate = rawNew.LoadingDate;

            return rawMaterialView;
        }

        private void dgRawMaterial_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGridColumn[] colDatetimeList = {    Column10_1, Column10_2,
                                                    Column8, Column9, 
                                                    Column11, Column12, 
                                                    Column14, Column15, 
                                                    Column17, Column18,
                                                    Column20, Column21,
                                                    Column26, Column27, 
                                                    Column29, Column30,
                                                    Column32, Column33,
                                                    Column35, Column36,
                                                    Column25_1, Column25_2,
                                                    Column25_4, Column25_5,
                                                    Column38 };
            var rawMaterialView = (RawMaterialViewModel)e.Row.Item;
            //int columnIndex = e.Column.DisplayIndex;
            if (colDatetimeList.Contains(e.Column) == true)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }

                //if (String.IsNullOrEmpty(txtElement.Text) == false && (e.Column == Column35 || e.Column == Column36))
                //{
                //    DateTime dateConvert = DateTime.Parse(txtElement.Text);
                //    txtElement.Text = string.Format("{0:dd-MM}", dateConvert);
                //}

                if (e.Column != Column38)
                {
                    int materialType = (int)e.Column.GetValue(TagProperty);
                    if (rawMaterialView != null)
                    {
                        rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                        {
                            ProductNo = rawMaterialView.ProductNo,
                            MaterialType = materialType,
                        });
                    }
                }
                else
                {
                    if (rawMaterialView != null)
                    {
                        orderExtraChangedList.Add(rawMaterialView.ProductNo);
                    }
                }
            }
            
            //Column Remark
            DataGridColumn[] colRemarkList = { Column10, 
                                               Column10_3,
                                               Column13, 
                                               Column16, 
                                               Column19,
                                               Column22, 
                                               Column25, 
                                               Column28, 
                                               Column31, 
                                               Column34, 
                                               Column37, 
                                               Column25_3, 
                                               Column25_6};
            //int columnIndex = e.Column.DisplayIndex;
            if (colRemarkList.Contains(e.Column) == true)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                int materialType = (int)e.Column.GetValue(TagProperty);
                if (rawMaterialView != null)
                {
                    rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                    {
                        ProductNo = rawMaterialView.ProductNo,
                        MaterialType = materialType,
                    });
                }
            }

        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                rawMaterialViewToImportList = dgRawMaterial.Items.OfType<RawMaterialViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwImport.RunWorkerAsync();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                rawMaterialViewToImportList = dgRawMaterial.Items.OfType<RawMaterialViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var rawMaterialView in rawMaterialViewToImportList)
            {
                if (orderExtraChangedList.Contains(rawMaterialView.ProductNo) == true)
                {
                    if (string.IsNullOrEmpty(rawMaterialView.LoadingDate) == true)
                    {
                        OrderExtraModel model = new OrderExtraModel
                        {
                            ProductNo = rawMaterialView.ProductNo,
                            LoadingDate = "",
                        };
                        OrderExtraController.Insert(model);
                    }
                    else
                    {
                        DateTime loadingDate = TimeHelper.Convert(rawMaterialView.LoadingDate);
                        if (loadingDate != dtNothing)
                        {
                            OrderExtraModel model = new OrderExtraModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                LoadingDate = string.Format("{0:yyyy-MM-dd}", loadingDate),
                            };
                            OrderExtraController.Insert(model);
                        }
                    }
                }

                if (rawMaterialCellChangedList.Select(r => r.ProductNo).Contains(rawMaterialView.ProductNo) == true)
                {
                    var rawMaterialCellChangedList_D1 = rawMaterialCellChangedList.Where(r => r.ProductNo == rawMaterialView.ProductNo).ToList();

                    //LAMINATION 1
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 1) != null)
                    {
                        DateTime lamination_ETD = TimeHelper.Convert(rawMaterialView.LAMINATION_ETD);
                        DateTime lamination_ActualDate = TimeHelper.Convert(rawMaterialView.LAMINATION_ActualDate);
                        string lamination_Remarks = rawMaterialView.LAMINATION_Remarks;
                        if (lamination_Remarks == null)
                        {
                            lamination_Remarks = "";
                        }
                        //if (lamination_ETD != dtDefault
                        //    || lamination_ActualDate != dtDefault || String.IsNullOrEmpty(lamination_Remarks) == false)
                        {
                            var lamination_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 1,
                                ETD = lamination_ETD,
                                ActualDate = lamination_ActualDate,
                                Remarks = lamination_Remarks.Trim(),
                            };

                            RawMaterialController.Insert(lamination_Model);
                        }
                    }

                    //TAIWAN 10
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 10) != null)
                    {
                        DateTime taiwan_ETD = TimeHelper.Convert(rawMaterialView.TAIWAN_ETD);
                        DateTime taiwan_ActualDate = TimeHelper.Convert(rawMaterialView.TAIWAN_ActualDate);
                        string taiwan_Remarks = rawMaterialView.TAIWAN_Remarks;
                        if (taiwan_Remarks == null)
                        {
                            taiwan_Remarks = "";
                        }
                        //if (lamination_ETD != dtDefault
                        //    || lamination_ActualDate != dtDefault || String.IsNullOrEmpty(lamination_Remarks) == false)
                        {
                            var taiwan_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 10,
                                ETD = taiwan_ETD,
                                ActualDate = taiwan_ActualDate,
                                Remarks = taiwan_Remarks.Trim(),
                            };

                            RawMaterialController.Insert(taiwan_Model);
                        }
                    }

                    //CUTTING 2
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 2) != null)
                    {
                        DateTime cutting_ETD = TimeHelper.Convert(rawMaterialView.CUTTING_ETD);
                        DateTime cutting_ActualDate = TimeHelper.Convert(rawMaterialView.CUTTING_ActualDate);
                        string cutting_Remarks = rawMaterialView.CUTTING_Remarks;
                        if (cutting_Remarks == null)
                        {
                            cutting_Remarks = "";
                        }
                        //if (cutting_ETD != dtDefault
                        //    || cutting_ActualDate != dtDefault || String.IsNullOrEmpty(cutting_Remarks) == false)
                        {
                            var cutting_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 2,
                                ETD = cutting_ETD,
                                ActualDate = cutting_ActualDate,
                                Remarks = cutting_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(cutting_Model);
                        }
                    }

                    //LEATHER 3
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 3) != null)
                    {
                        DateTime leather_ETD = TimeHelper.Convert(rawMaterialView.LEATHER_ETD);
                        DateTime leather_ActualDate = TimeHelper.Convert(rawMaterialView.LEATHER_ActualDate);
                        string leather_Remarks = rawMaterialView.LEATHER_Remarks;
                        if (leather_Remarks == null)
                        {
                            leather_Remarks = "";
                        }
                        //if (leather_ETD != dtDefault
                        //    || leather_ActualDate != dtDefault || String.IsNullOrEmpty(leather_Remarks) == false)
                        {
                            var leather_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 3,
                                ETD = leather_ETD,
                                ActualDate = leather_ActualDate,
                                Remarks = leather_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(leather_Model);
                        }
                    }

                    //SEMIPROCESS 4
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(r => r.MaterialType == 4) != null)
                    {
                        DateTime semiprocess_ETD = TimeHelper.Convert(rawMaterialView.SEMIPROCESS_ETD);
                        DateTime semiprocess_ActualDate = TimeHelper.Convert(rawMaterialView.SEMIPROCESS_ActualDate);
                        string semiprocess_Remarks = rawMaterialView.SEMIPROCESS_Remarks;
                        if (semiprocess_Remarks == null)
                        {
                            semiprocess_Remarks = "";
                        }

                        //if (semiprocess_ETD != dtDefault
                        //    || semiprocess_ActualDate != dtDefault || String.IsNullOrEmpty(semiprocess_Remarks) == false)
                        {
                            var semiprocess_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 4,
                                ETD = semiprocess_ETD,
                                ActualDate = semiprocess_ActualDate,
                                Remarks = semiprocess_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(semiprocess_Model);
                        }
                    }

                    //SEWING 5
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(r => r.MaterialType == 5) != null)
                    {
                        DateTime sewing_ETD = TimeHelper.Convert(rawMaterialView.SEWING_ETD);
                        DateTime sewing_ActualDate = TimeHelper.Convert(rawMaterialView.SEWING_ActualDate);
                        string sewing_Remarks = rawMaterialView.SEWING_Remarks;
                        if (sewing_Remarks == null)
                        {
                            sewing_Remarks = "";
                        }
                        //if (sewing_ETD != dtDefault
                        //    || sewing_ActualDate != dtDefault || String.IsNullOrEmpty(sewing_Remarks) == false)
                        {
                            var sewing_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 5,
                                ETD = sewing_ETD,
                                ActualDate = sewing_ActualDate,
                                Remarks = sewing_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(sewing_Model);
                        }
                    }

                    //OUTSOLE 6
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 6) != null)
                    {
                        DateTime outsole_ETD = TimeHelper.Convert(rawMaterialView.OUTSOLE_ETD);
                        DateTime outsole_ActualDate = TimeHelper.Convert(rawMaterialView.OUTSOLE_ActualDate);
                        string outsole_Remarks = rawMaterialView.OUTSOLE_Remarks;
                        if (outsole_Remarks == null)
                        {
                            outsole_Remarks = "";
                        }
                        //if (outsole_ETD != dtDefault
                        //    || outsole_ActualDate != dtDefault || String.IsNullOrEmpty(outsole_Remarks) == false)
                        {
                            var outsole_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 6,
                                ETD = outsole_ETD,
                                ActualDate = outsole_ActualDate,
                                Remarks = outsole_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(outsole_Model);
                        }
                    }

                    //SECURITYLABEL 7
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 7) != null)
                    {
                        DateTime securityLabel_ETD = TimeHelper.Convert(rawMaterialView.SECURITYLABEL_ETD);
                        DateTime securityLabel_ActualDate = TimeHelper.Convert(rawMaterialView.SECURITYLABEL_ActualDate);
                        string securityLabel_Remarks = rawMaterialView.SECURITYLABEL_Remarks;
                        if (securityLabel_Remarks == null)
                        {
                            securityLabel_Remarks = "";
                        }
                        //if (securityLabel_ETD != dtDefault
                        //    || securityLabel_ActualDate != dtDefault || String.IsNullOrEmpty(securityLabel_Remarks) == false)
                        {
                            var securityLabel_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 7,
                                ETD = securityLabel_ETD,
                                ActualDate = securityLabel_ActualDate,
                                Remarks = securityLabel_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(securityLabel_Model);
                        }
                    }

                    //ASSEMBLY 8
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 8) != null)
                    {
                        DateTime assembly_ETD = TimeHelper.Convert(rawMaterialView.ASSEMBLY_ETD);
                        DateTime assembly_ActualDate = TimeHelper.Convert(rawMaterialView.ASSEMBLY_ActualDate);
                        string assembly_Remarks = rawMaterialView.ASSEMBLY_Remarks;
                        if (assembly_Remarks == null)
                        {
                            assembly_Remarks = "";
                        }
                        //if (assembly_ETD != dtDefault
                        //    || assembly_ActualDate != dtDefault || String.IsNullOrEmpty(assembly_Remarks) == false)
                        {
                            var assembly_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 8,
                                ETD = assembly_ETD,
                                ActualDate = assembly_ActualDate,
                                Remarks = assembly_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(assembly_Model);
                        }
                    }

                    //SOCKLINING 9
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 9) != null)
                    {
                        DateTime socklining_ETD = TimeHelper.Convert(rawMaterialView.SOCKLINING_ETD);
                        DateTime socklining_ActualDate = TimeHelper.Convert(rawMaterialView.SOCKLINING_ActualDate);
                        string socklining_Remarks = rawMaterialView.SOCKLINING_Remarks;
                        if (socklining_Remarks == null)
                        {
                            socklining_Remarks = "";
                        }
                        //if (socklining_ETD != dtDefault
                        //    || socklining_ActualDate != dtDefault || String.IsNullOrEmpty(socklining_Remarks) == false)
                        {
                            var socklining_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 9,
                                ETD = socklining_ETD,
                                ActualDate = socklining_ActualDate,
                                Remarks = socklining_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(socklining_Model);
                        }
                    }

                    //CARTON 11
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 11) != null)
                    {
                        DateTime carton_ETD = TimeHelper.Convert(rawMaterialView.CARTON_ETD);
                        DateTime carton_ActualDate = TimeHelper.Convert(rawMaterialView.CARTON_ActualDate);
                        string carton_Remarks = rawMaterialView.CARTON_Remarks;
                        if (carton_Remarks == null)
                        {
                            carton_Remarks = "";
                        }
                        //if (socklining_ETD != dtDefault
                        //    || socklining_ActualDate != dtDefault || String.IsNullOrEmpty(socklining_Remarks) == false)
                        {
                            var carton_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 11,
                                ETD = carton_ETD,
                                ActualDate = carton_ActualDate,
                                Remarks = carton_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(carton_Model);
                        }
                    }

                    // UPPER COMPONENT MATERIAL 12
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 12) != null)
                    {
                        DateTime upperComponentMaterial_ETD = TimeHelper.Convert(rawMaterialView.UPPERCOMPONENT_ETD);
                        DateTime upperComponentMaterial_ActualDate = TimeHelper.Convert(rawMaterialView.UPPERCOMPONENT_ActualDate);
                        string upperComponentMaterial_Remarks = rawMaterialView.UPPERCOMPONENT_Remarks;
                        if (upperComponentMaterial_Remarks == null)
                        {
                            upperComponentMaterial_Remarks = "";
                        }
                        //if (outsole_ETD != dtDefault
                        //    || outsole_ActualDate != dtDefault || String.IsNullOrEmpty(outsole_Remarks) == false)
                        {
                            var upperComponentMaterial_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 12,
                                ETD = upperComponentMaterial_ETD,
                                ActualDate = upperComponentMaterial_ActualDate,
                                Remarks = upperComponentMaterial_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(upperComponentMaterial_Model);
                        }
                    }

                    // INSOCK 13
                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 13) != null)
                    {
                        DateTime insocRawkMaterial_ETD = TimeHelper.Convert(rawMaterialView.INSOCK_ETD);
                        DateTime insockRawMaterial_ActualDate = TimeHelper.Convert(rawMaterialView.INSOCK_ActualDate);
                        string insockRawMaterial_Remarks = rawMaterialView.INSOCK_Remarks;
                        if (insockRawMaterial_Remarks == null)
                        {
                            insockRawMaterial_Remarks = "";
                        }
                        {
                            var insockRawMaterial_Model = new RawMaterialModel
                            {
                                ProductNo = rawMaterialView.ProductNo,
                                MaterialTypeId = 13,
                                ETD = insocRawkMaterial_ETD,
                                ActualDate = insockRawMaterial_ActualDate,
                                Remarks = insockRawMaterial_Remarks.Trim(),
                            };
                            RawMaterialController.Insert(insockRawMaterial_Model);
                        }
                    }

                    if (rawMaterialCellChangedList_D1.FirstOrDefault(f => f.MaterialType == 14) != null)
                    {
                        var etd = TimeHelper.Convert(rawMaterialView.UpperAccessories_ETD);
                        var actualDate = TimeHelper.Convert(rawMaterialView.UpperAccessories_ActualDate);
                        var remarks = rawMaterialView.UpperAccessories_Remarks != null ? rawMaterialView.UpperAccessories_Remarks : "";
                        var accessories_Model = new RawMaterialModel
                        {
                            ProductNo       = rawMaterialView.ProductNo,
                            MaterialTypeId  = 14,
                            ETD             = etd,
                            ActualDate      = actualDate,
                            Remarks         = remarks
                        };
                        RawMaterialController.Insert(accessories_Model);
                    }
                    // Insert ProductNoRevise
                    var productNoReviseInsertModel = new ProductNoReviseModel()
                    {
                        ProductNo = rawMaterialView.ProductNo,
                        ReviseDate = DateTime.Now.Date,
                        SectionId = "WH"
                    };
                    ProductNoReviseController.Insert(productNoReviseInsertModel);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        dgRawMaterial.ScrollIntoView(rawMaterialView);
                        dgRawMaterial.SelectedItem = rawMaterialView;
                    }));
                }
            }
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSave.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            rawMaterialCellChangedList.Clear();
            orderExtraChangedList.Clear();
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgRawMaterial_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                bool isVisible = searchBox.IsVisible;
                if (isVisible == false)
                {
                    searchBox = new RawMaterialSearchBoxWindow();
                    searchBox.GetFindWhat = new RawMaterialSearchBoxWindow.GetString(SearchRawMaterial);
                    searchBox.Show();
                }
            }
        }

        private void SearchRawMaterial(string findWhat, bool isMatch, bool isShow)
        {
            if (rawMaterialViewReloadList.Count > 0)
                rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewReloadList);
            else
                rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewList);

            if (String.IsNullOrEmpty(findWhat) == false)
            {
                if (isMatch == true)
                {
                    var rawMaterialViewSearched = rawMaterialViewSearchedList.Where(r =>
                        r.ProductNo.ToLower() == findWhat.ToLower() || r.Country.ToLower() == findWhat.ToLower() || r.ShoeName.ToLower() == findWhat.ToLower() || r.ArticleNo.ToLower() == findWhat.ToLower() ||
                        r.PatternNo.ToLower() == findWhat.ToLower() || r.OutsoleCode.ToLower() == findWhat.ToLower() || r.ETD.ToShortDateString().ToLower() == findWhat.ToLower()).FirstOrDefault();
                    if (rawMaterialViewSearched != null)
                    {
                        dgRawMaterial.SelectedItem = rawMaterialViewSearched;
                        dgRawMaterial.ScrollIntoView(rawMaterialViewSearched);
                    }
                    else
                    {
                        MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (isMatch == false)
                    {
                        if (isShow == true)
                        {
                            rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewSearchedList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == true || r.Country.ToLower().Contains(findWhat.ToLower()) == true || r.ShoeName.ToLower().Contains(findWhat.ToLower()) == true || r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == true ||
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == true || r.OutsoleCode.ToLower().Contains(findWhat.ToLower()) == true || r.ETD.ToShortDateString().ToLower() == findWhat.ToLower()));
                        }
                        else
                        {
                            rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewSearchedList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == false && r.Country.ToLower().Contains(findWhat.ToLower()) == false && r.ShoeName.ToLower().Contains(findWhat.ToLower()) == false && r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == false &&
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == false && r.OutsoleCode.ToLower().Contains(findWhat.ToLower()) == false && r.ETD.ToShortDateString().ToLower() != findWhat.ToLower()));
                        }

                        if (rawMaterialViewSearchedList.Count >= 1)
                        {
                            dgRawMaterial.ItemsSource = rawMaterialViewSearchedList;
                        }
                        else
                        {
                            MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            else
            {
                dgRawMaterial.ItemsSource = rawMaterialViewSearchedList;
            }
        }

        private void miDisable_Click(object sender, RoutedEventArgs e)
        {
            rawMaterialViewToRemoveList.Clear();
            rawMaterialViewToRemoveList = dgRawMaterial.SelectedItems.OfType<RawMaterialViewModel>().ToList();
            if (rawMaterialViewToRemoveList.Count <= 0 ||
                MessageBox.Show("Confirm Remove?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwRemoveOrder.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwRemoveOrder.RunWorkerAsync();
            }
        }

        private void bwRemoveOrder_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var model in rawMaterialViewToRemoveList)
            {
                OrdersController.Update(model.ProductNo, false, account.UserName);
            }
        }

        private void bwRemoveOrder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var rawMaterialView in rawMaterialViewToRemoveList)
            {
                rawMaterialViewList.Remove(rawMaterialView);
                rawMaterialViewSearchedList.Remove(rawMaterialView);
            }
            if (rawMaterialViewSearchedList.Count <= 0)
            {
                rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewList);
                dgRawMaterial.ItemsSource = rawMaterialViewSearchedList;
            }
            this.Cursor = null;
            MessageBox.Show("Removed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (rawMaterialCellChangedList.Count > 0 || orderExtraChangedList.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (bwImport.IsBusy == false)
                    {
                        e.Cancel = true;
                        this.Cursor = Cursors.Wait;
                        rawMaterialViewToImportList = dgRawMaterial.Items.OfType<RawMaterialViewModel>().ToList();
                        btnSave.IsEnabled = false;
                        bwImport.RunWorkerAsync();
                    }
                }
                else if (result == MessageBoxResult.No)
                { }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void dgRawMaterial_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                scrlHeader.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        private void dgRawMaterial_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridCellInfo cellCurrent = dgRawMaterial.CurrentCell;
                if (account.OutsoleRMSchedule == true)
                {
                    if (cellCurrent != null && cellCurrent.Column != null && cellCurrent.Column == Column24)
                    {
                        var rawMaterialView = (RawMaterialViewModel)cellCurrent.Item;
                        if (rawMaterialView == null)
                        {
                            return;
                        }
                        string productNo = rawMaterialView.ProductNo;
                        var window = new OutsoleInputMaterialWindow(productNo, account);
                        window.ShowDialog();
                        if (window.DialogResult == true)
                        {
                            rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                            {
                                ProductNo = productNo,
                                MaterialType = 6,
                            });

                            rawMaterialView.OUTSOLE_Remarks = window.rawMaterial.Remarks;
                            rawMaterialView.OUTSOLE_ActualDate = window.rawMaterial.ActualDate == dtDefault ? "" :
                                                                 String.Format("{0:M/d}", window.rawMaterial.ActualDate);
                            //if (window.rawMaterial.ActualDate != dtNothing)
                            //{
                            //    rawMaterialView.OUTSOLE_ActualDate = String.Format("{0:M/d}", window.rawMaterial.ActualDate);
                            //    if (window.rawMaterial.ActualDate == dtDefault)
                            //    {
                            //        rawMaterialView.OUTSOLE_ActualDate = "";
                            //        rawMaterialView.OUTSOLE_Remarks = "";
                            //    }
                            //}

                            rawMaterialView.OUTSOLE_ActualDate_BACKGROUND = Brushes.Transparent;
                            if (window.totalRejectAssemblyAndStockfitRespone > 0)
                                rawMaterialView.OUTSOLE_ActualDate_BACKGROUND = Brushes.Yellow;
                        }
                    }
                    if (cellCurrent != null && cellCurrent.Column != null && cellCurrent.Column == Column23)
                    {
                        var rawMaterialView = (RawMaterialViewModel)cellCurrent.Item;
                        if (rawMaterialView == null)
                        {
                            return;
                        }
                        string productNo = rawMaterialView.ProductNo;
                        var window = new OutsoleRawMaterialWindow(productNo);
                        window.ShowDialog();
                        if (window.DialogResult == true && window.rawMaterial.IsETDUpdate == true)
                        {
                            rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                            {
                                ProductNo = productNo,
                                MaterialType = 6,
                            });
                            rawMaterialView.OUTSOLE_ETD = String.Format("{0:M/d}", window.rawMaterial.ETD);
                        }
                    }
                }


                if (cellCurrent != null && cellCurrent.Column != null && (cellCurrent.Column == Column25_1 || cellCurrent.Column == Column25_2))
                {
                    var rawMaterialView = (RawMaterialViewModel)cellCurrent.Item;
                    if (rawMaterialView == null)
                    {
                        return;
                    }
                    string productNo = rawMaterialView.ProductNo;
                    var window = new InputUpperAccessoriesInspectWindow(productNo, rejectUpperAccessoriesList, account);
                    window.ShowDialog();
                    if (account.UpperAccessories)
                    {
                        if (window.materialPlanList.Count() > 0)
                        {
                            var materialPlanList = window.materialPlanList.ToList();
                            var etd = materialPlanList.Max(m => m.ETD);
                            if (String.IsNullOrEmpty(rawMaterialView.UpperAccessories_ActualDeliveryDate))
                                rawMaterialView.UpperAccessories_ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", etd);
                            
                            var actualDate = materialPlanList.Max(s => s.ActualDate);
                            if (actualDate != dtDefault)
                                rawMaterialView.UpperAccessories_ActualDate = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", actualDate);

                            var remarksNumber = materialPlanList.Max(m => m.RejectPO) + materialPlanList.Max(m => m.BalancePO);
                            rawMaterialView.UpperAccessories_Remarks = remarksNumber > 0 ? remarksNumber.ToString() : "";
                        }
                        else
                        {
                            rawMaterialView.UpperAccessories_ETD = "";
                            rawMaterialView.UpperAccessories_ActualDate = "";
                            rawMaterialView.UpperAccessories_ActualDeliveryDate = "";
                            rawMaterialView.UpperAccessories_Remarks = "";
                        }
                        rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                        {
                            ProductNo = productNo,
                            MaterialType = 14,
                        });
                    }
                }

                if (cellCurrent != null && cellCurrent.Column != null && cellCurrent.Column == Column25_2_1)
                {
                    var rawMaterialView = (RawMaterialViewModel)cellCurrent.Item;
                    if (rawMaterialView == null)
                        return;
                    var window = new InputUpperAccessoriesMaterialDeliveryWindow(rawMaterialView.ProductNo, account, rawMaterialView);
                    window.ShowDialog();
                }


                if (account.Insock == true)
                {
                    if (cellCurrent != null && cellCurrent.Column != null && cellCurrent.Column == Column25_4)
                    {
                        var rawMaterialView = (RawMaterialViewModel)cellCurrent.Item;
                        if (rawMaterialView == null)
                        {
                            return;
                        }
                        string productNo = rawMaterialView.ProductNo;
                        var window = new InsockRawMaterialWindow(productNo);
                        window.ShowDialog();
                        if (window.DialogResult == true && window.rawMaterial.IsETDUpdate == true)
                        {
                            rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                            {
                                ProductNo = productNo,
                                MaterialType = 13,
                            });
                            rawMaterialView.INSOCK_ETD = String.Format("{0:M/d}", window.rawMaterial.ETD);
                        }
                    }
                    if (cellCurrent != null && cellCurrent.Column != null && cellCurrent.Column == Column25_5)
                    {
                        var rawMaterialView = (RawMaterialViewModel)cellCurrent.Item;
                        if (rawMaterialView == null)
                        {
                            return;
                        }
                        string productNo = rawMaterialView.ProductNo;
                        var window = new InsockInputMaterialWindow(productNo);
                        window.ShowDialog();
                        if (window.DialogResult == true)
                        {
                            rawMaterialCellChangedList.Add(new RawMaterialCellChangedModel
                            {
                                ProductNo = productNo,
                                MaterialType = 13,
                            });

                            if (window.rawMaterial.ActualDate != dtNothing)
                            {
                                rawMaterialView.INSOCK_ActualDate = String.Format("{0:M/d}", window.rawMaterial.ActualDate);
                                if (window.rawMaterial.ActualDate == dtDefault)
                                {
                                    rawMaterialView.INSOCK_ActualDate = "";
                                }
                            }
                            rawMaterialView.INSOCK_Remarks = window.rawMaterial.Remarks != "0" ? window.rawMaterial.Remarks : "";
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        
        private void dgRawMaterial_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (DataGridCellInfo dataGridCellInfo in e.RemovedCells)
            {
                if (dataGridCellInfo.Item != DependencyProperty.UnsetValue)
                {
                    var rawMaterialView = (RawMaterialViewModel)dataGridCellInfo.Item;
                    if (rawMaterialView != null)
                    {
                        rawMaterialView.ProductNoBackground = Brushes.Transparent;
                    }
                }
            }
            foreach (DataGridCellInfo dataGridCellInfo in e.AddedCells)
            {
                var rawMaterialView = (RawMaterialViewModel)dataGridCellInfo.Item;
                if (rawMaterialView != null)
                {
                    rawMaterialView.ProductNoBackground = Brushes.RoyalBlue;
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            searchBox.Topmost = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            searchBox.Topmost = false;
        }

        private void dgRawMaterial_Sorting(object sender, DataGridSortingEventArgs e)
        {

        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            if (bwReload.IsBusy == false && !def.Factory.Equals("THIENLOC"))
            {
                rawMaterialViewList = new List<RawMaterialViewModel>();
                lblStatus.Text = "";
                lblStatus.Visibility = Visibility.Visible;
                this.Cursor = Cursors.Wait;
                dgRawMaterial.ItemsSource = null;
                btnReload.IsEnabled = false;
                bwReload.RunWorkerAsync();
            }
        }

        private void bwReload_DoWork(object sender, DoWorkEventArgs e)
        {
            rawMaterialViewModelNewList = RawMaterialController.Select_1();
            int index = 1;
            foreach (var x in rawMaterialViewModelNewList)
            {
                Dispatcher.Invoke(new Action(() => {
                    lblStatus.Text = String.Format("Creating {0}/{1} rows ...", index, rawMaterialViewModelNewList.Count());
                }));
                rawMaterialViewList.Add(ConvertX(x));
                index++;
            }
        }

        private void bwReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewReloadList);
            rawMaterialViewSearchedList = new ObservableCollection<RawMaterialViewModel>(rawMaterialViewList);
            dgRawMaterial.ItemsSource = rawMaterialViewSearchedList;
            btnReload.IsEnabled = true;
            lblStatus.Visibility = Visibility.Collapsed;
            this.Cursor = null;
        }
    }
}
