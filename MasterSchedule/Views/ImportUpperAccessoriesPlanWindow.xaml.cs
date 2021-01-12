using MasterSchedule.Controllers;
using MasterSchedule.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportUpperAccessoriesPlanWindow.xaml
    /// </summary>
    public partial class ImportUpperAccessoriesPlanWindow : Window
    {
        string[] filePathArray;
        List<MaterialPlanModel> materialPlanList;
        List<SupplierModel> supplierList;
        BackgroundWorker bwReadExcel;
        BackgroundWorker bwLoad;
        BackgroundWorker bwUpload;
        public ImportUpperAccessoriesPlanWindow()
        {
            bwReadExcel = new BackgroundWorker();
            bwReadExcel.DoWork += BwReadExcel_DoWork;
            bwReadExcel.RunWorkerCompleted += BwReadExcel_RunWorkerCompleted;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            bwUpload = new BackgroundWorker();
            bwUpload.DoWork += BwUpload_DoWork;
            bwUpload.RunWorkerCompleted += BwUpload_RunWorkerCompleted;

            supplierList = new List<SupplierModel>();
            materialPlanList = new List<MaterialPlanModel>();
            InitializeComponent();
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            supplierList = SupplierController.GetSuppliers();
        }
        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void btnOpenExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Accesories Material Excel File";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                filePathArray = openFileDialog.FileNames;
                if (bwReadExcel.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    btnOpenExcel.IsEnabled = false;
                    materialPlanList.Clear();
                    //dgMain.ItemsSource = null;
                    bwReadExcel.RunWorkerAsync();
                }
            }
            else
            {

            }
        }
        private void BwReadExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                txtStatus.Text = "Reading ...";
            }));
            int filePathIndex = 1;
            foreach (string filePath in filePathArray)
            {
                Excel.Application excelApplication = new Excel.Application();
                Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
                Excel.Worksheet excelWorksheet;
                Excel.Range excelRange;
                try
                {
                    excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                    excelRange = excelWorksheet.UsedRange;

                    var upperAccessoriesSuppliersList = supplierList.Where(w => String.IsNullOrEmpty(w.ProvideAccessories) == false).ToList();
                    int pgValue = 2;
                    Dispatcher.Invoke(new Action(() =>
                    {
                        prgStatus.Maximum = excelRange.Rows.Count;
                    }));
                    for (int r = 2; r <= excelRange.Rows.Count; r++)
                    {
                        var accessoriesNameCell = excelRange[r, 1].Value2;
                        var supplierNameCell = excelRange[r, 2].Value2;
                        var productNoCell = excelRange[r, 3].Value2;
                        var etdCell = excelRange[r, 4].Value2;

                        string accessoriesName  = accessoriesNameCell != null ? accessoriesNameCell.ToString() : "";
                        string supplierName     = supplierNameCell != null ? supplierNameCell.ToString() : "";
                        string etd              = etdCell != null ? etdCell.ToString() : "";
                        string productNo        = productNoCell != null ? productNoCell.ToString() : "";
                        if (productNoCell != null)
                        {
                            // Ignore Error Cell
                            if (String.IsNullOrEmpty(accessoriesName) ||
                                String.IsNullOrEmpty(supplierName))
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    MessageBox.Show(String.Format("Supplier or Accessories Name is empty\nRows {0} !", r), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                                }));
                                break;
                            }
                            var checkSupplier = upperAccessoriesSuppliersList.FirstOrDefault(f => f.ProvideAccessories.ToUpper().Equals(accessoriesName.Trim().ToUpper())
                                                                                                      && f.Name.ToUpper().Equals(supplierName.Trim().ToUpper()));

                            if (checkSupplier == null)
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    MessageBox.Show(String.Format("Supplier: {0} {1}\nDoes not exist in system, Please Check again !\nRow: {2}", accessoriesName, supplierName, r), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                                }));
                                break;
                            }

                            double etdDouble = 0;
                            Double.TryParse(etd, out etdDouble);
                            var etdDate = etdDouble > 0 ? DateTime.FromOADate(etdDouble) : new DateTime(2000, 01, 01);
                            var materialPlan = new MaterialPlanModel {
                                SupplierId  = checkSupplier.SupplierId,
                                ProductNo   = productNo,
                                ETD         = etdDate,
                                Name        = checkSupplier.Name,
                                ProvideAccessories = checkSupplier.ProvideAccessories,
                                ActualDate  = new DateTime(2000,01,01),
                                Remarks     = ""
                            };
                            materialPlanList.Add(materialPlan);
                        }

                        Dispatcher.Invoke(new Action(() =>
                        {
                            txtStatus.Text = String.Format("Reading PO: {0}   file: {1} / {2}", productNo, filePathIndex, filePathArray.Count());
                            prgStatus.Value = pgValue;
                        }));
                        pgValue++;
                    }
                }
                catch
                {
                }
                finally
                {
                    excelWorkbook.Close(false, Missing.Value, Missing.Value);
                    excelApplication.Quit();
                }
                filePathIndex++;
            }
        }
        private void BwReadExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (e.Cancelled == true)
            {
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show(string.Format("Error\n{0}", e.Error.Message), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (materialPlanList.Count <= 0)
            {
                MessageBox.Show("Cannot Read Excel File. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show(string.Format("Read Completed !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            txtStatus.Text = String.Format("Read completed: {0} records", materialPlanList.Count());
            
            prgStatus.Value = 0;
            dgMain.ItemsSource = materialPlanList;
            dgMain.Items.Refresh();
            btnOpenExcel.IsEnabled = true;
        }      
        
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (dgMain.ItemsSource == null)
                return;
            var importList = dgMain.ItemsSource.OfType<MaterialPlanModel>().ToList();
            if (bwUpload.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                bwUpload.RunWorkerAsync(importList);
            }
        }
        private void BwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            var importList = e.Argument as List<MaterialPlanModel>;
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    txtStatus.Text = "Importing ...";
                    prgStatus.Maximum = importList.Count();
                }));
                foreach (var item in importList)
                {
                    MaterialPlanController.Insert(item, isUpdateActualDate: false);
                    dgMain.Dispatcher.Invoke(new Action(() => {
                        dgMain.SelectedItem = item;
                        dgMain.ScrollIntoView(item);
                    }));
                }
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(ex.InnerException.InnerException.Message.ToString());
                }));
            }
        }
        private void BwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false && e.Error == null && (bool)e.Result == true)
            {
                MessageBox.Show(string.Format("Saved !"), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                txtStatus.Text = "Finished !";
                prgStatus.Value = 0;
            }
            this.Cursor = null;
            btnImport.IsEnabled = true;
        }

        private void btnSupplierInfo_Click(object sender, RoutedEventArgs e)
        {
            AddUpperAccessoriesSupplierWindow window = new AddUpperAccessoriesSupplierWindow(supplierList);
            window.ShowDialog();
            if (window.supplierListRespone != null)
                supplierList = window.supplierListRespone.ToList();
        }

        private void dgMain_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
