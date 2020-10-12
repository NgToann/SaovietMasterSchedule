using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialDeliveryDetailModel
    {
        public int IDRackUpdate { get; set; }

        public string ProductNo { get; set; }
        public string OutsoleSupplierName { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string SizeNo { get; set; }

        public int QuantityOld { get; set; }
        public int QuantityCurrent { get; set; }

        public int RejectOld { get; set; }
        public int RejectCurrent { get; set; }
    }
}
