using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class OutsoleWHDeliveryViewModel
    {
        public string ProductNo { get; set; }
        public string OutsoleCode { get; set; }
        public string OutsoleLine { get; set; }

        public int Delivery { get; set; }
        public int Reject { get; set; }
        public int Release { get; set; }

        public int WHQuantity { get; set; }

        public int Matching { get; set; }
    }
}
