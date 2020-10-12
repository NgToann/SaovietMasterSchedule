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
using MasterSchedule.Helpers;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class UpperWHInventoryWindow : Window
    {
        BackgroundWorker bwLoadData;

        List<SewingOutputModel> sewingOutputList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OrdersModel> orderList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<SockliningInputModel> sockliningInputList;
        List<SizeRunModel> sizeRunList;

        List<RawMaterialViewModelNew> rawMaterialViewModelNewList;

        String BUTTON_WIH_SOCKLINING = "  _Matching With Socklining  ";
        String BUTTON_WIH_OUT_SOCKLINING = "  _Matching Without Socklining  ";

        List<UpperWHInventoryViewModel> upperWHInventoryViewList;
        List<UpperWHInventoryViewModel> upperWHInventoryViewList_WithSocklining;

        public UpperWHInventoryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            sewingOutputList = new List<SewingOutputModel>();
            outsoleOutputList = new List<OutsoleOutputModel>();
            orderList = new List<OrdersModel>();

            assemblyReleaseList = new List<AssemblyReleaseModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            sockliningInputList = new List<SockliningInputModel>();

            upperWHInventoryViewList = new List<UpperWHInventoryViewModel>();
            upperWHInventoryViewList_WithSocklining = new List<UpperWHInventoryViewModel>();
            sizeRunList = new List<SizeRunModel>();

            rawMaterialViewModelNewList = new List<RawMaterialViewModelNew>();
            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            assemblyMasterList  = AssemblyMasterController.Select();
            sewingOutputList    = SewingOutputController.SelectByAssemblyMaster();
            outsoleOutputList   = OutsoleOutputController.SelectByAssemblyMaster();
            assemblyReleaseList = AssemblyReleaseController.SelectByAssemblyMaster();
            orderList           = OrdersController.Select();
            sockliningInputList = SockliningInputController.SelectByAssemblyMaster();
            sizeRunList         = SizeRunController.SelectIsEnable();
            rawMaterialViewModelNewList = RawMaterialController.Select_1();

            List<UpperWHInventoryViewModel> upperWHInventoryList = new List<UpperWHInventoryViewModel>();
            List<UpperWHInventoryViewModel> upperWHInventoryList_1 = new List<UpperWHInventoryViewModel>();
            var POList = orderList.Select(s => s.ProductNo).Distinct().ToList();
            foreach (var productNo in POList)
            {
                var assemblyMaster_PO   = assemblyMasterList.FirstOrDefault(w => w.ProductNo == productNo);
                var assemblyRelease_PO  = assemblyReleaseList.Where(w => w.ProductNo == productNo).ToList();
                var sewingOutput_PO     = sewingOutputList.Where(w => w.ProductNo == productNo).ToList();
                var outsoleOutput_PO    = outsoleOutputList.Where(w => w.ProductNo == productNo).ToList();
                var sockliningInput_PO  = sockliningInputList.Where(w => w.ProductNo == productNo).ToList();
                var sizeNoList          = sizeRunList.Where(w => w.ProductNo == productNo).Select(s => s.SizeNo).ToList();

                int qtyUpper_PO = 0, qtyOutsole_PO = 0, qtySocklining_PO = 0, qtyMatch_PO = 0, qtyMatchWithSocklining_PO = 0;
                foreach (var sizeNo in sizeNoList)
                {
                    int qtyAssemblyRelease_Size = assemblyRelease_PO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity);
                    int qtySewingOutput_Size    = sewingOutput_PO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity);
                    int qtyOutsoleOutput_Size   = outsoleOutput_PO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity);
                    int qtySocklining_Size      = sockliningInput_PO.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity);

                    int qtyUpper = qtySewingOutput_Size - qtyAssemblyRelease_Size;
                    if (qtyUpper < 0)
                        qtyUpper = 0;
                    qtyUpper_PO += qtyUpper;

                    int qtyOutsole = qtyOutsoleOutput_Size - qtyAssemblyRelease_Size;
                    if (qtyOutsole < 0)
                        qtyOutsole = 0;
                    qtyOutsole_PO += qtyOutsole;

                    int qtySocklining = qtySocklining_Size - qtyAssemblyRelease_Size;
                    if (qtySocklining < 0)
                        qtySocklining = 0;
                    qtySocklining_PO += qtySocklining;

                    int qtyMatch = Math.Min(qtySewingOutput_Size, qtyOutsoleOutput_Size) - qtyAssemblyRelease_Size;
                    if (qtyMatch < 0)
                        qtyMatch = 0;
                    qtyMatch_PO += qtyMatch;

                    List<Int32> materialList = new List<Int32>();
                    materialList.Add(qtySewingOutput_Size > 0 ? qtySewingOutput_Size : 0);
                    materialList.Add(qtyOutsoleOutput_Size > 0 ? qtyOutsoleOutput_Size : 0);
                    materialList.Add(qtySocklining_Size > 0 ? qtySocklining_Size : 0);

                    int qtyMatchWithSocklining = materialList.Min() - qtyAssemblyRelease_Size;
                    if (qtyMatchWithSocklining < 0)
                        qtyMatchWithSocklining = 0;
                    qtyMatchWithSocklining_PO += qtyMatchWithSocklining;
                }

                string assemblyLine_PO = assemblyMaster_PO != null ? assemblyMaster_PO.AssemblyLine : "";
                var upperInventory_PO = new UpperWHInventoryViewModel()
                {
                    AssemblyLine    = assemblyLine_PO,
                    SewingOutput    = qtyUpper_PO,
                    OutsoleOutput   = qtyOutsole_PO,
                    SockliningInput = qtySocklining_PO,
                    Matching        = qtyMatch_PO,
                    MatchingWithSocklining = qtyMatchWithSocklining_PO,
                };
                upperWHInventoryList.Add(upperInventory_PO);
            }

            var assemblyLineList = upperWHInventoryList.Select(s => s.AssemblyLine).Distinct().ToList();
            if (assemblyLineList.Count() > 0)
                assemblyLineList = assemblyLineList.OrderBy(o => o).ToList();
            var regex = new Regex(@"\D");
            var assemblyLineCustomList = assemblyLineList.Select(s => new { Line = s, LineNumber = regex.IsMatch(s) ? regex.Replace(s, "") : s }).ToList();
            if (assemblyLineCustomList.Count() > 0)
                assemblyLineCustomList = assemblyLineCustomList.OrderBy(o => String.IsNullOrEmpty(o.LineNumber) ? 100 : Int32.Parse(o.LineNumber)).ThenBy(th => th.Line).ToList();

            foreach (var assemblyLineCustom in assemblyLineCustomList)
            {
                var assemblyLine = assemblyLineCustom.Line;
                var upperWHInventoryByAssemblyLine = upperWHInventoryList.Where(w => w.AssemblyLine == assemblyLine).ToList();
                var productNoList_Assembly = assemblyMasterList.Where(w => w.AssemblyLine == assemblyLine).Select(s => s.ProductNo).Distinct().ToList();
                var upperInventory = new UpperWHInventoryViewModel()
                {
                    AssemblyLine    = assemblyLine,
                    ProductNoList   = productNoList_Assembly,
                    SewingOutput    = upperWHInventoryByAssemblyLine.Sum(s => s.SewingOutput),
                    OutsoleOutput   = upperWHInventoryByAssemblyLine.Sum(s => s.OutsoleOutput),
                    SockliningInput = upperWHInventoryByAssemblyLine.Sum(s => s.SockliningInput),
                    Matching        = upperWHInventoryByAssemblyLine.Sum(s => s.Matching),
                };
                upperWHInventoryViewList.Add(upperInventory);

                var upperInventory_1 = new UpperWHInventoryViewModel()
                {
                    AssemblyLine    = assemblyLine,
                    ProductNoList   = productNoList_Assembly,
                    SewingOutput    = upperWHInventoryByAssemblyLine.Sum(s => s.SewingOutput),
                    OutsoleOutput   = upperWHInventoryByAssemblyLine.Sum(s => s.OutsoleOutput),
                    SockliningInput = upperWHInventoryByAssemblyLine.Sum(s => s.SockliningInput),
                    Matching        = upperWHInventoryByAssemblyLine.Sum(s => s.MatchingWithSocklining),
                };
                upperWHInventoryViewList_WithSocklining.Add(upperInventory_1);
            }
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgInventory.ItemsSource = upperWHInventoryViewList;

            lblSewingOutput.Text    = upperWHInventoryViewList.Sum(u => u.SewingOutput).ToString();
            lblOutsoleOutput.Text   = upperWHInventoryViewList.Sum(u => u.OutsoleOutput).ToString();
            lblSocklining.Text      = upperWHInventoryViewList.Sum(u => u.SockliningInput).ToString();

            lblMatching.Text = upperWHInventoryViewList.Sum(o => o.Matching).ToString();
            
            this.Cursor = null;
            btnPrint.IsEnabled = true;
            btnMatchingWithSocklining.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void dgInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpperWHInventoryViewModel upperWHInventoryView = (UpperWHInventoryViewModel)dgInventory.CurrentItem;
            if (upperWHInventoryView != null)
            {
                List<String> productNoList = upperWHInventoryView.ProductNoList;
                UpperWHInventoryDetailWindow window = new UpperWHInventoryDetailWindow(
                    productNoList,
                    sewingOutputList.Where(s => productNoList.Contains(s.ProductNo)).ToList(),
                    outsoleOutputList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    assemblyReleaseList.Where(a => productNoList.Contains(a.ProductNo)).ToList(),
                    orderList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    rawMaterialViewModelNewList.Where(w => productNoList.Contains(w.ProductNo)).ToList()
                    );
                window.Title = String.Format("{0} for {1}", window.Title, upperWHInventoryView.AssemblyLine);
                window.Show();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            UpperWHInventoryDetailReportWindow upperWindow = new UpperWHInventoryDetailReportWindow();
            upperWindow.Show();
        }

        private void btnMatchingWithSocklining_Click(object sender, RoutedEventArgs e)
        {
            btnMatchingWithSocklining.IsEnabled = false;
            dgInventory.ItemsSource = null;

            if (btnMatchingWithSocklining.Content.ToString() == BUTTON_WIH_SOCKLINING)
            {
                btnMatchingWithSocklining.Content = BUTTON_WIH_OUT_SOCKLINING;

                dgInventory.ItemsSource = upperWHInventoryViewList_WithSocklining;
                lblSewingOutput.Text    = upperWHInventoryViewList_WithSocklining.Sum(u => u.SewingOutput).ToString();
                lblOutsoleOutput.Text   = upperWHInventoryViewList_WithSocklining.Sum(u => u.OutsoleOutput).ToString();
                lblSocklining.Text      = upperWHInventoryViewList_WithSocklining.Sum(u => u.SockliningInput).ToString();
                lblMatching.Text        = upperWHInventoryViewList_WithSocklining.Sum(o => o.Matching).ToString();
                
                btnMatchingWithSocklining.IsEnabled = true;
                return;
            }

            if (btnMatchingWithSocklining.Content.ToString() == BUTTON_WIH_OUT_SOCKLINING)
            {
                btnMatchingWithSocklining.Content = BUTTON_WIH_SOCKLINING;

                dgInventory.ItemsSource = upperWHInventoryViewList;
                lblSewingOutput.Text    = upperWHInventoryViewList.Sum(u => u.SewingOutput).ToString();
                lblOutsoleOutput.Text   = upperWHInventoryViewList.Sum(u => u.OutsoleOutput).ToString();
                lblSocklining.Text      = upperWHInventoryViewList.Sum(u => u.SockliningInput).ToString();
                lblMatching.Text        = upperWHInventoryViewList.Sum(o => o.Matching).ToString();

                btnMatchingWithSocklining.IsEnabled = true;
                return;
            }
        }
    }
}
