using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialBalanceReportModel
    {
        // Order
        public string OutsoleCode { get; set; }
        public string ProductNo { get; set; }
        public string ArticleNo { get; set; }
        public string SizeNo { get; set; }
        public DateTime ETD { get; set; }

        // RawMaterial
        public DateTime SupplierETD { get; set; }

        // Supplier
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        // OutsoleMaterial
        public int QuantityDelivery { get; set; }
        public int QuantityOrder { get; set; }
        public int QuantityBalance { get; set; }
        public int QuantityReject { get; set; }
    }
}
