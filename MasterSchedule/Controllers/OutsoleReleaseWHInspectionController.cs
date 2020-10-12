using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    class OutsoleReleaseWHInspectionController
    {
        public static List<OutsoleReleaseMaterialModel> Select(string productNo)
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                var @ProductNo = new SqlParameter("@ProductNo", productNo);
                return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseWHInspectionByProductNo @ProductNo", @ProductNo).ToList();
            }
        }

        public static List<OutsoleReleaseMaterialModel> SelectReportId()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialToWHInspectionReportId").ToList();
        }

        public static List<OutsoleReleaseMaterialModel> Select(string reportId, string productNo)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialToWHInspectionByReportId @ReportId", @ReportId).ToList();
        }

        public static List<OutsoleReleaseMaterialModel> SelectOutsoleReleaseToWHInspectionByOutsoleMaster()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleReleaseMaterialModel>("EXEC spm_SelectOutsoleReleaseMaterialToWHInspectionByOutsoleMaster").ToList();
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
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleReleaseMaterialWHInspection @ReportId,@ProductNo,@Cycle,@SizeNo,@Quantity", @ReportId, @ProductNo, @Cycle, @SizeNo, @Quantity) >= 1)
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
                if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleReleaseMaterialToWHInspectionByReportIdProductNo @ReportId, @ProductNo", @ReportId, @ProductNo) >= 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
