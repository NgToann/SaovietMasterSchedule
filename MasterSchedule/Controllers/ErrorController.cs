using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class ErrorController
    {
        public static List<ErrorModel> GetError()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ErrorModel>("spm_SelectError").ToList();
            }
        }
    }
}
