using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class AssemblyMasterSourceModel
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string OutsoleCode { get; set; }
        public string LastCode { get; set; }
        public string PatternNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public int Sequence { get; set; }

        public string AssemblyLine { get; set; }
        public DateTime AssemblyStartDate { get; set; }
        public DateTime AssemblyFinishDate { get; set; }
        public int AssemblyQuota { get; set; }
        public string AssemblyActualStartDate { get; set; }
        public string AssemblyActualFinishDate { get; set; }
        public string AssemblyBalance { get; set; }
        public string SewingLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public int SewingQuota { get; set; }
        public DateTime OutsoleFinishDate { get; set; }

        public string OutsoleMaterialStatus { get; set; }
        public DateTime OutsoleMaterialArrivalOriginal { get; set; }
        public string OutsoleMaterialRemarks { get; set; }

        public string OutsoleLine { get; set; }

        public string AssemblyMaterialStatus { get; set; }
        public DateTime AssemblyMaterialArrivalOriginal { get; set; }
        public string AssemblyMaterialRemarks { get; set; }

        public string SockliningMaterialStatus { get; set; }
        public DateTime SockliningMaterialArrivalOriginal { get; set; }
        public string SockliningMaterialRemarks { get; set; }

        public string CartonStatusMaterialStatus { get; set; }
        public DateTime CartonStatusMaterialArrivalOriginal { get; set; }
        public string CartonStatusMaterialRemarks { get; set; }

        public string SewingBalance { get; set; }
        public string OutsoleBalance { get; set; }

        public string InsoleBalance { get; set; }
        public string InsockBalance { get; set; }

        public string AssemblyReleasedQuantity { get; set; }
        public string OutsoleReleasedQuantity { get; set; }
    }
}
