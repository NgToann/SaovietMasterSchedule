using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class PrintSizeRunModel
    {
        public string ProductNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleSize { get; set; }
        public string MidsoleSize { get; set; }
        public string LastSize { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public DateTime EFD { get; set; }
        public DateTime SewingStartDate { get; set; }
    }
}
