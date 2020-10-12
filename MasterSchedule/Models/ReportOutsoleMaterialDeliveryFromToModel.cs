using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOutsoleMaterialDeliveryFromToModel
    {
        public string ProductNo { get; set; }
        public string ArticleNo { get; set; }
        public string OutsoleCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string SupplierName { get; set; }
        public DateTime SupplierEFD { get; set; }
        public int QuantityDelivery { get; set; }
        public int Balance { get; set; }
        public int Reject { get; set; }
    }
}
