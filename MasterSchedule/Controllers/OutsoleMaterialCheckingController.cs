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
        //
        public static List<OutsoleMaterialCheckingModel> SelectByPO_1(string POSearch)
        {
            var @ProductNo = new SqlParameter("@ProductNo", POSearch);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialCheckingModel>("EXEC spm_SelectOutsoleMaterialCheckingByPO_1 @ProductNo", @ProductNo).ToList();
            };
        }

        public static List<OutsoleMaterialCheckingModel> SelectByPOBorrow(string POSearch)
        {
            var @ProductNo = new SqlParameter("@ProductNo", POSearch);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialCheckingModel>("EXEC spm_SelectOutsoleMaterialCheckingBorrowByPO @ProductNo", @ProductNo).ToList();
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
            var @ReturnRemark = new SqlParameter("@ReturnRemark", model.ReturnRemark);
            //var @SizeNoUpper        = new SqlParameter("@SizeNoUpper", model.SizeNoUpper);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialChecking_4    @WorkerId, @ProductNo, @CheckingDate, @OutsoleSupplierId, @SizeNo, @Quantity, @Reject, @ErrorId, @Excess, @WorkingCard, @ReturnReject, @ReturnRemark",
                                                                                        @WorkerId, @ProductNo, @CheckingDate, @OutsoleSupplierId, @SizeNo, @Quantity, @Reject, @ErrorId, @Excess, @WorkingCard, @ReturnReject, @ReturnRemark) > 0)
                {
                    return true;
                }
                return false;
            };
        }
        
        public static bool updateBorrow (OutsoleMaterialCheckingModel model)
        {
            var @OSCheckingId = new SqlParameter("@OSCheckingId", model.OSCheckingId);
            var @ProductNoBorrow = new SqlParameter("@ProductNoBorrow", model.ProductNoBorrow);
            var @QuantityBorrow = new SqlParameter("@QuantityBorrow", model.QuantityBorrow);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOSWHBorrow @OSCheckingId, @ProductNoBorrow, @QuantityBorrow",
                                                                      @OSCheckingId, @ProductNoBorrow, @QuantityBorrow) > 0)
                {
                    return true;
                }
                return false;
            };
        }
    }
}
