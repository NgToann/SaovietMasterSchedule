using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class PrintSizeRunController
    {
        public static List<PrintSizeRunModel> GetByPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<PrintSizeRunModel>("reporter_SelectPrintSizeRunByPO @ProductNo", @ProductNo).ToList();
            }
        }
    }
}
