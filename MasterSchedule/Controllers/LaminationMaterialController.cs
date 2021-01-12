using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class LaminationMaterialController
    {
        public static List<LaminationMaterialModel> GetByOrderNo(string orderNo)
        {
            var @OrderNo = new SqlParameter("@OrderNo", orderNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<LaminationMaterialModel>("spm_SelectLaminationMaterialByOrderNo @OrderNo", @OrderNo).ToList();
            }
        }
        //
        public static bool Insert(LaminationMaterialModel model)
        {
            var @OrderNoId = new SqlParameter("@OrderNoId", model.OrderNoId);
            var @OrderNo = new SqlParameter("@OrderNo", model.OrderNo);
            var @ArticleNo = new SqlParameter("@ArticleNo", model.ArticleNo);
            var @ShoeName = new SqlParameter("@ShoeName", model.ShoeName);
            var @PatternNo = new SqlParameter("@PatternNo", model.PatternNo);
            var @ProductNoList = new SqlParameter("@ProductNoList", model.ProductNoList);
            var @Position = new SqlParameter("@Position", model.Position);
            var @MaterialPart = new SqlParameter("@MaterialPart", model.MaterialPart);
            var @MaterialName = new SqlParameter("@MaterialName", model.MaterialName);
            var @Unit = new SqlParameter("@Unit", model.Unit);
            var @POQuantity = new SqlParameter("@POQuantity", model.POQuantity);
            var @SendQuantity = new SqlParameter("@SendQuantity", model.SendQuantity);
            var @DeliveryDate = new SqlParameter("@DeliveryDate", model.DeliveryDate);
            var @PurchaseDate = new SqlParameter("@PurchaseDate", model.PurchaseDate);
            var @SupplierName = new SqlParameter("@SupplierName", model.SupplierName);
            var @Remarks = new SqlParameter("@Remarks", model.Remarks);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertLaminationMaterial   @OrderNoId, @OrderNo, @ArticleNo, @ShoeName, @PatternNo, @ProductNoList, @Position, @MaterialPart, @MaterialName, @Unit, @POQuantity, @SendQuantity, @DeliveryDate, @PurchaseDate, @SupplierName, @Remarks",
                                                                                @OrderNoId, @OrderNo, @ArticleNo, @ShoeName, @PatternNo, @ProductNoList, @Position, @MaterialPart, @MaterialName, @Unit, @POQuantity, @SendQuantity, @DeliveryDate, @PurchaseDate, @SupplierName, @Remarks) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool DeleteByOrderNoId(string orderNoId)
        {
            var @OrderNoId = new SqlParameter("@OrderNoId", orderNoId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteLaminationMaterialByOrderNoId @OrderNoId",
                                                                                         @OrderNoId) > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

