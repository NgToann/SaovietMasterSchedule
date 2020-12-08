using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class SupplierModel
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ProvideAccessories { get; set; }
        public string ProvideOutsole { get; set; }
        public string ProvideUpper { get; set; }
        public String Remarks { get; set; }
    }
}
