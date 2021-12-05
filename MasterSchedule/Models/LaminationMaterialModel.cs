using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    class LaminationMaterialModel
    {
        public int Id { get; set; }
        public string OrderNoId { get; set; }
        public string OrderNo { get; set; }
        public string ArticleNo { get; set; }
        public string  ShoeName { get; set; }
        public string PatternNo { get; set; }
        public string ProductNoList { get; set; }
        public string Position { get; set; }
        public string MaterialPart { get; set; }
        public string MaterialName { get; set; }
        public string Unit { get; set; }
        public int POQuantity { get; set; }
        public int SendQuantity { get; set; }
        public string DeliveryDate { get; set; }
        public string PurchaseDate { get; set; }
        public string  SupplierName { get; set; }
        public string Remarks { get; set; }
    }
}
