using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class MaterialPlanModel
    {
        public string ProductNo { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ProvideAccessories { get; set; }
        public string ProvideOutsole { get; set; }
        public string ProvideUpper { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
        public DateTime ActualDeliveryDate { get; set; }
        public string ActualDateString { get; set; }
        public string Remarks { get; set; }
        public int Balance { get; set; }
        public int BalanceDelivery { get; set; }
        public int TotalDeliveryQuantity { get; set; }
        public string RemarksPO { get; set; }
        public string ETDPO { get; set; }
        public string ActualDatePO { get; set; }
    }
}
