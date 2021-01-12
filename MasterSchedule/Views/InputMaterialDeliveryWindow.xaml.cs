using MasterSchedule.Controllers;
using MasterSchedule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputMaterialDeliveryWindow.xaml
    /// </summary>
    public partial class InputMaterialDeliveryWindow : Window
    {
        private string productNo;
        BackgroundWorker bwLoad;
        List<SizeRunModel> sizeRunList;
        List<MaterialDeliveryModel> matsDeliveryByPOList;
        List<MaterialPlanModel> matsPlanByPOList;
        List<SupplierModel> supplierList;
        DataTable dtDelivery;
        private const string RowQuantity = "Quantity", RowReject = "Reject", RowRejectSewing = "Reject Sewing";
        private DateTime dtDefault = new DateTime(2000, 01, 01);
        public InputMaterialDeliveryWindow(string productNo)
        {
            this.productNo = productNo;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            sizeRunList          = new List<SizeRunModel>();
            matsDeliveryByPOList = new List<MaterialDeliveryModel>();
            matsPlanByPOList     = new List<MaterialPlanModel>();
            dtDelivery           = new DataTable();
            supplierList = new List<SupplierModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy==false)
            {
                tblTitle.Text = tblTitle.Text + productNo;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sizeRunList             = SizeRunController.Select(productNo);
                matsDeliveryByPOList    = MaterialDeliveryController.GetMaterialDeliveryByPO(productNo);
                matsPlanByPOList        = MaterialPlanController.GetMaterialPlanByPO(productNo);
                supplierList            = SupplierController.GetSuppliersAccessories();
                Dispatcher.Invoke(new Action(() =>
                {
                    // Create Column Datagrid
                    dgDeliveryInfo.Columns.Clear();
                    dtDelivery = new DataTable();

                    dtDelivery.Columns.Add("Status", typeof(String));
                    dtDelivery.Columns.Add("Name", typeof(String));
                    dtDelivery.Columns.Add("SupplierId", typeof(String));
                    DataGridTemplateColumn colSuppName = new DataGridTemplateColumn();
                    colSuppName.Header = String.Format("Supplier");
                    DataTemplate templateSuppName = new DataTemplate();
                    FrameworkElementFactory tblSuppName = new FrameworkElementFactory(typeof(TextBlock));
                    templateSuppName.VisualTree = tblSuppName;
                    tblSuppName.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Name")));
                    tblSuppName.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblSuppName.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colSuppName.CellTemplate = templateSuppName;
                    colSuppName.ClipboardContentBinding = new Binding(String.Format("Name"));
                    dgDeliveryInfo.Columns.Add(colSuppName);

                    //Column ETD
                    dtDelivery.Columns.Add("ETD", typeof(String));
                    DataGridTemplateColumn colETD = new DataGridTemplateColumn();
                    colETD.Header = String.Format("EFD");
                    DataTemplate templateETD = new DataTemplate();
                    FrameworkElementFactory tblETD = new FrameworkElementFactory(typeof(TextBlock));
                    templateETD.VisualTree = tblETD;
                    tblETD.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ETD")));
                    tblETD.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblETD.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblETD.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colETD.CellTemplate = templateETD;
                    colETD.ClipboardContentBinding = new Binding(String.Format("ETD"));
                    dgDeliveryInfo.Columns.Add(colETD);

                    //Column Actual Date
                    dtDelivery.Columns.Add("ActualDate", typeof(String));
                    dtDelivery.Columns.Add("ActualDateDate", typeof(DateTime));
                    DataGridTemplateColumn colActualDate = new DataGridTemplateColumn();
                    colActualDate.Header = String.Format("{0}\n{1}\n{2}","Order Size","Qty","Actual Date");
                    DataTemplate templateActualDate = new DataTemplate();
                    FrameworkElementFactory tblActualDate = new FrameworkElementFactory(typeof(TextBlock));
                    templateActualDate.VisualTree = tblActualDate;
                    tblActualDate.SetBinding(TextBlock.TextProperty, new Binding(String.Format("ActualDate")));
                    tblActualDate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblActualDate.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblActualDate.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colActualDate.CellTemplate = templateActualDate;
                    colActualDate.ClipboardContentBinding = new Binding(String.Format("ActualDate"));
                    dgDeliveryInfo.Columns.Add(colActualDate);

                    var regex = new Regex("[a-z]|[A-Z]");
                    if (sizeRunList.Count() > 0)
                        sizeRunList = sizeRunList.OrderBy(s => regex.IsMatch(s.SizeNo) ? Double.Parse(regex.Replace(s.SizeNo, "100")) : Double.Parse(s.SizeNo)).ToList();
                    foreach (var sizeRun in sizeRunList)
                    {
                        string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                        dtDelivery.Columns.Add(String.Format("Column{0}", sizeBinding), typeof(String));
                        dtDelivery.Columns.Add(String.Format("Column{0}Foreground", sizeBinding), typeof(SolidColorBrush));
                        //dtDelDetail.Columns.Add(String.Format("Column{0}ToolTip", sizeBinding), typeof(String));
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.SetValue(TagProperty, sizeRun.SizeNo);
                        column.Header = string.Format("{0}\n{1}\n", sizeRun.SizeNo, sizeRun.Quantity);
                        column.MinWidth = 40;
                        column.MaxWidth = 200;
                        column.Binding = new Binding(String.Format("Column{0}", sizeBinding));

                        Style styleColumn = new Style();
                        Setter setterColumnForecolor = new Setter();
                        setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                        setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", sizeBinding));

                        styleColumn.Setters.Add(setterColumnForecolor);
                        column.CellStyle = styleColumn;

                        dgDeliveryInfo.Columns.Add(column);
                    }

                    //Column Total
                    dtDelivery.Columns.Add("Balance", typeof(String));
                    DataGridTemplateColumn colBalance = new DataGridTemplateColumn();
                    colBalance.Header = String.Format("Total\n{0}\n", sizeRunList.Sum(s => s.Quantity));
                    colBalance.MinWidth = 80;
                    colBalance.MaxWidth = 80;
                    DataTemplate templateBalance = new DataTemplate();
                    FrameworkElementFactory tblBalance = new FrameworkElementFactory(typeof(TextBlock));
                    templateBalance.VisualTree = tblBalance;
                    tblBalance.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Balance")));
                    tblBalance.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tblBalance.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    tblBalance.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
                    colBalance.CellTemplate = templateBalance;
                    colBalance.ClipboardContentBinding = new Binding(String.Format("Balance"));
                    dgDeliveryInfo.Columns.Add(colBalance);

                    DataGridTemplateColumn colButtonOK = new DataGridTemplateColumn();
                    colButtonOK.MinWidth = 40;
                    colButtonOK.MaxWidth = 40;
                    DataTemplate templateButtonOK = new DataTemplate();
                    FrameworkElementFactory fefButtonOK = new FrameworkElementFactory(typeof(Button));
                    templateButtonOK.VisualTree = fefButtonOK;
                    fefButtonOK.SetValue(Button.ContentProperty, "Ok");
                    fefButtonOK.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnOK_Click));
                    colButtonOK.CellTemplate = templateButtonOK;
                    dgDeliveryInfo.Columns.Add(colButtonOK);

                    // Binding Data
                    if (matsDeliveryByPOList.Count() == 0)
                    {
                        foreach (var matsPlan in matsPlanByPOList)
                        {
                            var supplierByPlan = supplierList.FirstOrDefault(f => f.SupplierId == matsPlan.SupplierId);
                            foreach (var sizeRun in sizeRunList)
                            {
                                matsDeliveryByPOList.Add(new MaterialDeliveryModel
                                {
                                    SupplierId = matsPlan.SupplierId,
                                    SupplierName = supplierByPlan != null ? supplierByPlan.Name : "",
                                    ProductNo = matsPlan.ProductNo,
                                    ETD = matsPlan.ETD,
                                    ActualDeliveryDate = dtDefault,
                                    SizeNo = sizeRun.SizeNo,
                                    Quantity        = 0,
                                    Reject          = 0,
                                    RejectSewing    = 0
                                });
                            }
                        }
                    }
                    
                    var supplierIdList = matsDeliveryByPOList.Select(s => s.SupplierId).Distinct().ToList();
                    foreach (var supplierId in supplierIdList)
                    {
                        var deliveryListBySupp = matsDeliveryByPOList.Where(w => w.SupplierId == supplierId).ToList();
                        var materialInfoBySupp = deliveryListBySupp.FirstOrDefault();

                        // Row Quantity, Reject, RejectSewing
                        DataRow drQuantity      = dtDelivery.NewRow();
                        DataRow drReject        = dtDelivery.NewRow();
                        DataRow drRejectSewing  = dtDelivery.NewRow();

                        drQuantity["Status"]        = RowQuantity;
                        drReject["Status"]          = RowReject;
                        drRejectSewing["Status"]    = RowRejectSewing;

                        drQuantity["Name"]      = materialInfoBySupp.SupplierName;
                        drReject["Name"]        = RowReject;
                        drRejectSewing["Name"]  = RowRejectSewing;

                        drQuantity["SupplierId"]        = materialInfoBySupp.SupplierId;
                        drReject["SupplierId"]          = materialInfoBySupp.SupplierId;
                        drRejectSewing["SupplierId"]    = materialInfoBySupp.SupplierId;

                        drQuantity["ETD"]           = string.Format("{0:MM/dd}", materialInfoBySupp.ETD);
                        if (materialInfoBySupp.ActualDeliveryDate != dtDefault)
                            drQuantity["ActualDate"] = string.Format("{0:MM/dd}", materialInfoBySupp.ActualDeliveryDate);

                        foreach (var sizeRun in sizeRunList)
                        {
                            string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;

                            var deliveryBySizeNo = deliveryListBySupp.FirstOrDefault(f => f.SizeNo == sizeRun.SizeNo);
                            if (deliveryBySizeNo != null)
                            {
                                if (deliveryBySizeNo.Quantity > 0)
                                {
                                    drQuantity[String.Format("Column{0}", sizeBinding)] = deliveryBySizeNo.Quantity;
                                    if (deliveryBySizeNo.Quantity == sizeRun.Quantity)
                                        drQuantity[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
                                }
                                if (deliveryBySizeNo.Reject > 0)
                                {
                                    drReject[String.Format("Column{0}", sizeBinding)] = deliveryBySizeNo.Reject;
                                    drReject[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                                }
                                if (deliveryBySizeNo.RejectSewing > 0)
                                {
                                    drRejectSewing[String.Format("Column{0}", sizeBinding)] = deliveryBySizeNo.RejectSewing;
                                    drRejectSewing[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Red;
                                }
                            }
                        }

                        dtDelivery.Rows.Add(drQuantity);
                        dtDelivery.Rows.Add(drReject);
                        dtDelivery.Rows.Add(drRejectSewing);
                    }

                    dgDeliveryInfo.ItemsSource = dtDelivery.AsDataView();
                }));
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.InnerException.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (dgDeliveryInfo.CurrentItem == null)
                return;
            var drClicked = ((DataRowView)dgDeliveryInfo.CurrentItem).Row;
            if (!drClicked["Status"].ToString().Equals(RowQuantity))
                return;
            var supplierIdOK = drClicked["SupplierId"].ToString();
            foreach (var sizeRun in sizeRunList)
            {
                string sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                drClicked[String.Format("Column{0}", sizeBinding)] = sizeRun.Quantity;
                drClicked[String.Format("Column{0}Foreground", sizeBinding)] = Brushes.Blue;
            }
            drClicked["Balance"] = "";

            // Update Actual Date
            int totalDeliveryOld = matsDeliveryByPOList.Where(w => w.SupplierId.ToString().Equals(supplierIdOK)).Sum(s => s.Quantity);
            if (totalDeliveryOld != sizeRunList.Sum(s => s.Quantity))
            {
                drClicked["ActualDate"]     = string.Format("{0:MM/dd}", DateTime.Now);
                drClicked["ActualDateDate"] = DateTime.Now;
            }
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void dgDeliveryInfo_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (dgDeliveryInfo.CurrentItem == null)
                return;
            var drEditting = ((DataRowView)dgDeliveryInfo.CurrentItem).Row;
            if (e.Column.GetValue(TagProperty) == null)
                return;
            string sizeNoEditting = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNoEditting) == false)
            {
                return;
            }

            var supplierEditting = drEditting["SupplierId"].ToString();
            var statusEditting = drEditting["Status"].ToString();

            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyCurrent = 0;
            Int32.TryParse(txtCurrent.Text.ToString(), out qtyCurrent);

            // Validate
            int qtyOld = 0;
            var matsOldBySize = matsDeliveryByPOList.FirstOrDefault(f => f.SupplierId.ToString().Equals(supplierEditting) && f.SizeNo.Equals(sizeNoEditting));
            if (matsOldBySize != null)
            {
                if (statusEditting.Equals(RowQuantity))
                    qtyOld = matsOldBySize.Quantity;
                else if (statusEditting.Equals(RowReject))
                    qtyOld = matsOldBySize.Reject;
                else
                    qtyOld = matsOldBySize.RejectSewing;
            }

            int qtyAfterPlusValue = qtyOld + qtyCurrent;
            int qtyOrder = sizeRunList.FirstOrDefault(f => f.SizeNo == sizeNoEditting).Quantity;

            if (qtyAfterPlusValue > qtyOrder)
                qtyAfterPlusValue = qtyOrder;
            else if (qtyAfterPlusValue <= 0)
                qtyAfterPlusValue = 0;

            txtCurrent.Text = qtyAfterPlusValue != 0 ? qtyAfterPlusValue.ToString() : "";
            int totalQtyOrder = sizeRunList.Sum(s => s.Quantity);

            // Update Balance
            // Collect Data At Row Editting
            int totalQuantityAtRow = 0;
            for (int r = 0; r < dtDelivery.Rows.Count; r++)
            {
                DataRow drCurrent = dtDelivery.Rows[r];
                if (drCurrent["Status"].ToString() != RowQuantity)
                    continue;
                sizeNoEditting = sizeNoEditting.Contains(".") ? sizeNoEditting.Replace(".", "@") : sizeNoEditting;
                drCurrent[String.Format("Column{0}", sizeNoEditting)] = txtCurrent.Text;

                foreach (var sizeRun in sizeRunList)
                {
                    var sizeBinding = sizeRun.SizeNo.Contains(".") ? sizeRun.SizeNo.Replace(".", "@") : sizeRun.SizeNo;
                    int qtyAtCell = 0;
                    Int32.TryParse(drCurrent[String.Format("Column{0}",sizeBinding)].ToString(), out qtyAtCell);
                    totalQuantityAtRow += qtyAtCell;
                }
            }
            
            if (statusEditting == RowQuantity)
                drEditting["Balance"] = totalQtyOrder - totalQuantityAtRow;
            else
                drEditting["Balance"] = totalQuantityAtRow;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgDeliveryInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

    }
}
