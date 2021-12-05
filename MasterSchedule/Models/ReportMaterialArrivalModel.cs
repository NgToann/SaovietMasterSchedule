using System;

namespace MasterSchedule.Models
{
    public class ReportMaterialArrivalModel
    {
        public string ProductNo { get; set; }
        public string SewingLine { get; set; }
        public string OutsoleLine { get; set; }
        public int SewingLineNo { get; set; }
        public int OutsoleLineNo { get; set; }
        public DateTime DateDisplay { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
    }
}
