using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class MachineModel
    {
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public int PhaseID { get; set; }

        public PhaseModel PhaseSelected { get; set; }
        public bool IsMachine { get; set; }
        public int Available { get; set; }
        public string Remarks { get; set; }
    }
}
