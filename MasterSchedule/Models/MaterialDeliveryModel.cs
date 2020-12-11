using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class MaterialDeliveryModel
    {
        public string ProductNo { get; set; }
        public string SizeNo { get; set; }
        public int SupplierId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public int RejectId { get; set; }
        //public int Excess { get; set; }
        public string  Reviser { get; set; }
    }
}
