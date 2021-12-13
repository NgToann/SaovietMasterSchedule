using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OSMaterialBorrowModel
    {
        public int Id { get; set; }
        public int OSCheckingId { get; set; }
        public string ProductNoBorrow { get; set; }
        public int QuantityBorrow { get; set; }
    }
}
