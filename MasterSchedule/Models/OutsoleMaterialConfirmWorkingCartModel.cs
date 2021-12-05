using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialConfirmWorkingCartModel
    {
        public int OSCheckingID { get; set; }
        public string ProductNo { get; set; }
        public DateTime CheckingDate { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string Name { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public int ReturnReject { get; set; }
        public int WorkingCard { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleCode { get; set; }
        public string OutsoleLine { get; set; }
        public DateTime OutsoleStartDate { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsRelease { get; set; }
        public Nullable<DateTime> ReleasedTime { get; set; }
    }
}
