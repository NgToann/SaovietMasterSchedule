using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class RejectModel
    {
        public int RejectId { get; set; }
        public string RejectName { get; set; }
        public string RejectName_1 { get; set; }
        public string RejectKey { get; set; }
        public bool Major { get; set; }
        public bool Minor { get; set; }
    }
}
