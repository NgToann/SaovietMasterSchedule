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
using System.Reflection;
using System.Threading;

using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddUpperAccessoriesSupplierWindow.xaml
    /// </summary>
    public partial class AddUpperAccessoriesSupplierWindow : Window
    {
        List<SupplierModel> supplierList;
        BackgroundWorker bwUpload;
        public List<SupplierModel> supplierListRespone;
        public AddUpperAccessoriesSupplierWindow(List<SupplierModel> supplierList)
        {
            this.supplierList = supplierList;
            
            bwUpload = new BackgroundWorker();
            bwUpload.DoWork += BwUpload_DoWork;
            bwUpload.RunWorkerCompleted += BwUpload_RunWorkerCompleted;

            InitializeComponent();
        }

        private void BwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                dgSuppliers.ItemsSource = supplierList.Where(w => String.IsNullOrEmpty(w.ProvideAccessories) == false).ToList();
                dgSuppliers.Items.Refresh();
                supplierListRespone = supplierList.ToList();
                this.DialogResult = true;
            }
            this.Cursor = null;
            btnAdd.IsEnabled = true;
        }

        private void BwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            var supplierAdd = e.Argument as SupplierModel;
            try
            {
                SupplierController.UploadUpperAccessoriesSupplier(supplierAdd);
                supplierList.Add(supplierAdd);
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.InnerException.InnerException.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }));
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string accessoriesName = txtAccessoriesName.Text.Trim().ToString();
            string supplierName = txtSupplierName.Text.Trim().ToString();

            if (String.IsNullOrEmpty(accessoriesName) || String.IsNullOrEmpty(supplierName))
            {
                MessageBox.Show(String.Format("Acessories Name or Supplier Name is empty !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var upperAccessoriesSuppliersList = supplierList.Where(w => String.IsNullOrEmpty(w.ProvideAccessories) == false).ToList();
            var checkExist = upperAccessoriesSuppliersList.FirstOrDefault(f => f.ProvideAccessories.ToUpper().Equals(accessoriesName.ToUpper())
                                                                            && f.Name.ToUpper().Equals(supplierName.ToUpper()));
            if (checkExist != null)
            {
                MessageBox.Show(String.Format("Supplier: {0} - {1} already exist !", checkExist.Name, checkExist.ProvideAccessories), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (bwUpload.IsBusy==false)
            {
                var supplierAdd = new SupplierModel
                {
                    SupplierId = supplierList.Max(m => m.SupplierId) + 1,
                    ProvideAccessories = accessoriesName,
                    Name = supplierName
                };

                this.Cursor = Cursors.Wait;
                btnAdd.IsEnabled = false;
                bwUpload.RunWorkerAsync(supplierAdd);
            }
        }

        private void dgSuppliers_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgSuppliers.ItemsSource = supplierList.Where(w => String.IsNullOrEmpty(w.ProvideAccessories) == false).ToList();
            dgSuppliers.Items.Refresh();
        }
    }
}
