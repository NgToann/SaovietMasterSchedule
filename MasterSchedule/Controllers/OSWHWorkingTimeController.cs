using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    public class OSWHWorkingTimeController
    {
        public static List<OSWHWorkingTimeModel> Select(string productNo)
        {
            using ( var db = new SaovietMasterScheduleEntities())
            {
                var @ProductNo = new SqlParameter("@ProductNo", productNo);
                return db.ExecuteStoreQuery<OSWHWorkingTimeModel>("EXEC spm_SelectOSWHCheckingTimeByProductNo @ProductNo", @ProductNo).ToList();
            };
        }

        public static bool Insert(OSWHWorkingTimeModel model)
        {
            var @WorkerId = new SqlParameter("@WorkerId", model.WorkerId);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @StartingTime = new SqlParameter("@StartingTime", model.StartingTime);
            var @TotalHours = new SqlParameter("@TotalHours", model.TotalHours);
            var @Pairs = new SqlParameter("@Pairs", model.Pairs);
            var @CheckingDate = new SqlParameter("@CheckingDate", model.CheckingDate);

            using( var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOSWHCheckingTime @WorkerId, @ProductNo, @OutsoleSupplierId, @StartingTime, @TotalHours, @Pairs, @CheckingDate",
                                                                            @WorkerId, @ProductNo, @OutsoleSupplierId, @StartingTime, @TotalHours, @Pairs, @CheckingDate) >= 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
