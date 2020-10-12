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
    /// Interaction logic for AddOutsoleMaterialRejectToRackWindow.xaml
    /// </summary>
    public partial class AddOutsoleMaterialRejectToRackWindow : Window
    {
        OutsoleSuppliersModel supplier;
        string productNo;
        List<OutsoleMaterialDeliveryDetailModel> osMaterialDeliveryDetailList;

        List<OutsoleMaterialRackPositionModel> rackPositionList;

        BackgroundWorker bwLoad;

        public AddOutsoleMaterialRejectToRackWindow(string productNo, OutsoleSuppliersModel supplier, List<OutsoleMaterialDeliveryDetailModel> osMaterialDeliveryDetailList)
        {
            this.productNo = productNo;
            this.supplier = supplier;
            this.osMaterialDeliveryDetailList = osMaterialDeliveryDetailList;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                txtSupplier.Text = supplier.Name;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            rackPositionList = OutsoleMaterialRackPositionController.Select(productNo).Where(w => w.OutsoleSupplierId == supplier.OutsoleSupplierId).ToList();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Remove sizeNo = 1, SizeNo = "", Quantity = ""
            if (e.Error != null)
            {
                MessageBox.Show(e.ToString(), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            rackPositionList = rackPositionList.Where(w => w.Pairs != 0 && w.SizeNo.Contains("1") == false).ToList();

            var rackNumberList = rackPositionList.Select(s => s.RackNumber).Distinct().ToList();
            cbRack.ItemsSource = rackNumberList;
            cbRack.SelectedItem = rackNumberList.FirstOrDefault();

            this.Cursor = null;
        }

        private void cbRack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string rackNumberSelected = cbRack.SelectedItem as String;
            var cartonNumber = rackPositionList.FirstOrDefault(w => w.RackNumber == rackNumberSelected);

            if (cartonNumber != null)
                txtCartonNumber.Text = cartonNumber.CartonNumber.ToString();

            // Display Data


        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
