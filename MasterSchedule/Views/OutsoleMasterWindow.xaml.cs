using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using wf = System.Windows.Forms;

using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Threading.Tasks;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SewingMasterWindow.xaml
    /// </summary>
    public partial class OutsoleMasterWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoad;
        List<OffDayModel> offDayList;
        List<OrdersModel> orderList;
        List<OutsoleMasterViewModel> outsoleMasterViewList;
        public ObservableCollection<OutsoleMasterViewModel> outsoleMasterViewFindList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<OutsoleMasterViewModel> outsoleMasterViewToInsertList;
        List<RawMaterialModel> rawMaterialList;
        DateTime dtNothing;
        DateTime dtDefault;
        BackgroundWorker bwInsert;
        bool isEditing;
        BackgroundWorker bwReload;

        bool isSequenceEditing;
        //List<Int32> sequenceUpdateList;

        List<String> lineOutsoleEditingList;

        List<String> outsoleLineUpdateList;

        List<String> outsoleQuotaUpdateList;
        List<String> outsoleActualStartDateUpdateList;
        List<String> outsoleActualFinishDateUpdateList;
        List<String> outsoleActualStartDateAutoUpdateList;
        List<String> outsoleActualFinishDateAutoUpdateList;
        List<String> outsoleBalanceUpdateList;

        List<String> outsoleRemarksUpdateList;

        RawMaterialSearchBoxWindow searchBox;

        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;

        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialToWHInspectionList;
        List<ProductionMemoModel> productionMemoList;

        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;

        List<OutsoleMaterialCheckingModel> outsoleMaterialCheckingWHList;
        List<OutsoleSuppliersModel> suppilerList;
        List<SizeRunModel> sizeRunList;
        //List<RawMaterialViewModelNew> rawMaterialViewModelNewList;
        List<OutsoleMasterSourceModel> outsoleMasterSourceList;


        List<String> linesNeedSaving;
        //private bool changingSequence = false;
        List<POSequenceModel> poSequenceSourceList;
        private PrivateDefineModel def;
        public OutsoleMasterWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            //bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork_1);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            offDayList = new List<OffDayModel>();
            orderList = new List<OrdersModel>();
            outsoleMasterViewList = new List<OutsoleMasterViewModel>();
            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            outsoleMasterViewToInsertList = new List<OutsoleMasterViewModel>();
            rawMaterialList = new List<RawMaterialModel>();
            dtNothing = new DateTime(1999, 12, 31);
            dtDefault = new DateTime(2000, 1, 1);
            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);
            isEditing = false;
            bwReload = new BackgroundWorker();
            bwReload.DoWork += new DoWorkEventHandler(bwReload_DoWork);
            bwReload.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwReload_RunWorkerCompleted);

            isSequenceEditing = false;
            //sequenceUpdateList = new List<int>();
            lineOutsoleEditingList = new List<String>();

            outsoleLineUpdateList = new List<String>();

            outsoleQuotaUpdateList = new List<String>();
            outsoleActualStartDateUpdateList = new List<String>();
            outsoleActualFinishDateUpdateList = new List<String>();
            outsoleActualStartDateAutoUpdateList = new List<String>();
            outsoleActualFinishDateAutoUpdateList = new List<String>();
            outsoleBalanceUpdateList = new List<String>();

            outsoleRemarksUpdateList = new List<String>();

            searchBox = new RawMaterialSearchBoxWindow();

            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
            outsoleReleaseMaterialToWHInspectionList = new List<OutsoleReleaseMaterialModel>();
            productionMemoList = new List<ProductionMemoModel>();

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleMaterialCheckingWHList = new List<OutsoleMaterialCheckingModel>();
            suppilerList = new List<OutsoleSuppliersModel>();
            sizeRunList = new List<SizeRunModel>();

            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
            //rawMaterialViewModelNewList = new List<RawMaterialViewModelNew>();

            outsoleMasterSourceList = new List<OutsoleMasterSourceModel>();

            linesNeedSaving = new List<string>();
            poSequenceSourceList = new List<POSequenceModel>();

            def = new PrivateDefineModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.OutsoleMaster == true)
            {
                colOutsoleLine.IsReadOnly = false;
                colOutsoleQuota.IsReadOnly = false;
                colOutsoleActualStartDate.IsReadOnly = false;
                colOutsoleActualFinishDate.IsReadOnly = false;
                colRemarks.IsReadOnly = false;
            }
            if (account.Sortable == true)
            {
                colCountry.CanUserSort = true;
                colStyle.CanUserSort = true;
                colETD.CanUserSort = true;
            }

            //if (account.Simulation == true)
            //{
            //    btnEnableSimulation.Visibility = Visibility.Visible;
            //}

            if (bwLoad.IsBusy == false)
            {
                prgStatus.Value = 0;
                lblStatus.Text = "Loading PO ...";
                prgStatus.Visibility = Visibility.Visible;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Task t1 = new Task(() => {
                orderList = OrdersController.Select();
            });
            t1.Start();

            Task t2 = new Task(() => {
                sewingMasterList = SewingMasterController.Select();
            });
            t2.Start();

            Task t3 = new Task(() => { 
                outsoleMasterList = OutsoleMasterController.Select_1();
            });
            t3.Start();

            Task t4 = new Task(() => {
                rawMaterialList = RawMaterialController.Select();
            });
            t4.Start();
            Task t5 = new Task(() => {
                outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectByOutsoleMaster();
            });
            t5 .Start();
            Task t6 = new Task(() => {
                outsoleReleaseMaterialToWHInspectionList = OutsoleReleaseWHInspectionController.SelectOutsoleReleaseToWHInspectionByOutsoleMaster();
            });
            t6.Start();
            Task t7 = new Task(() => {
                outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            });
            t7.Start();
            Task t8 = new Task(() => {
                outsoleMaterialList = OutsoleMaterialController.Select();
            });
            t8.Start();
            Task t9 = new Task(() => {
                outsoleMaterialCheckingWHList = OutsoleMaterialCheckingController.SelectByPOAvailable();
            });
            t9.Start();
            Task t10 = new Task(() => {
                sizeRunList = SizeRunController.SelectIsEnable();
            });
            t10.Start();

            offDayList = OffDayController.Select();
            productionMemoList = ProductionMemoController.Select();
            suppilerList = OutsoleSuppliersController.Select();
            //rawMaterialViewModelNewList = RawMaterialController.Select_1();

            Task.WaitAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };
            int[] materialIdAssemblyArray = { 8, 9 };

            Dispatcher.Invoke(new Action(() =>
            {
                prgStatus.Maximum = orderList.Count();
            }));
            for (int i = 0; i <= orderList.Count - 1; i++)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = String.Format("Loading {0} / {1} PO", i + 1, orderList.Count());
                    prgStatus.Value = i + 1;
                }));
                OrdersModel order = orderList[i];
                OutsoleMasterViewModel outsoleMasterView = new OutsoleMasterViewModel
                {
                    ProductNo = order.ProductNo,
                    ProductNoBackground = Brushes.Transparent,
                    Country = order.Country,
                    ShoeName = order.ShoeName,
                    ArticleNo = order.ArticleNo,
                    OutsoleCode = order.OutsoleCode,
                    PatternNo = order.PatternNo,
                    Quantity = order.Quantity,
                    ETD = order.ETD,
                };

                string memoId = "";
                List<ProductionMemoModel> productionMemoByProductionNumberList = productionMemoList.Where(p => p.ProductionNumbers.Contains(order.ProductNo) == true).ToList();
                for (int p = 0; p <= productionMemoByProductionNumberList.Count - 1; p++)
                {
                    ProductionMemoModel productionMemo = productionMemoByProductionNumberList[p];
                    memoId += productionMemo.MemoId;
                    if (p < productionMemoByProductionNumberList.Count - 1)
                    {
                        memoId += "\n";
                    }
                }
                outsoleMasterView.MemoId = memoId;

                //// Outsole Material Follow Material Input
                //var rawMaterialViewModelNew = rawMaterialViewModelNewList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                //outsoleMasterView.OSMatsArrivalForeground = Brushes.Blue;
                //outsoleMasterView.OSMatsArrivalBackground = Brushes.Transparent;

                //// Deliveried
                //if (String.IsNullOrEmpty(rawMaterialViewModelNew.OUTSOLE_Remarks) &&
                //    !String.IsNullOrEmpty(rawMaterialViewModelNew.OUTSOLE_ActualDate))
                //{
                //    outsoleMasterView.OSMatsArrival = rawMaterialViewModelNew.OUTSOLE_ActualDate;
                //    outsoleMasterView.OSMatsArrivalOrginal = rawMaterialViewModelNew.OUTSOLE_ActualDate_DATE;
                //}
                //else
                //{
                //    outsoleMasterView.OSMatsArrivalForeground = Brushes.Black;
                //    outsoleMasterView.OSMatsArrival = rawMaterialViewModelNew.OUTSOLE_ETD;

                //    // ETD Late
                //    if (rawMaterialViewModelNew.OUTSOLE_ETD_DATE < DateTime.Now.Date &&
                //        rawMaterialViewModelNew.OUTSOLE_ETD_DATE != dtDefault)
                //        outsoleMasterView.OSMatsArrivalBackground = Brushes.Red;

                //    // Still Have Balance
                //    if (!String.IsNullOrEmpty(rawMaterialViewModelNew.OUTSOLE_Remarks))
                //    {
                //        outsoleMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                //        if (rawMaterialViewModelNew.OUTSOLE_ETD_DATE < DateTime.Now.Date &&
                //            rawMaterialViewModelNew.OUTSOLE_ETD_DATE != dtDefault)
                //            outsoleMasterView.OSMatsArrivalForeground = Brushes.Red;
                //    }
                //}
                // only for outsole material type
                var osRawMaterial = outsoleRawMaterialList.Where(w => w.ProductNo == order.ProductNo).ToList();
                var actualDateList = osRawMaterial.Select(s => s.ActualDate).ToList();
                if (actualDateList.Count() > 0 && actualDateList.Contains(dtDefault) == false)
                {
                    outsoleMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", actualDateList.Max());
                    outsoleMasterView.OSMatsArrivalForeground = Brushes.Blue;
                    outsoleMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                }
                else
                {
                    var etdDateList = osRawMaterial.Select(s => s.ETD).ToList();
                    if (etdDateList.Count() > 0)
                    {
                        outsoleMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", etdDateList.Max());
                        outsoleMasterView.OSMatsArrivalForeground = Brushes.Black;
                        outsoleMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                        if (etdDateList.Max() < DateTime.Now.Date)
                        {
                            outsoleMasterView.OSMatsArrivalBackground = Brushes.Red;
                        }
                        else
                        {
                            var rawMaterial_PO = rawMaterialList.Where(w => w.ProductNo == order.ProductNo && materialIdOutsoleArray.Contains(w.MaterialTypeId) == true).ToList();
                            if (rawMaterial_PO.Where(w => String.IsNullOrEmpty(w.Remarks) == false).Count() > 0)
                            {
                                outsoleMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                            }
                        }
                    }
                }

                //outsoleMasterView.OutsoleWHBalance = rawMaterialViewModelNew.OUTSOLE_Remarks;
                RawMaterialModel outsoleRawMaterial = rawMaterialList.FirstOrDefault(f => f.ProductNo == order.ProductNo && materialIdOutsoleArray.Contains(f.MaterialTypeId));
                if (outsoleRawMaterial != null)
                {
                    outsoleMasterView.OutsoleWHBalance = outsoleRawMaterial.Remarks;
                }
                else
                {
                    outsoleMasterView.OutsoleWHBalance = "";
                }

                var outsoleMaster = outsoleMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (outsoleMaster != null)
                {
                    outsoleMasterView.Sequence = outsoleMaster.Sequence;
                    outsoleMasterView.OutsoleLine = outsoleMaster.OutsoleLine;
                    outsoleMasterView.OutsoleStartDate = outsoleMaster.OutsoleStartDate;
                    outsoleMasterView.OutsoleFinishDate = outsoleMaster.OutsoleFinishDate;
                    outsoleMasterView.OutsoleQuota = outsoleMaster.OutsoleQuota;

                    //outsoleMasterView.OutsoleActualStartDate = outsoleMaster.OutsoleActualStartDate;
                    outsoleMasterView.OutsoleActualStartDate = "";
                    if (outsoleMaster.OutsoleActualStartDate_Date != dtDefault)
                    {
                        outsoleMasterView.OutsoleActualStartDate = String.Format("{0:M/d}", outsoleMaster.OutsoleActualStartDate_Date);
                        outsoleMasterView.OutsoleActualStartDate_Date = outsoleMaster.OutsoleActualStartDate_Date;
                    }

                    //outsoleMasterView.OutsoleActualFinishDate = outsoleMaster.OutsoleActualFinishDate;
                    outsoleMasterView.OutsoleActualFinishDate = "";
                    if (outsoleMaster.OutsoleActualFinishDate_Date != dtDefault)
                    {
                        outsoleMasterView.OutsoleActualFinishDate = String.Format("{0:M/d}", outsoleMaster.OutsoleActualFinishDate_Date);
                        outsoleMasterView.OutsoleActualFinishDate_Date = outsoleMaster.OutsoleActualFinishDate_Date;
                    }

                    outsoleMasterView.OutsoleActualStartDateAuto = TimeHelper.ConvertDateToView(outsoleMaster.OutsoleActualStartDateAuto);
                    outsoleMasterView.OutsoleActualFinishDateAuto = TimeHelper.ConvertDateToView(outsoleMaster.OutsoleActualFinishDateAuto);

                    //outsoleMasterView.OutsoleBalance = outsoleMaster.OutsoleBalance;
                    if (outsoleMaster.OutsoleBalance.Contains("/"))
                    {
                        outsoleMasterView.OutsoleBalance_Date   = TimeHelper.Convert(outsoleMaster.OutsoleBalance);
                        outsoleMasterView.OutsoleBalance        = String.Format("{0:M/d}", outsoleMasterView.OutsoleBalance_Date);
                    }
                    else
                        outsoleMasterView.OutsoleBalance = outsoleMaster.OutsoleBalance;


                    outsoleMasterView.Remarks = outsoleMaster.Remarks;

                    double perCent = (double)outsoleMaster.WHCurrentCheck / (double)order.Quantity * 100;
                    perCent = perCent > 100 ? 100 : perCent;
                    string percentView = string.Format("{0}%", (int)perCent);
                    if (percentView.Equals("0%"))
                        percentView = "";
                    outsoleMasterView.OutsoleWHCheckingCurrent = percentView;
                    outsoleMasterView.OutsoleWHCheckingCurrentBackground = Brushes.Transparent;
                    if ((int)perCent >= 50)
                    {
                        outsoleMasterView.OutsoleWHCheckingCurrentBackground = Brushes.Yellow;
                    }
                    if (outsoleMaster.WHCurrentCheck >= order.Quantity)
                    {
                        outsoleMasterView.OutsoleWHCheckingCurrent = string.Format("{0:M/d}", outsoleMaster.WHLastDateCheck);
                        outsoleMasterView.OutsoleWHCheckingCurrentBackground = Brushes.Transparent;
                    }
                }
                else
                {
                    outsoleMasterView.Sequence = 0;
                    outsoleMasterView.OutsoleLine = "";
                    outsoleMasterView.OutsoleStartDate = dtDefault;
                    outsoleMasterView.OutsoleFinishDate = dtDefault;
                    outsoleMasterView.OutsoleQuota = 0;
                    outsoleMasterView.OutsoleActualStartDate = "";
                    outsoleMasterView.OutsoleActualFinishDate = "";
                    outsoleMasterView.OutsoleActualStartDateAuto = "";
                    outsoleMasterView.OutsoleActualFinishDateAuto = "";
                    outsoleMasterView.OutsoleBalance = "";
                    outsoleMasterView.Remarks = "";
                }

                SewingMasterModel sewingMaster = sewingMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (sewingMaster != null)
                {
                    outsoleMasterView.SewingLine        = sewingMaster.SewingLine;
                    outsoleMasterView.SewingStartDate   = sewingMaster.SewingStartDate;
                    outsoleMasterView.SewingFinishDate  = sewingMaster.SewingFinishDate;
                    outsoleMasterView.SewingQuota       = sewingMaster.SewingQuota;
                    
                    if (sewingMaster.SewingBalance.Contains("/"))
                    {
                        outsoleMasterView.SewingBalance = String.Format("{0:M/d}", TimeHelper.Convert(sewingMaster.SewingBalance));
                    }
                    else
                        outsoleMasterView.SewingBalance = sewingMaster.SewingBalance;
                }
                else
                {
                    outsoleMasterView.SewingLine = "";
                    outsoleMasterView.SewingStartDate = dtDefault;
                    outsoleMasterView.SewingFinishDate = dtDefault;
                    outsoleMasterView.SewingQuota = 0;
                    outsoleMasterView.SewingBalance = "";
                }

                outsoleMasterView.OutsoleStartDateForeground = Brushes.Black;
                outsoleMasterView.OutsoleFinishDateForeground = Brushes.Black;
                
                if (outsoleMasterView.OutsoleStartDate < outsoleMasterView.OSMatsArrivalOrginal || outsoleMasterView.OutsoleStartDate > outsoleMasterView.SewingStartDate)
                {
                    outsoleMasterView.OutsoleStartDateForeground = Brushes.Red;
                }
                if (outsoleMasterView.OutsoleFinishDate > outsoleMasterView.ETD)
                {
                    outsoleMasterView.OutsoleFinishDateForeground = Brushes.Red;
                }

                List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList_D1 = outsoleReleaseMaterialList.Where(o => o.ProductNo == order.ProductNo).ToList();
                int qtyReleased = outsoleReleaseMaterialList_D1.Sum(o => o.Quantity);
                outsoleMasterView.ReleasedQuantity = qtyReleased.ToString();
                if (qtyReleased <= 0)
                {
                    outsoleMasterView.ReleasedQuantity = "";
                }
                else if (qtyReleased >= order.Quantity)
                {
                    DateTime releasedDate = outsoleReleaseMaterialList_D1.OrderBy(o => o.ModifiedTime).LastOrDefault().ModifiedTime;
                    outsoleMasterView.ReleasedQuantity = string.Format("{0:M/d}", releasedDate);
                }

                var outsoleReleaseMaterialToWHInspectionList_D1 = outsoleReleaseMaterialToWHInspectionList.Where(w => w.ProductNo == order.ProductNo).ToList();
                int qtyReleasedToWHInspection = outsoleReleaseMaterialToWHInspectionList_D1.Sum(s => s.Quantity);
                outsoleMasterView.ReleasedToWHInspectionQuantity = qtyReleasedToWHInspection.ToString();
                if (qtyReleasedToWHInspection <= 0)
                {
                    outsoleMasterView.ReleasedToWHInspectionQuantity = "";
                }
                else if (qtyReleasedToWHInspection >= order.Quantity)
                {
                    DateTime releasedToWHInspectionDate = outsoleReleaseMaterialToWHInspectionList_D1.OrderBy(o => o.ModifiedTime).LastOrDefault().ModifiedTime;
                    outsoleMasterView.ReleasedToWHInspectionQuantity = string.Format("{0:M/d}", releasedToWHInspectionDate);
                }

                outsoleMasterViewList.Add(outsoleMasterView);
            }
            outsoleMasterViewList = outsoleMasterViewList.OrderBy(o => o.OutsoleLine).ThenBy(o => o.Sequence).ToList();
        }

        // Improve Speed
        private void bwLoad_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                def = PrivateDefineController.GetDefine();
                var productNoListWithAccount = OrdersController.Select().Where(w => account.TypeOfShoes != -1 ? w.TypeOfShoes == account.TypeOfShoes : w.TypeOfShoes != -16111992).Select(s => s.ProductNo).ToList();
                outsoleMasterList = OutsoleMasterController.Select_1().Where(w => productNoListWithAccount.Contains(w.ProductNo)).ToList();
                foreach (var os in outsoleMasterList)
                {
                    poSequenceSourceList.Add(new POSequenceModel
                    {
                        ProductNo = os.ProductNo,
                        Sequence = os.Sequence,
                        Id = string.Format("{0}-{1}", os.ProductNo, os.Sequence)
                    });
                }

                if (!string.IsNullOrEmpty(def.Factory) && !def.Factory.Equals("THIENLOC"))
                {
                    offDayList = OffDayController.Select();
                    productionMemoList = ProductionMemoController.Select().Where(w => productNoListWithAccount.Contains(w.ProductionNumbers)).ToList();
                    outsoleMasterSourceList = OutsoleMasterController.SelectSource().Where(w => productNoListWithAccount.Contains(w.ProductNo)).ToList();

                    Dispatcher.Invoke(new Action(() =>
                    {
                        prgStatus.Maximum = outsoleMasterSourceList.Count();
                    }));

                    int index = 1;
                    foreach (var osSource in outsoleMasterSourceList)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Text = String.Format("Loading {0} / {1} PO", index, outsoleMasterSourceList.Count());
                            prgStatus.Value = index;
                        }));
                        string memoId = "";
                        List<ProductionMemoModel> productionMemoByProductionNumberList = productionMemoList.Where(p => p.ProductionNumbers.Contains(osSource.ProductNo) == true).ToList();
                        for (int p = 0; p <= productionMemoByProductionNumberList.Count - 1; p++)
                        {
                            ProductionMemoModel productionMemo = productionMemoByProductionNumberList[p];
                            memoId += productionMemo.MemoId;
                            if (p < productionMemoByProductionNumberList.Count - 1)
                                memoId += "\n";
                        }

                        var outsoleMasterView = new OutsoleMasterViewModel
                        {
                            ProductNo = osSource.ProductNo,
                            ProductNoBackground = Brushes.Transparent,
                            Country = osSource.Country,
                            ShoeName = osSource.ShoeName,
                            ArticleNo = osSource.ArticleNo,
                            OutsoleCode = osSource.OutsoleCode,
                            PatternNo = osSource.PatternNo,
                            Quantity = osSource.Quantity,
                            ETD = osSource.ETD,
                            MemoId = memoId,
                            Sequence = osSource.Sequence,
                            OutsoleLine = osSource.OutsoleLine,
                            OutsoleStartDate = osSource.OutsoleStartDate,
                            OutsoleFinishDate = osSource.OutsoleFinishDate,
                            OutsoleQuota = osSource.OutsoleQuota,
                            Remarks = osSource.Remarks,
                            OutsoleWHBalance = osSource.OutsoleMaterialRemarks,

                            OutsoleActualStartDateAuto = TimeHelper.ConvertDateToView(osSource.OutsoleActualStartDateAuto),
                            OutsoleActualFinishDateAuto = TimeHelper.ConvertDateToView(osSource.OutsoleActualFinishDateAuto),

                            ReleasedQuantity = osSource.ReleasedQuantity,
                            ReleasedToWHInspectionQuantity = osSource.ReleasedToWHInspectionQuantity,
                            SewingLine = osSource.SewingLine,
                            SewingQuota = osSource.SewingQuota,
                            SewingStartDate = osSource.SewingStartDate,
                            SewingFinishDate = osSource.SewingFinishDate
                        };
                        if (osSource.OutsoleBalance.Contains("/"))
                        {
                            outsoleMasterView.OutsoleBalance = TimeHelper.DisplayDate(TimeHelper.Convert(osSource.OutsoleBalance), 1);
                            outsoleMasterView.OutsoleBalance_Date = TimeHelper.Convert(osSource.OutsoleBalance);
                        }
                        else
                            outsoleMasterView.OutsoleBalance = osSource.OutsoleBalance;

                        outsoleMasterView.OutsoleActualStartDate = "";
                        outsoleMasterView.OutsoleActualStartDate_Date = dtDefault;
                        if (osSource.OutsoleActualStartDate_Date != dtDefault)
                        {
                            outsoleMasterView.OutsoleActualStartDate = TimeHelper.DisplayDate(osSource.OutsoleActualStartDate_Date, 1);
                            outsoleMasterView.OutsoleActualStartDate_Date = osSource.OutsoleActualStartDate_Date;
                        }

                        outsoleMasterView.OutsoleActualFinishDate = "";
                        outsoleMasterView.OutsoleActualFinishDate_Date = dtDefault;
                        if (osSource.OutsoleActualFinishDate_Date != dtDefault)
                        {
                            outsoleMasterView.OutsoleActualFinishDate = TimeHelper.DisplayDate(osSource.OutsoleActualFinishDate_Date, 1);
                            outsoleMasterView.OutsoleActualFinishDate_Date = osSource.OutsoleActualFinishDate_Date;
                        }

                        if (osSource.SewingBalance.Contains("/"))
                            outsoleMasterView.SewingBalance = TimeHelper.DisplayDate(TimeHelper.Convert(osSource.SewingBalance), 1);
                        else
                            outsoleMasterView.SewingBalance = osSource.SewingBalance;

                        outsoleMasterView.OSMatsArrivalForeground = Brushes.Black;
                        outsoleMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                        if (osSource.OutsoleMaterialArrivalOriginal != dtDefault)
                        {
                            outsoleMasterView.OSMatsArrivalOrginal = osSource.OutsoleMaterialArrivalOriginal;
                            outsoleMasterView.OSMatsArrival = TimeHelper.DisplayDate(osSource.OutsoleMaterialArrivalOriginal, 0);
                        }

                        if (osSource.OutsoleMaterialStatus == "OK")
                        {
                            outsoleMasterView.OSMatsArrivalForeground = Brushes.Blue;
                            outsoleMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                        }
                        else if (osSource.OutsoleMaterialArrivalOriginal != dtDefault)
                        {
                            if (outsoleMasterView.OSMatsArrivalOrginal < DateTime.Now.Date)
                                outsoleMasterView.OSMatsArrivalForeground = Brushes.Red;
                        }
                        if (!String.IsNullOrEmpty(osSource.OutsoleMaterialRemarks))
                            outsoleMasterView.OSMatsArrivalBackground = Brushes.Yellow;

                        outsoleMasterView.OutsoleStartDateForeground = Brushes.Black;
                        outsoleMasterView.OutsoleFinishDateForeground = Brushes.Black;
                        if ((outsoleMasterView.OutsoleStartDate < outsoleMasterView.OSMatsArrivalOrginal ||
                            outsoleMasterView.OutsoleStartDate > outsoleMasterView.SewingStartDate)
                            && osSource.OutsoleStartDate != dtDefault)
                            outsoleMasterView.OutsoleStartDateForeground = Brushes.Red;

                        if (outsoleMasterView.OutsoleFinishDate > outsoleMasterView.ETD)
                            outsoleMasterView.OutsoleFinishDateForeground = Brushes.Red;

                        double perCent = (double)osSource.WHCurrentCheck / (double)osSource.Quantity * 100;
                        perCent = perCent > 100 ? 100 : perCent;
                        string percentView = string.Format("{0}%", (int)perCent);
                        if (percentView.Equals("0%"))
                            percentView = "";
                        outsoleMasterView.OutsoleWHCheckingCurrent = percentView;
                        outsoleMasterView.OutsoleWHCheckingCurrentBackground = Brushes.Transparent;
                        if ((int)perCent >= 50)
                        {
                            outsoleMasterView.OutsoleWHCheckingCurrentBackground = Brushes.Yellow;
                        }
                        if (osSource.WHCurrentCheck >= osSource.Quantity)
                        {
                            outsoleMasterView.OutsoleWHCheckingCurrent = TimeHelper.DisplayDate(osSource.WHLastDateCheck, 1);
                            outsoleMasterView.OutsoleWHCheckingCurrentBackground = Brushes.Transparent;
                        }

                        outsoleMasterViewList.Add(outsoleMasterView);
                        index++;
                    }
                    outsoleMasterViewList = outsoleMasterViewList.OrderBy(o => o.OutsoleLine).ThenBy(o => o.Sequence).ToList();
                }
                else
                {
                    bwLoad_DoWork(sender, e);
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
                return;
            }
        }
        
        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            //
            List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(w => w.ProductNo == productNo && materialIdArray.Contains(w.MaterialTypeId)).ToList();
            rawMaterialTypeList.RemoveAll(r => r.ETD.Date == dtDefault.Date);
            MaterialArrivalViewModel materialArrivalView = new MaterialArrivalViewModel();
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
            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewList);
            dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
            dgSewingMaster.Items.Refresh();

            btnCaculate.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnRefresh.IsEnabled = true;
            lblStatus.Text = "";
            prgStatus.Visibility = Visibility.Collapsed;
            //btnEnableSimulation.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnCaculate_Click(object sender, RoutedEventArgs e)
        {
            if (bwReload.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnCaculate.IsEnabled = false;
                bwReload.RunWorkerAsync();
            }
        }

        private void bwReload_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMasterList = OutsoleMasterController.Select_2();
        }

        private void bwReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var currentYear = DateTime.Now.Year;
            //Load Newest Data
            foreach (OutsoleMasterViewModel outsoleMasterView in outsoleMasterViewFindList)
            {
                OutsoleMasterModel outsoleMaster = outsoleMasterList.Where(o => o.ProductNo == outsoleMasterView.ProductNo).FirstOrDefault();
                if (outsoleMaster != null)
                {
                    string productNo = outsoleMaster.ProductNo;
                    if (isSequenceEditing == false)
                    {
                        outsoleMasterView.Sequence = outsoleMaster.Sequence;
                    }

                    if (outsoleLineUpdateList.Contains(productNo) == false)
                    {
                        outsoleMasterView.OutsoleLine = outsoleMaster.OutsoleLine;
                    }
                    if (outsoleQuotaUpdateList.Contains(productNo) == false)
                    {
                        outsoleMasterView.OutsoleQuota = outsoleMaster.OutsoleQuota;
                    }
                    if (outsoleActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        //outsoleMasterView.OutsoleActualStartDate = outsoleMaster.OutsoleActualStartDate;
                        outsoleMasterView.OutsoleActualStartDate = "";
                        if (outsoleMaster.OutsoleActualStartDate_Date != dtDefault)
                        {
                            outsoleMasterView.OutsoleActualStartDate = String.Format("{0:M/d}", outsoleMaster.OutsoleActualStartDate_Date);
                            outsoleMasterView.OutsoleActualStartDate_Date = outsoleMaster.OutsoleActualStartDate_Date;
                        }
                    }
                    if (outsoleActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        //outsoleMasterView.OutsoleActualFinishDate = outsoleMaster.OutsoleActualFinishDate;
                        outsoleMasterView.OutsoleActualFinishDate = "";
                        if (outsoleMaster.OutsoleActualFinishDate_Date != dtDefault)
                        {
                            outsoleMasterView.OutsoleActualFinishDate = String.Format("{0:M/d}", outsoleMaster.OutsoleActualFinishDate_Date);
                            outsoleMasterView.OutsoleActualStartDate_Date = outsoleMaster.OutsoleActualFinishDate_Date;
                        }
                    }

                    if (outsoleActualStartDateAutoUpdateList.Contains(productNo) == false)
                    {
                        outsoleMasterView.OutsoleActualStartDateAuto = TimeHelper.ConvertDateToView(outsoleMaster.OutsoleActualStartDateAuto);
                    }
                    if (outsoleActualFinishDateAutoUpdateList.Contains(productNo) == false)
                    {
                        outsoleMasterView.OutsoleActualFinishDateAuto = TimeHelper.ConvertDateToView(outsoleMaster.OutsoleActualFinishDateAuto);
                    }
                    if (outsoleBalanceUpdateList.Contains(productNo) == false)
                    {
                        //outsoleMasterView.OutsoleBalance = outsoleMaster.OutsoleBalance;
                        if (outsoleMaster.OutsoleBalance.Contains("/"))
                        {
                            outsoleMasterView.OutsoleBalance_Date = TimeHelper.Convert(outsoleMaster.OutsoleBalance);
                            outsoleMasterView.OutsoleBalance = String.Format("{0:M/d}", outsoleMasterView.OutsoleBalance_Date);
                        }
                        else
                            outsoleMasterView.OutsoleBalance = outsoleMaster.OutsoleBalance;
                    }
                    if (outsoleRemarksUpdateList.Contains(productNo) == false)
                    {
                        outsoleMasterView.Remarks = outsoleMaster.Remarks;
                    }
                }
            }

            //Sort By LineId, Sequence
            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewList.OrderBy(o => o.OutsoleLine).ThenBy(s => s.Sequence));
            dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
            for (int i = 0; i <= outsoleMasterViewFindList.Count - 1; i++)
            {
                outsoleMasterViewFindList[i].Sequence = i;
            }

            //Caculate
            List<String> outsoleLineList = outsoleMasterViewFindList.Select(o => o.OutsoleLine).Distinct().ToList();
            foreach (string outsoleLine in outsoleLineList)
            {
                if (String.IsNullOrEmpty(outsoleLine) == false)
                {
                    List<OutsoleMasterViewModel> outsoleMasterViewLineList = outsoleMasterViewFindList.Where(o => o.OutsoleLine == outsoleLine).ToList();
                    if (outsoleMasterViewLineList.Count > 0)
                    {
                        //DateTime dtOutsoleFinishDate = new DateTime();
                        //DateTime dtOutsoleStartDate = new DateTime();
                        DateTime dtOutsoleFinishDate = dtDefault;
                        DateTime dtOutsoleStartDate = dtDefault;
                        int dayOutsoleAddition = 0;
                        for (int i = 0; i <= outsoleMasterViewLineList.Count - 1; i++)
                        {
                            
                            #region Caculate for Outsole
                            OutsoleMasterViewModel outsoleMasterView = outsoleMasterViewLineList[i];
                            
                            int qtyOutsoleQuota = outsoleMasterView.OutsoleQuota;
                            int optOutsole = 0;
                            if (qtyOutsoleQuota > 0)
                            {
                                DateTime dtOutsoleStartDateTemp = TimeHelper.Convert(outsoleMasterView.OutsoleActualStartDate);

                                var startNow = TimeHelper.Convert(outsoleMasterView.OutsoleActualStartDate);
                                var startOld = outsoleMasterView.OutsoleActualStartDate_Date;
                                if (startNow.Month == startOld.Month && startNow.Day == startOld.Day && startNow.Year != startOld.Year)
                                {
                                    if (currentYear < startOld.Year)
                                        dtOutsoleStartDateTemp = startNow > startOld ? startNow : startOld;
                                    else
                                        dtOutsoleStartDateTemp = startNow < startOld ? startNow : startOld;
                                }
                                // For Input Directly
                                if (outsoleMasterView.OutsoleActualStartDate.Contains("/") && outsoleMasterView.OutsoleActualStartDate.Split('/').Count() > 2)
                                {
                                    dtOutsoleStartDateTemp = startNow;
                                }

                                if ((String.IsNullOrEmpty(outsoleMasterView.OutsoleActualStartDate) == false && dtOutsoleStartDateTemp != dtNothing)
                                    || outsoleMasterView == outsoleMasterViewLineList.First())
                                {
                                    dtOutsoleStartDate = dtOutsoleStartDateTemp;
                                }
                                else
                                {
                                    dtOutsoleStartDate = dtOutsoleFinishDate.AddDays(dayOutsoleAddition);
                                }
                                //sewingMasterView.SewingStartDate = dtSewingStartDate;
                                dayOutsoleAddition = 0;
                                DateTime dtOutsoleFinishDateTemp = TimeHelper.Convert(outsoleMasterView.OutsoleActualFinishDate);
                                var finishNow = TimeHelper.Convert(outsoleMasterView.OutsoleActualFinishDate);
                                var finishOld = outsoleMasterView.OutsoleActualStartDate_Date;
                                if (finishNow.Month == finishOld.Month && finishNow.Day == finishOld.Day && finishNow.Year != finishOld.Year)
                                {
                                    if (currentYear < startOld.Year)
                                        dtOutsoleFinishDateTemp = finishNow > finishOld ? finishNow : finishOld;
                                    else
                                        dtOutsoleFinishDateTemp = finishNow < finishOld ? finishNow : finishOld;
                                }
                                // For Input Directly
                                if (outsoleMasterView.OutsoleActualFinishDate.Contains("/") && outsoleMasterView.OutsoleActualFinishDate.Split('/').Count() > 2)
                                {
                                    dtOutsoleFinishDateTemp = finishNow;
                                }

                                if (String.IsNullOrEmpty(outsoleMasterView.OutsoleActualFinishDate) == false && dtOutsoleFinishDateTemp != dtNothing)
                                {
                                    dtOutsoleFinishDate = dtOutsoleFinishDateTemp;
                                }
                                else
                                {
                                    int qtyOutsoleBalance = 0;
                                    outsoleMasterView.OutsoleBalance = outsoleMasterView.OutsoleBalance.Trim();
                                    int.TryParse(outsoleMasterView.OutsoleBalance, out qtyOutsoleBalance);
                                    if (qtyOutsoleBalance > 0)
                                    {
                                        dtOutsoleFinishDate = DateTime.Now.Date.AddDays((double)(qtyOutsoleBalance) / (double)qtyOutsoleQuota);
                                        optOutsole = 1;
                                    }
                                    else
                                    {
                                        //DateTime dtOutsoleBalance = TimeHelper.Convert(outsoleMasterView.OutsoleBalance);
                                        DateTime dtOutsoleBalance = outsoleMasterView.OutsoleBalance_Date;
                                        if (String.IsNullOrEmpty(outsoleMasterView.OutsoleBalance) == true)
                                        {
                                            dtOutsoleFinishDate = dtOutsoleStartDate.AddDays((double)outsoleMasterView.Quantity / (double)qtyOutsoleQuota);
                                            optOutsole = 2;
                                        }
                                        else if (String.IsNullOrEmpty(outsoleMasterView.OutsoleBalance) == false && dtOutsoleBalance != dtNothing)
                                        {
                                            dtOutsoleFinishDate = dtOutsoleBalance.AddDays(0);
                                            optOutsole = 0;
                                            dayOutsoleAddition = 1;
                                        }
                                    }
                                }
                                //sewingMasterView.SewingFinishDate = dtSewingFinishDate;
                                if (optOutsole == 0)
                                {
                                    outsoleMasterView.OutsoleStartDate = dtOutsoleStartDate;
                                    outsoleMasterView.OutsoleFinishDate = dtOutsoleFinishDate;
                                }
                                else if (optOutsole == 1)
                                {
                                    //List<DateTime> dtCheckOffDateList1_1 = CheckOffDay(dtSewingStartDate, DateTime.Now.Date);
                                    List<DateTime> dtCheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtOutsoleFinishDate);
                                    //sewingMasterView.SewingStartDate = new DateTime(dtCheckOffDateList1.First().Year, dtCheckOffDateList1.First().Month, dtCheckOffDateList1.First().Day,
                                    //dtSewingStartDate.Hour, dtSewingStartDate.Minute, dtSewingStartDate.Second);
                                    outsoleMasterView.OutsoleStartDate = dtOutsoleStartDate;
                                    outsoleMasterView.OutsoleFinishDate = new DateTime(dtCheckOffDateList1.Last().Year, dtCheckOffDateList1.Last().Month, dtCheckOffDateList1.Last().Day,
                                        dtOutsoleFinishDate.Hour, dtOutsoleFinishDate.Minute, dtOutsoleFinishDate.Second);
                                }
                                else if (optOutsole == 2)
                                {
                                    List<DateTime> dtCheckOffDateList2 = CheckOffDay(dtOutsoleStartDate, dtOutsoleFinishDate);
                                    outsoleMasterView.OutsoleStartDate = new DateTime(dtCheckOffDateList2.First().Year, dtCheckOffDateList2.First().Month, dtCheckOffDateList2.First().Day,
                                        dtOutsoleStartDate.Hour, dtOutsoleStartDate.Minute, dtOutsoleStartDate.Second);
                                    outsoleMasterView.OutsoleFinishDate = new DateTime(dtCheckOffDateList2.Last().Year, dtCheckOffDateList2.Last().Month, dtCheckOffDateList2.Last().Day,
                                        dtOutsoleFinishDate.Hour, dtOutsoleFinishDate.Minute, dtOutsoleFinishDate.Second);
                                }

                                // Highlight OutsoleStartDate
                                outsoleMasterView.OutsoleStartDateForeground = Brushes.Black;
                                if (outsoleMasterView.OutsoleStartDate < outsoleMasterView.OSMatsArrivalOrginal || outsoleMasterView.OutsoleStartDate > outsoleMasterView.SewingStartDate)
                                {
                                    outsoleMasterView.OutsoleStartDateForeground = Brushes.Red;
                                }

                                // Highlight OutsoleFinishDate
                                outsoleMasterView.OutsoleFinishDateForeground = Brushes.Black;
                                if (outsoleMasterView.OutsoleFinishDate > outsoleMasterView.ETD)
                                {
                                    outsoleMasterView.OutsoleFinishDateForeground = Brushes.Red;
                                }

                                dtOutsoleFinishDate = outsoleMasterView.OutsoleFinishDate;

                                outsoleMasterView.OutsoleActualStartDate_Date = dtOutsoleStartDateTemp;
                                outsoleMasterView.OutsoleActualFinishDate_Date = dtOutsoleFinishDateTemp;
                            }

                            else
                            {
                                outsoleMasterView.OutsoleStartDate = dtDefault;
                                outsoleMasterView.OutsoleFinishDate = dtDefault;
                                outsoleMasterView.OutsoleActualStartDate = "";
                                outsoleMasterView.OutsoleActualFinishDate = "";
                                outsoleMasterView.OutsoleActualStartDate_Date = dtDefault;
                                outsoleMasterView.OutsoleActualFinishDate_Date = dtDefault;
                            }
                            #endregion
                        }
                    }
                }
            }

            btnCaculate.IsEnabled = true;
            this.Cursor = null;
        }

        private List<DateTime> CheckOffDay(DateTime dtStartDate, DateTime dtFinishDate)
        {
            List<DateTime> dtResultList = new List<DateTime>();
            for (DateTime dt = dtStartDate.Date; dt <= dtFinishDate.Date; dt = dt.AddDays(1))
            {
                dtResultList.Add(dt);
            }
            do
            {
                dtStartDate = dtResultList.First();
                dtFinishDate = dtResultList.Last();
                for (DateTime dt = dtStartDate.Date; dt <= dtFinishDate.Date; dt = dt.AddDays(1))
                {
                    if (offDayList.Select(o => o.Date).Contains(dt) == true && dtResultList.Contains(dt) == true)
                    {
                        dtResultList.Add(dtResultList.Last().AddDays(1));
                        dtResultList.Remove(dt);
                    }
                }
            }
            while (offDayList.Where(o => dtResultList.Contains(o.Date) == true).Count() > 0);
            return dtResultList;
        }

        private void dgSewingMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                bool isVisible = searchBox.IsVisible;
                if (isVisible == false)
                {
                    searchBox = new RawMaterialSearchBoxWindow();
                    searchBox.GetFindWhat = new RawMaterialSearchBoxWindow.GetString(SearchOutsoleMaster);
                    searchBox.Show();
                }
            }
        }

        private void SearchOutsoleMaster(string findWhat, bool isMatch, bool isShow)
        {
            outsoleMasterViewList = outsoleMasterViewList.OrderBy(o => o.OutsoleLine).ThenBy(o => o.Sequence).ToList();
            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewList);
            if (String.IsNullOrEmpty(findWhat) == false)
            {
                if (isMatch == true)
                {
                    OutsoleMasterViewModel outsoleMasterViewFind = outsoleMasterViewFindList.Where(r =>
                        r.ProductNo.ToLower() == findWhat.ToLower() || r.Country.ToLower() == findWhat.ToLower() || r.ShoeName.ToLower() == findWhat.ToLower() || r.ArticleNo.ToLower() == findWhat.ToLower() ||
                        r.OutsoleCode.ToLower() == findWhat.ToLower() || r.ETD.ToString("dd/MM/yyyy") == findWhat.ToLower() || r.OutsoleLine.ToLower() == findWhat.ToLower()).FirstOrDefault();
                    if (outsoleMasterViewFind != null)
                    {
                        dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
                        dgSewingMaster.SelectedItem = outsoleMasterViewFind;
                        dgSewingMaster.ScrollIntoView(outsoleMasterViewFind);
                        colOutsoleLine.CanUserSort = true;
                    }
                    else
                    {
                        dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
                        MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (isMatch == false)
                    {
                        if (isShow == true)
                        {
                            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == true || r.Country.ToLower().Contains(findWhat.ToLower()) == true || r.ShoeName.ToLower().Contains(findWhat.ToLower()) == true || r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == true ||
                            r.OutsoleCode.ToLower().Contains(findWhat.ToLower()) == true || r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == true || r.OutsoleLine.ToLower().Contains(findWhat.ToLower()) == true));
                        }
                        else
                        {
                            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == false && r.Country.ToLower().Contains(findWhat.ToLower()) == false && r.ShoeName.ToLower().Contains(findWhat.ToLower()) == false && r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == false &&
                            r.OutsoleCode.ToLower().Contains(findWhat.ToLower()) == false && r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == false && r.OutsoleLine.ToLower().Contains(findWhat.ToLower()) == false));
                        }

                        if (outsoleMasterViewFindList.Count > 0)
                        {
                            dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
                            colOutsoleLine.CanUserSort = false;
                        }
                        else
                        {
                            dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
                            MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                            outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewList);
                            dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
                        }
                    }
                }
            }
            else
            {
                colOutsoleLine.CanUserSort = true;
                dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
            }
        }

        private void dgSewingMaster_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            OutsoleMasterViewModel outsoleMasterView = (OutsoleMasterViewModel)e.Row.Item;
            if (outsoleMasterView == null)
            {
                return;
            }
            if (!linesNeedSaving.Contains(outsoleMasterView.OutsoleLine))
                linesNeedSaving.Add(outsoleMasterView.OutsoleLine);

            string productNo = outsoleMasterView.ProductNo;

            if (e.Column == colOutsoleLine || e.Column == colOutsoleQuota || e.Column == colOutsoleActualStartDate ||
                e.Column == colOutsoleActualFinishDate || e.Column == colOutsoleBalance || e.Column == colRemarks)
            {
                if ( e.Column != colRemarks)
                    lineOutsoleEditingList.Add(outsoleMasterView.OutsoleLine);

                if (e.Column == colOutsoleLine)
                {
                    outsoleLineUpdateList.Add(productNo);
                }
                if (e.Column == colOutsoleQuota)
                {
                    outsoleQuotaUpdateList.Add(productNo);
                }
                if (e.Column == colOutsoleActualStartDate)
                {
                    outsoleActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colOutsoleActualFinishDate)
                {
                    outsoleActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colOutsoleBalance)
                {
                    outsoleBalanceUpdateList.Add(productNo);
                }
                if (e.Column == colRemarks)
                {
                    outsoleRemarksUpdateList.Add(productNo);
                }
            }

            if (e.Column == colOutsoleLine)
            {
                //SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
                string outsoleLine = outsoleMasterView.OutsoleLine;
                if (String.IsNullOrEmpty(outsoleLine) == true)
                {
                    return;
                }
                int outsoleSequence = 0;
                if (outsoleMasterViewList.Where(o => o.OutsoleLine == outsoleLine).Count() > 0)
                {
                    outsoleSequence = outsoleMasterViewList.Where(o => o.OutsoleLine == outsoleLine).Select(o => o.Sequence).Max() + 1;
                }
                outsoleMasterView.Sequence = outsoleSequence;
                //sequenceUpdateList.Add(outsoleSequence);
                isSequenceEditing = true;

                //changingSequence = true;
            }
            if (e.Column == colOutsoleActualStartDate || e.Column == colOutsoleActualFinishDate)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }
            }
            isEditing = false;
        }

        private void dgSewingMaster_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column == colOutsoleLine)
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    outsoleMasterViewList = outsoleMasterViewList.OrderBy(o => o.OutsoleLine).ThenBy(o => o.Sequence).ToList();
                    outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewList.OrderBy(o => o.OutsoleLine).ThenBy(o => o.Sequence));
                }
                else
                {
                    outsoleMasterViewList = outsoleMasterViewList.OrderByDescending(o => o.OutsoleLine).ThenBy(o => o.Sequence).ToList();
                    outsoleMasterViewFindList = new ObservableCollection<OutsoleMasterViewModel>(outsoleMasterViewList.OrderByDescending(o => o.OutsoleLine).ThenBy(o => o.Sequence));
                }
                dgSewingMaster.ItemsSource = outsoleMasterViewFindList;
                for (int i = 0; i <= outsoleMasterViewFindList.Count - 1; i++)
                {
                    outsoleMasterViewFindList[i].Sequence = i;
                }
                dgSewingMaster.ScrollIntoView(outsoleMasterViewFindList.Where(o => String.IsNullOrEmpty(o.OutsoleLine) == false).FirstOrDefault());
                e.Handled = true;
            }
        }

        private void dgSewingMaster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OutsoleMasterViewModel outsoleMasterView = (OutsoleMasterViewModel)dgSewingMaster.CurrentItem;
            if (account.OutsoleMaster == true && dgSewingMaster.CurrentCell.Column == colOutsoleBalance && outsoleMasterView != null)
            {
                OutsoleInputOutputWindow window = new OutsoleInputOutputWindow(outsoleMasterView.ProductNo,
                                                                                outsoleMasterView.OutsoleActualStartDateAuto,
                                                                                outsoleMasterView.OutsoleActualFinishDateAuto);
                if (window.ShowDialog() == true)
                {
                    string productNo = outsoleMasterView.ProductNo;
                    string sewingLine = outsoleMasterView.SewingLine;
                    outsoleMasterView.OutsoleBalance_Date   = TimeHelper.Convert(window.resultString);
                    if (outsoleMasterView.OutsoleBalance_Date == dtNothing)
                        outsoleMasterView.OutsoleBalance_Date = dtDefault;
                    outsoleMasterView.OutsoleBalance        = window.resultString;
                    if (outsoleMasterView.OutsoleBalance_Date != dtDefault)
                        outsoleMasterView.OutsoleBalance = String.Format("{0:M/d}", outsoleMasterView.OutsoleBalance_Date);

                    outsoleMasterView.OutsoleActualStartDateAuto = window.outsoleActualStartDateAuto;
                    outsoleMasterView.OutsoleActualFinishDateAuto = window.outsoleActualFinishDateAuto;
                    if (String.IsNullOrEmpty(window.resultString) == true)
                    {
                        outsoleMasterView.OutsoleActualStartDateAuto = "";
                        outsoleMasterView.OutsoleActualFinishDateAuto = "";
                    }
                    lineOutsoleEditingList.Add(outsoleMasterView.OutsoleLine);
                    outsoleActualStartDateAutoUpdateList.Add(productNo);
                    outsoleActualFinishDateAutoUpdateList.Add(productNo);
                    outsoleBalanceUpdateList.Add(productNo);

                    if (!linesNeedSaving.Contains(outsoleMasterView.OutsoleLine))
                        linesNeedSaving.Add(outsoleMasterView.OutsoleLine);
                }
            }
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                outsoleMasterViewToInsertList = dgSewingMaster.Items.OfType<OutsoleMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                outsoleMasterViewToInsertList = dgSewingMaster.Items.OfType<OutsoleMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = true;
                Dispatcher.Invoke(new Action(() =>
                {
                    prgStatus.Visibility = Visibility.Visible;
                    prgStatus.Value = 0;
                }));
                var sourceList = outsoleMasterViewFindList.ToList();

                // Insert New PO
                var productNoSourceList = outsoleMasterList.Select(s => s.ProductNo).ToList();
                var insertNewPOList = sourceList.Where(w => !productNoSourceList.Contains(w.ProductNo)).ToList();
                if (insertNewPOList.Count() > 0)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        prgStatus.Value = 0;
                        prgStatus.Maximum = insertNewPOList.Count();
                        lblStatus.Text = "Inserting New PO ...";
                    }));
                    int index = 1;
                    foreach (var item in insertNewPOList)
                    {
                        InsertAModel(item, true);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Text = String.Format("Inserting {0} / {1} PO", index, insertNewPOList.Count());
                            prgStatus.Value = index;
                        }));
                        index++;
                    }
                }

                // Update OSMaster Info
                var updateList = sourceList.Where(w => linesNeedSaving.Contains(w.OutsoleLine)).ToList();
                var osMasterUpdateList = new List<OutsoleMasterModel>();
                if (updateList.Count() > 0)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        lblStatus.Text = "Saving PO ...";
                        prgStatus.Value = 0;
                        prgStatus.Maximum = updateList.Count();
                    }));
                    int index = 1;
                    foreach (var item in updateList)
                    {
                        InsertAModel(item, false);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Text = String.Format("Saving {0} / {1} PO", index, updateList.Count());
                            prgStatus.Value = index;
                        }));
                        index++;
                    }
                    linesNeedSaving.Clear();
                }

                if (sourceList.Count() == outsoleMasterViewList.Count() && account.OutsoleMaster)
                {
                    // Get the sequence list
                    int sqNo = 0;
                    var productNoList = sourceList.Select(s => s.ProductNo).ToList();
                    var sequenceCurrentList = new List<POSequenceModel>();
                    foreach (var po in productNoList)
                    {
                        sequenceCurrentList.Add(new POSequenceModel
                        {
                            ProductNo = po,
                            Sequence = sqNo,
                            Id = po + "-" + sqNo.ToString()
                        });
                        sqNo++;
                    }

                    var sqNeedUpdateList = new List<POSequenceModel>();
                    foreach (var item in sequenceCurrentList)
                    {
                        var checkSqChange = poSequenceSourceList.FirstOrDefault(f => f.Id == item.Id);
                        if (checkSqChange == null)
                            sqNeedUpdateList.Add(item);
                    }
                    poSequenceSourceList.Clear();
                    poSequenceSourceList = sequenceCurrentList.ToList();
                    if (sqNeedUpdateList.Count() > 0)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Text = "Saving Sequence PO ...";
                            prgStatus.Value = 0;
                            prgStatus.Maximum = sqNeedUpdateList.Count();
                        }));
                        int index = 1;
                        foreach (var item in sqNeedUpdateList)
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                lblStatus.Text = String.Format("Saving {0} / {1} Sq", index, sqNeedUpdateList.Count());
                                prgStatus.Value = index;
                            }));
                            CommonController.UpdateSequenceByPO(item.ProductNo, item.Sequence, "Outsole");
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Result = false;
                    return;
                }));
            }
        }

        private void InsertAModel (OutsoleMasterViewModel viewModel, bool isNewPO)
        {
            string productNo = viewModel.ProductNo;
            string outsoleLine = viewModel.OutsoleLine;
            OutsoleMasterModel model = new OutsoleMasterModel
            {
                ProductNo = viewModel.ProductNo,
                Sequence = viewModel.Sequence,
                OutsoleLine = viewModel.OutsoleLine,
                OutsoleStartDate = viewModel.OutsoleStartDate,
                OutsoleFinishDate = viewModel.OutsoleFinishDate,
                OutsoleQuota = viewModel.OutsoleQuota,
                OutsoleActualStartDate = viewModel.OutsoleActualStartDate,
                OutsoleActualFinishDate = viewModel.OutsoleActualFinishDate,
                OutsoleActualStartDate_Date = viewModel.OutsoleActualStartDate_Date,
                OutsoleActualFinishDate_Date = viewModel.OutsoleActualFinishDate_Date,

                OutsoleActualStartDateAuto = viewModel.OutsoleActualStartDateAuto,
                OutsoleActualFinishDateAuto = viewModel.OutsoleActualFinishDateAuto,
                OutsoleBalance = viewModel.OutsoleBalance,
                Remarks = viewModel.Remarks,

                IsSequenceUpdate = false,
                IsOutsoleLineUpdate = false,
                IsOutsoleStartDateUpdate = false,
                IsOutsoleFinishDateUpdate = false,
                IsOutsoleQuotaUpdate = false,
                IsOutsoleActualStartDateUpdate = false,
                IsOutsoleActualFinishDateUpdate = false,
                IsOutsoleActualStartDateAutoUpdate = false,
                IsOutsoleActualFinishDateAutoUpdate = false,
                IsOutsoleBalanceUpdate = false,
                IsRemarksUpdate = false
            };

            model.IsSequenceUpdate = isSequenceEditing;

            model.IsOutsoleLineUpdate = outsoleLineUpdateList.Contains(productNo);
            model.IsOutsoleStartDateUpdate = lineOutsoleEditingList.Contains(outsoleLine);
            model.IsOutsoleFinishDateUpdate = lineOutsoleEditingList.Contains(outsoleLine);
            model.IsOutsoleQuotaUpdate = outsoleQuotaUpdateList.Contains(productNo);
            model.IsOutsoleActualStartDateUpdate = outsoleActualStartDateUpdateList.Contains(productNo);
            model.IsOutsoleActualFinishDateUpdate = outsoleActualFinishDateUpdateList.Contains(productNo);
            model.IsOutsoleActualStartDateAutoUpdate = outsoleActualStartDateAutoUpdateList.Contains(productNo);
            model.IsOutsoleActualFinishDateAutoUpdate = outsoleActualFinishDateAutoUpdateList.Contains(productNo);
            model.IsOutsoleBalanceUpdate = outsoleBalanceUpdateList.Contains(productNo);
            model.IsRemarksUpdate = outsoleRemarksUpdateList.Contains(productNo);
            // && sequenceUpdateList.Contains(model.Sequence)
            if ((model.IsSequenceUpdate == true) ||
                model.IsOutsoleLineUpdate == true ||
                model.IsOutsoleStartDateUpdate == true ||
                model.IsOutsoleFinishDateUpdate == true ||
                model.IsOutsoleQuotaUpdate == true ||
                model.IsOutsoleActualStartDateUpdate == true ||
                model.IsOutsoleActualFinishDateUpdate == true ||
                model.IsOutsoleActualStartDateAutoUpdate == true ||
                model.IsOutsoleActualFinishDateAutoUpdate == true ||
                model.IsOutsoleBalanceUpdate == true ||
                model.IsRemarksUpdate == true ||
                isNewPO == true
                )
            {
                OutsoleMasterController.Insert_2(model, account);
            }
        }

        private void bwInsert_DoWork_Before(object sender, DoWorkEventArgs e)
        {
            foreach (OutsoleMasterViewModel outsoleMasterView in outsoleMasterViewToInsertList)
            {
                string productNo = outsoleMasterView.ProductNo;
                string outsoleLine = outsoleMasterView.OutsoleLine;
                OutsoleMasterModel model = new OutsoleMasterModel
                {
                    ProductNo = outsoleMasterView.ProductNo,
                    Sequence = outsoleMasterView.Sequence,
                    OutsoleLine = outsoleMasterView.OutsoleLine,
                    OutsoleStartDate = outsoleMasterView.OutsoleStartDate,
                    OutsoleFinishDate = outsoleMasterView.OutsoleFinishDate,
                    OutsoleQuota = outsoleMasterView.OutsoleQuota,
                    OutsoleActualStartDate = outsoleMasterView.OutsoleActualStartDate,
                    OutsoleActualFinishDate = outsoleMasterView.OutsoleActualFinishDate,
                    OutsoleActualStartDate_Date = outsoleMasterView.OutsoleActualStartDate_Date,
                    OutsoleActualFinishDate_Date = outsoleMasterView.OutsoleActualFinishDate_Date,

                    OutsoleActualStartDateAuto = outsoleMasterView.OutsoleActualStartDateAuto,
                    OutsoleActualFinishDateAuto = outsoleMasterView.OutsoleActualFinishDateAuto,
                    OutsoleBalance = outsoleMasterView.OutsoleBalance,
                    Remarks = outsoleMasterView.Remarks,

                    IsSequenceUpdate = false,
                    IsOutsoleLineUpdate = false,
                    IsOutsoleStartDateUpdate = false,
                    IsOutsoleFinishDateUpdate = false,
                    IsOutsoleQuotaUpdate = false,
                    IsOutsoleActualStartDateUpdate = false,
                    IsOutsoleActualFinishDateUpdate = false,
                    IsOutsoleActualStartDateAutoUpdate = false,
                    IsOutsoleActualFinishDateAutoUpdate = false,
                    IsOutsoleBalanceUpdate = false,
                    IsRemarksUpdate = false
                };

                model.IsSequenceUpdate = isSequenceEditing;

                model.IsOutsoleLineUpdate = outsoleLineUpdateList.Contains(productNo);
                model.IsOutsoleStartDateUpdate = lineOutsoleEditingList.Contains(outsoleLine);
                model.IsOutsoleFinishDateUpdate = lineOutsoleEditingList.Contains(outsoleLine);
                model.IsOutsoleQuotaUpdate = outsoleQuotaUpdateList.Contains(productNo);
                model.IsOutsoleActualStartDateUpdate = outsoleActualStartDateUpdateList.Contains(productNo);
                model.IsOutsoleActualFinishDateUpdate = outsoleActualFinishDateUpdateList.Contains(productNo);
                model.IsOutsoleActualStartDateAutoUpdate = outsoleActualStartDateAutoUpdateList.Contains(productNo);
                model.IsOutsoleActualFinishDateAutoUpdate = outsoleActualFinishDateAutoUpdateList.Contains(productNo);
                model.IsOutsoleBalanceUpdate = outsoleBalanceUpdateList.Contains(productNo);
                model.IsRemarksUpdate = outsoleRemarksUpdateList.Contains(productNo);
                // && sequenceUpdateList.Contains(model.Sequence)
                if ((model.IsSequenceUpdate == true) ||
                    model.IsOutsoleLineUpdate == true ||
                    model.IsOutsoleStartDateUpdate == true ||
                    model.IsOutsoleFinishDateUpdate == true ||
                    model.IsOutsoleQuotaUpdate == true ||
                    model.IsOutsoleActualStartDateUpdate == true ||
                    model.IsOutsoleActualFinishDateUpdate == true ||
                    model.IsOutsoleActualStartDateAutoUpdate == true ||
                    model.IsOutsoleActualFinishDateAutoUpdate == true ||
                    model.IsOutsoleBalanceUpdate == true ||
                    model.IsRemarksUpdate == true)
                {
                    OutsoleMasterController.Insert_2(model, account);
                }
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            if (e.Result != null && (bool)e.Result == false)
                return;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            isSequenceEditing = false;
            //sequenceUpdateList.Clear();
            lineOutsoleEditingList.Clear();

            outsoleLineUpdateList.Clear();

            outsoleQuotaUpdateList.Clear();
            outsoleActualStartDateUpdateList.Clear();
            outsoleActualFinishDateUpdateList.Clear();
            outsoleActualStartDateAutoUpdateList.Clear();
            outsoleActualFinishDateAutoUpdateList.Clear();
            outsoleBalanceUpdateList.Clear();
            outsoleRemarksUpdateList.Clear();

            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            prgStatus.Visibility = Visibility.Collapsed;
            lblStatus.Text = "";
        }

        private void dgSewingMaster_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private List<OutsoleMasterViewModel> outsoleMasterViewSelectList = new List<OutsoleMasterViewModel>();
        
        private void dgSewingMaster_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
            outsoleMasterViewSelectList.Clear();
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null)
            {
                foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
                {
                    outsoleMasterViewSelectList.Add((OutsoleMasterViewModel)cellInfo.Item);
                }
                outsoleMasterViewSelectList = outsoleMasterViewSelectList.Distinct().ToList();
            }
        }

        private void dgSewingMaster_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && account.OutsoleMaster == true && isEditing == false)
            {
                var dataGrid = (DataGrid)sender;
                if (dataGrid != null)
                {
                    if (e.OriginalSource.GetType() == typeof(Thumb))
                    {
                        return;
                    }
                    if ((OutsoleMasterViewModel)dataGrid.CurrentItem != null && outsoleMasterViewSelectList.Contains((OutsoleMasterViewModel)dataGrid.CurrentItem) == false)
                    {
                        outsoleMasterViewSelectList.Add((OutsoleMasterViewModel)dataGrid.CurrentItem);
                    }
                    if (outsoleMasterViewSelectList.Count > 0)
                    {
                        listView.ItemsSource = outsoleMasterViewSelectList;
                        popup.PlacementTarget = lblPopup;
                        DragDrop.DoDragDrop(dataGrid, outsoleMasterViewSelectList, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgSewingMaster_DragLeave(object sender, DragEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)e.OriginalSource;

            if (frameworkElement != null && frameworkElement.DataContext != null
                && frameworkElement.DataContext.GetType() == typeof(OutsoleMasterViewModel))
            {
                OutsoleMasterViewModel outsoleMasterView = (OutsoleMasterViewModel)frameworkElement.DataContext;
                dgSewingMaster.SelectedItem = outsoleMasterView;
                dgSewingMaster.ScrollIntoView(outsoleMasterView);
            }
            else
            {
                return;
            }
            Point point = new Point(wf.Control.MousePosition.X, wf.Control.MousePosition.Y);
            Point point1 = dgSewingMaster.PointFromScreen(point);
            popup.HorizontalOffset = point1.X + 5;
            popup.VerticalOffset = point1.Y + 5;
            popup.IsOpen = true;
        }

        private void dgSewingMaster_Drop(object sender, DragEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                popup.IsOpen = false;
                OutsoleMasterViewModel outsoleMasterView = (OutsoleMasterViewModel)dataGrid.SelectedItem;
                int index = dataGrid.Items.IndexOf(outsoleMasterView);
                int indexFirst = dataGrid.Items.IndexOf(outsoleMasterViewSelectList.First());
                int indexLast = dataGrid.Items.IndexOf(outsoleMasterViewSelectList.Last());
                if (index < indexFirst && index < indexLast)
                {
                    for (int i = outsoleMasterViewSelectList.Count - 1; i >= 0; i = i - 1)
                    {
                        outsoleMasterViewFindList.Remove(outsoleMasterViewSelectList[i]);
                        outsoleMasterViewFindList.Insert(index, outsoleMasterViewSelectList[i]);
                        outsoleMasterViewSelectList[i].Sequence = outsoleMasterView.Sequence + i;
                        //sequenceUpdateList.Add(outsoleMasterViewFindList[i].Sequence);
                    }
                    for (int i = index + outsoleMasterViewSelectList.Count; i <= outsoleMasterViewFindList.Count - 1; i++)
                    {
                        outsoleMasterViewFindList[i].Sequence = outsoleMasterViewFindList[i].Sequence + outsoleMasterViewSelectList.Count;
                        //sequenceUpdateList.Add(outsoleMasterViewFindList[i].Sequence);
                    }
                    isSequenceEditing = true;

                    //changingSequence = true;
                }
                else if (index > indexFirst && index > indexLast)
                {
                    for (int i = 0; i <= outsoleMasterViewSelectList.Count - 1; i = i + 1)
                    {
                        outsoleMasterViewFindList.Remove(outsoleMasterViewSelectList[i]);
                        outsoleMasterViewFindList.Insert(index - 1, outsoleMasterViewSelectList[i]);
                        outsoleMasterViewSelectList[i].Sequence = outsoleMasterView.Sequence + i;
                        //sequenceUpdateList.Add(outsoleMasterViewFindList[i].Sequence);
                    }
                    for (int i = index; i <= outsoleMasterViewFindList.Count - 1; i++)
                    {
                        outsoleMasterViewFindList[i].Sequence = outsoleMasterViewFindList[i].Sequence + outsoleMasterViewSelectList.Count;
                        //sequenceUpdateList.Add(outsoleMasterViewFindList[i].Sequence);
                    }
                    isSequenceEditing = true;

                    //changingSequence = true;
                }
                dgSewingMaster.SelectedItems.Clear();

            }
        }

        private void dgSewingMaster_DragOver(object sender, DragEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DependencyObject dependencyObject = dataGrid;
            while (dependencyObject.GetType() != typeof(ScrollViewer))
            {
                dependencyObject = VisualTreeHelper.GetChild(dependencyObject, 0);
            }
            ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
            if (scrollViewer == null)
            {
                return;
            }

            double toleranceHeight = 60;
            double verticalPosition = e.GetPosition(dataGrid).Y;
            //double offset = 5;

            if (verticalPosition < toleranceHeight) // Top of visible list? 
            {
                //Scroll up.
                scrollViewer.LineUp();
                //scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            }
            else if (verticalPosition > dgSewingMaster.ActualHeight - toleranceHeight) //Bottom of visible list? 
            {
                //Scroll down.
                scrollViewer.LineDown();
                //scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if ((isSequenceEditing == true || lineOutsoleEditingList.Count > 0 ||
                outsoleLineUpdateList.Count > 0 || outsoleQuotaUpdateList.Count > 0 || outsoleActualStartDateUpdateList.Count > 0 ||
                outsoleActualFinishDateUpdateList.Count > 0 || outsoleBalanceUpdateList.Count > 0 || outsoleRemarksUpdateList.Count > 0) && simulationMode == false)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (bwInsert.IsBusy == false)
                    {
                        e.Cancel = true;
                        this.Cursor = Cursors.Wait;
                        outsoleMasterViewToInsertList = dgSewingMaster.Items.OfType<OutsoleMasterViewModel>().ToList();
                        btnSave.IsEnabled = false;
                        bwInsert.RunWorkerAsync();
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

        private void dgSewingMaster_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (DataGridCellInfo dataGridCellInfo in e.RemovedCells)
            {
                if (dataGridCellInfo.Item != DependencyProperty.UnsetValue)
                {
                    OutsoleMasterViewModel outsoleMasterView = (OutsoleMasterViewModel)dataGridCellInfo.Item;
                    if (outsoleMasterView != null)
                    {
                        outsoleMasterView.ProductNoBackground = Brushes.Transparent;
                    }
                }
            }
            foreach (DataGridCellInfo dataGridCellInfo in e.AddedCells)
            {
                OutsoleMasterViewModel outsoleMasterView = (OutsoleMasterViewModel)dataGridCellInfo.Item;
                if (outsoleMasterView != null)
                {
                    outsoleMasterView.ProductNoBackground = Brushes.RoyalBlue;
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

        bool simulationMode = false;
        string title = "";
        private void btnEnableSimulation_Click(object sender, RoutedEventArgs e)
        {
            dgSewingMaster.AlternatingRowBackground = Brushes.White;
            dgSewingMaster.RowBackground = Brushes.White;

            title = "Master Schedule - Outsole Simulation File";
            this.Title = title;

            simulationMode = true;

            //btnEnableSimulation.IsEnabled = false;
            //btnDisableSimulation.IsEnabled = true;
            //btnDisableSimulation.Visibility = Visibility.Visible;

            ctmTranfer.Visibility = Visibility.Visible;
            btnSave.IsEnabled = false;
        }

        private void btnDisableSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                dgSewingMaster.ItemsSource = null;
                simulationMode = true;
                dgSewingMaster.AlternatingRowBackground = Brushes.LightCyan;
                dgSewingMaster.RowBackground = Brushes.White;

                title = "Master Schedule - Outsole Master File";
                this.Title = title;

                ctmTranfer.Visibility = Visibility.Collapsed;
                simulationMode = false;

                //btnDisableSimulation.IsEnabled = false;

                btnSave.IsEnabled = false;
                btnCaculate.IsEnabled = false;

                outsoleMasterViewList = new List<OutsoleMasterViewModel>();
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void miTranfer_Click(object sender, RoutedEventArgs e)
        {
            outsoleMasterViewToInsertList = dgSewingMaster.SelectedItems.OfType<OutsoleMasterViewModel>().ToList();
            if (outsoleMasterViewToInsertList.Count <= 0 ||
                MessageBox.Show("Confirm Tranfer?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwInsert.RunWorkerAsync();
            }
        }

        private class OSMWHCheckingCurrent
        {
            public string SupplierName { get; set; }
            public int QuantityCheck { get; set; }
            public string Percent { get; set; }
            public DateTime CheckingDate { get; set; }

            //public List<outsoletemporary> osDelivery = new List<outsoletemporary>();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!bwLoad.IsBusy)
            {
                outsoleMasterViewList.Clear();
                btnRefresh.IsEnabled = false;
                prgStatus.Value = 0;
                lblStatus.Text = "Re-loading ...";
                prgStatus.Visibility = Visibility.Visible;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
    }
}
