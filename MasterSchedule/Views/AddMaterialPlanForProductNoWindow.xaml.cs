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
using System.Windows.Media;
using System.Data.Metadata.Edm;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddMaterialPlanForProductNoWindow.xaml
    /// </summary>
    public partial class AddMaterialPlanForProductNoWindow : Window
    {
        private string productNo;
        private List<SupplierModel> supplierAccessoriesList;
        public MaterialPlanModel materialUpdate;
        public EExcute runModeRespone;
        private List<MaterialPlanModel> matsPlanCurrentList;
        private MaterialPlanModel matsSubmit;
        private EExcute runMode = EExcute.None;
        BackgroundWorker bwInsert;
        public AddMaterialPlanForProductNoWindow(string productNo, List<SupplierModel> supplierAccessoriesList, MaterialPlanModel matsSubmit, List<MaterialPlanModel> matsPlanCurrentList)
        {
            this.supplierAccessoriesList    = supplierAccessoriesList;
            this.productNo                  = productNo;
            this.matsSubmit                 = matsSubmit;
            this.matsPlanCurrentList        = matsPlanCurrentList;

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += BwInsert_DoWork; 
            bwInsert.RunWorkerCompleted += BwInsert_RunWorkerCompleted;

            InitializeComponent();

            this.Title = this.Title + " - " + productNo;
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpETD.SelectedDate = DateTime.Now.Date;
            var accessoriesNameList = supplierAccessoriesList.Select(s => s.ProvideAccessories).Distinct().ToList();
            cboAccessoryName.ItemsSource = accessoriesNameList;
            if (matsSubmit != null)
            {
                cboAccessoryName.SelectedItem = accessoriesNameList.FirstOrDefault(f => f.Equals(matsSubmit.ProvideAccessories));
                btnDelete.IsEnabled = true;
                dpETD.SelectedDate  = matsSubmit.ETD;
                txtRemarks.Text     = matsSubmit.Remarks;
            }
            else
            {
                cboAccessoryName.SelectedItem = accessoriesNameList.FirstOrDefault();
            }
        }

        private void cboAccessoryName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cboAccessorySelected = sender as ComboBox;
            string accessoryName = cboAccessorySelected.SelectedItem as String;
            var supplierList = supplierAccessoriesList.Where(w => w.ProvideAccessories.Equals(accessoryName)).ToList();
            cboSupplierName.ItemsSource = supplierList;
            if (matsSubmit != null && accessoryName.Equals(matsSubmit.ProvideAccessories))
            {
                cboSupplierName.SelectedItem = supplierList.FirstOrDefault(f => f.SupplierId == matsSubmit.SupplierId);
            }
            else
            {
                cboSupplierName.SelectedItem = supplierList.FirstOrDefault();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false)
            {
                var cboSupplierSelected = cboSupplierName.SelectedItem as SupplierModel;
                var etdPicked           = dpETD.SelectedDate.Value;
                var remarks             = txtRemarks.Text.Trim().ToString();
                if (matsSubmit != null)
                {
                    if (cboSupplierSelected.SupplierId != matsSubmit.SupplierId
                        && matsPlanCurrentList.Where(w => w.SupplierId != matsSubmit.SupplierId && w.SupplierId == cboSupplierSelected.SupplierId).Count() > 0)
                    {
                        MessageBox.Show(String.Format("Supplier Update :{0}\nAlready Exist !", cboSupplierSelected.Name), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    runMode = EExcute.Update;
                }
                else
                {
                    if (matsPlanCurrentList.Where(w => w.SupplierId.Equals(cboSupplierSelected.SupplierId)).Count() > 0)
                    {
                        MessageBox.Show(String.Format("Supplier Add :{0}\n Already Exist !", cboSupplierSelected.Name), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    runMode = EExcute.AddNew;
                }

                btnSave.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                object[] par = new object[] { cboSupplierSelected, etdPicked, remarks };
                bwInsert.RunWorkerAsync(par);
            }
        }
        
        private void BwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            var par             = e.Argument as object[];
            var suppSelected    = par[0] as SupplierModel;
            var etdPicked       = (DateTime)par[1];
            var remarks         = par[2] as String;
            var materialRevise  = new MaterialPlanModel()
            {
                ProductNo           = productNo,
                SupplierId          = suppSelected.SupplierId,
                Name                = suppSelected.Name,
                ProvideAccessories  = suppSelected.ProvideAccessories,
                Remarks             = remarks,
                ETD                 = etdPicked,
                ActualDate          = matsSubmit != null ? matsSubmit.ActualDate : new DateTime(2000, 01, 01)
            };
            try
            {
                if (runMode == EExcute.AddNew)
                {
                    MaterialPlanController.Insert(materialRevise, isUpdateActualDate: false);
                }
                else if (runMode == EExcute.Update)
                {
                    MaterialPlanController.Update(matsSubmit.ProductNo, matsSubmit.SupplierId, materialRevise);
                }
                else if (runMode == EExcute.Delete)
                {
                    MaterialPlanController.Delete(materialRevise);
                }
                materialUpdate = materialRevise as MaterialPlanModel;
                runModeRespone = runMode;
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(ex.Message.ToString());
                }));
            }
        }
        
        private void BwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result != false)
            {
                if (runMode == EExcute.Update)
                {
                    MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (runMode == EExcute.Delete)
                {
                    MessageBox.Show("Deleted !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            runMode = EExcute.None;
            Thread.Sleep(500);
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Confirm Delete ?"), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            if (bwInsert.IsBusy == false)
            {
                var cboSupplierSelected = cboSupplierName.SelectedItem as SupplierModel;
                var etdPicked           = dpETD.SelectedDate.Value;
                var remarks             = txtRemarks.Text.Trim().ToString();

                runMode = EExcute.Delete;
                btnDelete.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                object[] par = new object[] { cboSupplierSelected, etdPicked, remarks };
                bwInsert.RunWorkerAsync(par);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
                this.Close();
        }
    }
}
