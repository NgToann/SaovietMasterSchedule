using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.ComponentModel;
using System.Windows.Media;
using System.Text.RegularExpressions;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;

using EXCEL = Microsoft.Office.Interop.Excel;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHDeliveryDetailWindow_1.xaml
    /// </summary>
    public partial class OutsoleWHDeliveryDetailWindow_1 : Window
    {
        string outsoleCode;
        List<OrdersModel> orderList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseList;
        List<SizeRunModel> sizeRunList;
        OutsoleSuppliersModel supplier;
        BackgroundWorker bwLoad;
        BackgroundWorker bwExportExcel;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleRawMaterialModel> outsoleRawMaterial;
        List<ExportExcelModel> excelExportList;
        public OutsoleWHDeliveryDetailWindow_1(string outsoleCode, List<OutsoleMaterialModel> outsoleMaterialList, List<OutsoleReleaseMaterialModel> outsoleReleaseList, List<SizeRunModel> sizeRunList, OutsoleSuppliersModel supplier, List<OrdersModel> orderList)
        {
            this.outsoleCode = outsoleCode;
            this.orderList = orderList;
            this.outsoleMaterialList = outsoleMaterialList;
            this.outsoleReleaseList = outsoleReleaseList;
            this.sizeRunList = sizeRunList;
            this.supplier = supplier;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwExportExcel = new BackgroundWorker();
            bwExportExcel.DoWork += new DoWorkEventHandler(bwExportExcel_DoWork);
            bwExportExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwExportExcel_RunWorkerCompleted);
            bwExportExcel.WorkerSupportsCancellation = true;

            sewingMasterList = new List<SewingMasterModel>();
            outsoleRawMaterial = new List<OutsoleRawMaterialModel>();
            excelExportList = new List<ExportExcelModel>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtTitle.Text = string.Format("{0} {1}", supplier.Name, outsoleCode);
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            sewingMasterList = SewingMasterController.Select();
            outsoleRawMaterial = OutsoleRawMaterialController.Select();
            var productNoList = orderList.Select(s => s.ProductNo).Distinct().ToList();
            sewingMasterList = sewingMasterList.Where(w => productNoList.Contains(w.ProductNo)).ToList();
            outsoleRawMaterial = outsoleRawMaterial.Where(w => productNoList.Contains(w.ProductNo) && w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();

            var regex = new Regex("[a-z]|[A-Z]");
            var sizeNoList = sizeRunList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

            // Create Column
            DataTable dt = new DataTable();
            Dispatcher.Invoke(new Action(() =>
            {
                // Column OutsoleCode
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTemplateColumn colPO = new DataGridTemplateColumn();
                colPO.Header = String.Format("ProductNo");
                DataTemplate tplPO = new DataTemplate();
                FrameworkElementFactory tblPO = new FrameworkElementFactory(typeof(TextBlock));
                tplPO.VisualTree = tblPO;
                tblPO.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ProductNo")));
                tblPO.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
                tblPO.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblPO.SetValue(TextBlock.PaddingProperty, new Thickness(3, 2, 3, 2));
                colPO.CellTemplate = tplPO;
                colPO.SortMemberPath = "ProductNo";
                colPO.ClipboardContentBinding = new Binding(String.Format("ProductNo"));
                dgWHDeliveryDetail.Columns.Add(colPO);

                // Column ArticleNo
                dt.Columns.Add("ArticleNo", typeof(String));
                DataGridTemplateColumn colArticleNo = new DataGridTemplateColumn();
                colArticleNo.Header = String.Format("ArticleNo");
                DataTemplate tplArticleNo = new DataTemplate();
                FrameworkElementFactory tblArticleNo = new FrameworkElementFactory(typeof(TextBlock));
                tplArticleNo.VisualTree = tblArticleNo;
                tblArticleNo.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ArticleNo")));
                tblArticleNo.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblArticleNo.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colArticleNo.CellTemplate = tplArticleNo;
                colArticleNo.SortMemberPath = "ArticleNo";
                colArticleNo.ClipboardContentBinding = new Binding(String.Format("ArticleNo"));
                dgWHDeliveryDetail.Columns.Add(colArticleNo);

                // Column OrderEFD
                dt.Columns.Add("OrderEFD", typeof(DateTime));
                DataGridTemplateColumn colEFDOrder = new DataGridTemplateColumn();
                colEFDOrder.Header = String.Format("Order\nEFD");
                DataTemplate tplEFDOrder = new DataTemplate();
                FrameworkElementFactory tblEFDOrder = new FrameworkElementFactory(typeof(TextBlock));
                tplEFDOrder.VisualTree = tblEFDOrder;

                Binding bindingOrderEFD = new Binding();
                bindingOrderEFD.Path = new PropertyPath("OrderEFD");
                bindingOrderEFD.StringFormat = "dd-MMM";

                //tblEFDOrder.SetBinding(TextBlock.TextProperty, new Binding(String.Format("OrderEFD")));
                tblEFDOrder.SetBinding(TextBlock.TextProperty, bindingOrderEFD);
                tblEFDOrder.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblEFDOrder.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colEFDOrder.SortMemberPath = "OrderEFD";
                colEFDOrder.ClipboardContentBinding = bindingOrderEFD;
                colEFDOrder.CellTemplate = tplEFDOrder;
                dgWHDeliveryDetail.Columns.Add(colEFDOrder);

                // Column DeliveryEFD
                dt.Columns.Add("DeliveryEFD", typeof(DateTime));
                DataGridTemplateColumn colEFDDelivery = new DataGridTemplateColumn();
                colEFDDelivery.Header = String.Format("Delivery\nEFD");
                DataTemplate tplEFDDelivery = new DataTemplate();
                FrameworkElementFactory tblEFDDelivery = new FrameworkElementFactory(typeof(TextBlock));
                tplEFDDelivery.VisualTree = tblEFDDelivery;

                Binding bindingDeliveryEFD = new Binding();
                bindingDeliveryEFD.Path = new PropertyPath("DeliveryEFD");
                bindingDeliveryEFD.StringFormat = "dd-MMM";

                //tblEFDDelivery.SetBinding(TextBlock.TextProperty, new Binding(String.Format("DeliveryEFD")));
                tblEFDDelivery.SetBinding(TextBlock.TextProperty, bindingDeliveryEFD);
                tblEFDDelivery.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblEFDDelivery.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colEFDDelivery.SortMemberPath = "DeliveryEFD";
                colEFDDelivery.ClipboardContentBinding = bindingDeliveryEFD;
                colEFDDelivery.CellTemplate = tplEFDDelivery;
                dgWHDeliveryDetail.Columns.Add(colEFDDelivery);

                // Column SewingStart
                dt.Columns.Add("SewingStartDate", typeof(DateTime));
                DataGridTemplateColumn colSewingStart = new DataGridTemplateColumn();
                colSewingStart.Header = String.Format("Sewing\nStart");
                DataTemplate tplSewingStart = new DataTemplate();
                FrameworkElementFactory tblSewingStart = new FrameworkElementFactory(typeof(TextBlock));
                tplSewingStart.VisualTree = tblSewingStart;

                Binding bindingSewingStart = new Binding();
                bindingSewingStart.Path = new PropertyPath("SewingStartDate");
                bindingSewingStart.StringFormat = "dd-MMM";

                //tblSewingStart.SetBinding(TextBlock.TextProperty, new Binding(String.Format("SewingStartDate")));
                tblSewingStart.SetBinding(TextBlock.TextProperty, bindingSewingStart);
                tblSewingStart.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblSewingStart.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colSewingStart.SortMemberPath = "SewingStartDate";
                colSewingStart.ClipboardContentBinding = bindingSewingStart;
                colSewingStart.CellTemplate = tplSewingStart;
                dgWHDeliveryDetail.Columns.Add(colSewingStart);

                // Column QuantityOrder
                dt.Columns.Add("QuantityOrder", typeof(Int32));
                DataGridTemplateColumn colQtyOrder = new DataGridTemplateColumn();
                colQtyOrder.Header = String.Format("Quantity\nOrder");
                DataTemplate tplQtyOrder = new DataTemplate();
                FrameworkElementFactory tblQtyOrder = new FrameworkElementFactory(typeof(TextBlock));
                tplQtyOrder.VisualTree = tblQtyOrder;
                tblQtyOrder.SetBinding(TextBlock.TextProperty, new Binding(String.Format("QuantityOrder")));
                tblQtyOrder.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblQtyOrder.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblQtyOrder.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colQtyOrder.CellTemplate = tplQtyOrder;
                colQtyOrder.SortMemberPath = "QuantityOrder";
                colQtyOrder.ClipboardContentBinding = new Binding(String.Format("QuantityOrder"));
                dgWHDeliveryDetail.Columns.Add(colQtyOrder);

                // Column Release
                dt.Columns.Add("Release", typeof(Int32));
                DataGridTemplateColumn colRelease = new DataGridTemplateColumn();
                colRelease.Header = String.Format("Release");
                DataTemplate tplRelease = new DataTemplate();
                FrameworkElementFactory tblRelease = new FrameworkElementFactory(typeof(TextBlock));
                tplRelease.VisualTree = tblRelease;
                tblRelease.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Release")));
                tblRelease.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblRelease.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblRelease.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colRelease.CellTemplate = tplRelease;
                colRelease.SortMemberPath = "Release";
                colRelease.ClipboardContentBinding = new Binding(String.Format("Release"));
                dgWHDeliveryDetail.Columns.Add(colRelease);

                // Column Delivery
                dt.Columns.Add("Delivery", typeof(Int32));
                DataGridTemplateColumn colDelivery = new DataGridTemplateColumn();
                colDelivery.Header = String.Format("Delivery");
                DataTemplate tplDelivery = new DataTemplate();
                FrameworkElementFactory tblDelivery = new FrameworkElementFactory(typeof(TextBlock));
                tplDelivery.VisualTree = tblDelivery;
                tblDelivery.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Delivery")));
                tblDelivery.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblDelivery.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblDelivery.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colDelivery.CellTemplate = tplDelivery;
                colDelivery.SortMemberPath = "Delivery";
                colDelivery.ClipboardContentBinding = new Binding(String.Format("Delivery"));
                dgWHDeliveryDetail.Columns.Add(colDelivery);

                // Column Reject
                dt.Columns.Add("Reject", typeof(Int32));
                DataGridTemplateColumn colReject = new DataGridTemplateColumn();
                colReject.Header = String.Format("Reject");
                DataTemplate tplReject = new DataTemplate();
                FrameworkElementFactory tblReject = new FrameworkElementFactory(typeof(TextBlock));
                tplReject.VisualTree = tblReject;
                tblReject.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Reject")));
                tblReject.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblReject.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblReject.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colReject.CellTemplate = tplReject;
                colReject.SortMemberPath = "Reject";
                colReject.ClipboardContentBinding = new Binding(String.Format("Reject"));
                dgWHDeliveryDetail.Columns.Add(colReject);

                // Column SizeNo
                for (int i = 0; i < sizeNoList.Count(); i++)
                {
                    var sizeRun_Size = sizeRunList.FirstOrDefault(w => w.SizeNo == sizeNoList[i]);
                    if (sizeRun_Size == null)
                        continue;
                    string outsoleSize = "", midsoleSize = "";
                    if (sizeRun_Size != null)
                    {
                        outsoleSize = sizeRun_Size.OutsoleSize;
                        midsoleSize = sizeRun_Size.MidsoleSize;
                    }

                    string sizeID = sizeRun_Size.SizeNo.Contains(".") == true ? sizeRun_Size.SizeNo.Replace(".", "@") : sizeRun_Size.SizeNo;

                    dt.Columns.Add(String.Format("Column{0}", sizeID), typeof(String));
                    dt.Columns.Add(String.Format("ColumnBackground{0}", sizeID), typeof(SolidColorBrush));
                    dt.Columns.Add(String.Format("ColumnForeground{0}", sizeID), typeof(SolidColorBrush));
                    dt.Columns.Add(String.Format("ColumnTooltip{0}", sizeID), typeof(String));
                    DataGridTemplateColumn colSize = new DataGridTemplateColumn();
                    colSize.Header = String.Format("{0}\n{1}\n", sizeNoList[i], outsoleSize);
                    colSize.MinWidth = 35;
                    DataTemplate tplSize = new DataTemplate();
                    FrameworkElementFactory tblSize = new FrameworkElementFactory(typeof(TextBlock));
                    tplSize.VisualTree = tblSize;
                    tblSize.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Column{0}", sizeID)));
                    tblSize.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblSize.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblSize.SetValue(TextBlock.PaddingProperty, new Thickness(3, 3, 3, 3));

                    tblSize.SetValue(TextBlock.BackgroundProperty, new Binding(String.Format("ColumnBackground{0}", sizeID)));
                    tblSize.SetValue(TextBlock.ForegroundProperty, new Binding(String.Format("ColumnForeground{0}", sizeID)));
                    tblSize.SetValue(TextBlock.ToolTipProperty, new Binding(String.Format("ColumnTooltip{0}", sizeID)));

                    colSize.CellTemplate = tplSize;
                    colSize.ClipboardContentBinding = new Binding(String.Format("Column{0}", sizeID));
                    dgWHDeliveryDetail.Columns.Add(colSize);
                }
                // Column Balance
                dt.Columns.Add("TotalBalance", typeof(Int32));
                DataGridTemplateColumn colBalance = new DataGridTemplateColumn();
                colBalance.Header = String.Format("Total\nBalance");
                DataTemplate tplBalance = new DataTemplate();
                FrameworkElementFactory tblBalance = new FrameworkElementFactory(typeof(TextBlock));
                tplBalance.VisualTree = tblBalance;
                tblBalance.SetBinding(TextBlock.TextProperty, new Binding(String.Format("TotalBalance")));
                tblBalance.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblBalance.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblBalance.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colBalance.CellTemplate = tplBalance;
                colBalance.SortMemberPath = "TotalBalance";
                colBalance.ClipboardContentBinding = new Binding(String.Format("TotalBalance"));
                dgWHDeliveryDetail.Columns.Add(colBalance);

                // Column Reject
                dt.Columns.Add("ReleasePainting", typeof(Int32));
                DataGridTemplateColumn colReleasePainting = new DataGridTemplateColumn();
                colReleasePainting.Header = String.Format("Release\nPainting");
                DataTemplate tplReleasePainting = new DataTemplate();
                FrameworkElementFactory tblReleasePainting = new FrameworkElementFactory(typeof(TextBlock));
                tplReleasePainting.VisualTree = tblReleasePainting;
                tblReleasePainting.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ReleasePainting")));
                tblReleasePainting.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                tblReleasePainting.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                tblReleasePainting.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 0, 0));
                colReleasePainting.CellTemplate = tplReleasePainting;
                colReleasePainting.SortMemberPath = "ReleasePainting";
                colReleasePainting.ClipboardContentBinding = new Binding(String.Format("ReleasePainting"));
                dgWHDeliveryDetail.Columns.Add(colReleasePainting);
            }));

            // Binding Data
            if (productNoList.Count > 0)
                productNoList = productNoList.OrderBy(o => o).ToList();

            foreach (var productNo in productNoList)
            {
                DataRow dr = dt.NewRow();

                var order_PO            = orderList.FirstOrDefault(w => w.ProductNo == productNo);
                var osMaterial_PO       = outsoleMaterialList.Where(w => w.ProductNo == productNo).ToList();
                var osRawMaterial_PO    = outsoleRawMaterial.FirstOrDefault(w => w.ProductNo == productNo);
                var sewingMaster_PO     = sewingMasterList.FirstOrDefault(w => w.ProductNo == productNo);
                var osRelease_PO        = outsoleReleaseList.Where(w => w.ProductNo == productNo).ToList();

                var contentStatusList = new List<ContentStatus>();
                var sizeRunList_PO = sizeRunList.Where(w => w.ProductNo == productNo).ToList();
                for (int i = 0; i < sizeNoList.Count(); i++)
                {
                    string sizeNo = sizeNoList[i];
                    var contentStatus = new ContentStatus();
                    var sizeRun_Size = sizeRunList_PO.FirstOrDefault(w => w.SizeNo == sizeNo);                   
                    string sizeID = sizeNo.Contains(".") == true ? sizeNo.Replace(".", "@") : sizeNo;

                    // if has balance, show balance = qty - delivery + reject
                    // if contains reject highlight
                    int qtyOrder_Size       = sizeRun_Size != null ? sizeRun_Size.Quantity : 0;
                    int qtyDelivery_Size    = osMaterial_PO.FirstOrDefault(w => w.SizeNo == sizeNo) != null ? osMaterial_PO.FirstOrDefault(w => w.SizeNo == sizeNo).Quantity : 0;
                    int qtyReject_Size      = osMaterial_PO.FirstOrDefault(w => w.SizeNo == sizeNo) != null ? osMaterial_PO.FirstOrDefault(w => w.SizeNo == sizeNo).QuantityReject : 0;

                    // Default Color
                    contentStatus.WidthDefaultCell  = 5;
                    contentStatus.BorderColorCell   = System.Drawing.Color.LightGray;
                    contentStatus.ForegroundCell    = System.Drawing.Color.Black;
                    contentStatus.BackgroundCell    = System.Drawing.Color.Transparent;

                    int qtyBalance_Size = qtyOrder_Size - qtyDelivery_Size;
                    if (qtyBalance_Size > 0)
                    {
                        dr[String.Format("Column{0}", sizeID)]              = qtyBalance_Size;
                        dr[String.Format("ColumnBackground{0}", sizeID)]    = Brushes.Tomato;
                        contentStatus.Quantity = qtyBalance_Size;
                    }
                    if (qtyReject_Size > 0)
                    {
                        dr[String.Format("Column{0}", sizeID)]              = qtyReject_Size;
                        dr[String.Format("ColumnForeground{0}", sizeID)]    = Brushes.Red;

                        contentStatus.Quantity = qtyReject_Size;
                        contentStatus.ForegroundCell = System.Drawing.Color.Red;
                        if (qtyBalance_Size > 0)
                        {
                            int qtyBalanceTotal = qtyBalance_Size + qtyReject_Size;

                            dr[String.Format("Column{0}", sizeID)]              = qtyBalanceTotal;
                            dr[String.Format("ColumnBackground{0}", sizeID)]    = Brushes.Yellow;
                            dr[String.Format("ColumnTooltip{0}", sizeID)]       = String.Format("Balance: {0}\nReject : {1}", qtyBalance_Size, qtyReject_Size);

                            contentStatus.Quantity = qtyBalanceTotal;
                            contentStatus.BackgroundCell = System.Drawing.Color.Yellow;
                        }
                    }
                    //
                    //if (qtyBalance_Size > 0 || qtyReject_Size > 0)
                    contentStatusList.Add(contentStatus);
                }

                var articleNo   = order_PO != null ? order_PO.ArticleNo : "";
                var orderEFD    = order_PO != null ? order_PO.ETD : new DateTime(2000, 1, 1);
                var delEFD      = osRawMaterial_PO != null ? osRawMaterial_PO.ETD : new DateTime(2000, 1, 1);
                var sewStart    = sewingMaster_PO != null ? sewingMaster_PO.SewingStartDate : new DateTime(2000, 1, 1);
                var release     = osRelease_PO.Sum(s => s.Quantity);
                var qtyOrder    = sizeRunList_PO.Sum(s => s.Quantity);
                var qtyDelivery = osMaterial_PO.Sum(s => s.Quantity);
                var qtyReject   = osMaterial_PO.Sum(s => s.QuantityReject);
                var qtyReleasePainting = osMaterial_PO.FirstOrDefault().TotalReleasePainting;

                dr["ProductNo"]         = productNo;
                dr["ArticleNo"]         = articleNo;
                dr["OrderEFD"]          = orderEFD;
                dr["DeliveryEFD"]       = delEFD;
                dr["SewingStartDate"]   = sewStart;
                dr["QuantityOrder"]     = qtyOrder;
                dr["Release"]           = release;
                dr["Delivery"]          = qtyDelivery;
                dr["Reject"]            = qtyReject;
                dr["TotalBalance"]      = qtyOrder - qtyDelivery + qtyReject;
                dr["ReleasePainting"]      = qtyReleasePainting;


                dt.Rows.Add(dr);
                var excelExportModel = new ExportExcelModel()
                {
                    ProductNo           = productNo,
                    ArticleNo           = articleNo,
                    OrderEFD            = orderEFD,
                    DeliveryEFD         = delEFD,
                    SewingStartDate     = sewStart,
                    QuantityOrder       = qtyOrder,
                    Release             = release,
                    Delivery            = qtyDelivery,
                    Reject              = qtyReject,
                    ContentStatusList   = contentStatusList,
                    TotalBalance        = qtyOrder - qtyDelivery + qtyReject,
                    TotalReleasePainting= qtyReleasePainting,
                };
                excelExportList.Add(excelExportModel);
            }
            DataRow drTotal = dt.NewRow();

            drTotal["ProductNo"]        = "Total";
            drTotal["QuantityOrder"]    = string.Format("{0:#,0}", dt.Compute("Sum(QuantityOrder)", "").ToString());
            drTotal["Release"]          = string.Format("{0:#,0}", dt.Compute("Sum(Release)", "").ToString());
            drTotal["Delivery"]         = string.Format("{0:#,0}", dt.Compute("Sum(Delivery)", "").ToString());
            drTotal["Reject"]           = string.Format("{0:#,0}", dt.Compute("Sum(Reject)", "").ToString());
            drTotal["TotalBalance"]     = string.Format("{0:#,0}", dt.Compute("Sum(TotalBalance)", "").ToString());
            drTotal["ReleasePainting"]  = string.Format("{0:#,0}", dt.Compute("Sum(ReleasePainting)", "").ToString());

            dt.Rows.Add(drTotal);
            e.Result = dt;
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            var dt = e.Result as DataTable;
            dgWHDeliveryDetail.ItemsSource = dt.AsDataView();
            dgWHDeliveryDetail.Items.Refresh();
            btnExcelExport.IsEnabled = true;
        }

        private void btnExcelExport_Click(object sender, RoutedEventArgs e)
        {
            if (dgWHDeliveryDetail.ItemsSource == null)
                return;
            if (bwExportExcel.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnExcelExport.IsEnabled = false;
                progressBar.Value = 0;
                lblStatus.Text = "";
                bwExportExcel.RunWorkerAsync();
            }
        }
        private void bwExportExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            EXCEL._Application excel = new Microsoft.Office.Interop.Excel.Application();
            EXCEL._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            EXCEL._Worksheet worksheet = null;
            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Cells.HorizontalAlignment = EXCEL.XlHAlign.xlHAlignCenter;
                worksheet.Cells.Font.Name = "Arial";
                worksheet.Cells.Font.Size = 10;

                // Title, Header.
                int rowTitle = 1;
                int rowHeader = 2;
                int columnNumber = 0;
                Dispatcher.Invoke(new Action(() =>
                {
                    columnNumber = dgWHDeliveryDetail.Columns.Count();
                    // Title
                    worksheet.Range[worksheet.Cells[rowTitle, 1], worksheet.Cells[rowTitle, columnNumber]].Merge();
                    worksheet.Cells.Rows[rowTitle].Font.Size = 15;
                    worksheet.Cells.Rows[rowTitle].Font.FontStyle = "Bold";

                    // Header
                    worksheet.Cells[rowTitle, 1] = txtTitle.Text;
                    for (int i = 0; i < columnNumber; i++)
                    {
                        var column = dgWHDeliveryDetail.Columns[i] as DataGridTemplateColumn;
                        worksheet.Cells[rowHeader, i + 1] = column.Header;
                    }
                    worksheet.Cells.Rows[rowHeader].Font.Size = 10;
                    worksheet.Cells.Rows[rowHeader].Font.FontStyle = "Bold";

                    progressBar.Maximum = (double)excelExportList.Count();
                }));

                int rowContent = 3;
                for (int i = 0; i < excelExportList.Count(); i++)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        progressBar.Value = rowContent - 2;
                        lblStatus.Text = string.Format("Writing {0} rows / {1}", rowContent - 2, excelExportList.Count());
                    }));

                    var export = excelExportList[i];
                    worksheet.Cells[rowContent, 1] = export.ProductNo;
                    worksheet.Cells[rowContent, 2] = export.ArticleNo;
                    worksheet.Cells[rowContent, 3] = export.OrderEFD;
                    worksheet.Cells[rowContent, 4] = export.DeliveryEFD;
                    worksheet.Cells[rowContent, 5] = export.SewingStartDate;
                    worksheet.Cells[rowContent, 6] = export.QuantityOrder;
                    worksheet.Cells[rowContent, 7] = export.Release;
                    worksheet.Cells[rowContent, 8] = export.Delivery;
                    worksheet.Cells[rowContent, 9] = export.Reject;
                    for (int j = 0; j < export.ContentStatusList.Count(); j++)
                    {
                        worksheet.Cells[rowContent, j + 10].ColumnWidth     = export.ContentStatusList[j].WidthDefaultCell;
                        worksheet.Cells[rowContent, j + 10].Font.Color      = System.Drawing.ColorTranslator.ToOle(export.ContentStatusList[j].ForegroundCell);
                        worksheet.Cells[rowContent, j + 10].Interior.Color  = System.Drawing.ColorTranslator.ToOle(export.ContentStatusList[j].BackgroundCell);
                        worksheet.Cells[rowContent, j + 10].Borders.Color   = System.Drawing.ColorTranslator.ToOle(export.ContentStatusList[j].BorderColorCell);

                        if (export.ContentStatusList[j].Quantity > 0)
                            worksheet.Cells[rowContent, j + 10]                 = export.ContentStatusList[j].Quantity;
                        if (export.ContentStatusList[j].ForegroundCell != null)
                            worksheet.Cells[rowContent, j + 10].Font.Color      = System.Drawing.ColorTranslator.ToOle(export.ContentStatusList[j].ForegroundCell);
                        if (export.ContentStatusList[j].BackgroundCell != null)
                            worksheet.Cells[rowContent, j + 10].Interior.Color  = System.Drawing.ColorTranslator.ToOle(export.ContentStatusList[j].BackgroundCell);
                    }
                    worksheet.Cells[rowContent, columnNumber - 1] = export.TotalBalance;
                    worksheet.Cells[rowContent, columnNumber] = export.TotalReleasePainting;
                    rowContent++;
                }

                Dispatcher.Invoke(new Action(() =>
                {
                    if (workbook != null)
                    {
                        var sfd = new System.Windows.Forms.SaveFileDialog();
                        sfd.Title = "Master Schedule - Export Excel File";
                        sfd.Filter = "Excel Documents (*.xls)|*.xls|Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FilterIndex = 2;
                        sfd.FileName = String.Format("Outsole WH Delivery Report");
                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("Export Successful !", "Master Schedule - Export Excel File", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }));
            }
            catch (System.Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message, "Master Schedule - Export Excel File", MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }
        private void bwExportExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnExcelExport.IsEnabled = true;
            progressBar.Value = 0;
            lblStatus.Text = "";
        }

        public class ExportExcelModel
        {
            public string ProductNo { get; set; }
            public string ArticleNo { get; set; }
            public DateTime OrderEFD { get; set; }
            public DateTime DeliveryEFD { get; set; }
            public DateTime SewingStartDate { get; set; }
            public int QuantityOrder { get; set; }
            public int Release { get; set; }
            public int Delivery { get; set; }
            public int Reject { get; set; }
            public List<ContentStatus> ContentStatusList { get; set; }
            public int TotalBalance { get; set; }
            public int TotalReleasePainting { get; set; }
            public string Remarks { get; set; }
        }
        public class ContentStatus
        {
            public int Quantity { get; set; }
            public System.Drawing.Color ForegroundCell { get; set; }
            public System.Drawing.Color BackgroundCell { get; set; }
            public System.Drawing.Color BorderColorCell { get; set; }
            public int WidthDefaultCell { get; set; }
        }

        private void dgWHDeliveryDetail_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
