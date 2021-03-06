﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;
namespace MasterSchedule.Controllers
{
    class OutsoleRawMaterialController
    {
        public static bool Insert(OutsoleRawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleRawMaterial @ProductNo, @OutsoleSupplierId, @ETD", @ProductNo, @OutsoleSupplierId, @ETD) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Delete(string productNo, int outsoleSupplierId)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", outsoleSupplierId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleRawMaterial @ProductNo, @OutsoleSupplierId", @ProductNo, @OutsoleSupplierId) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateActualDate(OutsoleRawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleRawMaterialActualDate @ProductNo, @OutsoleSupplierId, @ActualDate", @ProductNo, @OutsoleSupplierId, @ActualDate) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        // IsEnable = 1
        public static List<OutsoleRawMaterialModel> Select()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleRawMaterialModel>("EXEC spm_SelectOutsoleRawMaterial").ToList();
            }
        }

        // IsEnable = 1 || 0
        public static List<OutsoleRawMaterialModel> SelectFull (DateTime etdStart, DateTime etdEnd)
        {
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleRawMaterialModel>("EXEC spm_SelectOutsoleRawMaterialFull @ETDStart, @ETDEnd", ETDStart, ETDEnd).ToList();
            }
        }

        public static List<OutsoleRawMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleRawMaterialModel>("EXEC spm_SelectOutsoleRawMaterialByProductNo @ProductNo", @ProductNo).ToList();
            }
        }

        public static bool IsFull(List<SizeRunModel> sizeRunList, List<OutsoleRawMaterialModel> outsoleRawMaterialList, List<OutsoleMaterialModel> outsoleMaterialList)
        {
            foreach (OutsoleRawMaterialModel outsoleRawMaterial in outsoleRawMaterialList)
            {
                foreach (SizeRunModel sizeRun in sizeRunList)
                {
                    //int quantity = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => (o.Quantity - o.QuantityReject));
                    int quantity = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => (o.Quantity));
                    if (quantity < sizeRun.Quantity)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public static List<OutsoleRawMaterialModel> SelectOutsoleRawMaterialNotInOutsoleMaterial(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleRawMaterialModel>("EXEC spm_SelectOutsoleRawMaterialNotInOutSoleMaterial @ProductNo", @ProductNo).ToList();
            }
        }
    }
}
