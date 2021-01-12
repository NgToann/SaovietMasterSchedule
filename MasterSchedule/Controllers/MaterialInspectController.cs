using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;
namespace MasterSchedule.Controllers
{
    public class MaterialInspectController
    {
        public static List<MaterialInspectModel> GetMaterialInspectByPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<MaterialInspectModel>("spm_SelectMaterialInspectByPO @ProductNo", @ProductNo).ToList();
            }
        }
        
        public static bool DeleteByPOBySupplier (string productNo, int supplierId)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @SupplierId = new SqlParameter("@SupplierId", supplierId);
            
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteMaterialDeliveryByPOBySupplier @ProductNo, @SupplierId",
                                                                                          @ProductNo, @SupplierId) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Insert(MaterialInspectModel model, bool insertQty, bool insertReject, bool deleteReject)
        {
            var @ProductNo  = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId = new SqlParameter("@SupplierId", model.SupplierId);
            var @DeliveryDate = new SqlParameter("@DeliveryDate", model.DeliveryDate);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @Reject = new SqlParameter("@Reject", model.Reject);
            var @RejectId = new SqlParameter("@RejectId", model.RejectId);
            var @Reviser = new SqlParameter("@Reviser", model.Reviser);
            var @InsertQuantity = new SqlParameter("@InsertQuantity", insertQty);
            var @InsertReject = new SqlParameter("@InsertReject", insertReject);
            
            var @DeleteReject = new SqlParameter("@DeleteReject", deleteReject);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMaterialDelivery @ProductNo, @SupplierId, @DeliveryDate, @SizeNo, @Quantity, @Reject, @RejectId, @Reviser, @InsertQuantity, @InsertReject, @DeleteReject",
                                                                            @ProductNo, @SupplierId, @DeliveryDate, @SizeNo, @Quantity, @Reject, @RejectId, @Reviser, @InsertQuantity, @InsertReject, @DeleteReject) > 0)
                {
                    return true;
                }
                return false;
            }
        }

    }
}
