using MasterSchedule.Entities;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MasterSchedule.Controllers
{
    public class MachineRequestController
    {
        public static List<MachineRequestModel> SelectByArticle(string articleNo)
        {
            var @ArticleNo = new SqlParameter("@ArticleNo", articleNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<MachineRequestModel>("EXEC spm_SelectMachineRequestByArticle @ArticleNo", @ArticleNo).ToList();
            }
        }

        public static bool Insert(MachineRequestModel model)
        {
            var @ArticleNo      = new SqlParameter("@MachineID",    model.MachineID);
            var @ShoeName       = new SqlParameter("@ShoeName",     model.ShoeName);
            var @PatternNo      = new SqlParameter("@PatternNo",    model.PatternNo);
            var @PhaseID        = new SqlParameter("@PhaseID",      model.PhaseID);
            var @Quota          = new SqlParameter("@Quota",        model.Quota);
            var @MachineID      = new SqlParameter("@MachineID",    model.MachineID);
            var @Quantity       = new SqlParameter("@Quantity",     model.Quantity);
            var @Worker         = new SqlParameter("@Worker",       model.Worker);
            

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertMachine @MachineID, ",
                                                                   @MachineID) > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
