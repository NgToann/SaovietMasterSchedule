using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOSMaterialCheckDetailModel
    {
        public string ProductNo { get; set; }
        public string OutsoleCode { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string Name { get; set; }
        public string SizeNo { get; set; }
        public int Checked { get; set; }
        public int Reject { get; set; }
        public int QtyReturn { get; set; }
    }
}
