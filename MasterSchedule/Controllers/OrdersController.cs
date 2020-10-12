using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OrdersController
    {
        public static List<OrdersModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrders").ToList();
        }

        public static List<OrdersModel> SelectFull(DateTime etdStart, DateTime etdEnd)
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersFull @ETDStart, @ETDEnd", ETDStart, ETDEnd).ToList();
        }

        public static List<OrdersModel> SelectSubString()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersSubString").ToList();
        }
        
        public static OrdersModel SelectByArticleNo6(string articleNo)
        {
            var @ArticleNo = new SqlParameter("@ArticleNo", articleNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByArticle6Char @ArticleNo", @ArticleNo).FirstOrDefault();
        }

        // IsEnable = 1
        public static List<OrdersModel> SelectByOutsoleRawMaterial()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleRawMaterial").ToList();
        }
        
        // IsEnable = 1 || 0
        public static List<OrdersModel> SelectByOutsoleRawMaterialFull(DateTime etdStart, DateTime etdEnd)
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            var @ETDStart = new SqlParameter("@ETDStart", etdStart);
            var @ETDEnd = new SqlParameter("@ETDEnd", etdEnd);
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleRawMaterialFull @ETDStart, @ETDEnd", @ETDStart, @ETDEnd).ToList();
        }

        public static List<OrdersModel> SelectByUpperComponentRawMaterial()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByUpperComponentRawMaterial").ToList();
        }

        public static List<OrdersModel> SelectByOutsoleMaterial()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleMaterial").ToList();
        }

        public static List<OrdersModel> SelectByAssemblyMaster()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByAssemblyMaster").ToList();
        }

        public static List<OrdersModel> SelectByOutsoleMaterialReject()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleMaterialReject").ToList();
        }

        public static List<OrdersModel> SelectBySewingMaster()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersBySewingMaster").ToList();
        }
        
        public static OrdersModel SelectTop1(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByProductNo @ProductNo", @ProductNo).FirstOrDefault();
        }

        public static List<OrdersModel> SelectByOutsoleReleaseMaterial(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByOutsoleReleaseMaterialByReportId @ReportId", @ReportId).ToList();
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
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OrdersModel>("EXEC spm_SelectOrdersByAssemblyReleaseByReportId @ReportId", @ReportId).ToList();
        }

        public static bool Insert(OrdersModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ArticleNo = new SqlParameter("@ArticleNo", model.ArticleNo);
            var @ShoeName = new SqlParameter("@ShoeName", model.ShoeName);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @PatternNo = new SqlParameter("@PatternNo", model.PatternNo);
            var @MidsoleCode = new SqlParameter("@MidsoleCode", model.MidsoleCode);
            var @OutsoleCode = new SqlParameter("@OutsoleCode", model.OutsoleCode);
            var @LastCode = new SqlParameter("@LastCode", model.LastCode);
            var @Country = new SqlParameter("@Country", model.Country);
            var @GTNPONo = new SqlParameter("@GTNPONo", model.GTNPONo);
            var @UCustomerCode = new SqlParameter("@UCustomerCode", model.UCustomerCode);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertOrders_1 @ProductNo,@ETD,@ArticleNo,@ShoeName,@Quantity,@PatternNo,@MidsoleCode,@OutsoleCode,@LastCode,@Country,@GTNPONo,@UCustomerCode", @ProductNo, @ETD, @ArticleNo, @ShoeName, @Quantity, @PatternNo, @MidsoleCode, @OutsoleCode, @LastCode, @Country, @GTNPONo, @UCustomerCode) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Update(string productNo, bool isEnable)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @IsEnable = new SqlParameter("@IsEnable", isEnable);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_UpdateOrdersOfIsEnable @ProductNo, @IsEnable", @ProductNo, @IsEnable) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Update(OrdersModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @GTNPONo = new SqlParameter("@GTNPONo", model.GTNPONo);
            var @UCustomerCode = new SqlParameter("@UCustomerCode", model.UCustomerCode);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ArticleNo = new SqlParameter("@ArticleNo", model.ArticleNo);
            var @ShoeName = new SqlParameter("@ShoeName", model.ShoeName);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @PatternNo = new SqlParameter("@PatternNo", model.PatternNo);
            var @MidsoleCode = new SqlParameter("@MidsoleCode", model.MidsoleCode);
            var @OutsoleCode = new SqlParameter("@OutsoleCode", model.OutsoleCode);
            var @LastCode = new SqlParameter("@LastCode", model.LastCode);
            var @Country = new SqlParameter("@Country", model.Country);
            var @IsEnable = new SqlParameter("@IsEnable", model.IsEnable);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_UpdateOrders_1 @ProductNo,@GTNPONo,@UCustomerCode,@ETD,@ArticleNo,@ShoeName,@Quantity,@PatternNo,@MidsoleCode,@OutsoleCode,@LastCode,@Country,@IsEnable", @ProductNo, @GTNPONo, @UCustomerCode, @ETD, @ArticleNo, @ShoeName, @Quantity, @PatternNo, @MidsoleCode, @OutsoleCode, @LastCode, @Country, @IsEnable) > 0)
            {
                return true;
            }
            return false;
        }


        public static bool Update(OrdersModel model, List<String> updateWhatList)
        {
            //var propertyList = typeof(OrdersModel).GetProperties().Select(s=>s.Name.ToString()).ToList();

            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);

            // ETD
            var @ETD = new SqlParameter("@ETD", model.ETD);
            bool isUpdateETD = updateWhatList.Contains("ETD");
            var @IsUpdateETD = new SqlParameter("@IsUpdateETD", isUpdateETD);

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

            using (var db = new SaovietMasterScheduleEntities())
            {
                if (db.ExecuteStoreCommand("EXEC spm_UpdateOrdersSpecify @ProductNo, @GTNPONo, @IsUpdateGTNPONo, @UCustomerCode, @IsUpdateUCustomerCode, @ETD, @IsUpdateETD, @ArticleNo, @IsUpdateArticleNo, @ShoeName, @IsUpdateShoeName, @Quantity, @IsUpdateQuantity, @PatternNo, @IsUpdatePatternNo, @MidsoleCode, @IsUpdateMidsoleCode, @OutsoleCode, @IsUpdateOutsoleCode, @LastCode ,@IsUpdateLastCode, @Country, @IsUpdateCountry",
                                                                         @ProductNo, @GTNPONo, @IsUpdateGTNPONo, @UCustomerCode, @IsUpdateUCustomerCode, @ETD, @IsUpdateETD, @ArticleNo, @IsUpdateArticleNo, @ShoeName, @IsUpdateShoeName, @Quantity, @IsUpdateQuantity, @PatternNo, @IsUpdatePatternNo, @MidsoleCode, @IsUpdateMidsoleCode, @OutsoleCode, @IsUpdateOutsoleCode, @LastCode, @IsUpdateLastCode, @Country, @IsUpdateCountry) > 0)
                {
                    return true;
                }
                return false;
            };
        }
        /*
         @ProductNo,
         @GTNPONo, @IsUpdateGTNPONo,
         @UCustomerCode, @IsUpdateCustomerCode,
         @ETD, @IsUpdateETD,
	     @ArticleNo, @IsUpdateArticleNo,
	    @ShoeName, @IsUpdateShoeName,
	    @Quantity,@IsUpdateQuantity,
	    @PatternNo ,@IsUpdatePatternNo,
	    @MidsoleCode ,@IsUpdateMidsoleCode,
	    @OutsoleCode ,@IsUpdateOutsoleCode,
	    @LastCode ,@IsUpdateLastCode,
	    @Country,@IsUpdateCountry
         */

        public static bool Delete(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_DeleteOrders @ProductNo", @ProductNo) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
