using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOutsoleInventoryModel
    {
        public string OutsoleCode { get; set; }
        public string OutsoleLine { get; set; }
        public int Quantity { get; set; }
        public int Matching { get; set; }
        public int FinishedOutsoleQuantity { get; set; }
        public List<String> ProductNoList { get; set; }
    }
}
