using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class RawMaterialViewModelNew
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string PatternNo { get; set; }
        public string OutsoleCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }

        public DateTime CutAStartDate { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public DateTime OutsoleFinishDate { get; set; }
        public string CutAStartDateForeground { get; set; }

        public DateTime AssemblyStartDate { get; set; }

        public DateTime MatterialArrival { get; set; }
        public DateTime MatterialETD { get; set; }
        public string MatterialRemarks { get; set; }

        public string LAMINATION_ETD { get; set; }
        public string LAMINATION_ActualDate { get; set; }
        public string LAMINATION_Remarks { get; set; }

        public string TAIWAN_ETD { get; set; }
        public string TAIWAN_ActualDate { get; set; }
        public string TAIWAN_Remarks { get; set; }

        public string CUTTING_ETD { get; set; }
        public string CUTTING_ActualDate { get; set; }
        public string CUTTING_Remarks { get; set; }

        public string LEATHER_ETD { get; set; }
        public string LEATHER_ActualDate { get; set; }
        public string LEATHER_Remarks { get; set; }

        public string SEMIPROCESS_ETD { get; set; }
        public string SEMIPROCESS_ActualDate { get; set; }
        public string SEMIPROCESS_Remarks { get; set; }

        public string SEWING_ETD { get; set; }
        public string SEWING_ActualDate { get; set; }
        public string SEWING_Remarks { get; set; }
        

        public string OUTSOLE_ETD { get; set; }
        public DateTime OUTSOLE_ETD_DATE { get; set; }
        public string OUTSOLE_ActualDate { get; set; }
        public DateTime OUTSOLE_ActualDate_DATE { get; set; }
        public string OUTSOLE_Remarks { get;set; }
        public string OUTSOLE_AssemblyReject { get; set; }

        public string INSOCK_ETD { get; set; }        
        public string INSOCK_ActualDate { get; set; }
        public string INSOCK_Remarks { get; set; }

        public string UPPERCOMPONENT_ETD { get; set; }
        public string UPPERCOMPONENT_ActualDate { get; set; }
        public string UPPERCOMPONENT_Remarks { get; set; }

        public string UpperAccessories_ETD { get; set; }
        public string UpperAccessories_ActualDate { get; set; }
        public string UpperAccessories_ActualDeliveryDate { get; set; }
        public string UpperAccessories_Remarks { get; set; }

        public string SECURITYLABEL_ETD { get; set; }
        public string SECURITYLABEL_ActualDate { get; set; }
        public string SECURITYLABEL_Remarks { get; set; }

        public string ASSEMBLY_ETD { get; set; }
        public string ASSEMBLY_ActualDate { get; set; }
        public string ASSEMBLY_Remarks { get; set; }

        public string SOCKLINING_ETD { get; set; }
        public string SOCKLINING_ActualDate { get; set; }
        public string SOCKLINING_Remarks { get; set; }

        public string CARTON_ETD { get; set; }
        public string CARTON_ActualDate { get; set; }
        public string CARTON_Remarks { get; set; }

        public DateTime Carton_ETD_Sort { get; set; }
        public DateTime Carton_ActualDate_Sort { get; set; }

        public string LoadingDate { get; set; }
    }
}
