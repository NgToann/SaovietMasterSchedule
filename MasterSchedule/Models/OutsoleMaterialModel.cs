using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string Name { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public int QuantityReject { get; set; }
        public int RejectAssembly { get; set; }
        public int RejectStockfit { get; set; }
        public int TotalReleasePainting { get; set; }
        public int TotalBalance { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime ModifiedTimeReject { get; set; }
    }
}
