using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class MachineRequestModel
    {
        public int MachineRequestID { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string PatternNo { get; set; }
        public int PhaseID { get; set; }
        public int Quota { get; set; }
        public int MachineID { get; set; }
        public int Quantity { get; set; }
        public int Worker { get; set; }
    }
}
