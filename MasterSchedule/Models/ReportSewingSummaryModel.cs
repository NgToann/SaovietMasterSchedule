using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportSewingSummaryModel
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName  { get; set; }
        public string Article  { get; set; }
        public string PatternNo  { get; set; }
        public string Quantity  { get; set; }
        public string EFD { get; set; }
        public string MemMo  { get; set; }
        public string UpperMaterialArrival { get; set; }
        public string SewingMaterialArrival { get; set; }
        public string OutsoleMaterialArrival { get; set; }
        public string AssemblyMaterialArrival  { get; set; }
        public string SewingLine { get; set; }
        public string SewingPrep { get; set; }
        public string SewingStartDate { get; set; }
        public string SewingFinishDate { get; set; }
        public string SewingQuota { get; set; }
        public string SewingPlanStartDate { get; set; }
        public string SewingPlanFinishDate { get; set; }
        public string SewingActualStartDate { get; set; }
        public string SewingActualFinishDate { get; set; }
        public string SewingBalance { get; set; }
        public string StockfitFinishDate { get; set; }
        public string StockfitBalance { get; set; }

        public string CutAQuota { get; set; }
        public string AtomCutA { get; set; }
        public string AtomCutB { get; set; }
        public string LaserCutA { get; set; }
        public string LaserCutB { get; set; }
        public string HuasenCutA { get; set; }
        public string HuasenCutB { get; set; }
        public string CutAActualStart { get; set; }
        public string CutAActualFinish { get; set; }
        public string CutABalance { get; set; }
        public string CutprepBalance { get; set; }
        public string CutBBalance { get; set; }
        public string CutAStartDate { get; set; }
        public string CutAFinishDate { get; set; }
        public string AssemblyStartDate { get; set; }
        public string AssemblyFinishDate { get; set; }
        public string AssemblyBalance { get; set; }
        public string OutsoleStartDate { get; set; }
        public string OutsoleFinishDate { get; set; }
}
}
