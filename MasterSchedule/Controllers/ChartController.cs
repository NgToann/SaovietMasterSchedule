using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class ChartController
    {
        public static List<ChartRawMaterialModel> GetRawMaterial()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ChartRawMaterialModel>("EXEC chart_RawMaterialWithPOETDInThisYear").ToList();
            };
        }
    }
}
