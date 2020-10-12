using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddOutsoleMaterialRackPositionWindow.xaml
    /// </summary>
    public partial class AddOutsoleMaterialRackPositionWindow : Window
    {
        List<OutsoleMaterialRackPositionModel> rackUpdateList;
        OutsoleSuppliersModel supplier;

        string productNo;
        List<String> sizeNoList;
        List<String> quantityList;
        List<OutsoleMaterialDeliveryDetailModel> osMaterialDeliveryDetailList;
        List<OutsoleMaterialRackPositionModel> rackPositionCurrentList;
        BackgroundWorker bwSave;
        BackgroundWorker bwRemove;

        public bool Reload = false;
        ActionType actionType;

        public AddOutsoleMaterialRackPositionWindow(string productNo, 
                                                    OutsoleSuppliersModel supplier,
                                                    List<OutsoleMaterialRackPositionModel> rackUpdateList,
                                                    List<OutsoleMaterialDeliveryDetailModel> osMaterialDeliveryDetailList)
        {
            this.supplier = supplier;
            this.rackUpdateList = rackUpdateList;
            this.productNo = productNo;
            this.osMaterialDeliveryDetailList = osMaterialDeliveryDetailList;

            bwSave = new BackgroundWorker();
            bwSave.DoWork += new DoWorkEventHandler(bwSave_DoWork);
            bwSave.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSave_RunWorkerCompleted);

            bwRemove = new BackgroundWorker();
            bwRemove.DoWork += new DoWorkEventHandler(bwRemove_DoWork);
            bwRemove.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRemove_RunWorkerCompleted);

            sizeNoList = new List<String>();
            quantityList = new List<String>();
            rackPositionCurrentList = new List<OutsoleMaterialRackPositionModel>();

            actionType = ActionType.CREATE;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // CREATE
            if (rackUpdateList == null)
            {
                actionType = ActionType.CREATE;
                DisplayData(osMaterialDeliveryDetailList);
            }

            // UPDATE
            if (rackUpdateList != null && rackUpdateList.Count() > 0)
            {
                actionType = ActionType.UPDATE;

                txtRackNumber.Text = rackUpdateList.FirstOrDefault().RackNumber;
                txtCartonNumber.Text = rackUpdateList.FirstOrDefault().CartonNumber.ToString();

                List<OutsoleMaterialDeliveryDetailModel> osMaterialDeliveryUpdateList = new List<OutsoleMaterialDeliveryDetailModel>();
                foreach (var rackUpdate in rackUpdateList)
                {
                    var osMaterialDelivery = new OutsoleMaterialDeliveryDetailModel()
                    {
                        IDRackUpdate = rackUpdate.RackPositionID,
                        ProductNo = rackUpdate.ProductNo,
                        OutsoleSupplierId = rackUpdate.OutsoleSupplierId,
                        SizeNo = rackUpdate.SizeNo,
                        QuantityCurrent = rackUpdate.Pairs,
                    };
                    osMaterialDeliveryUpdateList.Add(osMaterialDelivery);
                }
                DisplayData(osMaterialDeliveryUpdateList);
                btnAdd.Content = "Save";
            }

            txtRackNumber.Focus();
            txtSupplier.Text = supplier.Name;
            btnAdd.IsDefault = true;
        }

        private void DisplayData(List<OutsoleMaterialDeliveryDetailModel> osmDeliveryDetailList)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Title", typeof(String));
            DataGridTextColumn colTitle = new DataGridTextColumn();
            colTitle.Header = "SizeNo";
            colTitle.FontWeight = FontWeights.Bold;
            colTitle.Width = 70;
            colTitle.Binding = new Binding("Title");
            dgSizeNoAndQuantity.Columns.Add(colTitle);

            var regex = new Regex("[a-z]|[A-Z]");
            sizeNoList = osmDeliveryDetailList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();

            for (int i = 0; i < sizeNoList.Count(); i++)
            {
                dt.Columns.Add(String.Format("Column{0}", i), typeof(String));
                DataGridTextColumn colSize = new DataGridTextColumn();
                colSize.Binding = new Binding(String.Format("Column{0}", i));
                colSize.Header = sizeNoList[i].ToString();
                colSize.MinWidth = 40;
                dgSizeNoAndQuantity.Columns.Add(colSize);
            }

            dt.Columns.Add("Total", typeof(Int32));
            DataGridTextColumn colTotal = new DataGridTextColumn();
            colTotal.Header = "Total";
            colTotal.FontWeight = FontWeights.Bold;
            colTotal.IsReadOnly = true;
            colTotal.Width = 70;
            colTotal.Binding = new Binding("Total");
            dgSizeNoAndQuantity.Columns.Add(colTotal);

            // Fill Data
            DataRow dr = dt.NewRow();
            dr["Title"] = "Quantity";
            int total = 0;
            for (int i = 0; i < sizeNoList.Count(); i++)
            {
                var osMaterialDetailPerSize = osmDeliveryDetailList.LastOrDefault(f => f.SizeNo == sizeNoList[i]);
                if (osMaterialDetailPerSize != null)
                {
                    dr[String.Format("Column{0}", i)] = osMaterialDetailPerSize.QuantityCurrent;
                    total += osMaterialDetailPerSize.QuantityCurrent;
                }
            }
            dr["Total"] = total;

            dt.Rows.Add(dr);
            dgSizeNoAndQuantity.ItemsSource = dt.AsDataView();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string rackNumber = "";
            int cartonNumber = 0;
            rackNumber = txtRackNumber.Text.ToUpper().ToString();
            Int32.TryParse(txtCartonNumber.Text.ToString(), out cartonNumber);

            if (String.IsNullOrEmpty(rackNumber))
            {
                txtRackNumber.SelectAll();
                txtRackNumber.Focus();
                MessageBox.Show("Rack Number: Empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cartonNumber == 0)
            {
                txtCartonNumber.SelectAll();
                txtCartonNumber.Focus();
                MessageBox.Show("CartonNo: Empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GetDataFromDatagrid(rackNumber, cartonNumber);
            if (bwSave.IsBusy == false && rackPositionCurrentList.Count() > 0)
            {
                this.Cursor = Cursors.Wait;
                bwSave.RunWorkerAsync();
            }
        }

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var rackInsert in rackPositionCurrentList)
            {
                if (actionType == ActionType.CREATE)
                    OutsoleMaterialRackPositionController.Insert(rackInsert);
                if (actionType == ActionType.UPDATE)
                    OutsoleMaterialRackPositionController.Update(rackInsert);
                Reload = true;
            }
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString(), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Cursor = null;
            MessageBox.Show("Saved !", "Master Schdedule", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            string rackNumber = "";
            int cartonNumber = 0;
            rackNumber = txtRackNumber.Text.ToUpper().ToString();
            Int32.TryParse(txtCartonNumber.Text.ToString(), out cartonNumber);

            if (String.IsNullOrEmpty(rackNumber))
            {
                txtRackNumber.SelectAll();
                txtRackNumber.Focus();
                return;
            }
            if (cartonNumber == 0)
            {
                txtCartonNumber.SelectAll();
                txtCartonNumber.Focus();
                return;
            }

            if (MessageBox.Show("Confirm Remove ?", "Master Schedule", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            
            GetDataFromDatagrid(rackNumber, cartonNumber);
            if (bwRemove.IsBusy == false && rackPositionCurrentList.Count() > 0)
            {
                this.Cursor = Cursors.Wait;
                bwRemove.RunWorkerAsync();
            }
        }

        private void bwRemove_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var rackRemove in rackPositionCurrentList)
            {
                OutsoleMaterialRackPositionController.Delete(rackRemove);
                Reload = true;
            }
        }

        private void bwRemove_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString(), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Cursor = null;
            MessageBox.Show("Removed !", "Master Schdedule", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void GetDataFromDatagrid(string rackNumber, int cartonNumber)
        {
            rackPositionCurrentList.Clear();
            // Get quantity and sizeno from datagrid.
            try {
                DataTable dt = ((DataView)dgSizeNoAndQuantity.ItemsSource).ToTable();
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < sizeNoList.Count(); i++)
                    {
                        string sizeNo = sizeNoList[i].ToString();
                        int quantity = 0;
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out quantity);

                        int rackPositionId = 0;
                        if (actionType == ActionType.UPDATE)
                        {
                            rackPositionId = rackUpdateList.FirstOrDefault(w => w.SizeNo == sizeNo).RackPositionID;
                        }

                        var rackModel = new OutsoleMaterialRackPositionModel()
                        {
                            RackPositionID = rackPositionId,
                            ProductNo = productNo,
                            OutsoleSupplierId = supplier.OutsoleSupplierId,
                            RackNumber = rackNumber,
                            CartonNumber = cartonNumber,
                            SizeNo = sizeNo,
                            Pairs = quantity,
                            //Quantity = quantity.ToString(),
                        };
                        rackPositionCurrentList.Add(rackModel);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        enum ActionType
        {
            CREATE,
            UPDATE
        }
    }
}
