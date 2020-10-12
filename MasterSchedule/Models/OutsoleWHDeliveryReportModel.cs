using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleWHDeliveryReportModel
    {
        public string OutsoleSupplierName { get; set; }
        public int OutsoleSupplierId { get; set; }
        public int QuantityOutsoleFinished { get; set; }
        public int QuantityMatching { get; set; }
        public int QuantityReject { get; set; }
        public int QuantityInventory { get; set; }
    }
}
