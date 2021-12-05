using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class SewingMasterSourceModel
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string PatternNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime UpperMaterialArrivalOriginal { get; set; }
        public string UpperMaterialStatus { get; set; }
        public string UpperMaterialRemarks { get; set; }
        public DateTime OutsoleMaterialArrivalOriginal { get; set; }
        public string OutsoleMaterialStatus { get; set; }
        public string OutsoleMaterialRemarks { get; set; }

        public DateTime SewingMaterialArrivalOriginal { get; set; }
        public string SewingMaterialStatus { get; set; }
        public string SewingMaterialRemarks { get; set; }

        public DateTime AssemblyMaterialArrivalOriginal { get; set; }
        public string AssemblyMaterialStatus { get; set; }
        public string AssemblyMaterialRemarks { get; set; }

        public int Sequence { get; set; }
        public string SewingLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public int SewingQuota { get; set; }
        //
        public string SewingPrep { get; set; }

        public string SewingActualStartDate { get; set; }
        public DateTime SewingActualStartDate_Date { get; set; }
        public string SewingActualFinishDate { get; set; }
        public DateTime SewingActualFinishDate_Date { get; set; }

        public string SewingActualStartDateAuto { get; set; }
        public string SewingActualFinishDateAuto { get; set; }

        public string SewingBalance { get; set; }
        public DateTime SewingBalance_Date { get; set; } = new DateTime(2000, 01, 01);
        public DateTime CutAStartDate { get; set; }
        public DateTime CutAFinishDate { get; set; }
        public int CutAQuota { get; set; }
        public string CutAActualStartDate { get; set; }
        public DateTime CutAActualStartDate_Date { get; set; }
        public string CutAActualFinishDate { get; set; }
        public DateTime CutAActualFinishDate_Date { get; set; }
        public string CutABalance { get; set; }
        public DateTime CutABalance_Date { get; set; }
        public string PrintingBalance { get; set; }
        public string H_FBalance { get; set; }
        public string EmbroideryBalance { get; set; }

        public string CutBActualStartDate { get; set; }
        public string CutBBalance { get; set; }
        public string AutoCut { get; set; }

        //
        public string LaserCut { get; set; }
        public string HuasenCut { get; set; }

        //
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
        public string OutsoleBalance { get; set; }

        public DateTime AssemblyStartDate { get; set; }

        public DateTime AssemblyFinishDate { get; set; }
        public string OutsoleCode { get; set; }
        public string AssemblyBalance { get; set; }
    }
}
