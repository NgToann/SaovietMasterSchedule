using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class PrivateDefineController
    {
        public static PrivateDefineModel GetDefine()
        {
            using(var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<PrivateDefineModel>("EXEC spm_SelectPrivateDefine").FirstOrDefault();
            }
        }
    }
}
