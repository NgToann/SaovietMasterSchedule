using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OutsoleOutputController
    {
        public static List<OutsoleOutputModel> SelectByAssemblyMaster()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleOutputModel>("EXEC spm_SelectOutsoleOutputByAssemblyMaster").ToList();
            }
        }

        public static List<OutsoleOutputModel> SelectByIsEnable()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleOutputModel>("EXEC spm_SelectOutsoleOutputIsEnable").ToList();
            }
        }

        public static List<OutsoleOutputModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);

            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleOutputModel>("EXEC spm_SelectOutsoleOutputByProductNo @ProductNo", @ProductNo).ToList();
            }
        }

        public static bool Insert(OutsoleOutputModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleOutput @ProductNo,@SizeNo,@Quantity", @ProductNo, @SizeNo, @Quantity) >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        public static List<OutsoleOutputModel> SelectByAssemblyRelease(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleOutputModel>("EXEC spm_SelectOutsoleOutputByAssemblyReleaseByReportId @ReportId", @ReportId).ToList();
            }
        }
    }
}
