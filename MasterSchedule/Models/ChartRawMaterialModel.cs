using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ChartRawMaterialModel
    {
        public string ProductNo { get; set; }
        public int MaterialTypeId { get; set; }
        public string GroupName { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
        public string Remarks { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime POETD { get; set; }
    }
}
