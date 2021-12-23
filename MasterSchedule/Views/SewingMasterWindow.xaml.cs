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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SewingMasterWindow.xaml
    /// </summary>
    public partial class SewingMasterWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoad;
        List<OffDayModel> offDayList;
        List<OrdersModel> orderList;
        List<SewingMasterViewModel> sewingMasterViewList;
        public ObservableCollection<SewingMasterViewModel> sewingMasterViewFindList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SewingMasterViewModel> sewingMasterViewToInsertList;
        List<RawMaterialModel> rawMaterialList;
        DateTime dtNothing;
        DateTime dtDefault;
        BackgroundWorker bwInsert;
        bool isEditing;
        BackgroundWorker bwReload;
        bool isSequenceEditing;

        //List<Int32> sequenceUpdateList;

        List<String> lineSewingEditingList;
        List<String> lineCutPrepEditingList;

        List<String> sewingLineUpdateList;

        List<String> sewingQuotaUpdateList;

        List<String> sewingPrepUpdateList;

        List<String> sewingActualStartDateUpdateList;
        List<String> sewingActualFinishDateUpdateList;

        List<String> sewingActualStartDateUpdateAutoList;
        List<String> sewingActualFinishDateUpdateAutoList;

        List<String> sewingBalanceUpdateList;

        List<String> cutAQuotaUpdateList;
        List<String> cutAActualStartDateUpdateList;
        List<String> cutAActualFinishDateUpdateList;
        List<String> cutABalanceUpdateList;

        List<String> printingBalanceUpdateList;
        List<String> h_fBalanceUpdateList;
        List<String> embroideryBalanceUpdateList;

        List<String> cutBActualStartDateUpdateList;
        List<String> cutBBalanceUpdateList;

        List<String> autoCutUpdateList;
        List<String> laserCutUpdateList;
        List<String> huasenCutUpdateList;

        List<String> cutBStartDateUpdateList;

        List<String> atomCutAUpdateList;
        List<String> atomCutBUpdateList;

        List<String> laserCutAUpdateList;
        List<String> laserCutBUpdateList;

        List<String> huasenCutAUpdateList;
        List<String> huasenCutBUpdateList;

        List<String> comelzCutAUpdateList;
        List<String> comelzCutBUpdateList;

        RawMaterialSearchBoxWindow searchBox;
        List<ProductionMemoModel> productionMemoList;

        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;

        List<SewingMasterViewModelNew> sewingMasterViewModelNewList;
        //List<RawMaterialViewModelNew> rawMaterialViewModelNewList;

        List<SewingMasterSourceModel> sewingMasterSourceList;

        private int _SEW_VS_CUTA = 0;

        private int _SEW_VS_OTHERS_CUT_A = 0;
        private int _SEW_VS_OTHERS_CUT_B = 0;

        List<String> productNoPrintList;
        List<String> linesNeedSaving;
        List<POSequenceModel> poSequenceSourceList;
        private PrivateDefineModel def;

        public SewingMasterWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork_2);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            offDayList = new List<OffDayModel>();
            orderList = new List<OrdersModel>();
            sewingMasterViewList = new List<SewingMasterViewModel>();
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sewingMasterViewToInsertList = new List<SewingMasterViewModel>();
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

            lineSewingEditingList = new List<String>();
            lineCutPrepEditingList = new List<String>();

            sewingLineUpdateList = new List<String>();

            sewingQuotaUpdateList = new List<String>();

            sewingPrepUpdateList = new List<String>();

            sewingActualStartDateUpdateList = new List<String>();
            sewingActualFinishDateUpdateList = new List<String>();

            sewingActualStartDateUpdateAutoList = new List<String>();
            sewingActualFinishDateUpdateAutoList = new List<String>();

            sewingBalanceUpdateList = new List<String>();

            cutAQuotaUpdateList = new List<String>();
            cutAActualStartDateUpdateList = new List<String>();
            cutAActualFinishDateUpdateList = new List<String>();
            cutABalanceUpdateList = new List<String>();

            printingBalanceUpdateList = new List<String>();
            h_fBalanceUpdateList = new List<String>();
            embroideryBalanceUpdateList = new List<String>();

            cutBActualStartDateUpdateList = new List<String>();
            cutBBalanceUpdateList = new List<String>();

            autoCutUpdateList = new List<String>();
            laserCutUpdateList = new List<String>();
            huasenCutUpdateList = new List<String>();

            cutBStartDateUpdateList = new List<String>();

            atomCutAUpdateList = new List<String>();
            atomCutBUpdateList = new List<String>();

            laserCutAUpdateList = new List<String>();
            laserCutBUpdateList = new List<String>();

            huasenCutAUpdateList = new List<String>();
            huasenCutBUpdateList = new List<String>();

            comelzCutAUpdateList = new List<string>();
            comelzCutBUpdateList = new List<string>();

            searchBox = new RawMaterialSearchBoxWindow();

            productionMemoList = new List<ProductionMemoModel>();

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();

            sewingMasterViewModelNewList = new List<SewingMasterViewModelNew>();
            //rawMaterialViewModelNewList = new List<RawMaterialViewModelNew>();
            productNoPrintList = new List<string>();
            
            sewingMasterSourceList = new List<SewingMasterSourceModel>();

            linesNeedSaving = new List<string>();
            poSequenceSourceList = new List<POSequenceModel>();

            def = new PrivateDefineModel();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.SewingMaster == true)
            {
                colSewingLine.IsReadOnly = false;
                colSewingQuota.IsReadOnly = false;
                colSewingActualStartDate.IsReadOnly = false;
                colSewingActualFinishDate.IsReadOnly = false;
                colSewingPrep.IsReadOnly = false;
            }
            if (account.CutPrepMaster == true)
            {
                colCutAQuota.IsReadOnly = false;
                colCutAActualStartDate.IsReadOnly = false;
                colCutAActualFinishDate.IsReadOnly = false;
                colCutABalance.IsReadOnly = false;
                //colPrintingBalance.IsReadOnly = false;
                colH_FBalance.IsReadOnly = false;
                //colEmbroideryBalance.IsReadOnly = false;
                //colCutBActualStartDate.IsReadOnly = false;
                colCutBBalance.IsReadOnly = false;
                //colAutoCut.IsReadOnly = false;
                //colLaserCut.IsReadOnly = false;
                //colHuasenCut.IsReadOnly = false;
                colCutBStartDate.IsReadOnly = false;
                colAtomCutA.IsReadOnly = false;
                colAtomCutB.IsReadOnly = false;
                colLaserCutA.IsReadOnly = false;
                colLaserCutB.IsReadOnly = false;
                colHuasenCutA.IsReadOnly = false;
                colHuasenCutB.IsReadOnly = false;
                colComelzCutA.IsReadOnly = false;
                colComelzCutB.IsReadOnly = false;
            }

            if (account.Sortable == true)
            {
                colCountry.CanUserSort = true;
                colStyle.CanUserSort = true;
                colETD.CanUserSort = true;
            }

            //if (account.Simulation == true)
            //{
            //    //btnEnableSimulation.Visibility = Visibility.Visible;
            //}
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                prgStatus.Visibility = Visibility.Visible;
                prgStatus.Value = 0;
                lblStatus.Text = "Loading PO ...";
                bwLoad.RunWorkerAsync();
            }
        }
        
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                sewingMasterViewList.Clear();
                this.Cursor = Cursors.Wait;
                btnCaculate.IsEnabled = false;
                btnRefresh.IsEnabled = false;
                prgStatus.Value = 0;
                lblStatus.Text = "Re-loading ...";
                prgStatus.Visibility = Visibility.Visible;
                bwLoad.RunWorkerAsync();
            }
        }

        private object CallStaticMethod(string typeName, string methodName)
        {
            object result = null;
            var type = Type.GetType(typeName);
            if (type != null)
            {
                var method = type.GetMethod(methodName);
                if (method != null)
                {
                    return method.Invoke(null, null);
                }
            }
            return result;
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = (List<OrdersModel>)CallStaticMethod("OrdersController", "Select");

            Task t1 = new Task(()=> { 
                offDayList = OffDayController.Select();
            });
            t1.Start();
            Task t2 = new Task(() =>
            {
                orderList = OrdersController.Select();
            });
            t2.Start();

            Task t3 = new Task(() => { 
                sewingMasterList = SewingMasterController.Select();
            });
            t3.Start();

            Task t4 = new Task(() =>
            {
                rawMaterialList = RawMaterialController.Select();
            });
            t4.Start();

            Task t5 = new Task(() =>
            {
                outsoleMasterList = OutsoleMasterController.Select();
            });
            t5.Start();

            Task t6 = new Task(() =>
            {
                outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            });
            t6.Start();
            Task t7 = new Task(() =>
            {
                outsoleMaterialList = OutsoleMaterialController.Select();
            });
            t7.Start();

            Task.WaitAll(t1, t2, t3, t4, t5, t6, t7);

            productionMemoList = ProductionMemoController.Select();
            //rawMaterialViewModelNewList = RawMaterialController.Select_1();
            def = PrivateDefineController.GetDefine();

            _SEW_VS_CUTA = def.SewingVsCutAStartDate;
            _SEW_VS_OTHERS_CUT_A = def.SewingVsOthersCutTypeA;
            _SEW_VS_OTHERS_CUT_B = def.SewingVsOthersCutTypeB;

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
                SewingMasterViewModel sewingMasterView = new SewingMasterViewModel
                {
                    ProductNo = order.ProductNo,
                    ProductNoBackground = Brushes.Transparent,
                    Country = order.Country,
                    ShoeName = order.ShoeName,
                    ArticleNo = order.ArticleNo,
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
                        memoId += "\n";
                }
                sewingMasterView.MemoId = memoId;

                MaterialArrivalViewModel materialArrivalUpper = MaterialArrival(order.ProductNo, materialIdUpperArray);
                sewingMasterView.UpperMatsArrivalOrginal = dtDefault;
                if (materialArrivalUpper != null)
                {
                    sewingMasterView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalUpper.Date);
                    sewingMasterView.UpperMatsArrivalOrginal = materialArrivalUpper.Date;
                    sewingMasterView.UpperMatsArrivalForeground = materialArrivalUpper.Foreground;
                    sewingMasterView.UpperMatsArrivalBackground = materialArrivalUpper.Background;
                }

                MaterialArrivalViewModel materialArrivalSewing = MaterialArrival(order.ProductNo, materialIdSewingArray);
                sewingMasterView.SewingMatsArrivalOrginal = dtDefault;
                if (materialArrivalSewing != null)
                {
                    sewingMasterView.SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalSewing.Date);
                    sewingMasterView.SewingMatsArrivalOrginal = materialArrivalSewing.Date;
                    sewingMasterView.SewingMatsArrivalForeground = materialArrivalSewing.Foreground;
                    sewingMasterView.SewingMatsArrivalBackground = materialArrivalSewing.Background;
                }

                // Outsole Material Follow Material Input
                //var rawMaterialViewModelNew = rawMaterialViewModelNewList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                //sewingMasterView.OSMatsArrivalForeground = Brushes.Blue;
                //sewingMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                //// Deliveried
                //if (String.IsNullOrEmpty(rawMaterialViewModelNew.OUTSOLE_Remarks) &&
                //    !String.IsNullOrEmpty(rawMaterialViewModelNew.OUTSOLE_ActualDate))
                //    sewingMasterView.OSMatsArrival = rawMaterialViewModelNew.OUTSOLE_ActualDate;
                //else
                //{
                //    sewingMasterView.OSMatsArrivalForeground = Brushes.Black;
                //    sewingMasterView.OSMatsArrival = rawMaterialViewModelNew.OUTSOLE_ETD;

                //    // ETD Late
                //    if (rawMaterialViewModelNew.OUTSOLE_ETD_DATE < DateTime.Now.Date &&
                //        rawMaterialViewModelNew.OUTSOLE_ETD_DATE != dtDefault)
                //        sewingMasterView.OSMatsArrivalBackground = Brushes.Red;

                //    // Still Have Balance
                //    if (!String.IsNullOrEmpty(rawMaterialViewModelNew.OUTSOLE_Remarks))
                //    {
                //        sewingMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                //        if (rawMaterialViewModelNew.OUTSOLE_ETD_DATE < DateTime.Now.Date &&
                //            rawMaterialViewModelNew.OUTSOLE_ETD_DATE != dtDefault)
                //            sewingMasterView.OSMatsArrivalForeground = Brushes.Red;
                //    }
                //}
                var osRawMaterial = outsoleRawMaterialList.Where(w => w.ProductNo == order.ProductNo).ToList();
                var actualDateList = osRawMaterial.Select(s => s.ActualDate).ToList();
                if (actualDateList.Count() > 0 && actualDateList.Contains(dtDefault) == false)
                {
                    sewingMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", actualDateList.Max());
                    sewingMasterView.OSMatsArrivalForeground = Brushes.Blue;
                    sewingMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                }
                else
                {
                    var etdDateList = osRawMaterial.Select(s => s.ETD).ToList();
                    if (etdDateList.Count() > 0)
                    {
                        sewingMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", etdDateList.Max());
                        sewingMasterView.OSMatsArrivalForeground = Brushes.Black;
                        sewingMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                        if (etdDateList.Max() < DateTime.Now.Date)
                        {
                            sewingMasterView.OSMatsArrivalBackground = Brushes.Red;
                        }
                        else
                        {
                            var rawMaterial_PO = rawMaterialList.Where(w => w.ProductNo == order.ProductNo && materialIdOutsoleArray.Contains(w.MaterialTypeId) == true).ToList();
                            if (rawMaterial_PO.Where(w => String.IsNullOrEmpty(w.Remarks) == false).Count() > 0)
                            {
                                sewingMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                            }
                        }
                    }
                }

                MaterialArrivalViewModel materialArrivalAssembly = MaterialArrival(order.ProductNo, materialIdAssemblyArray);
                if (materialArrivalAssembly != null)
                {
                    sewingMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalAssembly.Date);
                    sewingMasterView.AssemblyMatsArrivalForeground = materialArrivalAssembly.Foreground;
                    sewingMasterView.AssemblyMatsArrivalBackground = materialArrivalAssembly.Background;
                }

                SewingMasterModel sewingMaster = sewingMasterList.Where(s => s.ProductNo == order.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {
                    sewingMasterView.Sequence = sewingMaster.Sequence;
                    sewingMasterView.SewingLine = sewingMaster.SewingLine;
                    sewingMasterView.SewingStartDate = sewingMaster.SewingStartDate;
                    sewingMasterView.SewingFinishDate = sewingMaster.SewingFinishDate;
                    sewingMasterView.SewingQuota = sewingMaster.SewingQuota;

                    sewingMasterView.SewingPrep = sewingMaster.SewingPrep;

                    //sewingMasterView.SewingActualStartDate = sewingMaster.SewingActualStartDate;
                    sewingMasterView.SewingActualStartDate = "";
                    if (sewingMaster.SewingActualStartDate_Date != dtDefault)
                    {
                        sewingMasterView.SewingActualStartDate = String.Format("{0:M/d}", sewingMaster.SewingActualStartDate_Date);
                        sewingMasterView.SewingActualStartDate_Date = sewingMaster.SewingActualStartDate_Date;
                    }

                    //sewingMasterView.SewingActualFinishDate = sewingMaster.SewingActualFinishDate;
                    sewingMasterView.SewingActualFinishDate = "";
                    if (sewingMaster.SewingActualFinishDate_Date != dtDefault)
                    {
                        sewingMasterView.SewingActualFinishDate = String.Format("{0:M/d}", sewingMaster.SewingActualFinishDate_Date);
                        sewingMasterView.SewingActualFinishDate_Date = sewingMaster.SewingActualFinishDate_Date;
                    }

                    sewingMasterView.SewingActualStartDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualStartDateAuto);
                    sewingMasterView.SewingActualFinishDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualFinishDateAuto);

                    //sewingMasterView.SewingBalance = sewingMaster.SewingBalance;
                    if (sewingMaster.SewingBalance.Contains("/"))
                    {
                        sewingMasterView.SewingBalance_Date = TimeHelper.Convert(sewingMaster.SewingBalance);
                        sewingMasterView.SewingBalance = String.Format("{0:M/d}", sewingMasterView.SewingBalance_Date);
                    }
                    else
                        sewingMasterView.SewingBalance = sewingMaster.SewingBalance;


                    sewingMasterView.CutAStartDate = sewingMaster.CutAStartDate;
                    sewingMasterView.CutAFinishDate = sewingMaster.CutAFinishDate;
                    sewingMasterView.CutAQuota = sewingMaster.CutAQuota;
                    //sewingMasterView.CutAActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualStartDate);
                    //sewingMasterView.CutAActualFinishDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualFinishDate);

                    //sewingMasterView.CutAActualStartDate = sewingMaster.CutAActualStartDate;
                    sewingMasterView.CutAActualStartDate = "";
                    if (sewingMaster.CutAActualStartDate_Date != dtDefault)
                    {
                        sewingMasterView.CutAActualStartDate = String.Format("{0:M/d}", sewingMaster.CutAActualStartDate_Date);
                        sewingMasterView.CutAActualStartDate_Date = sewingMaster.CutAActualStartDate_Date;
                    }

                    //sewingMasterView.CutAActualFinishDate = sewingMaster.CutAActualFinishDate;
                    sewingMasterView.CutAActualFinishDate = "";
                    if (sewingMaster.CutAActualFinishDate_Date != dtDefault)
                    {
                        sewingMasterView.CutAActualFinishDate = String.Format("{0:M/d}", sewingMaster.CutAActualFinishDate_Date);
                        sewingMasterView.CutAActualFinishDate_Date = sewingMaster.CutAActualFinishDate_Date;
                    }

                    //sewingMasterView.CutBActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutBActualStartDate);
                    sewingMasterView.CutBActualStartDate = sewingMaster.CutBActualStartDate;

                    //sewingMasterView.CutABalance = sewingMaster.CutABalance;
                    if (sewingMaster.CutABalance.Contains("/"))
                    {
                        sewingMasterView.CutABalance_Date = TimeHelper.Convert(sewingMaster.CutABalance);
                        sewingMasterView.CutABalance = String.Format("{0:M/d}", sewingMasterView.CutABalance_Date);
                    }
                    else
                        sewingMasterView.CutABalance = sewingMaster.CutABalance;

                    sewingMasterView.PrintingBalance = sewingMaster.PrintingBalance;
                    sewingMasterView.H_FBalance = sewingMaster.H_FBalance;
                    sewingMasterView.EmbroideryBalance = sewingMaster.EmbroideryBalance;
                    sewingMasterView.CutBBalance = sewingMaster.CutBBalance;

                    sewingMasterView.AutoCut = sewingMaster.AutoCut;
                    sewingMasterView.LaserCut = sewingMaster.LaserCut;
                    sewingMasterView.HuasenCut = sewingMaster.HuasenCut;

                    //var beforeSewingX1Days = sewingMaster.SewingStartDate.AddDays(-15);
                    var beforeSewingOfCutTypeADays = sewingMaster.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                    var dtCheckOffDateCutTypeAList = CheckOffDay(beforeSewingOfCutTypeADays, sewingMaster.SewingStartDate);

                    //var beforeSewingXDays = sewingMaster.SewingStartDate.AddDays(-10);
                    var beforeSewingOfCutTypeB = sewingMaster.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                    var dtCheckOffDateCutTypeBList = CheckOffDay(beforeSewingOfCutTypeB, sewingMaster.SewingStartDate);

                    var firstCheckOffDateCutTypeA = String.Format("{0:M/d}", dtCheckOffDateCutTypeAList.First());
                    var firstCheckOffDateCutTypeB = String.Format("{0:M/d}", dtCheckOffDateCutTypeBList.First());

                    // Cut B
                    if (!String.IsNullOrEmpty(sewingMaster.CutBStartDate))
                        sewingMasterView.CutBStartDate = sewingMaster.CutBStartDate;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.CutBStartDate = firstCheckOffDateCutTypeB;
                    else
                        sewingMasterView.CutBStartDate = "";

                    // Atom CutAB
                    if (!String.IsNullOrEmpty(sewingMaster.AtomCutA))
                        sewingMasterView.AtomCutA = sewingMaster.AtomCutA;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.AtomCutA = firstCheckOffDateCutTypeA;
                    else
                        sewingMasterView.AtomCutA = "";

                    if (!String.IsNullOrEmpty(sewingMaster.AtomCutB))
                        sewingMasterView.AtomCutB = sewingMaster.AtomCutB;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.AtomCutB = firstCheckOffDateCutTypeB;
                    else
                        sewingMasterView.AtomCutB = "";

                    // Laser CutAB
                    if (!String.IsNullOrEmpty(sewingMaster.LaserCutA))
                        sewingMasterView.LaserCutA = sewingMaster.LaserCutA;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.LaserCutA = firstCheckOffDateCutTypeA;
                    else
                        sewingMasterView.LaserCutA = "";

                    if (!String.IsNullOrEmpty(sewingMaster.LaserCutB))
                        sewingMasterView.LaserCutB = sewingMaster.LaserCutB;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.LaserCutB = firstCheckOffDateCutTypeB;
                    else
                        sewingMasterView.LaserCutB = "";

                    // Huasen CutAB
                    if (!String.IsNullOrEmpty(sewingMaster.HuasenCutA))
                        sewingMasterView.HuasenCutA = sewingMaster.HuasenCutA;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.HuasenCutA = firstCheckOffDateCutTypeA;
                    else
                        sewingMasterView.HuasenCutA = "";

                    if (!String.IsNullOrEmpty(sewingMaster.HuasenCutB))
                        sewingMasterView.HuasenCutB = sewingMaster.HuasenCutB;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.HuasenCutB = firstCheckOffDateCutTypeB;
                    else
                        sewingMasterView.HuasenCutB = "";

                    // Comelz CutAB
                    if (!String.IsNullOrEmpty(sewingMaster.ComelzCutA))
                        sewingMasterView.ComelzCutA = sewingMaster.ComelzCutA;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.ComelzCutA = firstCheckOffDateCutTypeA;
                    else
                        sewingMasterView.ComelzCutA = "";

                    if (!String.IsNullOrEmpty(sewingMaster.ComelzCutB))
                        sewingMasterView.ComelzCutB = sewingMaster.ComelzCutB;
                    else if (sewingMaster.SewingStartDate != dtDefault)
                        sewingMasterView.ComelzCutB = firstCheckOffDateCutTypeB;
                    else
                        sewingMasterView.ComelzCutB = "";
                }
                else
                {
                    sewingMasterView.Sequence = 0;
                    sewingMasterView.SewingLine = "";
                    sewingMasterView.SewingStartDate = dtDefault;
                    sewingMasterView.SewingFinishDate = dtDefault;
                    sewingMasterView.SewingQuota = 0;

                    sewingMasterView.SewingPrep = "";

                    sewingMasterView.SewingActualStartDate = "";
                    sewingMasterView.SewingActualFinishDate = "";

                    sewingMasterView.SewingActualStartDateAuto = "";
                    sewingMasterView.SewingActualFinishDateAuto = "";

                    sewingMasterView.SewingBalance = "";
                    sewingMasterView.CutAStartDate = dtDefault;
                    sewingMasterView.CutAFinishDate = dtDefault;
                    sewingMasterView.CutAQuota = 0;
                    sewingMasterView.CutAActualStartDate = "";
                    sewingMasterView.CutAActualFinishDate = "";
                    sewingMasterView.CutABalance = "";
                    sewingMasterView.PrintingBalance = "";
                    sewingMasterView.H_FBalance = "";
                    sewingMasterView.EmbroideryBalance = "";
                    sewingMasterView.CutBActualStartDate = "";
                    sewingMasterView.CutBBalance = "";
                    sewingMasterView.AutoCut = "";
                    sewingMasterView.LaserCut = "";
                    sewingMasterView.HuasenCut = "";

                    sewingMasterView.CutBStartDate = "";
                    sewingMasterView.AtomCutA = "";
                    sewingMasterView.AtomCutB = "";
                    sewingMasterView.LaserCutA = "";
                    sewingMasterView.LaserCutB = "";
                    sewingMasterView.HuasenCutA = "";
                    sewingMasterView.HuasenCutB = "";
                    sewingMasterView.ComelzCutA = "";
                    sewingMasterView.ComelzCutB = "";
                }

                OutsoleMasterModel outsoleMaster = outsoleMasterList.Where(o => o.ProductNo == order.ProductNo).FirstOrDefault();
                if (outsoleMaster != null)
                {
                    sewingMasterView.OSFinishDate = outsoleMaster.OutsoleFinishDate;
                    if (outsoleMaster.OutsoleBalance.Contains("/"))
                    {
                        sewingMasterView.OSBalance = String.Format("{0:M/d}", TimeHelper.Convert(outsoleMaster.OutsoleBalance));
                    }
                    else
                        sewingMasterView.OSBalance = outsoleMaster.OutsoleBalance;
                }
                else
                {
                    sewingMasterView.OSFinishDate = dtDefault;
                    sewingMasterView.OSBalance = "";
                }

                sewingMasterView.SewingStartDateForeground = Brushes.Black;
                sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                sewingMasterView.CutAStartDateForeground = Brushes.Black;

                // addition code: Orange is: sewing startdate start after uppermaterial come in 5 days
                //int rangeSewing = TimeHelper.CalculateDate(sewingMasterView.UpperMatsArrivalOrginal, sewingMasterView.SewingStartDate);
                int rangeSewing = (Int32)((sewingMasterView.SewingStartDate - sewingMasterView.UpperMatsArrivalOrginal).TotalDays);
                //if (0 <= rangeSewing && rangeSewing <= 5)
                if (sewingMasterView.UpperMatsArrivalOrginal <= sewingMasterView.SewingStartDate && sewingMasterView.SewingStartDate <= sewingMasterView.UpperMatsArrivalOrginal.AddDays(5))
                    sewingMasterView.SewingStartDateForeground = Brushes.Orange;

                if (sewingMasterView.SewingStartDate < new DateTime(Math.Max(sewingMasterView.UpperMatsArrivalOrginal.Ticks, sewingMasterView.SewingMatsArrivalOrginal.Ticks)))
                    sewingMasterView.SewingStartDateForeground = Brushes.Red;

                if (sewingMasterView.SewingFinishDate > sewingMasterView.ETD)
                    sewingMasterView.SewingFinishDateForeground = Brushes.Red;

                if (sewingMasterView.CutAStartDate < sewingMasterView.UpperMatsArrivalOrginal)
                    sewingMasterView.CutAStartDateForeground = Brushes.Red;
                else
                {
                    if (materialArrivalUpper != null)
                    {
                        if (sewingMasterView.CutAStartDateForeground == Brushes.Orange)
                            sewingMasterView.CutAStartDateForeground = Brushes.Orange;
                        else
                            sewingMasterView.CutAStartDateForeground = materialArrivalUpper.Foreground;
                    }
                    // addtion code : Orange is: cut a startdate start after uppermaterial come in 3 days
                    //if (0 <= rangeCutA && rangeCutA <= 3)
                    //int rangeCutA = TimeHelper.CalculateDate(sewingMasterView.UpperMatsArrivalOrginal, sewingMasterView.CutAStartDate);
                    int rangeCutA = (Int32)((sewingMasterView.CutAStartDate - sewingMasterView.UpperMatsArrivalOrginal).TotalDays);
                    //if (sewingMasterView.UpperMatsArrivalOrginal <= sewingMasterView.CutAStartDate && sewingMasterView.CutAStartDate <= sewingMasterView.UpperMatsArrivalOrginal.AddDays(3))
                    if (0 <= rangeCutA && rangeCutA <= 3)
                        sewingMasterView.CutAStartDateForeground = Brushes.Orange;
                }
                sewingMasterViewList.Add(sewingMasterView);
            }
            sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
        }

        private void bwLoad_DoWork_1(object sender, DoWorkEventArgs e)
        {
            offDayList = OffDayController.Select();
            productionMemoList = ProductionMemoController.Select();
            sewingMasterViewModelNewList = SewingMasterController.SelectViewModel();
            int index = 1;
            foreach (var sewNew in sewingMasterViewModelNewList)
            {
                var sewingMasterView = new SewingMasterViewModel();
                // Order Information
                sewingMasterView.ProductNoBackground = Brushes.Transparent;
                sewingMasterView.ProductNo = sewNew.ProductNo;
                sewingMasterView.Country = sewNew.Country;
                sewingMasterView.ShoeName = sewNew.ShoeName;
                sewingMasterView.ArticleNo = sewNew.ArticleNo;
                sewingMasterView.PatternNo = sewNew.PatternNo;
                sewingMasterView.Quantity = sewNew.Quantity;
                sewingMasterView.ETD = sewNew.ETD;

                var memoByPOList = productionMemoList.Where(p => p.ProductionNumbers.Contains(sewNew.ProductNo) == true).Select(s => s.MemoId).ToList();
                sewingMasterView.MemoId = memoByPOList.Count() > 0 ? String.Join("\n", memoByPOList) : "";

                // Sewing Master Information
                sewingMasterView.Sequence = sewNew.Sequence;
                sewingMasterView.SewingLine = sewNew.SewingLine;
                sewingMasterView.SewingQuota = sewNew.SewingQuota;
                sewingMasterView.SewingStartDate = sewNew.SewingStartDate;
                sewingMasterView.SewingFinishDate = sewNew.SewingFinishDate;
                sewingMasterView.SewingPrep = sewNew.SewingPrep;
                sewingMasterView.SewingActualStartDate = sewNew.SewingActualStartDate;
                sewingMasterView.SewingActualFinishDate = sewNew.SewingActualFinishDate;
                sewingMasterView.SewingActualStartDateAuto = sewNew.SewingActualStartDateAuto;
                sewingMasterView.SewingActualFinishDateAuto = sewNew.SewingActualFinishDateAuto;
                sewingMasterView.SewingBalance = sewNew.SewingBalance;
                sewingMasterView.CutAStartDate = sewNew.CutAStartDate;
                sewingMasterView.CutAFinishDate = sewNew.CutAFinishDate;
                sewingMasterView.CutAQuota = sewNew.CutAQuota;
                sewingMasterView.CutAActualStartDate = sewNew.CutAActualStartDate;
                sewingMasterView.CutAActualFinishDate = sewNew.CutAActualFinishDate;
                sewingMasterView.PrintingBalance = sewNew.PrintingBalance;
                sewingMasterView.H_FBalance = sewNew.H_FBalance;
                sewingMasterView.EmbroideryBalance = sewNew.EmbroideryBalance;
                sewingMasterView.CutBBalance = sewNew.CutBBalance;
                sewingMasterView.AutoCut = sewNew.AutoCut;
                sewingMasterView.LaserCut = sewNew.LaserCut;
                sewingMasterView.HuasenCut = sewNew.HuasenCut;

                // Outsole Master Infor
                sewingMasterView.OSFinishDate = sewNew.OSFinishDate;
                sewingMasterView.OSBalance = sewNew.OSBalance;

                sewingMasterView.SewingStartDateForeground = Brushes.Black;
                sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                sewingMasterView.CutAStartDateForeground = Brushes.Black;


                var bfSewCutTypeB = sewNew.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                var bfSewCutTypeBHolidayList = CheckOffDay(bfSewCutTypeB, sewNew.SewingStartDate);

                var bfSewCutTypeA = sewNew.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                var bfSewCutTypeAHolidayList = CheckOffDay(bfSewCutTypeA, sewNew.SewingStartDate);

                var firstDateOfHolidayCutTypeB = String.Format("{0:M/d}", bfSewCutTypeBHolidayList.FirstOrDefault());
                var firstDateOfHolidayCutTypeA = String.Format("{0:M/d}", bfSewCutTypeAHolidayList.FirstOrDefault());

                if (!String.IsNullOrEmpty(sewNew.CutBStartDate))
                    sewingMasterView.CutBStartDate = sewNew.CutBStartDate;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.CutBStartDate = firstDateOfHolidayCutTypeB;
                else
                    sewingMasterView.CutBStartDate = "";


                if (!String.IsNullOrEmpty(sewNew.AtomCutA))
                    sewingMasterView.AtomCutA = sewNew.AtomCutA;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.AtomCutA = firstDateOfHolidayCutTypeA;
                else
                    sewingMasterView.AtomCutA = "";

                if (!String.IsNullOrEmpty(sewNew.AtomCutB))
                    sewingMasterView.AtomCutB = sewNew.AtomCutB;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.AtomCutB = firstDateOfHolidayCutTypeB;
                else
                    sewingMasterView.AtomCutB = "";

                if (!String.IsNullOrEmpty(sewNew.LaserCutA))
                    sewingMasterView.LaserCutA = sewNew.LaserCutA;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.LaserCutA = firstDateOfHolidayCutTypeA;
                else
                    sewingMasterView.LaserCutA = "";

                if (!String.IsNullOrEmpty(sewNew.LaserCutB))
                    sewingMasterView.LaserCutB = sewNew.LaserCutB;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.LaserCutB = firstDateOfHolidayCutTypeB;
                else
                    sewingMasterView.LaserCutB = "";

                if (!String.IsNullOrEmpty(sewNew.HuasenCutA))
                    sewingMasterView.HuasenCutA = sewNew.HuasenCutA;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.HuasenCutA = firstDateOfHolidayCutTypeA;
                else
                    sewingMasterView.HuasenCutA = "";

                if (!String.IsNullOrEmpty(sewNew.HuasenCutB))
                    sewingMasterView.HuasenCutB = sewNew.HuasenCutB;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.HuasenCutB = firstDateOfHolidayCutTypeB;
                else
                    sewingMasterView.HuasenCutB = "";

                if (!String.IsNullOrEmpty(sewNew.ComelzCutA))
                    sewingMasterView.ComelzCutA = sewNew.ComelzCutA;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.ComelzCutA = firstDateOfHolidayCutTypeA;
                else sewingMasterView.ComelzCutA = "";

                if (!String.IsNullOrEmpty(sewNew.ComelzCutB))
                    sewingMasterView.ComelzCutB = sewNew.ComelzCutB;
                else if (sewNew.SewingStartDate != dtDefault)
                    sewingMasterView.ComelzCutB = firstDateOfHolidayCutTypeB;
                else sewingMasterView.ComelzCutB = "";

                // UpperMaterial 1, 2, 3, 4, 10
                sewingMasterView.UpperMatsArrivalForeground = Brushes.Black;
                sewingMasterView.UpperMatsArrivalBackground = Brushes.Transparent;
                if (string.IsNullOrEmpty(sewNew.UpperMaterialRemarks) == false && sewNew.UpperMaterialETD != dtDefault)
                    sewingMasterView.UpperMatsArrivalBackground = Brushes.Yellow;

                if (sewNew.UpperMaterialFinisedDate != dtDefault)
                {
                    sewingMasterView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.UpperMaterialFinisedDate);
                    sewingMasterView.UpperMatsArrivalForeground = Brushes.Blue;
                }
                else if (sewNew.UpperMaterialETD != dtDefault)
                {
                    sewingMasterView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.UpperMaterialETD);
                    if (sewNew.UpperMaterialETD.Date < DateTime.Now.Date)
                        sewingMasterView.UpperMatsArrivalBackground = Brushes.Red;
                }

                //Sewing Material 5, 7
                sewingMasterView.SewingMatsArrivalForeground = Brushes.Black;
                sewingMasterView.SewingMatsArrivalBackground = Brushes.Transparent;
                if (string.IsNullOrEmpty(sewNew.SewingMaterialRemarks) == false && sewNew.SewingMaterialETD != dtDefault)
                    sewingMasterView.SewingMatsArrivalBackground = Brushes.Yellow;

                if (sewNew.SewingMaterialFinisedDate != dtDefault)
                {
                    sewingMasterView.SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.SewingMaterialFinisedDate);
                    sewingMasterView.SewingMatsArrivalForeground = Brushes.Blue;
                }
                else if (sewNew.SewingMaterialETD != dtDefault)
                {
                    sewingMasterView.SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.SewingMaterialETD);
                    if (sewNew.SewingMaterialETD.Date < DateTime.Now.Date)
                        sewingMasterView.SewingMatsArrivalBackground = Brushes.Red;
                }

                // Assembly Material 8, 9
                sewingMasterView.AssemblyMatsArrivalForeground = Brushes.Black;
                sewingMasterView.AssemblyMatsArrivalBackground = Brushes.Transparent;
                if (string.IsNullOrEmpty(sewNew.AssemblyMaterialRemarks) == false && sewNew.AssemblyMaterialETD != dtDefault)
                    sewingMasterView.AssemblyMatsArrivalBackground = Brushes.Yellow;

                if (sewNew.AssemblyMaterialFinisedDate != dtDefault)
                {
                    sewingMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.AssemblyMaterialFinisedDate);
                    sewingMasterView.AssemblyMatsArrivalForeground = Brushes.Blue;
                }
                else if (sewNew.AssemblyMaterialETD != dtDefault)
                {
                    sewingMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.AssemblyMaterialETD);
                    if (sewNew.AssemblyMaterialETD.Date < DateTime.Now.Date)
                        sewingMasterView.AssemblyMatsArrivalBackground = Brushes.Red;
                }

                // Outsole Material 6
                sewingMasterView.OSMatsArrivalForeground = Brushes.Black;
                sewingMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                if (string.IsNullOrEmpty(sewNew.OutsoleMaterialRemarks) == false && sewNew.OutsoleMaterialETD != dtDefault)
                    sewingMasterView.OSMatsArrivalBackground = Brushes.Yellow;

                if (sewNew.OutsoleMaterialFinisedDate != dtDefault)
                {
                    sewingMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.OutsoleMaterialFinisedDate);
                    sewingMasterView.OSMatsArrivalForeground = Brushes.Blue;
                }
                else if (sewNew.OutsoleMaterialETD != dtDefault)
                {
                    sewingMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.OutsoleMaterialETD);
                    if (sewNew.OutsoleMaterialETD.Date < DateTime.Now.Date)
                        sewingMasterView.OSMatsArrivalBackground = Brushes.Red;
                }

                // Highlight Sewing Start Date
                if (sewNew.SewingStartDate != dtDefault)
                {
                    var sewStartAndUpperArrival = (sewNew.SewingStartDate - sewNew.UpperMaterialFinisedDate).TotalDays;
                    if (0 <= sewStartAndUpperArrival && sewStartAndUpperArrival <= 5)
                        sewingMasterView.SewingStartDateForeground = Brushes.Orange;
                    if (sewNew.SewingStartDate < new DateTime(Math.Max(sewNew.UpperMaterialFinisedDate.Ticks, sewNew.SewingMaterialFinisedDate.Ticks)))
                        sewingMasterView.SewingStartDateForeground = Brushes.Red;
                }
                if (sewNew.SewingFinishDate > sewNew.ETD)
                    sewingMasterView.SewingFinishDateForeground = Brushes.Red;

                // Highlight CutA Start Date
                if (sewNew.CutAStartDate != dtDefault)
                {
                    if (sewNew.CutAStartDate < sewNew.UpperMaterialFinisedDate)
                        sewingMasterView.CutAStartDateForeground = Brushes.Red;
                    else
                        sewingMasterView.CutAStartDateForeground = sewingMasterView.UpperMatsArrivalForeground;

                    var cutAStartAndUpperArrival = (sewNew.CutAStartDate - sewNew.UpperMaterialFinisedDate).TotalDays;
                    if (0 <= cutAStartAndUpperArrival && cutAStartAndUpperArrival <= 5)
                        sewingMasterView.CutAStartDateForeground = Brushes.Orange;
                }

                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = String.Format("Creating {0}/{1} rows ...", index, sewingMasterViewModelNewList.Count());
                }));
                sewingMasterViewList.Add(sewingMasterView);
                index++;
            }
            sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
        }

        private void bwLoad_DoWork_2(object sender, DoWorkEventArgs e)
        {
            try
            {
                def = PrivateDefineController.GetDefine();
                var productNoListWithAccount = OrdersController.Select().Where(w => account.TypeOfShoes != -1 ? w.TypeOfShoes == account.TypeOfShoes : w.TypeOfShoes != -16111992).Select(s => s.ProductNo).ToList();
                sewingMasterList = SewingMasterController.Select().Where(w => productNoListWithAccount.Contains(w.ProductNo)).ToList();
                foreach (var sm in sewingMasterList)
                {
                    poSequenceSourceList.Add(new POSequenceModel
                    {
                        ProductNo = sm.ProductNo,
                        Sequence = sm.Sequence,
                        Id = string.Format("{0}-{1}", sm.ProductNo, sm.Sequence)
                    });
                }
                if (!string.IsNullOrEmpty(def.Factory) && !def.Factory.Equals("THIENLOC"))
                { 
                    offDayList = OffDayController.Select();
                    sewingMasterSourceList = SewingMasterController.SelectSewingMasterSource_1().Where(w => productNoListWithAccount.Contains(w.ProductNo)).ToList();
                    productionMemoList = ProductionMemoController.Select().Where(w => productNoListWithAccount.Contains(w.ProductionNumbers)).ToList();
                    def = PrivateDefineController.GetDefine();

                    _SEW_VS_CUTA = def.SewingVsCutAStartDate;
                    _SEW_VS_OTHERS_CUT_A = def.SewingVsOthersCutTypeA;
                    _SEW_VS_OTHERS_CUT_B = def.SewingVsOthersCutTypeB;

                    Dispatcher.Invoke(new Action(() =>
                    {
                        prgStatus.Maximum = sewingMasterSourceList.Count();
                    }));
                    int index = 1;
                    foreach (var sewSource in sewingMasterSourceList)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Text = String.Format("Loading {0} / {1} PO", index, sewingMasterSourceList.Count());
                            prgStatus.Value = index;
                        }));
                        string memoId = "";
                        List<ProductionMemoModel> productionMemoByProductionNumberList = productionMemoList.Where(p => p.ProductionNumbers.Contains(sewSource.ProductNo) == true).ToList();
                        for (int p = 0; p <= productionMemoByProductionNumberList.Count - 1; p++)
                        {
                            ProductionMemoModel productionMemo = productionMemoByProductionNumberList[p];
                            memoId += productionMemo.MemoId;
                            if (p < productionMemoByProductionNumberList.Count - 1)
                                memoId += "\n";
                        }
                        var sewingMasterView = new SewingMasterViewModel
                        {
                            ProductNo = sewSource.ProductNo,
                            ProductNoBackground = Brushes.Transparent,
                            Country = sewSource.Country,
                            ShoeName = sewSource.ShoeName,
                            ArticleNo = sewSource.ArticleNo,
                            PatternNo = sewSource.PatternNo,
                            Quantity = sewSource.Quantity,
                            ETD = sewSource.ETD,
                            ShipDateDisplay = sewSource.ShipDate != dtDefault && sewSource.ShipDate != sewSource.ETD ? TimeHelper.DisplayDate(sewSource.ShipDate, 0) : "",
                            MemoId = memoId,
                            Sequence = sewSource.Sequence,
                            SewingLine = sewSource.SewingLine,
                            SewingStartDate = sewSource.SewingStartDate,
                            SewingFinishDate = sewSource.SewingFinishDate,
                            SewingQuota = sewSource.SewingQuota,
                            SewingPrep = sewSource.SewingPrep,

                            CutAStartDate = sewSource.CutAStartDate,
                            CutAFinishDate = sewSource.CutAFinishDate,
                            CutAQuota = sewSource.CutAQuota,

                            CutBActualStartDate = sewSource.CutBActualStartDate,

                            PrintingBalance = sewSource.PrintingBalance,
                            H_FBalance = sewSource.H_FBalance,
                            EmbroideryBalance = sewSource.EmbroideryBalance,
                            CutBBalance = sewSource.CutBBalance,

                            AutoCut = sewSource.AutoCut,
                            LaserCut = sewSource.LaserCut,
                            HuasenCut = sewSource.HuasenCut
                        };

                        sewingMasterView.SewingActualStartDate_Date = dtDefault;
                        sewingMasterView.SewingActualStartDate = "";
                        if (sewSource.SewingActualStartDate_Date != dtDefault)
                        {
                            sewingMasterView.SewingActualStartDate_Date = sewSource.SewingActualStartDate_Date;
                            sewingMasterView.SewingActualStartDate = TimeHelper.DisplayDate(sewSource.SewingActualStartDate_Date, 1);
                        }

                        sewingMasterView.SewingActualFinishDate = "";
                        sewingMasterView.SewingActualFinishDate_Date = dtDefault;
                        if (sewSource.SewingActualFinishDate_Date != dtDefault)
                        {
                            sewingMasterView.SewingActualFinishDate = TimeHelper.DisplayDate(sewSource.SewingActualFinishDate_Date, 1);
                            sewingMasterView.SewingActualFinishDate_Date = sewSource.SewingActualFinishDate_Date;
                        }

                        sewingMasterView.SewingActualStartDateAuto = TimeHelper.ConvertDateToView(sewSource.SewingActualStartDateAuto);
                        sewingMasterView.SewingActualFinishDateAuto = TimeHelper.ConvertDateToView(sewSource.SewingActualFinishDateAuto);

                        if (sewSource.SewingBalance.Contains("/"))
                        {
                            sewingMasterView.SewingBalance_Date = TimeHelper.Convert(sewSource.SewingBalance);
                            sewingMasterView.SewingBalance = TimeHelper.DisplayDate(sewingMasterView.SewingBalance_Date, 1);
                        }
                        else
                            sewingMasterView.SewingBalance = sewSource.SewingBalance;


                        sewingMasterView.CutAActualStartDate_Date = dtDefault;
                        sewingMasterView.CutAActualStartDate = "";
                        if (sewSource.CutAActualStartDate_Date != dtDefault)
                        {
                            sewingMasterView.CutAActualStartDate_Date = sewSource.CutAActualStartDate_Date;
                            sewingMasterView.CutAActualStartDate = TimeHelper.DisplayDate(sewSource.CutAActualStartDate_Date, 1);
                        }


                        sewingMasterView.CutAActualFinishDate_Date = dtDefault;
                        sewingMasterView.CutAActualFinishDate = "";
                        if (sewSource.CutAActualFinishDate_Date != dtDefault)
                        {
                            sewingMasterView.CutAActualFinishDate_Date = sewSource.CutAActualFinishDate_Date;
                            sewingMasterView.CutAActualFinishDate = TimeHelper.DisplayDate(sewSource.CutAActualFinishDate_Date, 1);
                        }

                        if (sewSource.CutABalance.Contains("/"))
                        {
                            sewingMasterView.CutABalance_Date = TimeHelper.Convert(sewSource.CutABalance);
                            sewingMasterView.CutABalance = TimeHelper.DisplayDate(sewingMasterView.CutABalance_Date, 1);
                        }
                        else
                            sewingMasterView.CutABalance = sewSource.CutABalance;

                        sewingMasterView.OSFinishDate = sewSource.OutsoleFinishDate;
                        if (sewSource.OutsoleBalance.Contains("/"))
                            sewingMasterView.OSBalance = TimeHelper.DisplayDate(TimeHelper.Convert(sewSource.OutsoleBalance), 1);
                        else
                            sewingMasterView.OSBalance = sewSource.OutsoleBalance;


                        // UPPER
                        if (!String.IsNullOrEmpty(sewSource.UpperMaterialStatus))
                        {
                            sewingMasterView.UpperMatsArrival = TimeHelper.DisplayDate(sewSource.UpperMaterialArrivalOriginal, 0);
                            sewingMasterView.UpperMatsArrivalOrginal = sewSource.UpperMaterialArrivalOriginal;

                            sewingMasterView.UpperMatsArrivalForeground = Brushes.Black;
                            sewingMasterView.UpperMatsArrivalBackground = Brushes.Transparent;
                            if (sewSource.UpperMaterialStatus == "OK")
                            {
                                sewingMasterView.UpperMatsArrivalForeground = Brushes.Blue;
                            }
                            else
                            {
                                if (sewSource.UpperMaterialArrivalOriginal < DateTime.Now)
                                    sewingMasterView.UpperMatsArrivalBackground = Brushes.Red;
                            }
                            if (!String.IsNullOrEmpty(sewSource.UpperMaterialRemarks))
                                sewingMasterView.UpperMatsArrivalBackground = Brushes.Yellow;
                        }

                        // OUTSOLE
                        if (!String.IsNullOrEmpty(sewSource.OutsoleMaterialStatus))
                        {
                            sewingMasterView.OSMatsArrival = TimeHelper.DisplayDate(sewSource.OutsoleMaterialArrivalOriginal, 0);

                            sewingMasterView.OSMatsArrivalForeground = Brushes.Black;
                            sewingMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                            if (sewSource.OutsoleMaterialStatus == "OK")
                            {
                                sewingMasterView.OSMatsArrivalForeground = Brushes.Blue;
                            }
                            else
                            {
                                if (sewSource.OutsoleMaterialArrivalOriginal < DateTime.Now)
                                    sewingMasterView.OSMatsArrivalBackground = Brushes.Red;
                            }
                            if (!String.IsNullOrEmpty(sewSource.OutsoleMaterialRemarks))
                                sewingMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                        }

                        // SEWING
                        if (!String.IsNullOrEmpty(sewSource.SewingMaterialStatus))
                        {
                            sewingMasterView.SewingMatsArrival = TimeHelper.DisplayDate(sewSource.SewingMaterialArrivalOriginal, 0);
                            sewingMasterView.SewingMatsArrivalOrginal = sewSource.SewingMaterialArrivalOriginal;

                            sewingMasterView.SewingMatsArrivalForeground = Brushes.Black;
                            sewingMasterView.SewingMatsArrivalBackground = Brushes.Transparent;
                            if (sewSource.SewingMaterialStatus == "OK")
                            {
                                sewingMasterView.SewingMatsArrivalForeground = Brushes.Blue;
                            }
                            else
                            {
                                if (sewSource.SewingMaterialArrivalOriginal < DateTime.Now)
                                    sewingMasterView.SewingMatsArrivalBackground = Brushes.Red;
                            }
                            if (!String.IsNullOrEmpty(sewSource.SewingMaterialRemarks))
                                sewingMasterView.SewingMatsArrivalBackground = Brushes.Yellow;
                        }

                        // ASSY
                        if (!String.IsNullOrEmpty(sewSource.AssemblyMaterialStatus))
                        {
                            sewingMasterView.AssemblyMatsArrival = TimeHelper.DisplayDate(sewSource.AssemblyMaterialArrivalOriginal, 0);
                            sewingMasterView.AssemblyMatsArrivalForeground = Brushes.Black;
                            sewingMasterView.AssemblyMatsArrivalBackground = Brushes.Transparent;
                            if (sewSource.AssemblyMaterialStatus == "OK")
                            {
                                sewingMasterView.AssemblyMatsArrivalForeground = Brushes.Blue;
                            }
                            else
                            {
                                if (sewSource.AssemblyMaterialArrivalOriginal < DateTime.Now)
                                    sewingMasterView.AssemblyMatsArrivalBackground = Brushes.Red;
                            }
                            if (!String.IsNullOrEmpty(sewSource.AssemblyMaterialRemarks))
                                sewingMasterView.AssemblyMatsArrivalBackground = Brushes.Yellow;
                        }

                        sewingMasterView.SewingStartDateForeground = Brushes.Black;
                        sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                        sewingMasterView.CutAStartDateForeground = Brushes.Black;

                        if (sewingMasterView.UpperMatsArrivalOrginal <= sewingMasterView.SewingStartDate &&
                            sewingMasterView.SewingStartDate <= sewingMasterView.UpperMatsArrivalOrginal.AddDays(5))
                            sewingMasterView.SewingStartDateForeground = Brushes.Orange;

                        if (sewingMasterView.SewingStartDate < new DateTime(Math.Max(sewingMasterView.UpperMatsArrivalOrginal.Ticks, sewingMasterView.SewingMatsArrivalOrginal.Ticks))
                            && sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.SewingStartDateForeground = Brushes.Red;

                        if (sewingMasterView.SewingFinishDate > sewingMasterView.ETD)
                            sewingMasterView.SewingFinishDateForeground = Brushes.Red;

                        int rangeCutA = (Int32)((sewingMasterView.CutAStartDate - sewingMasterView.UpperMatsArrivalOrginal).TotalDays);
                        if (sewingMasterView.CutAStartDate < sewingMasterView.UpperMatsArrivalOrginal)
                            sewingMasterView.CutAStartDateForeground = Brushes.Red;
                        else if (0 <= rangeCutA && rangeCutA <= 3)
                        {
                            sewingMasterView.CutAStartDateForeground = Brushes.Orange;
                        }

                        var bfSewCutTypeB = sewSource.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                        var bfSewCutTypeBHolidayList = CheckOffDay(bfSewCutTypeB, sewSource.SewingStartDate);

                        var bfSewCutTypeA = sewSource.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                        var bfSewCutTypeAHolidayList = CheckOffDay(bfSewCutTypeA, sewSource.SewingStartDate);

                        var firstDateOfHolidayCutTypeB = TimeHelper.DisplayDate(bfSewCutTypeBHolidayList.FirstOrDefault(), 1);
                        var firstDateOfHolidayCutTypeA = TimeHelper.DisplayDate(bfSewCutTypeAHolidayList.FirstOrDefault(), 1);

                        if (!String.IsNullOrEmpty(sewSource.CutBStartDate))
                            sewingMasterView.CutBStartDate = sewSource.CutBStartDate;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.CutBStartDate = firstDateOfHolidayCutTypeB;
                        else
                            sewingMasterView.CutBStartDate = "";


                        if (!String.IsNullOrEmpty(sewSource.AtomCutA))
                            sewingMasterView.AtomCutA = sewSource.AtomCutA;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.AtomCutA = firstDateOfHolidayCutTypeA;
                        else
                            sewingMasterView.AtomCutA = "";

                        if (!String.IsNullOrEmpty(sewSource.AtomCutB))
                            sewingMasterView.AtomCutB = sewSource.AtomCutB;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.AtomCutB = firstDateOfHolidayCutTypeB;
                        else
                            sewingMasterView.AtomCutB = "";

                        if (!String.IsNullOrEmpty(sewSource.LaserCutA))
                            sewingMasterView.LaserCutA = sewSource.LaserCutA;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.LaserCutA = firstDateOfHolidayCutTypeA;
                        else
                            sewingMasterView.LaserCutA = "";

                        if (!String.IsNullOrEmpty(sewSource.LaserCutB))
                            sewingMasterView.LaserCutB = sewSource.LaserCutB;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.LaserCutB = firstDateOfHolidayCutTypeB;
                        else
                            sewingMasterView.LaserCutB = "";

                        if (!String.IsNullOrEmpty(sewSource.HuasenCutA))
                            sewingMasterView.HuasenCutA = sewSource.HuasenCutA;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.HuasenCutA = firstDateOfHolidayCutTypeA;
                        else
                            sewingMasterView.HuasenCutA = "";

                        if (!String.IsNullOrEmpty(sewSource.HuasenCutB))
                            sewingMasterView.HuasenCutB = sewSource.HuasenCutB;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.HuasenCutB = firstDateOfHolidayCutTypeB;
                        else
                            sewingMasterView.HuasenCutB = "";

                        if (!String.IsNullOrEmpty(sewSource.ComelzCutA))
                            sewingMasterView.ComelzCutA = sewSource.ComelzCutA;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.ComelzCutA = firstDateOfHolidayCutTypeA;
                        else sewingMasterView.ComelzCutA = "";

                        if (!String.IsNullOrEmpty(sewSource.ComelzCutB))
                            sewingMasterView.ComelzCutB = sewSource.ComelzCutB;
                        else if (sewSource.SewingStartDate != dtDefault)
                            sewingMasterView.ComelzCutB = firstDateOfHolidayCutTypeB;
                        else sewingMasterView.ComelzCutB = "";

                        sewingMasterViewList.Add(sewingMasterView);

                        index++;
                    }

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
            }
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            var rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            //foreach (var materialId in materialIdArray)
            //{
            //    var rawMaterialNA = rawMaterialTypeList.Where(w => w.Remarks.Contains("N/A") || w.Remarks.Contains("n/a")).FirstOrDefault();
            //    if (rawMaterialNA != null)
            //        rawMaterialTypeList.Remove(rawMaterialNA);
            //}

            var materialArrivalView = new MaterialArrivalViewModel();
            // Blue is: actual date
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
                        materialArrivalView.Background = Brushes.Red;
                    else
                    {
                        if (rawMaterialTypeList.Where(r => String.IsNullOrEmpty(r.Remarks) == false).Count() > 0)
                            materialArrivalView.Background = Brushes.Yellow;
                    }
                }
                else
                    materialArrivalView = null;
            }

            //// Yellow is:
            //if (materialArrivalView != null && (rawMaterialTypeList.Where(w => w.Remarks != "" && w.Remarks != " ").Count() > 0))
            if (materialArrivalView != null && (rawMaterialTypeList.Where(w => String.IsNullOrEmpty(w.Remarks) == false
                                                                        && (w.Remarks != "TL" || w.Remarks != "DL")
                                                                        && w.ETD == dtDefault
                                                                        && w.ActualDate == dtDefault).Count() > 0))
            {
                materialArrivalView.Foreground = Brushes.Black;
                materialArrivalView.Background = Brushes.Yellow;
            }

            return materialArrivalView;
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList);
            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
            btnCaculate.IsEnabled = true;
            btnSave.IsEnabled = true;
            lblStatus.Text = "";
            prgStatus.Visibility = Visibility.Collapsed;
            btnRefresh.IsEnabled = true;
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
            sewingMasterList = SewingMasterController.Select();
        }

        private void bwReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var currentYear = DateTime.Now.Year;
            //Load Newest Data
            foreach (SewingMasterViewModel sewingMasterView in sewingMasterViewFindList)
            {
                SewingMasterModel sewingMaster = sewingMasterList.Where(s => s.ProductNo == sewingMasterView.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {
                    string productNo = sewingMaster.ProductNo;
                    if (isSequenceEditing == false)
                    {
                        sewingMasterView.Sequence = sewingMaster.Sequence;
                    }
                    if (sewingLineUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingLine = sewingMaster.SewingLine;
                    }
                    if (sewingQuotaUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingQuota = sewingMaster.SewingQuota;
                    }

                    if (sewingPrepUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingPrep = sewingMaster.SewingPrep;
                    }

                    if (sewingActualStartDateUpdateAutoList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualStartDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualStartDateAuto);
                    }
                    if (sewingActualFinishDateUpdateAutoList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualFinishDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualFinishDateAuto);
                    }

                    if (sewingActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualStartDate = "";
                        if (sewingMaster.SewingActualStartDate_Date != dtDefault)
                        {
                            sewingMasterView.SewingActualStartDate = String.Format("{0:M/d}", sewingMaster.SewingActualStartDate_Date);
                            sewingMasterView.SewingActualStartDate_Date = sewingMaster.SewingActualStartDate_Date;
                        }
                    }
                    if (sewingActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        //sewingMasterView.SewingActualFinishDate = sewingMaster.SewingActualFinishDate;
                        sewingMasterView.SewingActualFinishDate = "";
                        if (sewingMaster.SewingActualFinishDate_Date != dtDefault)
                        {
                            sewingMasterView.SewingActualFinishDate = String.Format("{0:M/d}", sewingMaster.SewingActualFinishDate_Date);
                            sewingMasterView.SewingActualFinishDate_Date = sewingMaster.SewingActualFinishDate_Date;
                        }
                    }

                    if (sewingBalanceUpdateList.Contains(productNo) == false)
                    {
                        //sewingMasterView.SewingBalance = sewingMaster.SewingBalance;
                        if (sewingMaster.SewingBalance.Contains("/"))
                        {
                            sewingMasterView.SewingBalance_Date = TimeHelper.Convert(sewingMaster.SewingBalance);
                            sewingMasterView.SewingBalance = String.Format("{0:M/d}", sewingMasterView.SewingBalance_Date);
                        }
                        else
                            sewingMasterView.SewingBalance = sewingMaster.SewingBalance;
                    }

                    if (cutAQuotaUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutAQuota = sewingMaster.CutAQuota;
                    }
                    if (cutAActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        //sewingMasterView.CutAActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualStartDate);
                        //sewingMasterView.CutAActualStartDate = sewingMaster.CutAActualStartDate;
                        sewingMasterView.CutAActualStartDate = "";
                        if (sewingMaster.CutAActualStartDate_Date != dtDefault)
                        {
                            sewingMasterView.CutAActualStartDate = String.Format("{0:M/d}", sewingMaster.CutAActualStartDate_Date);
                            sewingMasterView.CutAActualStartDate_Date = sewingMaster.CutAActualStartDate_Date;
                        }
                    }
                    if (cutAActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        //sewingMasterView.CutAActualFinishDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualFinishDate);
                        //sewingMasterView.CutAActualFinishDate = sewingMaster.CutAActualFinishDate;
                        sewingMasterView.CutAActualFinishDate = "";
                        if (sewingMaster.CutAActualFinishDate_Date != dtDefault)
                        {
                            sewingMasterView.CutAActualFinishDate = String.Format("{0:M/d}", sewingMaster.CutAActualFinishDate_Date);
                            sewingMasterView.CutAActualFinishDate_Date = sewingMaster.CutAActualFinishDate_Date;
                        }
                    }
                    if (cutABalanceUpdateList.Contains(productNo) == false)
                    {
                        //sewingMasterView.CutABalance = sewingMaster.CutABalance;
                        if (sewingMaster.CutABalance.Contains("/"))
                        {
                            sewingMasterView.CutABalance_Date = TimeHelper.Convert(sewingMaster.CutABalance);
                            sewingMasterView.CutABalance = String.Format("{0:M/d}", sewingMasterView.CutABalance_Date);
                        }
                        else
                            sewingMasterView.CutABalance = sewingMaster.CutABalance;
                    }

                    if (printingBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.PrintingBalance = sewingMaster.PrintingBalance;
                    }
                    if (h_fBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.H_FBalance = sewingMaster.H_FBalance;
                    }
                    if (embroideryBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.EmbroideryBalance = sewingMaster.EmbroideryBalance;
                    }

                    if (cutBActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        //sewingMasterView.CutBActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutBActualStartDate);
                        sewingMasterView.CutBActualStartDate = sewingMaster.CutBActualStartDate;
                    }

                    if (cutBBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutBBalance = sewingMaster.CutBBalance;
                    }
                    if (autoCutUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.AutoCut = sewingMaster.AutoCut;
                    }
                    if (laserCutUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.LaserCut = sewingMaster.LaserCut;
                    }
                    if (huasenCutUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.HuasenCut = sewingMaster.HuasenCut;
                    }

                    //var beforeSewingX1Days = sewingMaster.SewingStartDate.AddDays(-15);
                    var bfSewingOfCutTypeA = sewingMaster.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                    var dtCheckOffDateCutTypeAList = CheckOffDay(bfSewingOfCutTypeA, sewingMaster.SewingStartDate);

                    //var beforeSewingXDays = sewingMaster.SewingStartDate.AddDays(-10);
                    var bfSewingOfCutTypeB = sewingMaster.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                    var dtCheckOffDateCutTypeBList = CheckOffDay(bfSewingOfCutTypeB, sewingMaster.SewingStartDate);

                    var firstDateCheckOffCutTypeA = String.Format("{0:M/d}", dtCheckOffDateCutTypeAList.FirstOrDefault());
                    var firstDateCheckOffCutTypeB = String.Format("{0:M/d}", dtCheckOffDateCutTypeBList.FirstOrDefault());

                    if (!cutBStartDateUpdateList.Contains(productNo))
                    {
                        if (!String.IsNullOrEmpty(sewingMaster.CutBStartDate))
                            sewingMasterView.CutBStartDate = sewingMaster.CutBStartDate;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.CutBStartDate = firstDateCheckOffCutTypeB;
                    }

                    // Atom CutAB
                    if (!atomCutAUpdateList.Contains(productNo))
                    {
                        if (!String.IsNullOrEmpty(sewingMaster.AtomCutA))
                            sewingMasterView.AtomCutA = sewingMaster.AtomCutA;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.AtomCutA = firstDateCheckOffCutTypeA;
                    }
                    if (!atomCutBUpdateList.Contains(productNo))
                    {
                        if (!String.IsNullOrEmpty(sewingMaster.AtomCutB))
                            sewingMasterView.AtomCutB = sewingMaster.AtomCutB;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.AtomCutB = firstDateCheckOffCutTypeB;
                    }

                    // Laser CutAB
                    if (!laserCutAUpdateList.Contains(productNo))
                    {
                        if (!String.IsNullOrEmpty(sewingMaster.LaserCutA))
                            sewingMasterView.LaserCutA = sewingMaster.LaserCutA;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.LaserCutA = firstDateCheckOffCutTypeA;
                    }
                    if (!laserCutBUpdateList.Contains(productNo))
                    {
                        if (!String.IsNullOrEmpty(sewingMaster.LaserCutB))
                            sewingMasterView.LaserCutB = sewingMaster.LaserCutB;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.LaserCutB = firstDateCheckOffCutTypeB;
                    }

                    // Huasen CutAB
                    if (!huasenCutAUpdateList.Contains(productNo))
                    {
                        if (String.IsNullOrEmpty(sewingMaster.HuasenCutA) == false)
                            sewingMasterView.HuasenCutA = sewingMaster.HuasenCutA;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.HuasenCutA = firstDateCheckOffCutTypeA;
                    }
                    if (!huasenCutBUpdateList.Contains(productNo))
                    {
                        if (!String.IsNullOrEmpty(sewingMaster.HuasenCutB))
                            sewingMasterView.HuasenCutB = sewingMaster.HuasenCutB;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.HuasenCutB = firstDateCheckOffCutTypeB;
                    }

                    // Comelz CutAB
                    if (!comelzCutAUpdateList.Contains(productNo))
                    {
                        if (String.IsNullOrEmpty(sewingMaster.ComelzCutA) == false)
                            sewingMasterView.ComelzCutA = sewingMaster.ComelzCutA;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.ComelzCutA = firstDateCheckOffCutTypeA;
                    }
                    if (!comelzCutBUpdateList.Contains(productNo))
                    {
                        if (String.IsNullOrEmpty(sewingMaster.ComelzCutB) == false)
                            sewingMasterView.ComelzCutB = sewingMaster.ComelzCutB;
                        else if (sewingMaster.SewingStartDate != dtDefault)
                            sewingMasterView.ComelzCutB = firstDateCheckOffCutTypeB;
                    }
                }
            }

            //Sort By LineId, Sequence
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence));
            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
            for (int i = 0; i <= sewingMasterViewFindList.Count - 1; i++)
            {
                sewingMasterViewFindList[i].Sequence = i;
            }

            //Caculate
            List<String> sewingLineList = sewingMasterViewFindList.Select(l => l.SewingLine).Distinct().ToList();
            foreach (string sewingLine in sewingLineList)
            {
                if (String.IsNullOrEmpty(sewingLine) == false)
                {
                    List<SewingMasterViewModel> sewingMasterViewLineList = sewingMasterViewFindList.Where(s => s.SewingLine == sewingLine).ToList();
                    if (sewingMasterViewLineList.Count > 0)
                    {
                        DateTime dtSewingFinishDate = dtDefault;
                        DateTime dtSewingStartDate = dtDefault;
                        DateTime dtCutAFinishDate = dtDefault;
                        DateTime dtCutAStartDate = dtDefault;
                        int daySewingAddition = 0;
                        int dayCutAAddition = 0;

                        for (int i = 0; i <= sewingMasterViewLineList.Count - 1; i++)
                        {
                            SewingMasterViewModel sewingMasterView = sewingMasterViewLineList[i];

                            #region Calculate for Sewing
                            int qtySewingQuota = sewingMasterView.SewingQuota;
                            int optSewing = 0;
                            // CHECK DATE ERROR
                            //if (sewingMasterView.ProductNo== "21-3101")
                            //{
                            //    var t = "21-2616";
                            //}
                            if (qtySewingQuota > 0)
                            {
                                DateTime dtSewingStartDateTemp = TimeHelper.Convert(sewingMasterView.SewingActualStartDate);
                                var startNow = TimeHelper.Convert(sewingMasterView.SewingActualStartDate);
                                var startOld = sewingMasterView.SewingActualStartDate_Date;
                                if (startNow.Month == startOld.Month && startNow.Day == startOld.Day && startNow.Year != startOld.Year)
                                {
                                    if (currentYear < startOld.Year)
                                        dtSewingStartDateTemp = startNow > startOld ? startNow : startOld;
                                    else
                                        dtSewingStartDateTemp = startNow < startOld ? startNow : startOld;
                                }

                                // For Input Directly
                                if (sewingMasterView.SewingActualStartDate.Contains("/") && sewingMasterView.SewingActualStartDate.Split('/').Count() > 2)
                                {
                                    dtSewingStartDateTemp = startNow;
                                }

                                if ((String.IsNullOrEmpty(sewingMasterView.SewingActualStartDate) == false && dtSewingStartDateTemp != dtNothing)
                                    || sewingMasterView == sewingMasterViewLineList.First())
                                {
                                    dtSewingStartDate = dtSewingStartDateTemp;
                                }
                                else
                                {
                                    dtSewingStartDate = dtSewingFinishDate.AddDays(daySewingAddition);
                                }

                                daySewingAddition = 0;
                                DateTime dtSewingFinishDateTemp = TimeHelper.Convert(sewingMasterView.SewingActualFinishDate);
                                var finishNow = TimeHelper.Convert(sewingMasterView.SewingActualFinishDate);
                                var finishOld = sewingMasterView.SewingActualFinishDate_Date;
                                if (finishNow.Month == finishOld.Month && finishNow.Day == finishOld.Day && finishNow.Year != finishOld.Year)
                                {
                                    if (currentYear < finishOld.Year)
                                        dtSewingFinishDateTemp = finishNow > finishOld ? finishNow : finishOld;
                                    else
                                        dtSewingFinishDateTemp = finishNow < finishOld ? finishNow : finishOld;
                                }
                                // For Input Directly
                                if (sewingMasterView.SewingActualFinishDate.Contains("/") && sewingMasterView.SewingActualFinishDate.Split('/').Count() > 2)
                                {
                                    dtSewingFinishDateTemp = finishNow;
                                }

                                if (String.IsNullOrEmpty(sewingMasterView.SewingActualFinishDate) == false && dtSewingFinishDateTemp != dtNothing)
                                {
                                    dtSewingFinishDate = dtSewingFinishDateTemp;
                                }
                                else
                                {
                                    int qtySewingBalance = 0;
                                    sewingMasterView.SewingBalance = sewingMasterView.SewingBalance.Trim();
                                    int.TryParse(sewingMasterView.SewingBalance, out qtySewingBalance);
                                    if (qtySewingBalance > 0)
                                    {
                                        dtSewingFinishDate = DateTime.Now.Date.AddDays((double)(qtySewingBalance) / (double)qtySewingQuota);
                                        optSewing = 1;
                                    }
                                    else
                                    {
                                        //DateTime dtSewingBalance    = TimeHelper.Convert(sewingMasterView.SewingBalance);
                                        DateTime dtSewingBalance = sewingMasterView.SewingBalance_Date;

                                        if (String.IsNullOrEmpty(sewingMasterView.SewingBalance) == true)
                                        {
                                            dtSewingFinishDate = dtSewingStartDate.AddDays((double)sewingMasterView.Quantity / (double)qtySewingQuota);
                                            optSewing = 2;
                                        }
                                        else if (String.IsNullOrEmpty(sewingMasterView.SewingBalance) == false && dtSewingBalance != dtNothing)
                                        {
                                            dtSewingFinishDate = dtSewingBalance.AddDays(0);
                                            optSewing = 0;
                                            daySewingAddition = 1;
                                        }
                                    }
                                }
                                if (optSewing == 0)
                                {
                                    sewingMasterView.SewingStartDate = dtSewingStartDate;
                                    sewingMasterView.SewingFinishDate = dtSewingFinishDate;
                                }
                                else if (optSewing == 1)
                                {
                                    List<DateTime> dtCheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtSewingFinishDate);
                                    sewingMasterView.SewingStartDate = dtSewingStartDate;
                                    sewingMasterView.SewingFinishDate = new DateTime(dtCheckOffDateList1.Last().Year, dtCheckOffDateList1.Last().Month, dtCheckOffDateList1.Last().Day,
                                        dtSewingFinishDate.Hour, dtSewingFinishDate.Minute, dtSewingFinishDate.Second);
                                }
                                else if (optSewing == 2)
                                {
                                    List<DateTime> dtCheckOffDateList2 = CheckOffDay(dtSewingStartDate, dtSewingFinishDate);
                                    sewingMasterView.SewingStartDate = new DateTime(dtCheckOffDateList2.First().Year, dtCheckOffDateList2.First().Month, dtCheckOffDateList2.First().Day,
                                        dtSewingStartDate.Hour, dtSewingStartDate.Minute, dtSewingStartDate.Second);
                                    sewingMasterView.SewingFinishDate = new DateTime(dtCheckOffDateList2.Last().Year, dtCheckOffDateList2.Last().Month, dtCheckOffDateList2.Last().Day,
                                        dtSewingFinishDate.Hour, dtSewingFinishDate.Minute, dtSewingFinishDate.Second);
                                }

                                sewingMasterView.SewingStartDateForeground = Brushes.Black;
                                if (sewingMasterView.SewingStartDate < new DateTime(Math.Max(sewingMasterView.UpperMatsArrivalOrginal.Ticks, sewingMasterView.SewingMatsArrivalOrginal.Ticks)))
                                {
                                    sewingMasterView.SewingStartDateForeground = Brushes.Red;
                                }
                                sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                                if (sewingMasterView.SewingFinishDate > sewingMasterView.ETD)
                                {
                                    sewingMasterView.SewingFinishDateForeground = Brushes.Red;
                                }

                                dtSewingFinishDate = sewingMasterView.SewingFinishDate;

                                sewingMasterView.SewingActualStartDate_Date     = dtSewingStartDateTemp;
                                sewingMasterView.SewingActualFinishDate_Date    = dtSewingFinishDateTemp;
                            }
                            else
                            {
                                sewingMasterView.SewingActualStartDate = "";
                                sewingMasterView.SewingActualFinishDate = "";
                                sewingMasterView.SewingActualStartDate_Date = dtDefault;
                                sewingMasterView.SewingActualFinishDate_Date = dtDefault;
                                sewingMasterView.SewingStartDate = dtDefault;
                                sewingMasterView.SewingFinishDate = dtDefault;
                            }
                            #endregion

                            // CutAStartDate should be start before sewingStartDate more than 10 days.
                            #region Caculate for CutA
                            int qtyCutAQuota = sewingMasterView.CutAQuota;
                            int optCutA = 0;
                            if (qtyCutAQuota > 0)
                            {
                                DateTime dtCutAStartDateTemp = TimeHelper.Convert(sewingMasterView.CutAActualStartDate);
                                var cutAStartNow = TimeHelper.Convert(sewingMasterView.CutAActualStartDate);
                                var cutAStartOld = sewingMasterView.CutAActualStartDate_Date;
                                if (cutAStartNow.Month == cutAStartOld.Month && cutAStartNow.Day == cutAStartOld.Day && cutAStartNow.Year != cutAStartOld.Year)
                                {
                                    if (currentYear < cutAStartOld.Year)
                                        dtCutAStartDateTemp = cutAStartNow > cutAStartOld ? cutAStartNow : cutAStartOld;
                                    else
                                        dtCutAStartDateTemp = cutAStartNow < cutAStartOld ? cutAStartNow : cutAStartOld;
                                }
                                // For Input Directly
                                if (sewingMasterView.CutAActualStartDate.Contains("/"))
                                {
                                    if (sewingMasterView.CutAActualStartDate.Split('/').Count() > 2)
                                        dtCutAStartDateTemp = cutAStartNow;
                                }

                                //if ((String.IsNullOrEmpty(sewingMasterView.CutAActualStartDate) == false && dtCutAStartDateTemp != dtNothing)
                                if ((String.IsNullOrEmpty(sewingMasterView.CutAActualStartDate) == false && dtCutAStartDateTemp != dtDefault)
                                    || sewingMasterView == sewingMasterViewLineList.First())
                                {
                                    dtCutAStartDate = dtCutAStartDateTemp;

                                    // CutAStartDate should be start before SewingStartDate more than 10 days
                                    //if ((sewingMasterView.SewingStartDate - dtCutAStartDate).TotalDays < 10)
                                    if ((sewingMasterView.SewingStartDate - dtCutAStartDate).TotalDays < _SEW_VS_CUTA)
                                    {
                                        //var beforeDate = sewingMasterView.SewingStartDate.AddDays(-10);
                                        var beforeDate = sewingMasterView.SewingStartDate.AddDays(-_SEW_VS_CUTA);
                                        var dtCheckOffDateSewingStartDateList = CheckOffDay(beforeDate, sewingMasterView.SewingStartDate);

                                        dtCutAStartDate = new DateTime(dtCheckOffDateSewingStartDateList.First().Year, dtCheckOffDateSewingStartDateList.First().Month, dtCheckOffDateSewingStartDateList.First().Day,
                                        dtCutAStartDate.Hour, dtCutAStartDate.Minute, dtCutAStartDate.Second);
                                    }
                                }
                                else
                                {
                                    dtCutAStartDate = dtCutAFinishDate.AddDays(dayCutAAddition);

                                    // CutAStartDate should be start before SewingStartDate more than 10 days
                                    //if ((sewingMasterView.SewingStartDate - dtCutAStartDate).TotalDays < 10)
                                    if ((sewingMasterView.SewingStartDate - dtCutAStartDate).TotalDays < _SEW_VS_CUTA)
                                    {
                                        //var beforeDate = sewingMasterView.SewingStartDate.AddDays(-10);
                                        var beforeDate = sewingMasterView.SewingStartDate.AddDays(-_SEW_VS_CUTA);
                                        var dtCheckOffDateSewingStartDateList = CheckOffDay(beforeDate, sewingMasterView.SewingStartDate);

                                        dtCutAStartDate = new DateTime(dtCheckOffDateSewingStartDateList.First().Year, dtCheckOffDateSewingStartDateList.First().Month, dtCheckOffDateSewingStartDateList.First().Day,
                                        dtCutAStartDate.Hour, dtCutAStartDate.Minute, dtCutAStartDate.Second);
                                    }
                                }
                                dayCutAAddition = 0;
                                DateTime dtCutAFinishDateTemp = TimeHelper.Convert(sewingMasterView.CutAActualFinishDate);
                                var cutAFinishNow = TimeHelper.Convert(sewingMasterView.CutAActualFinishDate);
                                var cutAFinishOld = sewingMasterView.CutAActualFinishDate_Date;
                                if (cutAFinishNow.Month == cutAFinishOld.Month && cutAFinishNow.Day == cutAFinishOld.Day && cutAFinishNow.Year != cutAFinishOld.Year)
                                {
                                    if (currentYear < cutAFinishOld.Year)
                                        dtCutAFinishDateTemp = cutAFinishNow > cutAFinishOld ? cutAFinishNow : cutAFinishOld;
                                    else
                                        dtCutAFinishDateTemp = cutAFinishNow < cutAFinishOld ? cutAFinishNow : cutAFinishOld;
                                }

                                // For Input Directly
                                if (sewingMasterView.CutAActualFinishDate.Contains("/"))
                                {
                                    if (sewingMasterView.CutAActualFinishDate.Split('/').Count() > 2)
                                        dtCutAFinishDateTemp = cutAFinishNow;
                                }

                                if (String.IsNullOrEmpty(sewingMasterView.CutAActualFinishDate) == false && dtCutAFinishDateTemp != dtNothing)
                                {
                                    dtCutAFinishDate = dtCutAFinishDateTemp;
                                }
                                else
                                {
                                    int qtyCutABalance = 0;
                                    sewingMasterView.CutABalance = sewingMasterView.CutABalance.Trim();
                                    int.TryParse(sewingMasterView.CutABalance, out qtyCutABalance);
                                    if (qtyCutABalance > 0)
                                    {
                                        dtCutAFinishDate = DateTime.Now.Date.AddDays((double)(qtyCutABalance) / (double)qtyCutAQuota);
                                        optCutA = 1;
                                    }
                                    else
                                    {
                                        DateTime dtCutABalance = TimeHelper.Convert(sewingMasterView.CutABalance);
                                        var balanceDateNow = TimeHelper.Convert(sewingMasterView.CutABalance);
                                        var balanceDateOld = sewingMasterView.CutABalance_Date;
                                        if (balanceDateNow.Month == balanceDateOld.Month && balanceDateNow.Day == balanceDateOld.Day && balanceDateNow.Year != balanceDateOld.Year)
                                        {
                                            if (currentYear < balanceDateOld.Year)
                                                dtCutABalance = balanceDateNow > balanceDateOld ? balanceDateNow : balanceDateOld;
                                            else
                                                dtCutABalance = balanceDateNow < balanceDateOld ? balanceDateNow : balanceDateOld;
                                        }

                                        // For Input Directly
                                        if (sewingMasterView.CutABalance.Contains("/") && sewingMasterView.CutABalance.Split('/').Count() > 2)
                                        {
                                            dtCutABalance = balanceDateNow;
                                        }

                                        sewingMasterView.CutABalance_Date = dtCutABalance;

                                        if (String.IsNullOrEmpty(sewingMasterView.CutABalance) == true)
                                        {
                                            dtCutAFinishDate = dtCutAStartDate.AddDays((double)sewingMasterView.Quantity / (double)qtyCutAQuota);
                                            optCutA = 2;
                                        }
                                        else if (String.IsNullOrEmpty(sewingMasterView.CutABalance) == false && dtCutABalance != dtNothing)
                                        {
                                            dtCutAFinishDate = dtCutABalance.AddDays(0);
                                            optCutA = 0;
                                            dayCutAAddition = 1;
                                        }
                                    }
                                }

                                if (optCutA == 0)
                                {
                                    sewingMasterView.CutAStartDate = dtCutAStartDate;
                                    sewingMasterView.CutAFinishDate = dtCutAFinishDate;
                                }
                                else if (optCutA == 1)
                                {
                                    List<DateTime> dtCutACheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtCutAFinishDate);
                                    sewingMasterView.CutAStartDate = dtCutAStartDate;
                                    sewingMasterView.CutAFinishDate = new DateTime(dtCutACheckOffDateList1.Last().Year, dtCutACheckOffDateList1.Last().Month, dtCutACheckOffDateList1.Last().Day,
                                        dtCutAFinishDate.Hour, dtCutAFinishDate.Minute, dtCutAFinishDate.Second);
                                }
                                else if (optCutA == 2)
                                {
                                    List<DateTime> dtCutACheckOffDateList2 = CheckOffDay(dtCutAStartDate, dtCutAFinishDate);
                                    sewingMasterView.CutAStartDate = new DateTime(dtCutACheckOffDateList2.First().Year, dtCutACheckOffDateList2.First().Month, dtCutACheckOffDateList2.First().Day,
                                        dtCutAStartDate.Hour, dtCutAStartDate.Minute, dtCutAStartDate.Second);
                                    sewingMasterView.CutAFinishDate = new DateTime(dtCutACheckOffDateList2.Last().Year, dtCutACheckOffDateList2.Last().Month, dtCutACheckOffDateList2.Last().Day,
                                        dtCutAFinishDate.Hour, dtCutAFinishDate.Minute, dtCutAFinishDate.Second);
                                }
                                dtCutAFinishDate = sewingMasterView.CutAFinishDate;

                                sewingMasterView.CutAActualStartDate_Date   = dtCutAStartDateTemp;
                                sewingMasterView.CutAActualFinishDate_Date  = dtCutAFinishDateTemp;
                            }
                            #endregion

                            #region Display the Date
                            var sewingMaster = sewingMasterList.Where(w => w.ProductNo == sewingMasterView.ProductNo).FirstOrDefault();
                            if (sewingMaster != null)
                            {

                                //var beforeSewing15 = sewingMasterView.SewingStartDate.AddDays(-15);
                                var bfSewingOfCutTypeA = sewingMasterView.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                                var dtCheckOffDateCutTypeAList = CheckOffDay(bfSewingOfCutTypeA, sewingMasterView.SewingStartDate);

                                var bfSewingOfCutTypeB = sewingMasterView.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                                var dtCheckOffDateCutTypeBList = CheckOffDay(bfSewingOfCutTypeB, sewingMasterView.SewingStartDate);

                                var firstDateCheckOffCutTypeA = String.Format("{0:M/d}", dtCheckOffDateCutTypeAList.FirstOrDefault());
                                var firstDateCheckOffCutTypeB = String.Format("{0:M/d}", dtCheckOffDateCutTypeBList.FirstOrDefault());

                                if (!String.IsNullOrEmpty(sewingMaster.CutBStartDate))
                                    sewingMasterView.CutBStartDate = sewingMaster.CutBStartDate;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.CutBStartDate = firstDateCheckOffCutTypeB;

                                // Atom CutAB
                                if (!String.IsNullOrEmpty(sewingMaster.AtomCutA))
                                    sewingMasterView.AtomCutA = sewingMaster.AtomCutA;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.AtomCutA = firstDateCheckOffCutTypeA;

                                if (!String.IsNullOrEmpty(sewingMaster.AtomCutB))
                                    sewingMasterView.AtomCutB = sewingMaster.AtomCutB;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.AtomCutB = firstDateCheckOffCutTypeB;

                                // Laser CutAB
                                if (!String.IsNullOrEmpty(sewingMaster.LaserCutA))
                                    sewingMasterView.LaserCutA = sewingMaster.LaserCutA;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.LaserCutA = firstDateCheckOffCutTypeA;

                                if (!String.IsNullOrEmpty(sewingMaster.LaserCutB))
                                    sewingMasterView.LaserCutB = sewingMaster.LaserCutB;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.LaserCutB = firstDateCheckOffCutTypeB;

                                // Huasen CutAB
                                if (!String.IsNullOrEmpty(sewingMaster.HuasenCutA))
                                    sewingMasterView.HuasenCutA = sewingMaster.HuasenCutA;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.HuasenCutA = firstDateCheckOffCutTypeA;

                                if (!String.IsNullOrEmpty(sewingMaster.HuasenCutB))
                                    sewingMasterView.HuasenCutB = sewingMaster.HuasenCutB;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.HuasenCutB = firstDateCheckOffCutTypeB;

                                // Comelz CutAB
                                if (!String.IsNullOrEmpty(sewingMaster.ComelzCutA))
                                    sewingMasterView.ComelzCutA = sewingMaster.ComelzCutA;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.ComelzCutA = firstDateCheckOffCutTypeA;

                                if (!String.IsNullOrEmpty(sewingMaster.ComelzCutB))
                                    sewingMasterView.ComelzCutB = sewingMaster.ComelzCutB;
                                else if (sewingMasterView.SewingStartDate != dtDefault)
                                    sewingMasterView.ComelzCutB = firstDateCheckOffCutTypeB;
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
                    searchBox.GetFindWhat = new RawMaterialSearchBoxWindow.GetString(SearchSewingMaster);
                    searchBox.Show();
                }
            }
        }

        private void SearchSewingMaster(string findWhat, bool isMatch, bool isShow)
        {
            //var a = dgSewingMaster.SelectedCells;
            //DataGridColumn b = a[0].Column;
            //Binding c = (Binding)b.ClipboardContentBinding;
            //PropertyPath path = c.Path;
            //DependencyProperty d = DependencyProperty.Register(path.Path, typeof(string), typeof(SewingMasterViewModel));
            //var e = sewingMasterViewList.Select(s => GetValue(d)).First();
            sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
            //var regexHasNumber = new Regex(@"[^\d]");
            //sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine)
            //    .ThenBy(th=> String.IsNullOrEmpty(regexHasNumber.Replace(th.SewingLine,"")) == false ? double.Parse(regexHasNumber.Replace(th.SewingLine, "")) : 999)
            //    .ThenBy(s => s.Sequence).ToList();
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList);
            if (String.IsNullOrEmpty(findWhat) == false)
            {
                if (isMatch == true)
                {
                    SewingMasterViewModel sewingMasterViewFind = sewingMasterViewFindList.Where(r =>
                        r.ProductNo.ToLower() == findWhat.ToLower() || r.Country.ToLower() == findWhat.ToLower() || r.ShoeName.ToLower() == findWhat.ToLower() || r.ArticleNo.ToLower() == findWhat.ToLower() ||
                        r.PatternNo.ToLower() == findWhat.ToLower() || r.ETD.ToString("dd/MM/yyyy") == findWhat.ToLower() || r.SewingLine.ToLower() == findWhat.ToLower()).FirstOrDefault();
                    if (sewingMasterViewFind != null)
                    {
                        dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                        dgSewingMaster.SelectedItem = sewingMasterViewFind;
                        dgSewingMaster.ScrollIntoView(sewingMasterViewFind);
                        colSewingLine.CanUserSort = true;
                    }
                    else
                    {
                        dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                        MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (isMatch == false)
                    {
                        if (isShow == true)
                        {
                            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == true || r.Country.ToLower().Contains(findWhat.ToLower()) == true || r.ShoeName.ToLower().Contains(findWhat.ToLower()) == true || r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == true ||
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == true || r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == true || r.SewingLine.ToLower().Contains(findWhat.ToLower()) == true));
                        }
                        else
                        {
                            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == false && r.Country.ToLower().Contains(findWhat.ToLower()) == false && r.ShoeName.ToLower().Contains(findWhat.ToLower()) == false && r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == false &&
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == false && r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == false && r.SewingLine.ToLower().Contains(findWhat.ToLower()) == false));
                        }

                        if (sewingMasterViewFindList.Count > 0)
                        {
                            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                            colSewingLine.CanUserSort = false;
                        }
                        else
                        {
                            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                            MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList);
                            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                        }
                    }
                }
            }
            else
            {
                colSewingLine.CanUserSort = true;
                dgSewingMaster.ItemsSource = sewingMasterViewFindList;
            }
        }

        private void dgSewingMaster_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
            if (sewingMasterView == null)
            {
                return;
            }

            if (!linesNeedSaving.Contains(sewingMasterView.SewingLine))
                linesNeedSaving.Add(sewingMasterView.SewingLine);

            string productNo = sewingMasterView.ProductNo;

            if (e.Column == colSewingLine || e.Column == colSewingQuota || e.Column == colSewingPrep || e.Column == colSewingActualStartDate ||
                e.Column == colSewingActualFinishDate || e.Column == colSewingBalance)
            {
                lineSewingEditingList.Add(sewingMasterView.SewingLine);
                if (e.Column == colSewingLine)
                {
                    sewingLineUpdateList.Add(productNo);
                }
                if (e.Column == colSewingQuota)
                {
                    sewingQuotaUpdateList.Add(productNo);
                }

                if (e.Column == colSewingPrep)
                {
                    sewingPrepUpdateList.Add(productNo);
                }

                if (e.Column == colSewingActualStartDate)
                {
                    sewingActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colSewingActualFinishDate)
                {
                    sewingActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colSewingBalance)
                {
                    sewingBalanceUpdateList.Add(productNo);
                }
            }
            else if (e.Column == colCutAQuota || e.Column == colCutAActualStartDate || e.Column == colCutAActualFinishDate ||
                e.Column == colCutABalance)
            {
                lineCutPrepEditingList.Add(sewingMasterView.SewingLine);
                if (e.Column == colCutAQuota)
                {
                    cutAQuotaUpdateList.Add(productNo);
                }
                if (e.Column == colCutAActualStartDate)
                {
                    cutAActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colCutAActualFinishDate)
                {
                    cutAActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colCutABalance)
                {
                    cutABalanceUpdateList.Add(productNo);
                }
            }
            else if (e.Column == colCutBStartDate || e.Column == colAtomCutA || e.Column == colAtomCutB || e.Column == colLaserCutA || e.Column == colLaserCutB
                || e.Column == colHuasenCutA || e.Column == colHuasenCutB || e.Column == colH_FBalance || e.Column == colCutBBalance || e.Column == colComelzCutA || e.Column == colComelzCutB)
            {
                if (e.Column == colCutBStartDate)
                {
                    cutBStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colAtomCutA)
                {
                    atomCutAUpdateList.Add(productNo);
                }
                if (e.Column == colAtomCutB)
                {
                    atomCutBUpdateList.Add(productNo);
                }
                if (e.Column == colLaserCutA)
                {
                    laserCutAUpdateList.Add(productNo);
                }
                if (e.Column == colLaserCutB)
                {
                    laserCutBUpdateList.Add(productNo);
                }
                if (e.Column == colHuasenCutA)
                {
                    huasenCutAUpdateList.Add(productNo);
                }
                if (e.Column == colHuasenCutB)
                {
                    huasenCutBUpdateList.Add(productNo);
                }
                if (e.Column == colComelzCutA)
                    comelzCutAUpdateList.Add(productNo);
                if (e.Column == colComelzCutB)
                    comelzCutBUpdateList.Add(productNo);

                if (e.Column == colH_FBalance)
                {
                    h_fBalanceUpdateList.Add(productNo);
                }
                if (e.Column == colCutBBalance)
                {
                    cutBBalanceUpdateList.Add(productNo);
                }
            }

            if (e.Column == colSewingLine)
            {
                //SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
                string sewingLine = sewingMasterView.SewingLine;
                if (String.IsNullOrEmpty(sewingLine) == true)
                {
                    return;
                }
                int sewingSequence = 0;
                if (sewingMasterViewList.Where(s => s.SewingLine == sewingLine).Count() > 0)
                {
                    sewingSequence = sewingMasterViewList.Where(s => s.SewingLine == sewingLine).Select(s => s.Sequence).Max() + 1;
                }
                sewingMasterView.Sequence = sewingSequence;
                //sequenceUpdateList.Add(sewingSequence);
                isSequenceEditing = true;

                //changingSequence = true;
            }
            if (e.Column == colSewingActualStartDate || e.Column == colSewingActualFinishDate)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;

                    if (e.Column == colSewingActualStartDate)
                        sewingMasterView.SewingStartDateForeground = Brushes.Red;
                    else
                        sewingMasterView.SewingFinishDateForeground = Brushes.Red;

                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }

                //var lineEditting = sewingMasterViewList.Where(w => w.SewingLine == sewingMasterView.SewingLine).FirstOrDefault();
                //if (lineEditting != null && !String.IsNullOrEmpty(lineEditting.SewingLine))
                //{
                //    var sewingMasterViewListByLine = sewingMasterViewList.Where(w => w.SewingLine == lineEditting.SewingLine).ToList();
                //    var sewingMasterViewListFromCurrentSequenceToTheEndOfLine = sewingMasterViewListByLine.Where(w => w.Sequence >= sewingMasterView.Sequence).ToList();
                //    foreach (var sewingViewModel in sewingMasterViewListFromCurrentSequenceToTheEndOfLine)
                //    {
                //        sewingViewModel.SewingStartDateForeground = Brushes.Green;
                //        sewingViewModel.SewingFinishDateForeground = Brushes.Blue;
                //    }
                //}
            }
            if (e.Column == colCutAActualStartDate || e.Column == colSewingActualFinishDate || e.Column == colCutBStartDate)
            {
                //additional code.
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {

                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }
            }

            if (e.Column == colSewingPrep)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                int sewingPrep = 0;
                Int32.TryParse(txtElement.Text, out sewingPrep);
                if (sewingPrep == sewingMasterView.SewingQuota && sewingPrep != 0)
                {
                    txtElement.Text = String.Format("{0:MM/dd}", DateTime.Now);
                }
            }

            isEditing = false;
        }

        private void dgSewingMaster_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column == colSewingLine)
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
                    sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence));
                }
                else
                {
                    sewingMasterViewList = sewingMasterViewList.OrderByDescending(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
                    sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList.OrderByDescending(s => s.SewingLine).ThenBy(s => s.Sequence));
                }
                dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                for (int i = 0; i <= sewingMasterViewFindList.Count - 1; i++)
                {
                    sewingMasterViewFindList[i].Sequence = i;
                }
                dgSewingMaster.ScrollIntoView(sewingMasterViewFindList.Where(s => String.IsNullOrEmpty(s.SewingLine) == false).FirstOrDefault());
                e.Handled = true;
            }
        }

        private void dgSewingMaster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)dgSewingMaster.CurrentItem;
            if (account.AssemblyMaster == true && dgSewingMaster.CurrentCell.Column == colSewingBalance && sewingMasterView != null)
            {
                SewingInputOutputWindow window = new SewingInputOutputWindow(sewingMasterView.ProductNo, sewingMasterView.SewingActualStartDateAuto, sewingMasterView.SewingActualFinishDateAuto);
                if (window.ShowDialog() == true)
                {
                    string productNo    = sewingMasterView.ProductNo;
                    string sewingLine   = sewingMasterView.SewingLine;

                    sewingMasterView.SewingBalance_Date = TimeHelper.Convert(window.resultString);
                    if (sewingMasterView.SewingBalance_Date == dtNothing)
                        sewingMasterView.SewingBalance_Date = dtDefault;

                    sewingMasterView.SewingBalance = window.resultString;
                    if (sewingMasterView.SewingBalance_Date != dtDefault)
                        sewingMasterView.SewingBalance = String.Format("{0:M/d}", sewingMasterView.SewingBalance_Date);

                    sewingMasterView.SewingActualStartDateAuto = window.sewingActualStartDateAuto;
                    sewingMasterView.SewingActualFinishDateAuto = window.sewingActualFinishDateAuto;
                    if (String.IsNullOrEmpty(window.resultString) == true)
                    {
                        sewingMasterView.SewingActualStartDateAuto = "";
                        sewingMasterView.SewingActualFinishDateAuto = "";
                    }
                    lineSewingEditingList.Add(sewingLine);
                    sewingActualStartDateUpdateAutoList.Add(productNo);
                    sewingActualFinishDateUpdateAutoList.Add(productNo);
                    sewingBalanceUpdateList.Add(productNo);
                }

                if (!linesNeedSaving.Contains(sewingMasterView.SewingLine))
                    linesNeedSaving.Add(sewingMasterView.SewingLine);
            }
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false && (account.SewingMaster || account.CutPrepMaster))
            {
                this.Cursor = Cursors.Wait;
                sewingMasterViewToInsertList = dgSewingMaster.Items.OfType<SewingMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false && (account.SewingMaster || account.CutPrepMaster))
            {
                this.Cursor = Cursors.Wait;
                sewingMasterViewToInsertList = dgSewingMaster.Items.OfType<SewingMasterViewModel>().ToList();
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
                var sourceList = sewingMasterViewFindList.ToList();

                var productNoSourceList = sewingMasterList.Select(s => s.ProductNo).ToList();
                // Insert the PO is the first time.
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
                            lblStatus.Text = String.Format("Saving {0} / {1} PO", index, insertNewPOList.Count());
                            prgStatus.Value = index;
                        }));
                        index++;
                    }
                }


                // Update SewingMaster Info ( Without Sequence )
                var updateList = sourceList.Where(w => linesNeedSaving.Contains(w.SewingLine)).ToList();
                if (updateList.Count() > 0)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        lblStatus.Text = "Saving PO ...";
                        prgStatus.Value = 0;
                        prgStatus.Maximum = updateList.Count();
                    }));
                    int index_1 = 1;

                    foreach (var item in updateList)
                    {
                        InsertAModel(item, false);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Text = String.Format("Saving {0} / {1} PO", index_1, updateList.Count());
                            prgStatus.Value = index_1;
                        }));
                        index_1++;
                    }
                    linesNeedSaving.Clear();
                }

                // Update Sequence
                if (sourceList.Count() == sewingMasterViewList.Count() && account.SewingMaster)
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
                            CommonController.UpdateSequenceByPO(item.ProductNo, item.Sequence, "Sewing");
                            index++;
                        }
                        //changingSequence = false;
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

        private void InsertAModel(SewingMasterViewModel sewingMasterViewToInsert, bool isNewPO)
        {
            var productNo = sewingMasterViewToInsert.ProductNo;
            var sewingLine = sewingMasterViewToInsert.SewingLine;
            var model = new SewingMasterModel
            {
                ProductNo = productNo,
                Sequence = sewingMasterViewToInsert.Sequence,
                SewingLine = sewingLine,
                SewingStartDate = sewingMasterViewToInsert.SewingStartDate,
                SewingFinishDate = sewingMasterViewToInsert.SewingFinishDate,
                SewingPrep = sewingMasterViewToInsert.SewingPrep,
                SewingQuota = sewingMasterViewToInsert.SewingQuota,

                SewingActualStartDate = sewingMasterViewToInsert.SewingActualStartDate,
                SewingActualStartDate_Date = sewingMasterViewToInsert.SewingActualStartDate_Date,

                SewingActualFinishDate = sewingMasterViewToInsert.SewingActualFinishDate,
                SewingActualFinishDate_Date = sewingMasterViewToInsert.SewingActualFinishDate_Date,

                SewingActualStartDateAuto = sewingMasterViewToInsert.SewingActualStartDateAuto,
                SewingActualFinishDateAuto = sewingMasterViewToInsert.SewingActualFinishDateAuto,

                SewingBalance = sewingMasterViewToInsert.SewingBalance,

                CutAStartDate = sewingMasterViewToInsert.CutAStartDate,
                CutAFinishDate = sewingMasterViewToInsert.CutAFinishDate,
                CutAQuota = sewingMasterViewToInsert.CutAQuota,

                CutAActualStartDate = sewingMasterViewToInsert.CutAActualStartDate,
                CutAActualStartDate_Date = sewingMasterViewToInsert.CutAActualStartDate_Date,

                CutAActualFinishDate = sewingMasterViewToInsert.CutAActualFinishDate,
                CutAActualFinishDate_Date = sewingMasterViewToInsert.CutAActualFinishDate_Date,

                CutABalance = sewingMasterViewToInsert.CutABalance,
                CutABalance_Date = sewingMasterViewToInsert.CutABalance_Date,

                PrintingBalance = sewingMasterViewToInsert.PrintingBalance,
                H_FBalance = sewingMasterViewToInsert.H_FBalance,
                EmbroideryBalance = sewingMasterViewToInsert.EmbroideryBalance,

                CutBActualStartDate = sewingMasterViewToInsert.CutBActualStartDate,
                CutBBalance = sewingMasterViewToInsert.CutBBalance,
                AutoCut = sewingMasterViewToInsert.AutoCut,
                LaserCut = sewingMasterViewToInsert.LaserCut,
                HuasenCut = sewingMasterViewToInsert.HuasenCut,

                CutBStartDate = sewingMasterViewToInsert.CutBStartDate,
                AtomCutA = sewingMasterViewToInsert.AtomCutA,
                AtomCutB = sewingMasterViewToInsert.AtomCutB,
                LaserCutA = sewingMasterViewToInsert.LaserCutA,
                LaserCutB = sewingMasterViewToInsert.LaserCutB,
                HuasenCutA = sewingMasterViewToInsert.HuasenCutA,
                HuasenCutB = sewingMasterViewToInsert.HuasenCutB,
                ComelzCutA = sewingMasterViewToInsert.ComelzCutA,
                ComelzCutB = sewingMasterViewToInsert.ComelzCutB,

                IsSequenceUpdate = false,
                IsSewingLineUpdate = false,
                IsSewingStartDateUpdate = false,
                IsSewingFinishDateUpdate = false,
                IsSewingQuotaUpdate = false,
                IsSewingPrepUpdate = false,

                IsSewingActualStartDateUpdate = false,
                IsSewingActualFinishDateUpdate = false,

                IsSewingActualStartDateAutoUpdate = false,
                IsSewingActualFinishDateAutoUpdate = false,

                IsSewingBalanceUpdate = false,
                IsCutAStartDateUpdate = false,
                IsCutAFinishDateUpdate = false,
                IsCutAQuotaUpdate = false,
                IsCutAActualStartDateUpdate = false,
                IsCutAActualFinishDateUpdate = false,
                IsCutABalanceUpdate = false,
                IsPrintingBalanceUpdate = false,
                IsH_FBalanceUpdate = false,
                IsEmbroideryBalanceUpdate = false,

                IsCutBActualStartDateUpdate = false,
                IsCutBBalanceUpdate = false,
                IsAutoCutUpdate = false,
                IsLaserCutUpdate = false,
                IsHuasenCutUpdate = false,

                //
                IsUpdateCutBStartDate = false,
                IsUpdateAtomCutA = false,
                IsUpdateAtomCutB = false,
                IsUpdateLaserCutA = false,
                IsUpdateLaserCutB = false,
                IsUpdateHuasenCutA = false,
                IsUpdateHuasenCutB = false,
                IsUpdateComelzCutA = false,
                IsUpdateComelzCutB = false,

            };

            model.IsSequenceUpdate = isSequenceEditing;

            model.IsSewingLineUpdate = sewingLineUpdateList.Contains(productNo);
            model.IsSewingStartDateUpdate = lineSewingEditingList.Contains(sewingLine);
            model.IsSewingFinishDateUpdate = lineSewingEditingList.Contains(sewingLine);
            model.IsSewingQuotaUpdate = sewingQuotaUpdateList.Contains(productNo);

            model.IsSewingPrepUpdate = sewingPrepUpdateList.Contains(productNo);

            model.IsSewingActualStartDateUpdate = sewingActualStartDateUpdateList.Contains(productNo);
            model.IsSewingActualFinishDateUpdate = sewingActualFinishDateUpdateList.Contains(productNo);

            model.IsSewingActualStartDateAutoUpdate = sewingActualStartDateUpdateAutoList.Contains(productNo);
            model.IsSewingActualFinishDateAutoUpdate = sewingActualFinishDateUpdateAutoList.Contains(productNo);

            model.IsSewingBalanceUpdate = sewingBalanceUpdateList.Contains(productNo);

            model.IsCutAStartDateUpdate = lineCutPrepEditingList.Contains(sewingLine);
            model.IsCutAFinishDateUpdate = lineCutPrepEditingList.Contains(sewingLine);
            model.IsCutAQuotaUpdate = cutAQuotaUpdateList.Contains(productNo);
            model.IsCutAActualStartDateUpdate = cutAActualStartDateUpdateList.Contains(productNo);
            model.IsCutAActualFinishDateUpdate = cutAActualFinishDateUpdateList.Contains(productNo);
            model.IsCutABalanceUpdate = cutABalanceUpdateList.Contains(productNo);

            model.IsPrintingBalanceUpdate = printingBalanceUpdateList.Contains(productNo);
            model.IsH_FBalanceUpdate = h_fBalanceUpdateList.Contains(productNo);
            model.IsEmbroideryBalanceUpdate = embroideryBalanceUpdateList.Contains(productNo);
            model.IsCutBActualStartDateUpdate = cutBActualStartDateUpdateList.Contains(productNo);
            model.IsCutBBalanceUpdate = cutBBalanceUpdateList.Contains(productNo);
            model.IsAutoCutUpdate = autoCutUpdateList.Contains(productNo);
            model.IsLaserCutUpdate = laserCutUpdateList.Contains(productNo);
            model.IsHuasenCutUpdate = huasenCutUpdateList.Contains(productNo);

            model.IsUpdateCutBStartDate = cutBStartDateUpdateList.Contains(productNo);
            model.IsUpdateAtomCutA = atomCutAUpdateList.Contains(productNo);
            model.IsUpdateAtomCutB = atomCutBUpdateList.Contains(productNo);
            model.IsUpdateLaserCutA = laserCutAUpdateList.Contains(productNo);
            model.IsUpdateLaserCutB = laserCutBUpdateList.Contains(productNo);
            model.IsUpdateHuasenCutA = huasenCutAUpdateList.Contains(productNo);
            model.IsUpdateHuasenCutB = huasenCutBUpdateList.Contains(productNo);
            model.IsUpdateComelzCutA = comelzCutAUpdateList.Contains(productNo);
            model.IsUpdateComelzCutB = comelzCutBUpdateList.Contains(productNo);
            //&& sequenceUpdateList.Contains(model.Sequence)
            if ((model.IsSequenceUpdate == true) ||
                model.IsSewingLineUpdate == true ||
                model.IsSewingStartDateUpdate == true ||
                model.IsSewingFinishDateUpdate == true ||
                model.IsSewingQuotaUpdate == true ||

                model.IsSewingPrepUpdate == true ||

                model.IsSewingActualStartDateUpdate == true ||
                model.IsSewingActualFinishDateUpdate == true ||

                model.IsSewingActualStartDateAutoUpdate == true ||
                model.IsSewingActualFinishDateAutoUpdate == true ||

                model.IsSewingBalanceUpdate == true ||
                model.IsCutAStartDateUpdate == true ||
                model.IsCutAFinishDateUpdate == true ||
                model.IsCutAQuotaUpdate == true ||
                model.IsCutAActualStartDateUpdate == true ||
                model.IsCutAActualFinishDateUpdate == true ||
                model.IsCutBActualStartDateUpdate == true ||
                model.IsCutABalanceUpdate == true ||
                model.IsPrintingBalanceUpdate == true ||
                model.IsH_FBalanceUpdate == true ||
                model.IsEmbroideryBalanceUpdate == true ||
                model.IsCutBBalanceUpdate == true ||
                model.IsAutoCutUpdate == true ||
                model.IsLaserCutUpdate == true ||
                model.IsHuasenCutUpdate == true ||

                //
                model.IsUpdateCutBStartDate == true ||
                model.IsUpdateAtomCutA == true ||
                model.IsUpdateAtomCutB == true ||
                model.IsUpdateLaserCutA == true ||
                model.IsUpdateLaserCutB == true ||
                model.IsUpdateHuasenCutA == true ||
                model.IsUpdateHuasenCutB == true ||
                model.IsUpdateComelzCutA == true ||
                model.IsUpdateComelzCutB == true ||
                isNewPO == true
                )
            {
                model.Reviser = account.UserName;
                SewingMasterController.Insert_2(model, account);
            }
        }

        private void bwInsert_DoWork_Before(object sender, DoWorkEventArgs e)
        {
            foreach (SewingMasterViewModel sewingMasterViewToInsert in sewingMasterViewToInsertList)
            {
                var productNo = sewingMasterViewToInsert.ProductNo;
                var sewingLine = sewingMasterViewToInsert.SewingLine;
                var model = new SewingMasterModel
                {
                    ProductNo = productNo,
                    Sequence = sewingMasterViewToInsert.Sequence,
                    SewingLine = sewingLine,
                    SewingStartDate = sewingMasterViewToInsert.SewingStartDate,
                    SewingFinishDate = sewingMasterViewToInsert.SewingFinishDate,
                    SewingPrep = sewingMasterViewToInsert.SewingPrep,
                    SewingQuota = sewingMasterViewToInsert.SewingQuota,

                    SewingActualStartDate = sewingMasterViewToInsert.SewingActualStartDate,
                    SewingActualStartDate_Date  = sewingMasterViewToInsert.SewingActualStartDate_Date,

                    SewingActualFinishDate = sewingMasterViewToInsert.SewingActualFinishDate,
                    SewingActualFinishDate_Date = sewingMasterViewToInsert.SewingActualFinishDate_Date,

                    SewingActualStartDateAuto   = sewingMasterViewToInsert.SewingActualStartDateAuto,
                    SewingActualFinishDateAuto  = sewingMasterViewToInsert.SewingActualFinishDateAuto,

                    SewingBalance   = sewingMasterViewToInsert.SewingBalance,

                    CutAStartDate   = sewingMasterViewToInsert.CutAStartDate,
                    CutAFinishDate  = sewingMasterViewToInsert.CutAFinishDate,
                    CutAQuota       = sewingMasterViewToInsert.CutAQuota,

                    CutAActualStartDate = sewingMasterViewToInsert.CutAActualStartDate,
                    CutAActualStartDate_Date    = sewingMasterViewToInsert.CutAActualStartDate_Date,

                    CutAActualFinishDate = sewingMasterViewToInsert.CutAActualFinishDate,
                    CutAActualFinishDate_Date   = sewingMasterViewToInsert.CutAActualFinishDate_Date,

                    CutABalance = sewingMasterViewToInsert.CutABalance,
                    CutABalance_Date    = sewingMasterViewToInsert.CutABalance_Date,

                    PrintingBalance     = sewingMasterViewToInsert.PrintingBalance,
                    H_FBalance          = sewingMasterViewToInsert.H_FBalance,
                    EmbroideryBalance   = sewingMasterViewToInsert.EmbroideryBalance,

                    CutBActualStartDate = sewingMasterViewToInsert.CutBActualStartDate,
                    CutBBalance     = sewingMasterViewToInsert.CutBBalance,
                    AutoCut         = sewingMasterViewToInsert.AutoCut,
                    LaserCut        = sewingMasterViewToInsert.LaserCut,
                    HuasenCut       = sewingMasterViewToInsert.HuasenCut,

                    CutBStartDate = sewingMasterViewToInsert.CutBStartDate,
                    AtomCutA = sewingMasterViewToInsert.AtomCutA,
                    AtomCutB = sewingMasterViewToInsert.AtomCutB,
                    LaserCutA = sewingMasterViewToInsert.LaserCutA,
                    LaserCutB = sewingMasterViewToInsert.LaserCutB,
                    HuasenCutA = sewingMasterViewToInsert.HuasenCutA,
                    HuasenCutB = sewingMasterViewToInsert.HuasenCutB,
                    ComelzCutA = sewingMasterViewToInsert.ComelzCutA,
                    ComelzCutB = sewingMasterViewToInsert.ComelzCutB,

                    IsSequenceUpdate = false,
                    IsSewingLineUpdate = false,
                    IsSewingStartDateUpdate = false,
                    IsSewingFinishDateUpdate = false,
                    IsSewingQuotaUpdate = false,
                    IsSewingPrepUpdate = false,

                    IsSewingActualStartDateUpdate = false,
                    IsSewingActualFinishDateUpdate = false,

                    IsSewingActualStartDateAutoUpdate = false,
                    IsSewingActualFinishDateAutoUpdate = false,

                    IsSewingBalanceUpdate = false,
                    IsCutAStartDateUpdate = false,
                    IsCutAFinishDateUpdate = false,
                    IsCutAQuotaUpdate = false,
                    IsCutAActualStartDateUpdate = false,
                    IsCutAActualFinishDateUpdate = false,
                    IsCutABalanceUpdate = false,
                    IsPrintingBalanceUpdate = false,
                    IsH_FBalanceUpdate = false,
                    IsEmbroideryBalanceUpdate = false,

                    IsCutBActualStartDateUpdate = false,
                    IsCutBBalanceUpdate = false,
                    IsAutoCutUpdate = false,
                    IsLaserCutUpdate = false,
                    IsHuasenCutUpdate = false,

                    //
                    IsUpdateCutBStartDate = false,
                    IsUpdateAtomCutA = false,
                    IsUpdateAtomCutB = false,
                    IsUpdateLaserCutA = false,
                    IsUpdateLaserCutB = false,
                    IsUpdateHuasenCutA = false,
                    IsUpdateHuasenCutB = false,
                    IsUpdateComelzCutA = false,
                    IsUpdateComelzCutB = false,

                };

                model.IsSequenceUpdate = isSequenceEditing;

                model.IsSewingLineUpdate = sewingLineUpdateList.Contains(productNo);
                model.IsSewingStartDateUpdate = lineSewingEditingList.Contains(sewingLine);
                model.IsSewingFinishDateUpdate = lineSewingEditingList.Contains(sewingLine);
                model.IsSewingQuotaUpdate = sewingQuotaUpdateList.Contains(productNo);

                model.IsSewingPrepUpdate = sewingPrepUpdateList.Contains(productNo);

                model.IsSewingActualStartDateUpdate = sewingActualStartDateUpdateList.Contains(productNo);
                model.IsSewingActualFinishDateUpdate = sewingActualFinishDateUpdateList.Contains(productNo);

                model.IsSewingActualStartDateAutoUpdate = sewingActualStartDateUpdateAutoList.Contains(productNo);
                model.IsSewingActualFinishDateAutoUpdate = sewingActualFinishDateUpdateAutoList.Contains(productNo);

                model.IsSewingBalanceUpdate = sewingBalanceUpdateList.Contains(productNo);

                model.IsCutAStartDateUpdate = lineCutPrepEditingList.Contains(sewingLine);
                model.IsCutAFinishDateUpdate = lineCutPrepEditingList.Contains(sewingLine);
                model.IsCutAQuotaUpdate = cutAQuotaUpdateList.Contains(productNo);
                model.IsCutAActualStartDateUpdate = cutAActualStartDateUpdateList.Contains(productNo);
                model.IsCutAActualFinishDateUpdate = cutAActualFinishDateUpdateList.Contains(productNo);
                model.IsCutABalanceUpdate = cutABalanceUpdateList.Contains(productNo);

                model.IsPrintingBalanceUpdate = printingBalanceUpdateList.Contains(productNo);
                model.IsH_FBalanceUpdate = h_fBalanceUpdateList.Contains(productNo);
                model.IsEmbroideryBalanceUpdate = embroideryBalanceUpdateList.Contains(productNo);
                model.IsCutBActualStartDateUpdate = cutBActualStartDateUpdateList.Contains(productNo);
                model.IsCutBBalanceUpdate = cutBBalanceUpdateList.Contains(productNo);
                model.IsAutoCutUpdate = autoCutUpdateList.Contains(productNo);
                model.IsLaserCutUpdate = laserCutUpdateList.Contains(productNo);
                model.IsHuasenCutUpdate = huasenCutUpdateList.Contains(productNo);

                model.IsUpdateCutBStartDate = cutBStartDateUpdateList.Contains(productNo);
                model.IsUpdateAtomCutA = atomCutAUpdateList.Contains(productNo);
                model.IsUpdateAtomCutB = atomCutBUpdateList.Contains(productNo);
                model.IsUpdateLaserCutA = laserCutAUpdateList.Contains(productNo);
                model.IsUpdateLaserCutB = laserCutBUpdateList.Contains(productNo);
                model.IsUpdateHuasenCutA = huasenCutAUpdateList.Contains(productNo);
                model.IsUpdateHuasenCutB = huasenCutBUpdateList.Contains(productNo);
                model.IsUpdateComelzCutA = comelzCutAUpdateList.Contains(productNo);
                model.IsUpdateComelzCutB = comelzCutBUpdateList.Contains(productNo);
                //&& sequenceUpdateList.Contains(model.Sequence)
                if ((model.IsSequenceUpdate == true) ||
                    model.IsSewingLineUpdate == true ||
                    model.IsSewingStartDateUpdate == true ||
                    model.IsSewingFinishDateUpdate == true ||
                    model.IsSewingQuotaUpdate == true ||

                    model.IsSewingPrepUpdate == true ||

                    model.IsSewingActualStartDateUpdate == true ||
                    model.IsSewingActualFinishDateUpdate == true ||

                    model.IsSewingActualStartDateAutoUpdate == true ||
                    model.IsSewingActualFinishDateAutoUpdate == true ||

                    model.IsSewingBalanceUpdate == true ||
                    model.IsCutAStartDateUpdate == true ||
                    model.IsCutAFinishDateUpdate == true ||
                    model.IsCutAQuotaUpdate == true ||
                    model.IsCutAActualStartDateUpdate == true ||
                    model.IsCutAActualFinishDateUpdate == true ||
                    model.IsCutBActualStartDateUpdate == true ||
                    model.IsCutABalanceUpdate == true ||
                    model.IsPrintingBalanceUpdate == true ||
                    model.IsH_FBalanceUpdate == true ||
                    model.IsEmbroideryBalanceUpdate == true ||
                    model.IsCutBBalanceUpdate == true ||
                    model.IsAutoCutUpdate == true ||
                    model.IsLaserCutUpdate == true ||
                    model.IsHuasenCutUpdate == true ||

                    //
                    model.IsUpdateCutBStartDate == true ||
                    model.IsUpdateAtomCutA == true ||
                    model.IsUpdateAtomCutB == true ||
                    model.IsUpdateLaserCutA == true ||
                    model.IsUpdateLaserCutB == true ||
                    model.IsUpdateHuasenCutA == true ||
                    model.IsUpdateHuasenCutB == true ||
                    model.IsUpdateComelzCutA == true ||
                    model.IsUpdateComelzCutB == true
                    )
                {
                    model.Reviser = account.UserName;
                    SewingMasterController.Insert_2(model, account);
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
            lineSewingEditingList.Clear();
            lineCutPrepEditingList.Clear();

            sewingLineUpdateList.Clear();
            sewingQuotaUpdateList.Clear();

            sewingPrepUpdateList.Clear();

            sewingActualStartDateUpdateList.Clear();
            sewingActualFinishDateUpdateList.Clear();

            sewingActualStartDateUpdateAutoList.Clear();
            sewingActualFinishDateUpdateAutoList.Clear();

            sewingBalanceUpdateList.Clear();

            cutAQuotaUpdateList.Clear();
            cutAActualStartDateUpdateList.Clear();
            cutAActualFinishDateUpdateList.Clear();
            cutABalanceUpdateList.Clear();

            printingBalanceUpdateList.Clear();
            h_fBalanceUpdateList.Clear();
            embroideryBalanceUpdateList.Clear();
            cutBActualStartDateUpdateList.Clear();
            cutBBalanceUpdateList.Clear();

            autoCutUpdateList.Clear();
            laserCutUpdateList.Clear();
            huasenCutUpdateList.Clear();

            cutBStartDateUpdateList.Clear();

            atomCutAUpdateList.Clear();
            atomCutBUpdateList.Clear();

            laserCutAUpdateList.Clear();
            laserCutBUpdateList.Clear();

            huasenCutAUpdateList.Clear();
            huasenCutBUpdateList.Clear();

            comelzCutAUpdateList.Clear();
            comelzCutBUpdateList.Clear();

            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);

            lblStatus.Text = "";
            prgStatus.Visibility = Visibility.Collapsed;
        }

        private void dgSewingMaster_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private List<SewingMasterViewModel> sewingMasterViewSelectList = new List<SewingMasterViewModel>();
        private void dgSewingMaster_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
            sewingMasterViewSelectList.Clear();
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null)
            {
                foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
                {
                    sewingMasterViewSelectList.Add((SewingMasterViewModel)cellInfo.Item);
                }
                sewingMasterViewSelectList = sewingMasterViewSelectList.Distinct().ToList();
            }
        }

        private void dgSewingMaster_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && account.SewingMaster == true && isEditing == false)
            {
                var dataGrid = (DataGrid)sender;
                if (dataGrid != null)
                {
                    if (e.OriginalSource.GetType() == typeof(Thumb))
                    {
                        return;
                    }
                    if ((SewingMasterViewModel)dataGrid.CurrentItem != null && sewingMasterViewSelectList.Contains((SewingMasterViewModel)dataGrid.CurrentItem) == false)
                    {
                        sewingMasterViewSelectList.Add((SewingMasterViewModel)dataGrid.CurrentItem);
                    }
                    if (sewingMasterViewSelectList.Count > 0)
                    {
                        listView.ItemsSource = sewingMasterViewSelectList;
                        popup.PlacementTarget = lblPopup;
                        DragDrop.DoDragDrop(dataGrid, sewingMasterViewSelectList, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgSewingMaster_DragLeave(object sender, DragEventArgs e)
        {
            var frameworkElement = (FrameworkElement)e.OriginalSource;

            if (frameworkElement != null && frameworkElement.DataContext != null
                && frameworkElement.DataContext.GetType() == typeof(SewingMasterViewModel))
            {
                SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)frameworkElement.DataContext;
                dgSewingMaster.SelectedItem = sewingMasterView;
                dgSewingMaster.ScrollIntoView(sewingMasterView);
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
                var sewingMasterView = (SewingMasterViewModel)dataGrid.SelectedItem;
                int index = dataGrid.Items.IndexOf(sewingMasterView);
                int indexFirst = dataGrid.Items.IndexOf(sewingMasterViewSelectList.First());
                int indexLast = dataGrid.Items.IndexOf(sewingMasterViewSelectList.Last());
                if (index < indexFirst && index < indexLast)
                {
                    for (int i = sewingMasterViewSelectList.Count - 1; i >= 0; i = i - 1)
                    {
                        sewingMasterViewFindList.Remove(sewingMasterViewSelectList[i]);
                        sewingMasterViewFindList.Insert(index, sewingMasterViewSelectList[i]);
                        sewingMasterViewSelectList[i].Sequence = sewingMasterView.Sequence + i;
                        //sequenceUpdateList.Add(sewingMasterViewFindList[i].Sequence);
                    }
                    for (int i = index + sewingMasterViewSelectList.Count; i <= sewingMasterViewFindList.Count - 1; i++)
                    {
                        sewingMasterViewFindList[i].Sequence = sewingMasterViewFindList[i].Sequence + sewingMasterViewSelectList.Count;
                        //sequenceUpdateList.Add(sewingMasterViewFindList[i].Sequence);
                    }
                    isSequenceEditing = true;
                    //changingSequence = true;
                }
                else if (index > indexFirst && index > indexLast)
                {
                    for (int i = 0; i <= sewingMasterViewSelectList.Count - 1; i = i + 1)
                    {
                        sewingMasterViewFindList.Remove(sewingMasterViewSelectList[i]);
                        sewingMasterViewFindList.Insert(index - 1, sewingMasterViewSelectList[i]);
                        sewingMasterViewSelectList[i].Sequence = sewingMasterView.Sequence + i;
                        //sequenceUpdateList.Add(sewingMasterViewFindList[i].Sequence);
                    }
                    for (int i = index; i <= sewingMasterViewFindList.Count - 1; i++)
                    {
                        sewingMasterViewFindList[i].Sequence = sewingMasterViewFindList[i].Sequence + sewingMasterViewSelectList.Count;
                        //sequenceUpdateList.Add(sewingMasterViewFindList[i].Sequence);
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
            if ((isSequenceEditing == true || lineSewingEditingList.Count > 0 || lineCutPrepEditingList.Count > 0 ||

                sewingLineUpdateList.Count > 0 || sewingQuotaUpdateList.Count > 0 || sewingActualStartDateUpdateList.Count > 0 ||
                sewingActualFinishDateUpdateList.Count > 0 || sewingBalanceUpdateList.Count > 0 ||

                cutAQuotaUpdateList.Count > 0 || cutAActualStartDateUpdateList.Count > 0 || cutAActualFinishDateUpdateList.Count > 0 ||
                cutABalanceUpdateList.Count > 0 ||

                printingBalanceUpdateList.Count > 0 || h_fBalanceUpdateList.Count > 0 || embroideryBalanceUpdateList.Count > 0 ||
                cutBBalanceUpdateList.Count > 0 || autoCutUpdateList.Count > 0 || atomCutAUpdateList.Count > 0 || atomCutBUpdateList.Count > 0 ||
                laserCutAUpdateList.Count > 0 || laserCutBUpdateList.Count > 0 || huasenCutAUpdateList.Count > 0 || huasenCutBUpdateList.Count > 0 ||
                comelzCutAUpdateList.Count() > 0 || comelzCutBUpdateList.Count() > 0 ||
                cutBStartDateUpdateList.Count > 0) && simulationMode == false)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (bwInsert.IsBusy == false)
                    {
                        e.Cancel = true;
                        this.Cursor = Cursors.Wait;
                        sewingMasterViewToInsertList = dgSewingMaster.Items.OfType<SewingMasterViewModel>().ToList();
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
                    SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)dataGridCellInfo.Item;
                    if (sewingMasterView != null)
                    {
                        sewingMasterView.ProductNoBackground = Brushes.Transparent;
                        productNoPrintList.RemoveAll(r => r.Equals(sewingMasterView.ProductNo));
                    }
                }
            }
            foreach (DataGridCellInfo dataGridCellInfo in e.AddedCells)
            {
                SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)dataGridCellInfo.Item;
                if (sewingMasterView != null)
                {
                    sewingMasterView.ProductNoBackground = Brushes.RoyalBlue;
                    if (!productNoPrintList.Contains(sewingMasterView.ProductNo))
                        productNoPrintList.Add(sewingMasterView.ProductNo);
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

        private void miTranfer_Click(object sender, RoutedEventArgs e)
        {
            sewingMasterViewToInsertList = dgSewingMaster.SelectedItems.OfType<SewingMasterViewModel>().ToList();
            if (sewingMasterViewToInsertList.Count <= 0 ||
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

        bool simulationMode = false;
        string title = "";
        private void btnEnableSimulation_Click(object sender, RoutedEventArgs e)
        {
            dgSewingMaster.AlternatingRowBackground = Brushes.White;
            dgSewingMaster.RowBackground = Brushes.White;

            title = "Master Schedule - Sewing Simulation File";
            this.Title = title;

            simulationMode = true;

            //btnEnableSimulation.IsEnabled = false;
            //btnDisableSimulation.IsEnabled = true;
            //btnDisableSimulation.Visibility = Visibility.Visible;
            // 1400 200 300 300

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

                title = "Master Schedule - Sewing Master File";
                this.Title = title;

                ctmTranfer.Visibility = Visibility.Collapsed;
                simulationMode = false;

                //btnDisableSimulation.IsEnabled = false;

                btnSave.IsEnabled = false;
                btnCaculate.IsEnabled = false;

                sewingMasterViewList = new List<SewingMasterViewModel>();
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void miPrintSizeRun_Click(object sender, RoutedEventArgs e)
        {
            if (productNoPrintList.Count <= 0 || MessageBox.Show(String.Format("Confirm Print {0} PO ?", productNoPrintList.Count()), this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            PrintSizeRunWindow window = new PrintSizeRunWindow(String.Join("; ", productNoPrintList));
            window.Show();
        }
        
        private void miReport_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var sourceList = dgSewingMaster.ItemsSource.OfType<SewingMasterViewModel>().ToList();
                var sewingMasterExportViewList = new List<SewingMasterExportViewModel>();
                foreach (var item in sourceList)
                {
                    sewingMasterExportViewList.Add(new SewingMasterExportViewModel
                    {
                        Sequence = item.Sequence,
                        ProductNo = item.ProductNo,
                        Country = item.Country,
                        ShoeName = item.ShoeName,
                        ArticleNo = item.ArticleNo,
                        PatternNo = item.PatternNo,
                        Quantity = item.Quantity,
                        ETD = item.ETD,
                        MemoId = item.MemoId,
                        UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", item.UpperMatsArrivalOrginal),
                        IsUpperMatsArrivalOk = false,
                        SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", item.SewingMatsArrivalOrginal),
                        IsSewingMatsArrivalOk = false,
                        OSMatsArrival = item.OSMatsArrival,
                        IsOSMatsArrivalOk = false,
                        SewingLine = item.SewingLine,
                        CutAStartDate = item.CutAStartDate,
                        CutBStartDate = item.CutBStartDate,
                        SewingStartDate = item.SewingStartDate,
                        SewingFinishDate = item.SewingFinishDate,
                        SewingQuota = item.SewingQuota,
                        CutABalance = item.CutABalance,
                        CutBBalance = item.CutBBalance,
                        OSBalance = item.OSBalance
                    });
                }
                SewingMasterReportWindow window = new SewingMasterReportWindow(sewingMasterExportViewList, "Sewing Master File", 1);
                window.Show();
            }));
        }
    }
}