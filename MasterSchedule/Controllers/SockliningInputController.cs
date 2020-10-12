using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class SockliningInputController
    {
        public static List<SockliningInputModel> SelectByAssemblyMaster()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<SockliningInputModel>("EXEC spm_SelectSockliningInputByAssemblyMaster").ToList();
            };
        }

        public static List<SockliningInputModel> SelectByPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<SockliningInputModel>("EXEC spm_SelectSockliningInputByPO @ProductNo", @ProductNo).ToList();
            };
        }

        public static bool Insert(SockliningInputModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertSockliningInput @ProductNo, @SizeNo, @Quantity", @ProductNo, @SizeNo, @Quantity) > 0)
                {
                    return true;
                }
                return false;
            };
        }
    }
}
