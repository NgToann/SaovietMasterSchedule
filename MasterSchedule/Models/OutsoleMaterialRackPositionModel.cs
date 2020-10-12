using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialRackPositionModel
    {
        public int RackPositionID { get; set; }
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string RackNumber { get; set; }
        public int CartonNumber { get; set; }
        public string SizeNo { get; set; }
        //public string Quantity { get; set; }
        public int Pairs { get; set; }
    }
}
