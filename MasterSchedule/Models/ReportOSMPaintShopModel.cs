using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOSMPaintShopModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string Name { get; set; }
        public int QtyOrder { get; set; }
        public int QtySend { get; set; }
        public int QtyDelivery { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int Balance { get; set; }
    }
}
