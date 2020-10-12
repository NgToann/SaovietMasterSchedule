using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class OutsoleMaterialRackPositionController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static List<OutsoleMaterialRackPositionModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<OutsoleMaterialRackPositionModel>("EXEC spm_SelectOutsoleMaterialRackPosition @ProductNo", @ProductNo).ToList();
        }

        public static List<OutsoleMaterialRackPositionModel> SelectAll()
        {
            return db.ExecuteStoreQuery<OutsoleMaterialRackPositionModel>("EXEC spm_SelectOutsoleMaterialRackPositionAll").ToList();
        }

        public static bool Insert(OutsoleMaterialRackPositionModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @RackNumber = new SqlParameter("@RackNumber", model.RackNumber);
            var @CartonNumber = new SqlParameter("@CartonNumber", model.CartonNumber);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Pairs = new SqlParameter("@Pairs", model.Pairs);

            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialRackPosition_3 @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber,@SizeNo,@Pairs", @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber, @SizeNo, @Pairs) >= 1)
            {
                return true;
            }
            return false;

            //var pairs = model.Pairs > 0 ? model.Pairs : 0;
        }

        public static bool Update(OutsoleMaterialRackPositionModel model)
        {
            var @RackPositionID = new SqlParameter("@RackPositionID", model.RackPositionID);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @RackNumber = new SqlParameter("@RackNumber", model.RackNumber);
            var @CartonNumber = new SqlParameter("@CartonNumber", model.CartonNumber);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Pairs = new SqlParameter("@Pairs", model.Pairs);

            if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterialRackPosition_2 @RackPositionID, @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber,@SizeNo,@Pairs", @RackPositionID, @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber, @SizeNo, @Pairs) >= 1)
            {
                return true;
            }
            return false;
        }

        public static bool Delete(OutsoleMaterialRackPositionModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @RackNumber = new SqlParameter("@RackNumber", model.RackNumber);
            var @CartonNumber = new SqlParameter("@CartonNumber", model.CartonNumber);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);

            if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleMaterialRackPosition_1 @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber, @SizeNo", @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber, @SizeNo) >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
