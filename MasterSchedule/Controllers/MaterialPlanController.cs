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

        public static bool Insert (MaterialPlanModel model)
        {
            var @ProductNo  = new SqlParameter("@ProductNo", model.ProductNo);
            var @SupplierId = new SqlParameter("@SupplierId", model.SupplierId);
            var @ETD        = new SqlParameter("@ETD", model.ETD);
            var @Remarks    = new SqlParameter("@Remarks", model.Remarks);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMaterialPlan @ProductNo, @SupplierId, @ETD, @Remarks", @ProductNo, @SupplierId, @ETD, @Remarks) > 0)
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
