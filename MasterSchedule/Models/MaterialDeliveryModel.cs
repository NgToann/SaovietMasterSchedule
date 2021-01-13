using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class MaterialDeliveryModel
    {
        public string ProductNo { get; set; }
        public int SupplierId { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public int RejectSewing { get; set; }
        public int Excess { get; set; }

        public string SupplierNameDisplay { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDeliveryDate { get; set; }
    }
}
