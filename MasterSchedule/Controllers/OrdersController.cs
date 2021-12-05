using System;
using System.Collections.Generic;
using System.Linq;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OrdersController
    {
        public static List<OrdersModel> Select()
        {
            using (var db = new SaovietMasterScheduleEntities()) {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrders").ToList();
            };
        }

        public static List<OrdersModel> SelectFull(DateTime etdStart, DateTime etdEnd)
        {
            
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersFull @ETDStart, @ETDEnd", ETDStart, ETDEnd).ToList();
            };
        }

        public static List<OrdersModel> SelectSubString()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersSubString").ToList();
            };
        }
        
        public static OrdersModel SelectByArticleNo6(string articleNo)
        {
            var @ArticleNo = new SqlParameter("@ArticleNo", articleNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByArticle6Char @ArticleNo", @ArticleNo).FirstOrDefault();
            };
        }

        // IsEnable = 1
        public static List<OrdersModel> SelectByOutsoleRawMaterial()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleRawMaterial").ToList();
            };
        }
        
        // IsEnable = 1 || 0
        public static List<OrdersModel> SelectByOutsoleRawMaterialFull(DateTime etdStart, DateTime etdEnd)
        {
            
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleRawMaterialFull @ETDStart, @ETDEnd", @ETDStart, @ETDEnd).ToList();
            };
        }

        public static List<OrdersModel> SelectByUpperComponentRawMaterial()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByUpperComponentRawMaterial").ToList();
            };
        }

        public static List<OrdersModel> SelectByOutsoleMaterial()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleMaterial").ToList();
            };
        }

        public static List<OrdersModel> SelectByAssemblyMaster()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByAssemblyMaster").ToList();
            };
        }

        public static List<OrdersModel> SelectByOutsoleMaterialReject()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleMaterialReject").ToList();
            };
        }

        public static List<OrdersModel> SelectBySewingMaster()
        {
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersBySewingMaster").ToList();
            };
        }
        
        public static OrdersModel SelectTop1(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByProductNo @ProductNo", @ProductNo).FirstOrDefault();
            };
        }

        public static List<OrdersModel> SelectByOutsoleReleaseMaterial(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities())
            {

                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleReleaseMaterialByReportId @ReportId", @ReportId).ToList();
            };
        }

        public static List<OrdersModel> SelectByOutsoleReleaseMaterialToWHInspection(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities()) 
            {
                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleReleaseMaterialToWHInspectionByReportId @ReportId", @ReportId).ToList();
            }
        }

        public static List<OrdersModel> SelectByAssemblyRelease(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            using (var db = new SaovietMasterScheduleEntities())
            {

                return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByAssemblyReleaseByReportId @ReportId", @ReportId).ToList();
            };
        }

        public static bool Insert(OrdersModel model)
        {
            string computerName = "";
            try { computerName = System.Environment.MachineName; }
            catch { computerName = ""; }

            var @ProductNo      = new SqlParameter("@ProductNo", model.ProductNo);
            var @ETD            = new SqlParameter("@ETD", model.ETD);
            var @ArticleNo      = new SqlParameter("@ArticleNo", model.ArticleNo);
            var @ShoeName       = new SqlParameter("@ShoeName", model.ShoeName);
            var @Quantity       = new SqlParameter("@Quantity", model.Quantity);
            var @PatternNo      = new SqlParameter("@PatternNo", model.PatternNo);
            var @MidsoleCode    = new SqlParameter("@MidsoleCode", model.MidsoleCode);
            var @OutsoleCode    = new SqlParameter("@OutsoleCode", model.OutsoleCode);
            var @LastCode       = new SqlParameter("@LastCode", model.LastCode);
            var @Country        = new SqlParameter("@Country", model.Country);
            var @GTNPONo        = new SqlParameter("@GTNPONo", model.GTNPONo);
            var @UCustomerCode  = new SqlParameter("@UCustomerCode", model.UCustomerCode);
            var @Reviser        = new SqlParameter("@Reviser", String.Format("{0}-{1}", model.Reviser, computerName));

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_InsertOrders_2 @ProductNo, @ETD, @ArticleNo, @ShoeName, @Quantity, @PatternNo, @MidsoleCode, @OutsoleCode, @LastCode, @Country, @GTNPONo, @UCustomerCode, @Reviser",
                                                                    @ProductNo, @ETD, @ArticleNo, @ShoeName, @Quantity, @PatternNo, @MidsoleCode, @OutsoleCode, @LastCode, @Country, @GTNPONo, @UCustomerCode, @Reviser) > 0)
                {
                    return true;
                }
                return false;
            };
        }

        public static bool Update(string productNo, bool isEnable, string reviser)
        {
            string computerName = "";
            try { computerName = System.Environment.MachineName; }
            catch { computerName = ""; }

            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @IsEnable = new SqlParameter("@IsEnable", isEnable);
            var @Reviser = new SqlParameter("@Reviser", String.Format("{0}-{1}", reviser, computerName));

            using (var db = new SaovietMasterScheduleEntities()) {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOrdersOfIsEnable_1 @ProductNo, @IsEnable, @Reviser",
                                                                          @ProductNo, @IsEnable, @Reviser) > 0)
                {
                    return true;
                }
                return false;
            };
        }

        public static bool Update(OrdersModel model)
        {
            string computerName = "";
            try { computerName = System.Environment.MachineName; }
            catch { computerName = ""; }

            var @ProductNo      = new SqlParameter("@ProductNo", model.ProductNo);
            var @GTNPONo        = new SqlParameter("@GTNPONo", model.GTNPONo);
            var @UCustomerCode  = new SqlParameter("@UCustomerCode", model.UCustomerCode);
            var @ETD            = new SqlParameter("@ETD", model.ETD);
            var @ArticleNo      = new SqlParameter("@ArticleNo", model.ArticleNo);
            var @ShoeName       = new SqlParameter("@ShoeName", model.ShoeName);
            var @Quantity       = new SqlParameter("@Quantity", model.Quantity);
            var @PatternNo      = new SqlParameter("@PatternNo", model.PatternNo);
            var @MidsoleCode    = new SqlParameter("@MidsoleCode", model.MidsoleCode);
            var @OutsoleCode    = new SqlParameter("@OutsoleCode", model.OutsoleCode);
            var @LastCode       = new SqlParameter("@LastCode", model.LastCode);
            var @Country        = new SqlParameter("@Country", model.Country);
            var @IsEnable       = new SqlParameter("@IsEnable", model.IsEnable);
            var @TypeOfShoes    = new SqlParameter("@TypeOfShoes", model.TypeOfShoes);
            var @Reviser        = new SqlParameter("@Reviser", String.Format("{0}-{1}", model.Reviser, computerName));
            var @ShipDate = new SqlParameter("@ShipDate", model.ShipDate);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOrders_4 @ProductNo, @GTNPONo, @UCustomerCode, @ETD, @ArticleNo, @ShoeName, @Quantity, @PatternNo, @MidsoleCode, @OutsoleCode, @LastCode, @Country, @IsEnable, @Reviser, @TypeOfShoes, @ShipDate",
                                                                    @ProductNo, @GTNPONo, @UCustomerCode, @ETD, @ArticleNo, @ShoeName, @Quantity, @PatternNo, @MidsoleCode, @OutsoleCode, @LastCode, @Country, @IsEnable, @Reviser, @TypeOfShoes, @ShipDate) > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public static bool Update(OrdersModel model, List<String> updateWhatList)
        {
            //var propertyList = typeof(OrdersModel).GetProperties().Select(s=>s.Name.ToString()).ToList();
            string computerName = "";
            try { computerName = System.Environment.MachineName; }
            catch { computerName = ""; }

            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);

            // ETD
            var @ETD = new SqlParameter("@ETD", model.ETD);
            bool isUpdateETD = updateWhatList.Contains("ETD");
            var @IsUpdateETD = new SqlParameter("@IsUpdateETD", isUpdateETD);

            // ShipDate
            var @ShipDate = new SqlParameter("@ShipDate", model.ShipDate);
            var isUpdateShipDate = updateWhatList.Contains("ShipDate");
            var @IsUpDateShipDate = new SqlParameter("@IsUpDateShipDate", isUpdateShipDate);

            // Article
            var @ArticleNo = new SqlParameter("@ArticleNo", model.ArticleNo);
            bool isUpdateArticleNo = updateWhatList.Contains("ArticleNo");
            var @IsUpdateArticleNo = new SqlParameter("@IsUpdateArticleNo", isUpdateArticleNo);

            // ShoeName
            var @ShoeName = new SqlParameter("@ShoeName", model.ShoeName);
            bool isUpdateShoeName = updateWhatList.Contains("ShoeName");
            var @IsUpdateShoeName = new SqlParameter("@IsUpdateShoeName", isUpdateShoeName);

            // Quantity
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            bool isUpdateQuantity = updateWhatList.Contains("Quantity");
            var @IsUpdateQuantity = new SqlParameter("@IsUpdateQuantity", isUpdateQuantity);

            // PatternNo
            var @PatternNo = new SqlParameter("@PatternNo", model.PatternNo);
            bool isUpdatePatternNo = updateWhatList.Contains("PatternNo");
            var @IsUpdatePatternNo = new SqlParameter("@IsUpdatePatternNo", isUpdatePatternNo);

            // MidsoleCode
            var @MidsoleCode = new SqlParameter("@MidsoleCode", model.MidsoleCode);
            bool isUpdateMidsoleCode = updateWhatList.Contains("MidsoleCode");
            var @IsUpdateMidsoleCode = new SqlParameter("@IsUpdateMidsoleCode", isUpdateMidsoleCode);

            // OutsoleCode
            var @OutsoleCode = new SqlParameter("@OutsoleCode", model.OutsoleCode);
            bool isUpdateOutsoleCode = updateWhatList.Contains("OutsoleCode");
            var @IsUpdateOutsoleCode = new SqlParameter("@IsUpdateOutsoleCode", isUpdateOutsoleCode);

            // LastCode
            var @LastCode = new SqlParameter("@LastCode", model.LastCode);
            bool isUpdateLastCode = updateWhatList.Contains("LastCode");
            var @IsUpdateLastCode = new SqlParameter("@IsUpdateLastCode", isUpdateLastCode);

            // Country
            var @Country = new SqlParameter("@Country", model.Country);
            bool isUpdateCountry = updateWhatList.Contains("Country");
            var @IsUpdateCountry = new SqlParameter("@IsUpdateCountry", isUpdateCountry);

            // GTNPONo
            var @GTNPONo = new SqlParameter("@GTNPONo", model.GTNPONo);
            bool isUpdateGTNPONo = updateWhatList.Contains("GTNPONo");
            var @IsUpdateGTNPONo = new SqlParameter("@IsUpdateGTNPONo", isUpdateGTNPONo);

            // UCustomerCode
            var @UCustomerCode = new SqlParameter("@UCustomerCode", model.UCustomerCode);
            bool isUpdateUCustomerCode = updateWhatList.Contains("UCustomerCode");
            var @IsUpdateUCustomerCode = new SqlParameter("@IsUpdateUCustomerCode", isUpdateUCustomerCode);

            var @Reviser = new SqlParameter("@Reviser", String.Format("{0}-{1}", model.Reviser, computerName));

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOrdersSpecify_2 @ProductNo, @GTNPONo, @IsUpdateGTNPONo, @UCustomerCode, @IsUpdateUCustomerCode, @ETD, @IsUpdateETD, @ArticleNo, @IsUpdateArticleNo, @ShoeName, @IsUpdateShoeName, @Quantity, @IsUpdateQuantity, @PatternNo, @IsUpdatePatternNo, @MidsoleCode, @IsUpdateMidsoleCode, @OutsoleCode, @IsUpdateOutsoleCode, @LastCode ,@IsUpdateLastCode, @Country, @IsUpdateCountry, @Reviser, @ShipDate, @IsUpDateShipDate",
                                                                           @ProductNo, @GTNPONo, @IsUpdateGTNPONo, @UCustomerCode, @IsUpdateUCustomerCode, @ETD, @IsUpdateETD, @ArticleNo, @IsUpdateArticleNo, @ShoeName, @IsUpdateShoeName, @Quantity, @IsUpdateQuantity, @PatternNo, @IsUpdatePatternNo, @MidsoleCode, @IsUpdateMidsoleCode, @OutsoleCode, @IsUpdateOutsoleCode, @LastCode, @IsUpdateLastCode, @Country, @IsUpdateCountry, @Reviser, @ShipDate, @IsUpDateShipDate) > 0)
                {
                    return true;
                }
                return false;
            };
        }

        public static bool Delete(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_DeleteOrders @ProductNo", @ProductNo) > 0)
                {
                    return true;
                }
                return false;
            };
        }
    }
}
