using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class MaterialDeliveryController
    {
        public static List<MaterialDeliveryModel> GetMaterialDeliveryByPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<MaterialDeliveryModel>("spm_SelectMaterialDeliveryByPO_1 @ProductNo", @ProductNo).ToList();
            }
        }

        public static bool Insert(MaterialDeliveryModel model)
        {
            var @ProductNo  = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId = new SqlParameter("@SupplierId", model.SupplierId);
            var @SizeNo     = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity   = new SqlParameter("@Quantity", model.Quantity);
            var @Reject     = new SqlParameter("@Reject", model.Reject);
            var @RejectSewing = new SqlParameter("@RejectSewing", model.RejectSewing);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMaterialDelivery_1 @ProductNo, @SupplierId, @SizeNo, @Quantity, @Reject, @RejectSewing",
                                                                              @ProductNo, @SupplierId, @SizeNo, @Quantity, @Reject, @RejectSewing) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateRejectWhenInspect (MaterialPlanModel model)
        {
            var @ProductNo  = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId = new SqlParameter("@SupplierId", model.SupplierId);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateRejectWhenInspectUpperAccesoriesMaterial @ProductNo, @SupplierId",
                                                                                                    @ProductNo, @SupplierId) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool DeleteByPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteMaterialDeliveryByPO @ProductNo",
                                                                                @ProductNo) > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
