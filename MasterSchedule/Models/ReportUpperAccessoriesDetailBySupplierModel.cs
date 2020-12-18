using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportUpperAccessoriesDetailBySupplierModel
    {
        public string ProductNo { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public  int RejectId { get; set; }
        public string RejectName { get; set; }
        public string  RejectName_1 { get; set; }
        public string Reviser { get; set; }
    }
}
