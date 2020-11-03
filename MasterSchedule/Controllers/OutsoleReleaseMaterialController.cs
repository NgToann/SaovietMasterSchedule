using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    class OutsoleReleaseMaterialController
    {
        public static List<OutsoleReleaseMaterialModel> SelectReportId()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialReportId_2").ToList();
            }
        }

        public static List<OutsoleReleaseMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);

            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialByProductNo @ProductNo", @ProductNo).ToList();
            }
        }

        public static List<OutsoleReleaseMaterialModel> Select(string reportId, string productNo)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialByReportId @ReportId", @ReportId).ToList();
            }
        }

        public static List<OutsoleReleaseMaterialModel> SelectByOutsoleMaterial()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialByOutsoleMaterial").ToList();
            }
        }

        public static List<OutsoleReleaseMaterialModel> SelectByOutsoleMaster()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialByOutsoleMaster").ToList();
            }
        }

        public static bool Insert(OutsoleReleaseMaterialModel model)
        {
            var @ReportId = new SqlParameter("@ReportId", model.ReportId);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Cycle = new SqlParameter("@Cycle", model.Cycle);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleReleaseMaterial   @ReportId, @ProductNo, @Cycle, @SizeNo, @Quantity", 
                                                                                    @ReportId, @ProductNo, @Cycle, @SizeNo, @Quantity) >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Delete(string reportId, string productNo)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleReleaseMaterialByReportIdProductNo @ReportId, @ProductNo", @ReportId, @ProductNo) >= 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
