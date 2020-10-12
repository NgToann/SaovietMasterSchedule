using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
using System.Globalization;
using System.Collections.ObjectModel;
using MasterSchedule.Helpers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SewingMasterReportWindow.xaml
    /// </summary>
    public partial class CutprepMasterFilterWindow : Window
    {
        BackgroundWorker bwLoad;
        List<OrdersModel> orderList;
        List<RawMaterialModel> rawMaterialList;
        List<SewingMasterModel> sewingMasterList;
        List<LineFilterViewModel> lineFilterViewList;
        List<ETDFilterViewModel> etdFilterViewList;
        List<CutprepMasterExportViewModel> cutprepMasterExportViewList;
        ObservableCollection<CutprepMasterExportViewModel> cutprepMasterExportViewFilteredList;
        DateTime dtDefault;
        DateTime dtNothing;
        List<ProductionMemoModel> productionMemoList;
        List<OffDayModel> offDayList;

        private PrivateDefineModel privateDefine;
        private int _SEW_VS_OTHERS_CUT_A    = 0;
        private int _SEW_VS_OTHERS_CUT_B    = 0;

        public CutprepMasterFilterWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            orderList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            sewingMasterList = new List<SewingMasterModel>();
            lineFilterViewList = new List<LineFilterViewModel>();
            etdFilterViewList = new List<ETDFilterViewModel>();
            cutprepMasterExportViewList = new List<CutprepMasterExportViewModel>();
            cutprepMasterExportViewFilteredList = new ObservableCollection<CutprepMasterExportViewModel>();
            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            productionMemoList = new List<ProductionMemoModel>();
            offDayList = new List<OffDayModel>();
            privateDefine = new PrivateDefineModel();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnFilter.IsEnabled = false;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                orderList = OrdersController.Select();
                rawMaterialList = RawMaterialController.Select();
                sewingMasterList = SewingMasterController.Select();
                productionMemoList = ProductionMemoController.Select();
                offDayList = OffDayController.Select();
                privateDefine = PrivateDefineController.GetDefine();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
                return;
            }

            _SEW_VS_OTHERS_CUT_A = privateDefine.SewingVsOthersCutTypeA;
            _SEW_VS_OTHERS_CUT_B = privateDefine.SewingVsOthersCutTypeB;

            //sewingMasterList.RemoveAll(s => DateTimeHelper.Create(s.SewingBalance) != dtDefault && DateTimeHelper.Create(s.SewingBalance) != dtNothing);
            sewingMasterList = sewingMasterList.OrderBy(s => s.Sequence).ToList();

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };

            foreach (SewingMasterModel sewingMaster in sewingMasterList)
            {
                CutprepMasterExportViewModel cutprepMasterExportView = new CutprepMasterExportViewModel();
                cutprepMasterExportView.Sequence = sewingMaster.Sequence;
                cutprepMasterExportView.ProductNo = sewingMaster.ProductNo;
                OrdersModel order = orderList.FirstOrDefault(f => f.ProductNo == sewingMaster.ProductNo);
                string memoId = "";
                if (order != null)
                {
                    cutprepMasterExportView.Country = order.Country;
                    cutprepMasterExportView.ShoeName = order.ShoeName;
                    cutprepMasterExportView.ArticleNo = order.ArticleNo;
                    cutprepMasterExportView.PatternNo = order.PatternNo;
                    cutprepMasterExportView.Quantity = order.Quantity;
                    cutprepMasterExportView.ETD = order.ETD;

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
                    cutprepMasterExportView.MemoId = memoId;
                }

                MaterialArrivalViewModel materialArrivalUpper = MaterialArrival(order.ProductNo, materialIdUpperArray);
                cutprepMasterExportView.IsUpperMatsArrivalOk = false;
                if (materialArrivalUpper != null)
                {
                    cutprepMasterExportView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalUpper.Date);
                    cutprepMasterExportView.IsUpperMatsArrivalOk = materialArrivalUpper.IsMaterialArrivalOk;
                }
                cutprepMasterExportView.SewingLine = sewingMaster.SewingLine;
                cutprepMasterExportView.SewingStartDate = sewingMaster.SewingStartDate;
                cutprepMasterExportView.SewingQuota = sewingMaster.SewingQuota;
                cutprepMasterExportView.SewingBalance = sewingMaster.SewingBalance;
                cutprepMasterExportView.CutAStartDate = sewingMaster.CutAStartDate;
                cutprepMasterExportView.CutAFinishDate = sewingMaster.CutAFinishDate;
                cutprepMasterExportView.CutAQuota = sewingMaster.CutAQuota;
                cutprepMasterExportView.AutoCut = sewingMaster.AutoCut;
                cutprepMasterExportView.LaserCut = sewingMaster.LaserCut;
                cutprepMasterExportView.HuasenCut = sewingMaster.HuasenCut;
                cutprepMasterExportView.CutABalance = sewingMaster.CutABalance;
                cutprepMasterExportView.PrintingBalance = sewingMaster.PrintingBalance;
                cutprepMasterExportView.H_FBalance = sewingMaster.H_FBalance;
                cutprepMasterExportView.EmbroideryBalance = sewingMaster.EmbroideryBalance;
                cutprepMasterExportView.CutBBalance = sewingMaster.CutBBalance;

                // Cut type A before sewing 18days
                var cutTypeABeforeSewing        = sewingMaster.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                var dtCheckOffDateCutTypeAList = CheckOffDay(cutTypeABeforeSewing, sewingMaster.SewingStartDate);

                // Cut type B before sewing 10days
                var cutTypeBBeforeSewing        = sewingMaster.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                var dtCheckOffDateCutTypeBList  = CheckOffDay(cutTypeBBeforeSewing, sewingMaster.SewingStartDate);

                var firstDateCheckOffCutTypeA = String.Format("{0:M/d}", dtCheckOffDateCutTypeAList.FirstOrDefault());
                var firstDateCheckOffCutTypeB = String.Format("{0:M/d}", dtCheckOffDateCutTypeBList.FirstOrDefault());

                if (!String.IsNullOrEmpty(sewingMaster.CutBStartDate))
                    cutprepMasterExportView.CutBStartDate = sewingMaster.CutBStartDate;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.CutBStartDate = firstDateCheckOffCutTypeB;
                else cutprepMasterExportView.CutBStartDate = "";


                if (!String.IsNullOrEmpty(sewingMaster.AtomCutA))
                    cutprepMasterExportView.AtomCutA = sewingMaster.AtomCutA;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.AtomCutA = firstDateCheckOffCutTypeA;
                else cutprepMasterExportView.AtomCutA = "";


                if (!String.IsNullOrEmpty(sewingMaster.AtomCutB))
                    cutprepMasterExportView.AtomCutB = sewingMaster.AtomCutB;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.AtomCutB = firstDateCheckOffCutTypeB;
                else cutprepMasterExportView.AtomCutB = "";


                if (!String.IsNullOrEmpty(sewingMaster.LaserCutA))
                    cutprepMasterExportView.LaserCutA = sewingMaster.LaserCutA;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.LaserCutA = firstDateCheckOffCutTypeA;
                else cutprepMasterExportView.LaserCutA = "";

                if (!String.IsNullOrEmpty(sewingMaster.LaserCutB))
                    cutprepMasterExportView.LaserCutB = sewingMaster.LaserCutB;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.LaserCutB = firstDateCheckOffCutTypeB;
                else cutprepMasterExportView.LaserCutB = "";


                if (!String.IsNullOrEmpty(sewingMaster.HuasenCutA))
                    cutprepMasterExportView.HuasenCutA = sewingMaster.HuasenCutA;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.HuasenCutA = firstDateCheckOffCutTypeA;
                else cutprepMasterExportView.HuasenCutA = "";


                if (!String.IsNullOrEmpty(sewingMaster.HuasenCutB))
                    cutprepMasterExportView.HuasenCutB = sewingMaster.HuasenCutB;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.HuasenCutB = firstDateCheckOffCutTypeB;
                else cutprepMasterExportView.HuasenCutB = "";

                if (!String.IsNullOrEmpty(sewingMaster.ComelzCutA))
                    cutprepMasterExportView.ComelzCutA = sewingMaster.ComelzCutA;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.ComelzCutA = firstDateCheckOffCutTypeA;
                else cutprepMasterExportView.ComelzCutA = "";

                if (!String.IsNullOrEmpty(sewingMaster.ComelzCutB))
                    cutprepMasterExportView.ComelzCutB = sewingMaster.ComelzCutB;
                else if (sewingMaster.SewingStartDate != dtDefault)
                    cutprepMasterExportView.ComelzCutB = firstDateCheckOffCutTypeB;
                else cutprepMasterExportView.ComelzCutB = "";


                cutprepMasterExportViewList.Add(cutprepMasterExportView);
            }
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            rawMaterialTypeList.RemoveAll(r => r.ETD.Date == dtDefault.Date);
            MaterialArrivalViewModel materialArrivalView = new MaterialArrivalViewModel();
            materialArrivalView.IsMaterialArrivalOk = false;
            if (rawMaterialTypeList.Select(r => r.ActualDate).Count() > 0 && rawMaterialTypeList.Select(r => r.ActualDate.Date).Contains(dtDefault.Date) == false)
            {
                materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ActualDate).Max();
                materialArrivalView.IsMaterialArrivalOk = true;
            }
            else
            {
                if (rawMaterialTypeList.Select(r => r.ETD).Count() > 0 && rawMaterialTypeList.Where(r => r.ETD.Date != dtDefault.Date).Count() > 0)
                {
                    materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ETD).Max();
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
            lineFilterViewList.Add(new
                LineFilterViewModel { IsSelected = false, Content = "All", IsRoot = true });
            foreach (string line in cutprepMasterExportViewList.Select(s => s.SewingLine).OrderBy(l => l).Distinct())
            {
                LineFilterViewModel lineFilter = new LineFilterViewModel
                {
                    IsSelected = false,
                    Content = line,
                    IsRoot = false,
                };
                lineFilterViewList.Add(lineFilter);
            }
            lvLine.ItemsSource = lineFilterViewList;

            etdFilterViewList.Add(new
                ETDFilterViewModel { IsSelected = false, Content = "All", IsRoot = true });
            foreach (DateTime etd in cutprepMasterExportViewList.Select(s => s.ETD).OrderBy(d => d).Distinct())
            {
                ETDFilterViewModel etdFilter = new ETDFilterViewModel
                {
                    IsSelected = false,
                    Date = etd,
                    Content = String.Format(new CultureInfo("en-US"), "{0:MM/dd/yyyy}", etd),
                    IsRoot = false,
                };
                etdFilterViewList.Add(etdFilter);
            }
            lvETD.ItemsSource = etdFilterViewList;

            cutprepMasterExportViewFilteredList = new ObservableCollection<CutprepMasterExportViewModel>(cutprepMasterExportViewList);
            //dgMaster.ItemsSource = cutprepMasterExportViewFilteredList;
            btnFilter.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            if (popupLine.IsOpen == false)
            {
                popupLine.IsOpen = true;
                return;
            }
            else
            {
                popupLine.IsOpen = false;
                return;
            }
        }

        bool isAllChecked = false;
        private void chbIsSelected_Checked(object sender, RoutedEventArgs e)
        {
            if (isAllChecked == true)
            {
                return;
            }
            CheckBox chbIsSelected = (CheckBox)sender;
            if (chbIsSelected == null)
            {
                return;
            }
            bool isRoot = chbIsSelected.IsThreeState;
            if (isRoot == true)
            {
                isAllChecked = true;
                bool isChecked = (chbIsSelected.IsChecked == true);
                foreach (LineFilterViewModel lineFilterView in lineFilterViewList)
                {
                    if (lineFilterView.IsRoot == false)
                    {
                        lineFilterView.IsSelected = isChecked;
                    }
                }
                isAllChecked = false;
                return;
            }
            else
            {
                foreach (LineFilterViewModel lineFilterView in lineFilterViewList)
                {
                    if (lineFilterView.IsRoot == true)
                    {
                        lineFilterView.IsSelected = null;
                    }
                }
                if (lineFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().Count() == 1)
                {
                    foreach (LineFilterViewModel lineFilterView in lineFilterViewList)
                    {
                        if (lineFilterView.IsRoot == true)
                        {
                            lineFilterView.IsSelected = lineFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().FirstOrDefault();
                        }
                    }
                }
                return;
            }
        }   

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            List<LineFilterViewModel> lineFilterViewSelectedList = lineFilterViewList.Where(l => l.IsSelected == true && l.IsRoot == false).ToList();
            if (lineFilterViewSelectedList.Count() == 0)
            {
                return;
            }
            string lineSelected = "";
            foreach (LineFilterViewModel lineFilterView in lineFilterViewSelectedList)
            {
                lineSelected += lineFilterView.Content + "; ";
            }
            lblLine.ToolTip = lineSelected;
            if (lineFilterViewSelectedList.Count() == 1)
            {
                lblLine.Text = lineSelected;
            }
            else
            {
                lblLine.Text = "(Multiple Items)";
            }
            popupLine.IsOpen = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            popupLine.IsOpen = false;
        }

        private void btnETD_Click(object sender, RoutedEventArgs e)
        {
            if (popupETD.IsOpen == false)
            {
                popupETD.IsOpen = true;
                return;
            }
            else
            {
                popupETD.IsOpen = false;
                return;
            }
        }

        bool isAllETDChecked = false;
        private void chbETDIsSelected_Checked(object sender, RoutedEventArgs e)
        {
            if (isAllETDChecked == true)
            {
                return;
            }
            CheckBox chbETDIsSelected = (CheckBox)sender;
            if (chbETDIsSelected == null)
            {
                return;
            }
            bool isRoot = chbETDIsSelected.IsThreeState;
            if (isRoot == true)
            {
                isAllETDChecked = true;
                bool isChecked = (chbETDIsSelected.IsChecked == true);
                foreach (ETDFilterViewModel etdFilterView in etdFilterViewList)
                {
                    if (etdFilterView.IsRoot == false)
                    {
                        etdFilterView.IsSelected = isChecked;
                    }
                }
                isAllETDChecked = false;
                return;
            }
            else
            {
                foreach (ETDFilterViewModel etdFilterView in etdFilterViewList)
                {
                    if (etdFilterView.IsRoot == true)
                    {
                        etdFilterView.IsSelected = null;
                    }
                }
                if (etdFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().Count() == 1)
                {
                    foreach (ETDFilterViewModel etdFilterView in etdFilterViewList)
                    {
                        if (etdFilterView.IsRoot == true)
                        {
                            etdFilterView.IsSelected = etdFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().FirstOrDefault();
                        }
                    }
                }
                return;
            }
        } 

        private void btnETDOK_Click(object sender, RoutedEventArgs e)
        {
            List<ETDFilterViewModel> etdFilterViewSelectedList = etdFilterViewList.Where(d => d.IsSelected == true && d.IsRoot == false).ToList();
            if (etdFilterViewSelectedList.Count() == 0)
            {
                return;
            }
            string etdSelected = "";
            foreach (ETDFilterViewModel etdFilterView in etdFilterViewSelectedList)
            {
                etdSelected += etdFilterView.Content + "; ";
            }
            lblETD.ToolTip = etdSelected;
            if (etdFilterViewSelectedList.Count() == 1)
            {
                lblETD.Text = etdSelected;
            }
            else
            {
                lblETD.Text = "(Multiple Items)";
            }
            popupETD.IsOpen = false;
        }

        private void btnETDCancel_Click(object sender, RoutedEventArgs e)
        {
            popupETD.IsOpen = false;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            cutprepMasterExportViewFilteredList = new ObservableCollection<CutprepMasterExportViewModel>(cutprepMasterExportViewList.Where(s => (lineFilterViewList.Where(l => l.IsRoot == false && l.IsSelected == true).Select(l => l.Content).Contains(s.SewingLine))
                && (etdFilterViewList.Where(d => d.IsRoot == false && d.IsSelected == true).Select(d => d.Date).Contains(s.ETD))));
            dgMaster.ItemsSource = cutprepMasterExportViewFilteredList;
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            List<CutprepMasterExportViewModel> cutprepMasterExportViewRemoveList = dgMaster.SelectedItems.Cast<CutprepMasterExportViewModel>().ToList();
            if (cutprepMasterExportViewRemoveList.Count <= 0)
            {
                return;
            }
            foreach (CutprepMasterExportViewModel cutprepMasterExportView in cutprepMasterExportViewRemoveList)
            {
                cutprepMasterExportViewList.Remove(cutprepMasterExportView);
                cutprepMasterExportViewFilteredList.Remove(cutprepMasterExportView);
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            List<CutprepMasterExportViewModel> cutprepMasterExportViewReportList = dgMaster.Items.Cast<CutprepMasterExportViewModel>().ToList();
            if (cutprepMasterExportViewReportList.Count() <= 0)
            {
                return;
            }
            var contentPrint = new object[] { cutprepMasterExportViewReportList, lblLine.ToolTip.ToString() };
            ConfirmColumnPrintWindow window = new ConfirmColumnPrintWindow(EPrintSchedule.CutprepMaster, contentPrint);
            window.ShowDialog();
            //CutprepMasterReportWindow window = new CutprepMasterReportWindow(cutprepMasterExportViewReportList, lblLine.ToolTip.ToString());
            //window.Show();
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
    }
}
