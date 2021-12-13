using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    public class OSMaterialBorrowController
    {
        public static List<OSMaterialBorrowModel> GetByPO(string po)
        {
            var @ProductNo = new SqlParameter("@ProductNo", po);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OSMaterialBorrowModel>("EXEC spm_SelectOSMaterialBorrowByPO @ProductNo", @ProductNo).ToList();
            }
        }
        public static List<OSMaterialBorrowModel> GetByOSCheckingId(int osCheckingId)
        {
            var @OSCheckingId= new SqlParameter("@OSCheckingId", osCheckingId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OSMaterialBorrowModel>("EXEC spm_SelectOSMaterialBorrowByOSCheckingId @OSCheckingId", @OSCheckingId).ToList();
            }
        }
        public static bool Insert(OSMaterialBorrowModel model)
        {
            var @OSCheckingID= new SqlParameter("@OSCheckingID", model.OSCheckingId);
            var @ProductNoBorrow = new SqlParameter("@ProductNoBorrow", model.ProductNoBorrow);
            var @QuantityBorrow= new SqlParameter("@QuantityBorrow", model.QuantityBorrow);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOSMaterialBorrow @OSCheckingID, @ProductNoBorrow, @QuantityBorrow", @OSCheckingID, @ProductNoBorrow, @QuantityBorrow) >= 1)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool Update(OSMaterialBorrowModel model)
        {
            var @Id= new SqlParameter("@Id", model.Id);
            var @ProductNoBorrow = new SqlParameter("@ProductNoBorrow", model.ProductNoBorrow);
            var @QuantityBorrow = new SqlParameter("@QuantityBorrow", model.QuantityBorrow);

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOSMaterialBorrow @Id, @ProductNoBorrow, @QuantityBorrow", @Id, @ProductNoBorrow, @QuantityBorrow) >= 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
