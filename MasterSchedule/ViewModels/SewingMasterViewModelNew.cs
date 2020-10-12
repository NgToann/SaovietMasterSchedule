using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class SewingMasterViewModelNew
    {
        public string MemoId { get; set; }
        public int Sequence { get; set; }
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string OutsoleCode { get; set; }
        public string PatternNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string UpperMatsArrival { get; set; }

        // UpperMaterial
        public DateTime UpperMaterialFinisedDate { get; set; }
        public DateTime UpperMaterialETD { get;set;}
        public string UpperMaterialRemarks { get; set; }

        // SewingMaterial
        public DateTime SewingMaterialFinisedDate { get; set; }
        public DateTime SewingMaterialETD { get; set; }
        public string SewingMaterialRemarks { get; set; }

        // AssemblyMaterial
        public DateTime AssemblyMaterialFinisedDate { get; set; }
        public DateTime AssemblyMaterialETD { get; set; }
        public string AssemblyMaterialRemarks { get; set; }

        // OutsoleMaterial
        public DateTime OutsoleMaterialFinisedDate { get; set; }
        public DateTime OutsoleMaterialETD { get; set; }
        public string OutsoleMaterialRemarks { get; set; }

        public DateTime UpperMatsArrivalOrginal { get; set; }
        public string SewingMatsArrival { get; set; }
        public DateTime SewingMatsArrivalOrginal { get; set; }
        public string OSMatsArrival { get; set; }
        public string AssemblyMatsArrival { get; set; }
        public string SewingLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public DateTime OSFinishDate { get; set; }
        public string OSBalance { get; set; }
        public int SewingQuota { get; set; }
        public string SewingPrep { get; set; }
        public string SewingActualStartDate { get; set; }
        public string SewingActualFinishDate { get; set; }
        public string SewingActualStartDateAuto { get; set; }
        public string SewingActualFinishDateAuto { get; set; }
        public string SewingBalance { get; set; }
        public DateTime CutAStartDate { get; set; }
        public DateTime CutAFinishDate { get; set; }
        public int CutAQuota { get; set; }
        public string CutAActualStartDate { get; set; }
        public string CutAActualFinishDate { get; set; }
        public string CutABalance { get; set; }
        public string PrintingBalance { get; set; }
        public string H_FBalance { get; set; }
        public string EmbroideryBalance { get; set; }
        public string CutBActualStartDate { get; set; }
        public string CutBBalance { get; set; }
        public string AutoCut { get; set; }
        public string LaserCut { get; set; }
        public string HuasenCut { get; set; }
        public string CutBStartDate { get; set; }
        public string AtomCutA { get; set; }
        public string AtomCutB { get; set; }
        public string LaserCutA { get; set; }
        public string LaserCutB { get; set; }
        public string HuasenCutA { get; set; }
        public string HuasenCutB { get; set; }

        public string ComelzCutA { get; set; }
        public string ComelzCutB { get; set; }
        public DateTime OutsoleStartDate { get; set; }
        public DateTime OutsoleFinishDate { get; set; }
        public DateTime AssemblyStartDate { get; set; }
        public DateTime AssemblyFinishDate { get; set; }
        public string StockfitBalance { get; set; }
        public string CutprepBalance { get; set; }
        public string AssemblyBalance { get; set; }
    }
}
