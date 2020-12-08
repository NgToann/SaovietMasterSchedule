using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOSMaterialCheckWHInventoryModel
    {
        public string OutsoleCode { get; set; }
        public int QuantityChecked { get; set; }
        public int QuantityReject { get; set; }
        public int QuantityReturn { get; set; }
        public int QuantityDelivery { get; set; }
        public int QuantityNotCheck { get; set; }
    }
}
