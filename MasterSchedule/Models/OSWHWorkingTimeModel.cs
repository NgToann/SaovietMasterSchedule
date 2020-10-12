using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OSWHWorkingTimeModel
    {
        public string WorkerId { get; set; }
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public DateTime StartingTime { get; set; }
        public double TotalHours { get; set; }
        public int Pairs { get; set; }
        public DateTime CheckingDate { get; set; }
    }
}
