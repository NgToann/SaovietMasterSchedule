using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialCheckingModel
    {
        public string WorkerId { get; set; }
        public string ProductNo { get; set; }
        public DateTime CheckingDate { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string SizeNo { get; set; }
        //public string SizeNoUpper { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public int ReturnReject { get; set; }
        public int ReturnRemark { get; set; }
        public int ErrorId { get; set; }
        public int Excess { get; set; }
        public int WorkingCard { get; set; }
        public bool UpdateReject { get; set; } = true;
        public bool UpdateQuantity { get; set; } = true;
        public bool UpdateReturnReject { get; set; } = true;
    }
}
