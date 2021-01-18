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
    public class ReportController
    {
        /// <summary>
        /// Get outsole material balance
        /// </summary>
        /// <returns></returns>
        public static List<OutsoleMaterialBalanceReportModel> GetOutsoleBalanceReportList()
        {
            using (var db = new SaovietMasterScheduleEntities()) 
            {
                return db.ExecuteStoreQuery<OutsoleMaterialBalanceReportModel>("EXEC reporter_OutsoleMaterialBalance").ToList();
                //return db.ExecuteStoreQuery<OutsoleMaterialBalanceReportModel>("EXEC reporter_OutsoleMaterialBalance").ToList();
            };
        }

        public static List<OutsoleMaterialBalanceReportModel> GetOutsoleMaterialHasReject()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleMaterialBalanceReportModel>("EXEC reporter_OutsoleMaterialReject").ToList();
            };
        }

        public static List<OutsoleWHDeliveryReportModel> GetOutsoleWHDeliveryReport()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OutsoleWHDeliveryReportModel>("EXEC ").ToList();
            };
        }

        public static List<ReportOutsoleMaterialDeliveryFromToModel> GetOutsoleDeliveryFromTo(DateTime dateFrom, DateTime dateTo)
        {
            var @DateFrom = new SqlParameter("@DateFrom", dateFrom);
            var @DateTo = new SqlParameter("@DateTo", dateTo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOutsoleMaterialDeliveryFromToModel>("EXEC reporter_SelectOutsoleDeliveryFromTo @DateFrom, @DateTo", @DateFrom, @DateTo).ToList();
            };
        }

        public static List<SewingMasterViewModelNew> GetSewingSummaryReport()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<SewingMasterViewModelNew>("EXEC reporter_ReportSewingSummary").ToList();
            };
        }

        public static List<ReportOutsoleMaterialDeliverySummary> GetOSDeliverySummary()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOutsoleMaterialDeliverySummary>("EXEC reporter_OutsoleMaterialDeliverySummary").ToList();
            };
        }

        public static List<ReportOSMWHCheckingModel> GetOSMWHCheckingFromTo(DateTime dateFrom, DateTime dateTo)
        {
            var @DateFrom = new SqlParameter("@DateFrom", dateFrom);
            var @DateTo = new SqlParameter("@DateTo", dateTo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOSMWHCheckingModel>("EXEC reporter_SelectOSMWHCheckingFromTo @DateFrom, @DateTo", @DateFrom, @DateTo).ToList();
            };
        }
        public static List<ReportOSMWHCheckingModel> GetOSMWHCheckingByPO(String poSearch)
        {
            var @ProductNo = new SqlParameter("@ProductNo", poSearch);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOSMWHCheckingModel>("EXEC reporter_SelectOSMWHCheckingByPO @ProductNo", @ProductNo).ToList();
            };
        }

        public static List<ReportOSMWHCheckingModel> SelectOSWHMCheckingFromTo(DateTime dateFrom, DateTime dateTo)
        {
            var @DateFrom = new SqlParameter("@DateFrom", dateFrom);
            var @DateTo = new SqlParameter("@DateTo", dateTo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOSMWHCheckingModel>("EXEC reporter_SelectOSMWHCheckingListFromTo @DateFrom, @DateTo", @DateFrom, @DateTo).ToList();
            };
        }

        public static List<ReportOSMWHCheckingModel> SelectOSMWHCheckingByPO(String poSearch)
        {
            var @ProductNo = new SqlParameter("@ProductNo", poSearch);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOSMWHCheckingModel>("EXEC reporter_SelectOSMWHCheckingListByPO @ProductNo", @ProductNo).ToList();
            };
        }

        public static List<ReportOutsoleInventoryModel> InventoryByOutsoleCode()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOutsoleInventoryModel>("EXEC reporter_OutsoleInventoryByOSCode").ToList();
            };
        }

        public static List<ReportOutsoleInventoryModel> InventoryByOutsoleLine()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOutsoleInventoryModel>("EXEC reporter_OutsoleInventoryByOSLine").ToList();
            };
        }

        public static List<ReportOSMaterialCheckWHInventoryModel> SelectOSMaterialWHInventory()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOSMaterialCheckWHInventoryModel>("EXEC reporter_SelectOSMaterialWHInventory").ToList();
            };
        }

        public static List<ReportOSMaterialCheckDetailModel> SelectOSMaterialCheckByOSCode(string outsoleCode)
        {
            var @OutsoleCode = new SqlParameter("@OutsoleCode", outsoleCode);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportOSMaterialCheckDetailModel>("EXEC reporter_SelectOutsoleMaterialCheckingByOutsoleCode @OutsoleCode", @OutsoleCode).ToList();
            };
        }

        public static List<ReportUpperAccessoriesSummaryModel> SelectUpperAccessoriesDeliverySummary()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportUpperAccessoriesSummaryModel>("EXEC reporter_AccessoriesDeliverySummary").ToList();
            };
        }

        public static List<ReportUpperAccessoriesDetailBySupplierModel> GetUpperAccessoriesMaterialDeliveryBySupplier(int supplierId)
        {
            var @SupplierId = new SqlParameter("@SupplierId", supplierId);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportUpperAccessoriesDetailBySupplierModel>("reporter_AccessoriesDeliveryBySupplier @SupplierId", @SupplierId).ToList();
            }
        }

        //
        public static List<ReportUpperAccessoriesModel> GetUpperAccessories()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<ReportUpperAccessoriesModel>("reporter_ReportUpperAccessories").ToList();
            }
        }
    }
}
