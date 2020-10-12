using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    class PhaseController
    {
        public static List<PhaseModel> Select()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<PhaseModel>("EXEC spm_SelectPhase").ToList();
            }
        }
    }
}
