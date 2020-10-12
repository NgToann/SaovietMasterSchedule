using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;

namespace MasterSchedule.ViewModels
{
    public class OutsoleWHInventoryViewModel
    {
        public string OSLineHeader { get; set; }
        public string OutsoleLine { get; set; }
        public string OutsoleCode { get; set; }
        public List<String> ProductNoList { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public int Matching { get; set; }
        public string SupplierName { get; set; }
        //public List<String> SupplierNameList { get; set; }
        public List<Int32> SupplierIdList { get; set; }
        public int FinishedOutsoleQuantity { get; set; }



    }
}
