using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOutsoleMaterialDeliverySummary
    {
        public string OutsoleCode { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string Name { get; set; }
        public int QtyOrder { get; set; }
        public int QtyMatch { get; set; }
        public int QtyRelease { get; set; }
        public int QtyWH { get; set; }
        public int QtyDelivery { get; set; }
        public int QtyReject { get; set; }
        public int QtyBalance { get; set; }
        public int NoOfSupp { get; set; }
        public int NoOfPO { get; set; }
    }
}
