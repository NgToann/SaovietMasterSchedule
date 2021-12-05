using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class RawMaterialSummarizeModel
    {
        public string ProductNo { get; set; }
        public int  MaterialTypeId { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public DateTime ETD { get; set; }
        public DateTime dateTime { get; set; }
        public string Remarks { get; set; }
    }
}
