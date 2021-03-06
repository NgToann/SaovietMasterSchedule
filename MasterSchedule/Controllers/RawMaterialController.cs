﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class RawMaterialController
    {
        public static bool Insert(RawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @MaterialTypeId = new SqlParameter("@MaterialTypeId", model.MaterialTypeId);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);
            var @Remarks = new SqlParameter("@Remarks", model.Remarks);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertRawMaterial @ProductNo,@MaterialTypeId,@ETD,@ActualDate,@Remarks", @ProductNo, @MaterialTypeId, @ETD, @ActualDate, @Remarks) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Insert_2(RawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @MaterialTypeId = new SqlParameter("@MaterialTypeId", model.MaterialTypeId);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);
            var @Remarks = new SqlParameter("@Remarks", model.Remarks);

            var @IsETDUpdate = new SqlParameter("@IsETDUpdate", model.IsETDUpdate);
            var @IsActualDateUpdate = new SqlParameter("@IsActualDateUpdate", model.IsActualDateUpdate);
            var @IsRemarksUpdate = new SqlParameter("@IsRemarksUpdate", model.IsRemarksUpdate);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertRawMaterial_2 @ProductNo, @MaterialTypeId, @ETD, @ActualDate, @Remarks, @IsETDUpdate, @IsActualDateUpdate, @IsRemarksUpdate",
                                                                     @ProductNo, @MaterialTypeId, @ETD, @ActualDate, @Remarks, @IsETDUpdate, @IsActualDateUpdate, @IsRemarksUpdate) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool InsertFromExcel(RawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @MaterialTypeId = new SqlParameter("@MaterialTypeId", model.MaterialTypeId);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertRawMaterialFromExcel @ProductNo, @MaterialTypeId, @ETD, @ActualDate",
                                                                            @ProductNo, @MaterialTypeId, @ETD, @ActualDate) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        // IsEnable = 1
        public static List<RawMaterialModel> Select()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<RawMaterialModel>("EXEC spm_SelectRawMaterial").ToList();
            }
        }

        public static List<RawMaterialViewModelNew> Select_1()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                db.CommandTimeout = 90;
                return db.ExecuteStoreQuery<RawMaterialViewModelNew>("EXEC spm_SelectRawMaterialViewModel").ToList();
            };
        }

        // IsEnable = 1 || 0
        public static List<RawMaterialModel> SelectFull(DateTime etdStart, DateTime etdEnd)
        {
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<RawMaterialModel>("EXEC spm_SelectRawMaterialFull @ETDStart, @ETDEnd", ETDStart, ETDEnd).ToList();
            }
        }
    }
}
