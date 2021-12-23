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
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class UpperWHInventoryWindow : Window
    {
        BackgroundWorker bwLoadData;
        BackgroundWorker bwPreviewUpperArrival;

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
        List<ReportMaterialArrivalModel> matsArrivalList;
        public UpperWHInventoryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            bwPreviewUpperArrival = new BackgroundWorker();
            bwPreviewUpperArrival.DoWork += BwPreviewUpperArrival_DoWork;
            bwPreviewUpperArrival.RunWorkerCompleted += BwPreviewUpperArrival_RunWorkerCompleted;

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
            matsArrivalList = new List<ReportMaterialArrivalModel>();

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
                dpFrom.SelectedDate = DateTime.Now.Date;
                dpTo.SelectedDate = DateTime.Now.Date;
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

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (!bwPreviewUpperArrival.IsBusy)
            {
                var dateFrom = dpFrom.SelectedDate.Value.Date;
                var dateTo = dpTo.SelectedDate.Value.Date;
                dgUpperArrival.Columns.Clear();
                dgUpperArrival.ItemsSource = null;
                this.Cursor = Cursors.Wait;
                btnPreview.IsEnabled = false;
                var pars = new object[] { dateFrom, dateTo };
                bwPreviewUpperArrival.RunWorkerAsync(pars);
            }
        }
        private void BwPreviewUpperArrival_DoWork(object sender, DoWorkEventArgs e)
        {
            var pars = e.Argument as object[];
            var dateFrom = (DateTime)pars[0];
            var dateTo = (DateTime)pars[1];
            if (eMats == EMaterial.Upper)
                matsArrivalList = ReportController.GetUpperMatsArrivalFromTo(dateFrom, dateTo);
            else if (eMats == EMaterial.Outsole)
                matsArrivalList = ReportController.GetOutsoleMatsArrivalFromTo_1(dateFrom, dateTo);

            if (matsArrivalList.Count() <= 0)
                return;

            Dispatcher.Invoke(new Action(() =>
            {
                var dt = new DataTable();
                dt.Columns.Add("Line", typeof(String));
                DataGridTextColumn colSewLine = new DataGridTextColumn();
                if (eMats == EMaterial.Upper)
                    colSewLine.Header = "Sewing Line";
                else if (eMats == EMaterial.Outsole)
                    colSewLine.Header = "Outsole Line";

                //colSewLine.Width = 120;
                colSewLine.Binding = new Binding("Line");
                colSewLine.ClipboardContentBinding = new Binding("Line");
                dgUpperArrival.Columns.Add(colSewLine);

                int colId = 0;
                for(DateTime date = dateFrom; date <= dateTo; date = date.AddDays(8)) 
                {
                    var month = String.Format(new CultureInfo("en-US"), "{0:MMM}", date);
                    var header = string.Format("{0} {1:dd}-{2:dd}", month, date, date.AddDays(7));
                    dt.Columns.Add(string.Format("Column{0}", colId), typeof(string));
                    var col = new DataGridTextColumn();
                    col.Header = header;
                    col.Binding = new Binding(string.Format("Column{0}", colId));
                    col.ClipboardContentBinding = new Binding(string.Format("Column{0}", colId));
                    dgUpperArrival.Columns.Add(col);
                    colId++;
                }

                var regex = new Regex("[a-z]|[A-Z]");
                foreach(var item in matsArrivalList)
                {
                    int sewingLineNo = 0, outsoleLineNo = 0;
                    if (!String.IsNullOrEmpty(item.SewingLine))
                    {
                        var x = regex.Replace(item.SewingLine, "");
                        int.TryParse(x, out sewingLineNo);
                        item.SewingLineNo = sewingLineNo;
                    }
                    if(!string.IsNullOrEmpty(item.OutsoleLine))
                    {
                        var y = regex.Replace(item.OutsoleLine, "");
                        int.TryParse(y, out outsoleLineNo);
                        item.OutsoleLineNo = outsoleLineNo;
                    }
                }
                if (eMats == EMaterial.Upper)
                    matsArrivalList = matsArrivalList.OrderBy(o => o.SewingLineNo).ThenBy(th => th.SewingLine).ToList();
                else if (eMats == EMaterial.Outsole)
                    matsArrivalList = matsArrivalList.OrderBy(o => o.OutsoleLineNo).ThenBy(th => th.OutsoleLine).ToList();
                List<string> lines = new List<String>();
                if (eMats == EMaterial.Upper)
                    lines = matsArrivalList.Select(s => s.SewingLine).Distinct().ToList();
                else if (eMats == EMaterial.Outsole)
                    lines = matsArrivalList.Select(s => s.OutsoleLine).Distinct().ToList();

                var osMatsTotalList = new List<OSMatsArrivedModel>();
                foreach (var line in lines)
                {
                    var dr = dt.NewRow();
                    dr["Line"] = line;
                    int colBindIdDetail = 0;
                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(8))
                    {
                        var totalPairsByLineByDate = 0;

                        if (eMats == EMaterial.Upper)
                            totalPairsByLineByDate = matsArrivalList.Where(w => w.SewingLine == line
                                                                                && w.DateDisplay >= date && w.DateDisplay < date.AddDays(8)
                                                                                && w.Status.Equals("OK"))
                                                                         .Sum(s => s.Quantity);
                        else if (eMats == EMaterial.Outsole)
                        {
                            var osMatsFromTo = matsArrivalList.Where(w => w.OutsoleLine == line && w.DateDisplay >= date && w.DateDisplay < date.AddDays(8)).ToList();
                            totalPairsByLineByDate = osMatsFromTo.Where(w => w.Status.Equals("OK")).Sum(s => s.Quantity);

                            var osMatsNotOK = osMatsFromTo.Where(w => w.Status.Equals("NOT_OK")).ToList();
                            var poList = osMatsNotOK.Select(s => s.ProductNo).Distinct().ToList();
                            foreach (var po in poList)
                            {
                                var osMatsByPO = osMatsNotOK.Where(w => w.ProductNo == po).FirstOrDefault();
                                var balance = osMatsByPO.Remarks;
                                int qtyBalance = 0;
                                Int32.TryParse(balance, out qtyBalance);
                                if (qtyBalance > 0)
                                {
                                    int tenPercentOfQty = osMatsByPO.Quantity * 10 / 100;
                                    if (qtyBalance < tenPercentOfQty)
                                    {
                                        totalPairsByLineByDate += (osMatsByPO.Quantity - qtyBalance);
                                    }
                                }
                            }
                        }
                        osMatsTotalList.Add(new OSMatsArrivedModel { Id = colBindIdDetail, Quantity = totalPairsByLineByDate });
                        if (totalPairsByLineByDate > 0)
                            dr[string.Format("Column{0}", colBindIdDetail)] = totalPairsByLineByDate;
                        colBindIdDetail++;
                    }
                    dt.Rows.Add(dr);
                }

                // dr total OK
                var drTotalOK = dt.NewRow();
                drTotalOK["Line"] = "Arrived";
                int colBindId = 0;
                for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(8))
                {
                    //var totalPair = matsArrivalList.Where(w => w.DateDisplay >= date && w.DateDisplay < date.AddDays(8) && w.Status.Equals("OK")).Sum(s => s.Quantity);
                    //if (totalPair > 0)
                    var totalArrived = osMatsTotalList.Where(w => w.Id == colBindId).Sum(s => s.Quantity);
                    if (totalArrived > 0)
                        //drTotalOK[string.Format("Column{0}", colBindId)] = totalPair;
                        drTotalOK[string.Format("Column{0}", colBindId)] = totalArrived;
                        colBindId++;
                }
                dt.Rows.Add(drTotalOK);

                // dr total OK + Plan
                var drTotalOKAndPlan = dt.NewRow();
                drTotalOKAndPlan["Line"] = "Arrived + Plan";
                colBindId = 0;
                for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(8))
                {
                    var totalPair = matsArrivalList.Where(w => w.DateDisplay >= date && w.DateDisplay < date.AddDays(8)).Sum(s => s.Quantity);
                    if (totalPair > 0)
                        drTotalOKAndPlan[string.Format("Column{0}", colBindId)] = totalPair;
                    colBindId++;
                }
                dt.Rows.Add(drTotalOKAndPlan);

                dgUpperArrival.ItemsSource = dt.AsDataView();
                dgUpperArrival.Items.Refresh();

            }));
        }

        class OSMatsArrivedModel
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }

        private void BwPreviewUpperArrival_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnPreview.IsEnabled = true;
        }

        private EMaterial eMats = EMaterial.Upper;
        private void radUpper_Checked(object sender, RoutedEventArgs e)
        {
            eMats = EMaterial.Upper;
        }

        private void radOutsole_Checked(object sender, RoutedEventArgs e)
        {
            eMats = EMaterial.Outsole;
        }

    }
}
