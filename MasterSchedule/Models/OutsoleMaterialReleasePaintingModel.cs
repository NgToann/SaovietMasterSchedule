using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialReleasePaintingModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
    }
}
