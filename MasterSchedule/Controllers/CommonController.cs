using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class CommonController
    {
        /// <summary>
        /// Update Sequence By ProductNo
        /// </summary>
        /// <param name="productNo"></param>
        /// <param name="sequence"></param>
        /// <param name="section">Sewing, Assembly, Outsole, Socklining</param>
        /// <returns></returns>
        public static bool UpdateSequenceByPO(string productNo, int sequence, string section)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @Sequence = new SqlParameter("@Sequence", sequence);
            var @Section = new SqlParameter("@Section", section);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_UpdateSequencePO   @ProductNo, @Sequence, @Section",
                                                                    @ProductNo, @Sequence, @Section) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
