using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class MaterialPlanController
    {
        public static List<MaterialPlanModel> GetMaterialPlanByPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<MaterialPlanModel>("spm_SelectMaterialPlanByPO @ProductNo", @ProductNo).ToList();
            }
        }

        public static bool Insert (MaterialPlanModel model, bool isUpdateActualDate)
        {
            var @ProductNo  = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId = new SqlParameter("@SupplierId", model.SupplierId);
            var @ETD        = new SqlParameter("@ETD", model.ETD);
            var @Remarks    = new SqlParameter("@Remarks", model.Remarks);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);
            var @IsUpdateActualDate = new SqlParameter("@IsUpdateActualDate", isUpdateActualDate);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMaterialPlan @ProductNo, @SupplierId, @ETD, @Remarks, @ActualDate, @IsUpdateActualDate", 
                                                                        @ProductNo, @SupplierId, @ETD, @Remarks, @ActualDate, @IsUpdateActualDate) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Update (string productNoOld, int supplierIdOld, MaterialPlanModel model)
        {
            //
            var @ProductNoOld   = new SqlParameter("@ProductNoOld", productNoOld);
            var @SupplierIdOld  = new SqlParameter("@SupplierIdOld", supplierIdOld);
            var @ProductNo      = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId     = new SqlParameter("@SupplierId", model.SupplierId);
            var @ETD            = new SqlParameter("@ETD", model.ETD);
            var @Remarks        = new SqlParameter("@Remarks", model.Remarks);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateMaterialPlanByPOBySupplier @ProductNoOld, @SupplierIdOld, @ProductNo, @SupplierId, @ETD, @Remarks", @ProductNoOld, @SupplierIdOld, @ProductNo, @SupplierId, @ETD, @Remarks) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //
        public static bool UpdateActualDateWhenDelivery(MaterialPlanModel model)
        {
            var @ProductNo          = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId         = new SqlParameter("@SupplierId", model.SupplierId);
            var @ActualDeliveryDate = new SqlParameter("@ActualDeliveryDate", model.ActualDeliveryDate);
            var @RemarksPO          = new SqlParameter("@RemarksPO", model.RemarksPO);
            var @ETDPO              = new SqlParameter("@ETDPO", model.ETDPO);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateMaterialPlanWhenDelivery @ProductNo, @SupplierId, @ActualDeliveryDate, @RemarksPO, @ETDPO",
                                                                                    @ProductNo, @SupplierId, @ActualDeliveryDate, @RemarksPO, @ETDPO) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Delete(MaterialPlanModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId = new SqlParameter("@SupplierId", model.SupplierId);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteMaterialPlanByPOBySupplier @ProductNo, @SupplierId", @ProductNo, @SupplierId) > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
