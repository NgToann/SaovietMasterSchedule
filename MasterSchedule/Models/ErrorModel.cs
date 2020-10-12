using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ErrorModel
    {
        public int ErrorID { get; set; }
        public string ErrorName { get; set; }
        public string ErrorVietNamese { get; set; }
        public string ErrorKey { get; set; }
        public bool MajorDefect { get; set; }
    }
}
