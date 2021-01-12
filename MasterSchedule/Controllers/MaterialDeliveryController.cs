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
                return db.ExecuteStoreQuery<MaterialDeliveryModel>("spm_SelectMaterialDeliveryByPO_1 @ProductNo", @ProductNo).ToList();
            }
        }
    }
}
