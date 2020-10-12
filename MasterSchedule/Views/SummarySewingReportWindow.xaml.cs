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
using Microsoft.Reporting.WinForms;
using System.Data;

using MasterSchedule.DataSets;
using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SummarySewingReportWindow.xaml
    /// </summary>
    public partial class SummarySewingReportWindow : Window
    {
        BackgroundWorker bwLoad;
        List<SewingMasterViewModel> sewingMasterViewList;
        List<SewingMasterViewModelNew> sewingMasterViewModelNewList;
        List<RawMaterialViewModelNew> rawMaterialViewModelNewList;
        DateTime  dtDefault = new DateTime(2000, 1, 1);
        List<ProductionMemoModel> productionMemoList;
        List<OffDayModel> offDayList;

        private PrivateDefineModel privateDefine;
       
        private int _SEW_VS_OTHERS_CUT_B = 0;
        private int _SEW_VS_OTHERS_CUT_A = 0;

        public SummarySewingReportWindow()
        {
            sewingMasterViewList = new List<SewingMasterViewModel>();
            sewingMasterViewModelNewList = new List<SewingMasterViewModelNew>();
            productionMemoList = new List<ProductionMemoModel>();
            offDayList = new List<OffDayModel>();
            rawMaterialViewModelNewList = new List<RawMaterialViewModelNew>();
            privateDefine = new PrivateDefineModel();

            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;


            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }        
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                offDayList = OffDayController.Select();
                productionMemoList = ProductionMemoController.Select();
                sewingMasterViewModelNewList = ReportController.GetSewingSummaryReport();
                rawMaterialViewModelNewList = RawMaterialController.Select_1();
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

            //int index = 1;
            DataTable dt = new SewingSummaryDataset().Tables["SewingSummaryDatatable"];
            foreach (var sewNew in sewingMasterViewModelNewList)
            {
                var memoByPOList = productionMemoList.Where(p => p.ProductionNumbers.Contains(sewNew.ProductNo) == true).Select(s => s.MemoId).ToList();
                var sewingMasterView = new SewingMasterViewModel();

                // Order Information
                sewingMasterView.ProductNoBackground = Brushes.Transparent;
                sewingMasterView.ProductNo  = sewNew.ProductNo;
                sewingMasterView.Country    = sewNew.Country;
                sewingMasterView.ShoeName   = sewNew.ShoeName;
                sewingMasterView.ArticleNo  = sewNew.ArticleNo;
                sewingMasterView.PatternNo  = sewNew.PatternNo;
                sewingMasterView.Quantity   = sewNew.Quantity;
                sewingMasterView.ETD        = sewNew.ETD;

                // Sewing Master Information
                sewingMasterView.Sequence                   = sewNew.Sequence;
                sewingMasterView.SewingLine                 = sewNew.SewingLine;
                sewingMasterView.SewingQuota                = sewNew.SewingQuota;
                sewingMasterView.SewingStartDate            = sewNew.SewingStartDate;
                sewingMasterView.SewingFinishDate           = sewNew.SewingFinishDate;
                sewingMasterView.SewingPrep                 = sewNew.SewingPrep;
                sewingMasterView.SewingActualStartDate      = sewNew.SewingActualStartDate;
                sewingMasterView.SewingActualFinishDate     = sewNew.SewingActualFinishDate;
                sewingMasterView.SewingActualStartDateAuto  = sewNew.SewingActualStartDateAuto;
                sewingMasterView.SewingActualFinishDateAuto = sewNew.SewingActualFinishDateAuto;
                sewingMasterView.SewingBalance              = sewNew.SewingBalance;
                sewingMasterView.CutAStartDate              = sewNew.CutAStartDate;
                sewingMasterView.CutAFinishDate             = sewNew.CutAFinishDate;
                sewingMasterView.CutAQuota                  = sewNew.CutAQuota;
                sewingMasterView.CutAActualStartDate        = sewNew.CutAActualStartDate;
                sewingMasterView.CutAActualFinishDate       = sewNew.CutAActualFinishDate;
                sewingMasterView.PrintingBalance            = sewNew.PrintingBalance;
                sewingMasterView.H_FBalance                 = sewNew.H_FBalance;
                sewingMasterView.EmbroideryBalance          = sewNew.EmbroideryBalance;
                sewingMasterView.CutBBalance                = sewNew.CutBBalance;
                sewingMasterView.AutoCut                    = sewNew.AutoCut;
                sewingMasterView.LaserCut                   = sewNew.LaserCut;
                sewingMasterView.HuasenCut                  = sewNew.HuasenCut;
                sewingMasterView.AssemblyBalance            = sewNew.AssemblyBalance;

                // Outsole Master Infor
                sewingMasterView.OSFinishDate   = sewNew.OSFinishDate;
                sewingMasterView.OSBalance      = sewNew.OSBalance;

                sewingMasterView.SewingStartDateForeground  = Brushes.Black;
                sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                sewingMasterView.CutAStartDateForeground    = Brushes.Black;


                //var bfSew10 = sewNew.SewingStartDate.AddDays(-10);
                var bfSew10 = sewNew.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_B);
                var bfSew10IncludeHolidayList = CheckOffDay(bfSew10, sewNew.SewingStartDate);
                //var bfSew15 = sewNew.SewingStartDate.AddDays(-15);
                var bfSew15 = sewNew.SewingStartDate.AddDays(-_SEW_VS_OTHERS_CUT_A);
                var bfSew15IncludeHolidayList = CheckOffDay(bfSew15, sewNew.SewingStartDate);
                var bf10 = bfSew10IncludeHolidayList.FirstOrDefault();
                var bf15 = bfSew15IncludeHolidayList.FirstOrDefault();

                if (string.IsNullOrEmpty(sewNew.CutBStartDate) == false)
                    sewingMasterView.CutBStartDate = sewNew.CutBStartDate;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.CutBStartDate = TimeHelper.ConvertDateToView(new DateTime(bf10.Year, bf10.Month, bf10.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.CutBStartDate = "";
                }

                if (string.IsNullOrEmpty(sewNew.AtomCutA) == false)
                    sewingMasterView.AtomCutA = sewNew.AtomCutA;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.AtomCutA = TimeHelper.ConvertDateToView(new DateTime(bf15.Year, bf15.Month, bf15.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.AtomCutA = "";
                }

                if (string.IsNullOrEmpty(sewNew.AtomCutB) == false)
                    sewingMasterView.AtomCutB = sewNew.AtomCutB;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.AtomCutB = TimeHelper.ConvertDateToView(new DateTime(bf10.Year, bf10.Month, bf10.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.AtomCutB = "";
                }

                if (string.IsNullOrEmpty(sewNew.LaserCutA) == false)
                    sewingMasterView.LaserCutA = sewNew.LaserCutA;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.LaserCutA = TimeHelper.ConvertDateToView(new DateTime(bf15.Year, bf15.Month, bf15.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.LaserCutA = "";
                }
                if (string.IsNullOrEmpty(sewNew.LaserCutB) == false)
                    sewingMasterView.LaserCutB = sewNew.LaserCutB;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.LaserCutB = TimeHelper.ConvertDateToView(new DateTime(bf10.Year, bf10.Month, bf10.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.LaserCutB = "";
                }

                if (string.IsNullOrEmpty(sewNew.HuasenCutA) == false)
                    sewingMasterView.HuasenCutA = sewNew.HuasenCutA;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.HuasenCutA = TimeHelper.ConvertDateToView(new DateTime(bf15.Year, bf15.Month, bf15.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.HuasenCutA = "";
                }
                if (string.IsNullOrEmpty(sewNew.HuasenCutB) == false)
                    sewingMasterView.HuasenCutB = sewNew.HuasenCutB;
                else
                {
                    if (sewNew.SewingStartDate != dtDefault)
                        sewingMasterView.HuasenCutB = TimeHelper.ConvertDateToView(new DateTime(bf10.Year, bf10.Month, bf10.Day).ToString("MM/dd/yyyy"));
                    else
                        sewingMasterView.HuasenCutB = "";
                }

                // UpperMaterial 1, 2, 3, 4, 10
                if (sewNew.UpperMaterialFinisedDate != dtDefault)
                    sewingMasterView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.UpperMaterialFinisedDate);
                else if (sewNew.UpperMaterialETD != dtDefault)
                    sewingMasterView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.UpperMaterialETD);

                //Sewing Material 5, 7
                if (sewNew.SewingMaterialFinisedDate != dtDefault)                
                    sewingMasterView.SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.SewingMaterialFinisedDate);                
                else if (sewNew.SewingMaterialETD != dtDefault)                
                    sewingMasterView.SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.SewingMaterialETD);                

                // Assembly Material 8, 9
                if (sewNew.AssemblyMaterialFinisedDate != dtDefault)                
                    sewingMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.AssemblyMaterialFinisedDate);                
                else if (sewNew.AssemblyMaterialETD != dtDefault)                
                    sewingMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.AssemblyMaterialETD);

                //// Outsole Material 6
                //if (sewNew.OutsoleMaterialFinisedDate != dtDefault)                
                //    sewingMasterView.OSMatsArrival  = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.OutsoleMaterialFinisedDate);                
                //else if (sewNew.OutsoleMaterialETD != dtDefault)                
                //    sewingMasterView.OSMatsArrival  = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", sewNew.OutsoleMaterialETD);

                // Base on rawmaterial
                sewingMasterView.OSMatsArrival = "";
                var osMaterial = rawMaterialViewModelNewList.FirstOrDefault(f => f.ProductNo == sewNew.ProductNo);
                if (String.IsNullOrEmpty(osMaterial.OUTSOLE_Remarks) &&
                    !String.IsNullOrEmpty(osMaterial.OUTSOLE_ActualDate))
                    sewingMasterView.OSMatsArrival = osMaterial.OUTSOLE_ActualDate;
                else if (osMaterial.OUTSOLE_ETD_DATE != dtDefault)
                    sewingMasterView.OSMatsArrival = osMaterial.OUTSOLE_ETD;


                sewingMasterView.OutsoleStartDate   = sewNew.OutsoleStartDate;
                sewingMasterView.OutsoleFinishDate  = sewNew.OutsoleFinishDate;

                sewingMasterView.AssemblyStartDate  = sewNew.AssemblyStartDate;
                sewingMasterView.AssemblyFinishDate = sewNew.AssemblyFinishDate;
                sewingMasterView.OutsoleCode        = sewNew.OutsoleCode;
                sewingMasterViewList.Add(sewingMasterView);

                DataRow dr = dt.NewRow();
                dr["MemoId"]                = memoByPOList.Count() > 0 ? String.Join("\n", memoByPOList) : "";
                dr["ProductNo"]             = sewNew.ProductNo;
                dr["Country"]               = sewNew.Country;
                dr["ShoeName"]              = sewNew.ShoeName;
                dr["ArticleNo"]             = sewNew.ArticleNo;
                dr["PatternNo"]             = sewNew.PatternNo;
                dr["Quantity"]              = sewNew.Quantity;
                dr["ETD"]                   = sewNew.ETD;
                dr["CSD"]                   = sewNew.ETD.AddDays(10);
                dr["UpperMatsArrival"]              = sewingMasterView.UpperMatsArrival;
                dr["SewingMatsArrival"]             = sewingMasterView.SewingMatsArrival;
                dr["OutsoleMatsArrival"]            = sewingMasterView.OSMatsArrival;
                dr["AssemblyMatsArrival"]           = sewingMasterView.AssemblyMatsArrival;
                dr["SewingLine"]                    = sewNew.SewingLine;
                dr["SewingStartDate"]               = sewNew.SewingStartDate;
                dr["SewingFinishDate"]              = sewNew.SewingFinishDate;
                dr["SewingActualStartDate"]         = sewNew.SewingActualStartDate;
                dr["SewingActualFinishDate"]        = sewNew.SewingActualFinishDate;
                dr["SewingActualStartDateAuto"]     = sewNew.SewingActualStartDateAuto;
                dr["SewingActualFinishDateAuto"]    = sewNew.SewingActualFinishDateAuto;
                dr["SewingBalance"]         = sewNew.SewingBalance;
                dr["CutAStartDate"]         = sewNew.CutAStartDate;
                dr["CutAFinishDate"]        = sewNew.CutAFinishDate;
                dr["CutAQuota"]             = sewNew.CutAQuota;
                dr["CutAActualStartDate"]   = sewNew.CutAActualStartDate;
                dr["CutAActualFinishDate"]  = sewNew.CutAActualFinishDate;
                dr["CutABalance"]           = sewNew.CutABalance;
                dr["PrintingBalance"]       = sewNew.PrintingBalance;
                dr["H_FBalance"]            = sewNew.H_FBalance;
                dr["EmbroideryBalance"]     = sewNew.EmbroideryBalance;
                dr["AutoCut"]               = sewNew.AutoCut;
                dr["LaserCut"]              = sewNew.LaserCut;
                dr["HuasenCut"]             = sewNew.HuasenCut;
                dr["CutBStartDate"]         = sewNew.CutBStartDate;
                dr["AtomCutA"]              = sewingMasterView.AtomCutA;
                dr["AtomCutB"]              = sewingMasterView.AtomCutB;
                dr["LaserCutA"]             = sewingMasterView.LaserCutA;
                dr["LaserCutB"]             = sewingMasterView.LaserCutB;
                dr["HuasenCutA"]            = sewingMasterView.HuasenCutA;
                dr["HuasenCutB"]            = sewingMasterView.HuasenCutB;
                dr["OutsoleStartDate"]      = sewNew.OutsoleStartDate;
                dr["OutsoleFinishDate"]     = sewNew.OutsoleFinishDate;
                dr["AssemblyStartDate"]     = sewNew.AssemblyStartDate;
                dr["AssemblyFinishDate"]    = sewNew.AssemblyFinishDate;
                dr["SewingPrep"]            = sewNew.SewingPrep;
                dr["StockfitFinishDate"]    = sewNew.OutsoleFinishDate;
                dr["StockfitBalance"]       = sewNew.OSBalance;
                dr["CutprepBalance"]        = sewNew.H_FBalance;
                dr["SewingQuota"]           = sewNew.SewingQuota;
                dr["OutsoleCode"]           = sewNew.OutsoleCode;
                dr["AssemblyBalance"]       = sewNew.AssemblyBalance;

                dt.Rows.Add(dr);
            }
            sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();

            e.Result = dt;
        }
        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;

            var dt = e.Result as DataTable;
            ReportParameter rp = new ReportParameter("Line", "T");
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "SewingSummaryReportDataset";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\AssemblyMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\SummarySewingReport.rdlc";
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
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
    }
}
