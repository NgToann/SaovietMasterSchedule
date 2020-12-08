using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    class OutsoleMaterialCheckingController
    {
        public static List<OutsoleMaterialCheckingModel> SelectByPO(string POSearch)
        {
            var @ProductNo = new SqlParameter("@ProductNo", POSearch);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialCheckingModel>("EXEC spm_SelectOutsoleMaterialCheckingByPO @ProductNo", @ProductNo).ToList();
            };
        }
        public static List<OutsoleMaterialCheckingModel> SelectByPOSumBySize(string POSearch)
        {
            var @ProductNo = new SqlParameter("@ProductNo", POSearch);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialCheckingModel>("EXEC spm_SelectOutsoleMaterialCheckingByPOSumBySize @ProductNo", @ProductNo).ToList();
            };
        }
        public static List<OutsoleMaterialCheckingModel> SelectByPOAvailable()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialCheckingModel>("EXEC spm_SelectOSWHMaterialCheckingPOAvailable").ToList();
            };
        }
        public static bool Insert(OutsoleMaterialCheckingModel model)
        {
            var @WorkerId           = new SqlParameter("@WorkerId", model.WorkerId);
            var @ProductNo          = new SqlParameter("@ProductNo", model.ProductNo);
            var @CheckingDate       = new SqlParameter("@CheckingDate", model.CheckingDate);
            var @OutsoleSupplierId  = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo             = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity           = new SqlParameter("@Quantity", model.Quantity);
            var @Reject             = new SqlParameter("@Reject", model.Reject);
            var @ErrorId            = new SqlParameter("@ErrorId", model.ErrorId);
            var @Excess             = new SqlParameter("@Excess", model.Excess);
            var @WorkingCard        = new SqlParameter("@WorkingCard", model.WorkingCard);
            var @ReturnReject       = new SqlParameter("@ReturnReject", model.ReturnReject);
            //var @SizeNoUpper        = new SqlParameter("@SizeNoUpper", model.SizeNoUpper);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialChecking_2    @WorkerId, @ProductNo, @CheckingDate, @OutsoleSupplierId, @SizeNo, @Quantity, @Reject, @ErrorId, @Excess, @WorkingCard, @ReturnReject",
                                                                                        @WorkerId, @ProductNo, @CheckingDate, @OutsoleSupplierId, @SizeNo, @Quantity, @Reject, @ErrorId, @Excess, @WorkingCard, @ReturnReject) > 0)
                {
                    return true;
                }
                return false;
            };
            
        }
    }
}
