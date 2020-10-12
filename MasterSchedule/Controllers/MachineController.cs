using MasterSchedule.Entities;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace MasterSchedule.Controllers
{
    public class MachineController
    {
        public static List<MachineModel> Select()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<MachineModel>("EXEC spm_SelectMachine").ToList();
            }  
        }
        //
        public static bool Insert(MachineModel model)
        {
            var @PhaseID        = new SqlParameter("@PhaseID",      model.PhaseSelected.PhaseID);
            var @MachineID      = new SqlParameter("@MachineID",    model.MachineID);
            var @MachineName    = new SqlParameter("@MachineName",  model.MachineName);
            var @IsMachine      = new SqlParameter("@IsMachine",    model.IsMachine);
            var @Available      = new SqlParameter("@Available",    model.Available);
            var @Remarks        = new SqlParameter("@Remarks",      string.IsNullOrEmpty(model.Remarks) == false ? model.Remarks : "");

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMachine @PhaseID, @MachineID, @MachineName, @IsMachine, @Available, @Remarks", 
                                                                   @PhaseID, @MachineID, @MachineName, @IsMachine, @Available, @Remarks) > 0)
                {
                    return true;
                }
                return false;
            }
        }
        //
        public static bool Delete(int machineID)
        {
            var @MachineID = new SqlParameter("@MachineID", machineID);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteMachine @MachineID", @MachineID) > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
