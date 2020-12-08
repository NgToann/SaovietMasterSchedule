using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class RejectController
    {
        public static List<RejectModel> GetRejectUpperAccessories()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<RejectModel>("spm_SelectRejectUpperAccessories").ToList();
            }
        }
    }
}
