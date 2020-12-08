using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class SupplierController
    {
        public static List<SupplierModel> GetSuppliers()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<SupplierModel>("spm_SelectSuppliers").ToList();
            }
        }
        public static List<SupplierModel> GetSuppliersAccessories()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<SupplierModel>("spm_SelectSuppliersAccessories").ToList();
            }
        }
    }
}
