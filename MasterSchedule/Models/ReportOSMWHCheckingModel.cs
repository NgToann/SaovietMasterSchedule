using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class ReportOSMWHCheckingModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string Name { get; set; }
        public string SupplierOperation { get; set; }
        public DateTime EFD { get; set; }
        public string OutsoleCode { get; set; }
        public string ArticleNo { get; set; }
        public DateTime SupplierETD { get; set; }
        public int DeliveryTimes { get; set; }
        public DateTime FinishedDate { get; set; }
        public int QuantityOrder { get; set; }
        public int QuantityDelivery { get; set; }
        public string WorkerId { get; set; }
        public string SizeNo { get; set; }
        public string SizeNoUpper { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public int ErrorId { get; set; }
        public string ErrorName { get; set; }
        public string ErrorVietNamese { get; set; }
        public int ReturnReject { get; set; }
        public double TotalHours { get; set; }
        public string POStatus { get; set; }
        public int TotalQuantityCheckedOK { get; set; }
        public DateTime CreatedTime { get; set; }
        public string RejectCurrentDetail { get; set; }

    }
}
