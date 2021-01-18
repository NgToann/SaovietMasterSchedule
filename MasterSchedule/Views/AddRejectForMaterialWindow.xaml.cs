using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Helpers;
using MasterSchedule.Controllers;
using System.Data;
using System.Globalization;
//using MasterSchedule.Helpers;
using System.Windows.Media;
using System.Data.Metadata.Edm;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddRejectBySizeForMaterialWindow.xaml
    /// </summary>
    public partial class AddRejectForMaterialWindow : Window
    {
        private List<RejectModel> rejectUpperAccessoriesList;
        List<RejectModel> rejectClickedList;

        private SizeRunModel sizeRunClicked;
        List<SizeRunModel> sizeRunList;
        private MaterialPlanModel materialPlanChecking;
        private DataRowView rowEditting;
        private List<MaterialInspectModel> inspectListCurrent;
        private string workerId;
        BackgroundWorker bwUpload;
        public List<MaterialInspectModel> inspectListHasReject;
        public EExcute eAction = EExcute.None;
        DataTable dtReject;
        int totalRejectCurrent;
        public AddRejectForMaterialWindow(List<RejectModel> rejectUpperAccessoriesList, SizeRunModel sizeRunClicked, MaterialPlanModel materialPlanChecking, DataRowView rowEditting, List<MaterialInspectModel> deliveryCurrentList, int totalRejectCurrent, string workerId, List<SizeRunModel> sizeRunList)
        {
            this.rejectUpperAccessoriesList = rejectUpperAccessoriesList;
            this.sizeRunClicked             = sizeRunClicked;
            this.materialPlanChecking       = materialPlanChecking;
            this.rowEditting                = rowEditting;
            this.inspectListCurrent         = deliveryCurrentList;
            this.totalRejectCurrent         = totalRejectCurrent;
            this.workerId                   = workerId;
            this.sizeRunList                = sizeRunList;
            bwUpload = new BackgroundWorker();
            bwUpload.DoWork += BwUpload_DoWork;
            bwUpload.RunWorkerCompleted += BwUpload_RunWorkerCompleted;

            rejectClickedList       = new List<RejectModel>();
            inspectListHasReject   = new List<MaterialInspectModel>();
            dtReject                = new DataTable();
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sizeRunClicked != null)
            {
                txtSizeNo.Text = sizeRunClicked.SizeNo;
                txtSizeNo.IsReadOnly = true;

                if (inspectListCurrent.Count() > 0)
                {
                    var rejectIdList = inspectListCurrent.Select(s => s.RejectId).Distinct().ToList();
                    foreach (var rId in rejectIdList)
                    {
                        var delById = inspectListCurrent.Where(w => w.RejectId.Equals(rId)).FirstOrDefault();
                        for (int i = 0; i < delById.Reject; i++)
                        {
                            rejectClickedList.Add(rejectUpperAccessoriesList.FirstOrDefault(f => f.RejectId.Equals(rId)));
                        }
                    }
                }
                DisplayRejectClicked(rejectClickedList);
            }
            else
            {
                txtSizeNo.Focus();
            }
            Thread.Sleep(100);
            LoadListOfDefects(rejectUpperAccessoriesList);
        }

        private void BwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (eAction.Equals(EExcute.AddNew))
                {
                    // Delete Reject Current List First
                    if (inspectListHasReject.Count() > 0)
                    {
                        foreach (var rejectItem in inspectListCurrent)
                        {
                            MaterialInspectController.Insert(rejectItem, insertQty: false, insertReject: false, deleteReject: true);
                        }
                    }
                    // Add New
                    foreach (var rejectItem in inspectListHasReject)
                    {
                        MaterialInspectController.Insert(rejectItem, insertQty: false, insertReject: true, deleteReject: false);
                    }
                }

                else if (eAction.Equals(EExcute.Delete))
                {
                    foreach (var rejectItem in inspectListHasReject)
                    {
                        MaterialInspectController.Insert(rejectItem, insertQty: false, insertReject: false, deleteReject: true);
                    }
                }

                // Update Reject Delivery
                MaterialDeliveryController.UpdateRejectWhenInspect(materialPlanChecking);
                
                e.Result = true;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(ex.InnerException.InnerException.Message.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
                eAction = EExcute.None;
                e.Result = false;
            }
        }
        
        private void BwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (eAction.Equals(EExcute.AddNew) && (bool)e.Result == true)
            {
                MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                btnSave.IsEnabled = true;
            }
            if (eAction.Equals(EExcute.Delete) && (bool)e.Result == true)
            {
                MessageBox.Show("Deleted !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                btnDelete.IsEnabled = true;
            }
            this.Cursor = null;
            Thread.Sleep(150);
            this.Close();
        }
        
        private void LoadListOfDefects(List<RejectModel> rejectUpperAccessoriesList)
        {
            // binding to error to grid
            int countColumn = gridError.ColumnDefinitions.Count();
            int countRow = countRow = rejectUpperAccessoriesList.Count / countColumn;
            if (rejectUpperAccessoriesList.Count % countColumn != 0)
            {
                countRow = rejectUpperAccessoriesList.Count / countColumn + 1;
            }
            gridError.RowDefinitions.Clear();
            for (int i = 1; i <= countRow; i++)
            {
                RowDefinition rd = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star),
                };
                gridError.RowDefinitions.Add(rd);
            }

            for (int i = 0; i <= rejectUpperAccessoriesList.Count() - 1; i++)
            {
                Button button = new Button();
                var template = FindResource("ButtonDefectTemplate") as ControlTemplate;
                button.Template = template;
                button.Name = Name = string.Format("button{0}", rejectUpperAccessoriesList[i].RejectKey);
                button.Margin = new Thickness(4, 4, 0, 0);
                if (i / countColumn == 0)
                {
                    if (i != 0)
                        button.Margin = new Thickness(4, 0, 0, 0);
                    else
                        button.Margin = new Thickness(0, 0, 0, 0);
                }
                if (i % countColumn == 0 && i / countColumn != 0)
                    button.Margin = new Thickness(0, 4, 0, 0);
                button.Tag = rejectUpperAccessoriesList[i];
                button.Click += Button_Click;
                button.MaxHeight = 60;
                Border br = new Border();
                br.Name = string.Format("border{0}", rejectUpperAccessoriesList[i].RejectKey);

                Grid.SetColumn(button, i % countColumn);
                Grid.SetRow(button, i / countColumn);

                Grid grid = new Grid();
                ColumnDefinition cld1 = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Auto),
                };
                ColumnDefinition cld2 = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star),
                };
                grid.ColumnDefinitions.Add(cld1);
                grid.ColumnDefinitions.Add(cld2);

                TextBlock tblKey = new TextBlock();
                tblKey.Text = rejectUpperAccessoriesList[i].RejectKey;
                tblKey.FontSize = 20;
                tblKey.Foreground = Brushes.Blue;
                tblKey.VerticalAlignment = VerticalAlignment.Center;
                tblKey.Margin = new Thickness(4, 0, 4, 0);
                Grid.SetColumn(tblKey, 0);

                TextBlock tblReject = new TextBlock();
                tblReject.Text = string.Format("{0}\n{1}", rejectUpperAccessoriesList[i].RejectName, rejectUpperAccessoriesList[i].RejectName_1);
                tblReject.FontSize = 14;
                tblReject.TextWrapping = TextWrapping.Wrap;
                tblReject.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(tblReject, 1);

                grid.Children.Add(tblKey);
                grid.Children.Add(tblReject);

                br.Child = grid;
                button.Content = br;

                gridError.Children.Add(button);
            }
        }

        // Button Reject Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonClicked = sender as Button;
            var rejectClicked = buttonClicked.Tag as RejectModel;
            int currentQty = totalRejectCurrent > 0 ? totalRejectCurrent - 1 : 0;
            if (rejectClickedList.Count() + currentQty >= sizeRunClicked.Quantity)
            {
                MessageBox.Show(String.Format("Total reject can't be greater than #size {0} quantity {1}", sizeRunClicked.SizeNo, sizeRunClicked.Quantity), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            rejectClickedList.Add(rejectClicked);
            HighLightError(rejectClicked.RejectKey);
            DisplayRejectClicked(rejectClickedList);
        }

        private void DisplayRejectClicked(List<RejectModel> rejectDisplayList)
        {
            // Create Column
            dgReject.Columns.Clear();
            dtReject = new DataTable();

            var rejectIdList = rejectDisplayList.Select(s => s.RejectId).Distinct().ToList();
            if (rejectIdList.Count() > 0)
                rejectIdList = rejectIdList.OrderBy(o => o).ToList();

            foreach (var rId in rejectIdList)
            {
                var rejectById = rejectDisplayList.FirstOrDefault(f => f.RejectId.Equals(rId));
                dtReject.Columns.Add(String.Format("Column{0}", rId), typeof(Int32));
                
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, rId);
                column.Header = string.Format("{0}\n{1}", rejectById.RejectName, rejectById.RejectName_1);
                column.Binding = new Binding(String.Format("Column{0}", rId));

                dgReject.Columns.Add(column);
                var noOfReject = rejectDisplayList.Where(w => w.RejectId.Equals(rId)).Count();
            }

            // Column Total
            dtReject.Columns.Add("Total", typeof(String));
            DataGridTemplateColumn colTotal = new DataGridTemplateColumn();
            colTotal.Header = String.Format("Total");
            DataTemplate templateTotal = new DataTemplate();
            FrameworkElementFactory tblTotal = new FrameworkElementFactory(typeof(TextBlock));
            templateTotal.VisualTree = tblTotal;
            tblTotal.SetBinding(TextBlock.TextProperty, new Binding(String.Format("Total")));
            tblTotal.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            tblTotal.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            tblTotal.SetValue(TextBlock.PaddingProperty, new Thickness(3, 0, 3, 0));
            colTotal.CellTemplate = templateTotal;
            colTotal.ClipboardContentBinding = new Binding(String.Format("Total"));
            dgReject.Columns.Add(colTotal);

            // Binding data
            DataRow dr = dtReject.NewRow();
            foreach (var rId in rejectIdList)
            {
                var noOfReject = rejectDisplayList.Where(w => w.RejectId.Equals(rId)).Count();
                dr[String.Format("Column{0}", rId)] = noOfReject;
            }
            dr["Total"] = rejectDisplayList.Count().ToString();
            dtReject.Rows.Add(dr);

            dgReject.ItemsSource = dtReject.AsDataView();
            dgReject.Items.Refresh();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int currentQty = totalRejectCurrent > 0 ? totalRejectCurrent - 1 : 0;
            if (rejectClickedList.Count() + currentQty > sizeRunClicked.Quantity)
            {
                MessageBox.Show(String.Format("Total reject can't be greater than #size {0} quantity {1}", sizeRunClicked.SizeNo, sizeRunClicked.Quantity), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var rejectIdList = rejectClickedList.Select(s => s.RejectId).Distinct().ToList();
            foreach (var rId in rejectIdList)
            {
                var rejectById = rejectClickedList.FirstOrDefault(f => f.RejectId.Equals(rId));
                var noOfReject = rejectClickedList.Where(w => w.RejectId.Equals(rId)).Count();
                var delReject = new MaterialInspectModel
                {
                    ProductNo = materialPlanChecking.ProductNo,
                    SupplierId = materialPlanChecking.SupplierId,
                    DeliveryDate = (DateTime)rowEditting["InspectionDateDate"],
                    SizeNo = sizeRunClicked.SizeNo,
                    Reject = noOfReject,
                    RejectId = rId,
                    Reviser = workerId
                };
                inspectListHasReject.Add(delReject);
            }

            if (inspectListHasReject.Count() == 0)
            {
                MessageBox.Show(String.Format("Reject list is empty !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (bwUpload.IsBusy == false)
            {
                btnSave.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                eAction = EExcute.AddNew;
                bwUpload.RunWorkerAsync();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Confirm Delete Reject Size: #{0} ?", sizeRunClicked.SizeNo), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            var rejectIdList = rejectClickedList.Select(s => s.RejectId).Distinct().ToList();
            foreach (var rId in rejectIdList)
            {
                var rejectById = rejectClickedList.FirstOrDefault(f => f.RejectId.Equals(rId));
                var noOfReject = rejectClickedList.Where(w => w.RejectId.Equals(rId)).Count();
                var delReject = new MaterialInspectModel
                {
                    ProductNo = materialPlanChecking.ProductNo,
                    SupplierId = materialPlanChecking.SupplierId,
                    DeliveryDate = (DateTime)rowEditting["InspectionDateDate"],
                    SizeNo = sizeRunClicked.SizeNo,
                    Reject = noOfReject,
                    RejectId = rId,
                    Reviser = workerId
                };
                inspectListHasReject.Add(delReject);
            }

            if (bwUpload.IsBusy == false)
            {
                btnDelete.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                eAction = EExcute.Delete;
                bwUpload.RunWorkerAsync();
            }
        }

        private void btnClearQuantity_Click(object sender, RoutedEventArgs e)
        {
            rejectClickedList.Clear();
            DisplayRejectClicked(rejectClickedList);
        }
        
        private void HighLightError(string rejectKey)
        {
            try
            {
                var childrenCount = VisualTreeHelper.GetChildrenCount(gridError);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(gridError, i);
                    if (child != null)
                    {
                        Button buttonClicked = child as Button;
                        var template = FindResource("ButtonDefectTemplate") as ControlTemplate;
                        var templateClicked = FindResource("ButtonDefectClickedTemplate") as ControlTemplate;
                        buttonClicked.Template = template;
                        if (buttonClicked.Name.Equals(String.Format("button{0}", rejectKey)))
                            buttonClicked.Template = templateClicked;
                    }
                }
            }
            catch { }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
                this.Close();
        }

        private void dgReject_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var rowEditting = (DataRowView)e.Row.Item;
            if (e.EditingElement == null || rowEditting == null)
                return;
            if (e.Column.GetValue(TagProperty) == null)
                return;
            var rIdCurrent = (Int32)e.Column.GetValue(TagProperty);
            var rejectCurrent = rejectUpperAccessoriesList.FirstOrDefault(f => f.RejectId.Equals(rIdCurrent));

            int qtyRej = 0;
            TextBox txtCurrent = (TextBox)e.EditingElement;
            Int32.TryParse(txtCurrent.Text.Trim(), out qtyRej);

            rejectClickedList.RemoveAll(r => r.RejectId == rejectCurrent.RejectId);
            for (int i = 0; i < qtyRej; i++)
            {
                rejectClickedList.Add(rejectCurrent);
            }

            // Update Total Cell
            for (int r = 0; r < dtReject.Rows.Count; r++)
            {
                DataRow dr = dtReject.Rows[r];
                dr["Total"] = rejectClickedList.Count().ToString();
                //foreach (var reject in rejectClickedList)
                //{
                //    dr[String.Format("Column{0}", reject.RejectId)]
                //}
            }
            //DisplayRejectClicked(rejectClickedList);
        }
      
    }
}
