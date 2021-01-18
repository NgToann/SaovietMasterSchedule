using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportUpperAccessoriesModel
    {
        public string ProductNo { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime SupplierEFD { get; set; }
        public DateTime ActualDeliveryDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProvideAccessories { get; set; }
        public string SizeNo { get; set; }
        public int QuantityOrder { get; set; }
        public int QuantityDelivery { get; set; }
        public int Reject { get; set; }
        public int Balance { get; set; }
        public int BalanceAndReject { get; set; }
    }
}
