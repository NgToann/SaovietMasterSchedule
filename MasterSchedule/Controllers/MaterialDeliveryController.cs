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
                return db.ExecuteStoreQuery<MaterialDeliveryModel>("spm_SelectMaterialDeliveryByPO @ProductNo", @ProductNo).ToList();
            }
        }
        
        public static bool Insert(MaterialDeliveryModel model, bool insertQty, bool insertReject)
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

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMaterialDelivery @ProductNo, @SupplierId, @DeliveryDate, @SizeNo, @Quantity, @Reject, @RejectId, @Reviser, @InsertQuantity, @InsertReject",
                                                                            @ProductNo, @SupplierId, @DeliveryDate, @SizeNo, @Quantity, @Reject, @RejectId, @Reviser, @InsertQuantity, @InsertReject) > 0)
                {
                    return true;
                }
                return false;
            }
        }

    }
}
