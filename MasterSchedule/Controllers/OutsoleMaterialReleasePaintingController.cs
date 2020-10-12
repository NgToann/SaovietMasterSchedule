using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    public class OutsoleMaterialReleasePaintingController
    {
        //
        public static List<OutsoleMaterialReleasePaintingModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialReleasePaintingModel>("EXEC spm_SelectOutsoleMaterialReleasePaintingByPO @ProductNo", @ProductNo).ToList();
            };
        }

        public static bool Insert(OutsoleMaterialReleasePaintingModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @ReleaseDate = new SqlParameter("@ReleaseDate", model.ReleaseDate);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialReleasePainting   @ProductNo, @OutsoleSupplierId, @ReleaseDate, @SizeNo, @Quantity",
                                                                                            @ProductNo, @OutsoleSupplierId, @ReleaseDate, @SizeNo, @Quantity) >= 1)
                {
                    return true;
                }
                return false;
            };
        }
    }
}
