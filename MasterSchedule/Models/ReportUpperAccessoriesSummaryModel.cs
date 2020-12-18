using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportUpperAccessoriesSummaryModel
    {
        public string AccessoriesName { get; set; }
        public string Name { get; set; }
        public int SupplierId { get; set; }
        public int QuantityOrder { get; set; }
        public int QuantityCheckOK { get; set; }
        public int QuantityReject { get; set; }

        public int Reject { get; set; }
        public int RejectId { get; set; }
        public string RejectName { get; set; }
        public string RejectName_1 { get; set; }
    }
}
