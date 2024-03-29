﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class PrivateDefineModel
    {
        public int DefineId { get; set; }
        public string Factory { get; set; }
        public int SewingVsCutAStartDate { get; set; }
        public int SewingVsCutAStartDate_1 { get; set; }
        public int SewingVsOthersCutTypeB { get; set; }
        public int SewingVsOthersCutTypeA { get; set; }
        public bool InputReject { get; set; }
        public bool ReadOnlyReject { get; set; }
        public string WarehouseCode { get; set; }
        public int NoOfColumnOrderExcelFile { get; set; }
        public bool ShowCuttingDieSizeValue { get; set; }
        public bool ShowOSSizeValue { get; set; }
        public bool ShowLastSizeValue { get; set; }
    }
}
