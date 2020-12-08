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
        public string Remarks { get; set; }
    }
}
