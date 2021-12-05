using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMasterSourceModel
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string OutsoleCode { get; set; }
        public string PatternNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }

        public int Sequence { get; set; }
        public string OutsoleLine { get; set; }
        public DateTime OutsoleStartDate { get; set; }
        public DateTime OutsoleFinishDate { get; set; }
        public int OutsoleQuota { get; set; }
        public string OutsoleActualStartDate { get; set; }
        public DateTime OutsoleActualStartDate_Date { get; set; } = new DateTime(2000, 01, 01);
        public string OutsoleActualFinishDate { get; set; }
        public DateTime OutsoleActualFinishDate_Date { get; set; } = new DateTime(2000, 01, 01);
        public string OutsoleActualStartDateAuto { get; set; }
        public string OutsoleActualFinishDateAuto { get; set; }
        public string OutsoleBalance { get; set; }
        public DateTime OutsoleBalance_Date { get; set; } = new DateTime(2000, 01, 01);
        public string Remarks { get; set; }
        public int WHCurrentCheck { get; set; }
        public DateTime WHLastDateCheck { get; set; }

        public string SewingLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public int SewingQuota { get; set; }
        public string SewingBalance { get; set; }
        public string OutsoleWHBalance { get; set; }
        public string ReleasedQuantity { get; set; }
        public string ReleasedToWHInspectionQuantity { get; set; }
        public string OutsoleWHCheckingCurrent { get; set; }
        public string OutsoleMaterialStatus { get; set; }
        public DateTime OutsoleMaterialArrivalOriginal { get; set; }
        public string OutsoleMaterialRemarks { get; set; }
    }
}
