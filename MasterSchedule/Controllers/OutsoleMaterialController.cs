﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OutsoleMaterialController
    {
        public static List<OutsoleMaterialModel> Select()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterial_1").ToList();
            }
        }

        public static List<OutsoleMaterialModel> SelectReject()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialReject").ToList();
            }
        }

        public static List<OutsoleMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByProductNo @ProductNo", @ProductNo).ToList();
            }
        }
        //
        public static List<OutsoleMaterialModel> SelectByOSCode(string osCode)
        {
            var @OutsoleCode = new SqlParameter("@OutsoleCode", osCode);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleCode @OutsoleCode", @OutsoleCode).ToList();
            }
        }

        // IsEnable = 1
        public static List<OutsoleMaterialModel> SelectByOutsoleRawMaterial()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleRawMaterial").ToList();
            }
        }

        // IsEnable = 0 || 1
        public static List<OutsoleMaterialModel> SelectByOutsoleRawMaterialFull(DateTime etdStart, DateTime etdEnd)
        {
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleRawMaterialFull @ETDStart, @ETDEnd", ETDStart, ETDEnd).ToList();
            }
        }

        public static List<OutsoleMaterialModel> SelectByOutsoleReleaseMaterial(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleReleaseMaterialByReportId @ReportId", @ReportId).ToList();
            }
        }

        public static List<OutsoleMaterialModel> SelectByOutsoleReleaseMaterialToWHInspection(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleReleaseMaterialToWHInspectionByReportId @ReportId", @ReportId).ToList();
            }
        }

        public static bool Insert(OutsoleMaterialModel model)
        {
            var @ProductNo          = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId  = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo             = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity           = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject     = new SqlParameter("@QuantityReject", model.QuantityReject);
            var @RejectAssembly     = new SqlParameter("@RejectAssembly", model.RejectAssembly);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterial_3    @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @QuantityReject, @RejectAssembly",
                                                                                @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @QuantityReject, @RejectAssembly) >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateByOSCheck(OutsoleMaterialModel model)
        {
            var @ProductNo          = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId  = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo             = new SqlParameter("@SizeNo", model.SizeNo);
            var @QuantityReject     = new SqlParameter("@QuantityReject", model.QuantityReject);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterialByOSCheck @ProductNo, @OutsoleSupplierId, @SizeNo, @QuantityReject",
                                                                                    @ProductNo, @OutsoleSupplierId, @SizeNo, @QuantityReject) >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateRejectFromOutsoleMaterialDetail(OutsoleMaterialModel model)
        {
            var @ProductNo          = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId  = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo             = new SqlParameter("@SizeNo", model.SizeNo);
            var @QuantityReject     = new SqlParameter("@QuantityReject", model.QuantityReject);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterialFromOutsoleMaterialDetail @ProductNo, @OutsoleSupplierId, @SizeNo, @QuantityReject", 
                                                                                                    @ProductNo, @OutsoleSupplierId, @SizeNo, @QuantityReject) >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Upadate Outsole Material
        /// </summary>
        /// <param name="model">Material Update</param>
        /// <param name="updateReject">Update Reject?</param>
        /// <param name="updateQuantity">Update Quantity?</param>
        /// <param name="updateRejectAssembly">Update Reject Assembly?</param>
        /// <returns></returns>
        public static bool Update(OutsoleMaterialModel model, bool updateReject, bool updateQuantity, bool updateRejectAssembly, bool updateRejectStockfit)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);
            var @RejectAssembly = new SqlParameter("@RejectAssembly", model.RejectAssembly);
            var @RejectStockfit = new SqlParameter("@RejectStockfit", model.RejectStockfit);

            var @UpdateQuantity = new SqlParameter("@UpdateQuantity", updateQuantity);
            var @UpdateReject = new SqlParameter("@UpdateReject", updateReject);
            var @UpdateRejectAssembly = new SqlParameter("@UpdateRejectAssembly", updateRejectAssembly);
            var @UpdateRejectStockfit = new SqlParameter("@UpdateRejectStockfit", updateRejectStockfit);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterial_2    @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @QuantityReject, @RejectAssembly, @RejectStockfit, @UpdateReject, @UpdateQuantity, @UpdateRejectAssembly, @UpdateRejectStockfit",
                                                                                @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @QuantityReject, @RejectAssembly, @RejectStockfit, @UpdateReject, @UpdateQuantity, @UpdateRejectAssembly, @UpdateRejectStockfit) >= 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
