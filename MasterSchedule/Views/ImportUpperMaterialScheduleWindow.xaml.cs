using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Microsoft.Win32;
using MasterSchedule.Models;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportUpperMaterialScheduleWindow.xaml
    /// </summary>
    public partial class ImportUpperMaterialScheduleWindow : Window
    {
        string filePath;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        List<MaterialTypeModel> materialTypeList;
        List<RawMaterialModel> upperMaterialFromExcelList;
        DateTime dtDefault = new DateTime(2000, 1, 1);
        public ImportUpperMaterialScheduleWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork +=new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);

            materialTypeList = new List<MaterialTypeModel>();
            upperMaterialFromExcelList = new List<RawMaterialModel>();
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select Upper Material Schedule Excel File";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    upperMaterialFromExcelList.Clear();
                    lblStatus.Text = "Reading...";
                    bwLoad.RunWorkerAsync();
                }
            }
            else
            {
                this.Close();
            }
        }
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            materialTypeList = MaterialTypeController.Select();
            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
            //excelApplication.Visible = true;
            Excel.Worksheet excelWorksheet;
            Excel.Range excelRange;
            try
            {
                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
                for (int i = 3; i <= excelRange.Rows.Count; i++)
                {
                    var productNoValue = (excelRange.Cells[i, 1] as Excel.Range).Value2;
                    if (productNoValue != null)
                    {
                        // column 2,3 = TAIWAN (10)
                        var taiwanMaterial = new RawMaterialModel();
                        taiwanMaterial.ProductNo = productNoValue.ToString();
                        taiwanMaterial.MaterialTypeId = 10;
                        taiwanMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 10) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 10).Name : "";
                        taiwanMaterial.ETD = dtDefault;
                        taiwanMaterial.ActualDate = dtDefault;
                        var efdTaiwan = (excelRange.Cells[i, 2] as Excel.Range).Value2;
                        if (efdTaiwan != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdTaiwan.ToString(), out efd);
                            taiwanMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualTaiwan = (excelRange.Cells[i, 3] as Excel.Range).Value2;
                        if (actualTaiwan != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualTaiwan.ToString(), out actual);
                            taiwanMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdTaiwan != null || actualTaiwan != null)
                        {
                            upperMaterialFromExcelList.Add(taiwanMaterial);
                        }

                        // Column 4,5 = LAMINATION (1)
                        var laminationMaterial = new RawMaterialModel();
                        laminationMaterial.ProductNo = productNoValue.ToString();
                        laminationMaterial.MaterialTypeId = 1;
                        laminationMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 1) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 1).Name : "";
                        laminationMaterial.ETD = dtDefault;
                        laminationMaterial.ActualDate = dtDefault;
                        var efdLamination = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                        if (efdLamination != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdLamination.ToString(), out efd);
                            laminationMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualLamination = (excelRange.Cells[i, 5] as Excel.Range).Value2;
                        if (actualLamination != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualLamination.ToString(), out actual);
                            laminationMaterial.ActualDate = DateTime.FromOADate(actual);
                        }

                        if (efdLamination != null || actualLamination != null)
                        {
                            upperMaterialFromExcelList.Add(laminationMaterial);
                        }

                        // Column 6,7 = CUTTING (1)
                        var cuttingMaterial = new RawMaterialModel();
                        cuttingMaterial.ProductNo = productNoValue.ToString();
                        cuttingMaterial.MaterialTypeId = 2;
                        cuttingMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 2) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 2).Name : "";
                        cuttingMaterial.ETD = dtDefault;
                        cuttingMaterial.ActualDate = dtDefault;
                        var efdCutting = (excelRange.Cells[i, 6] as Excel.Range).Value2;
                        if (efdCutting != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdCutting.ToString(), out efd);
                            cuttingMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualCutting = (excelRange.Cells[i, 7] as Excel.Range).Value2;
                        if (actualCutting != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualCutting.ToString(), out actual);
                            cuttingMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdCutting != null || actualCutting != null)
                        {
                            upperMaterialFromExcelList.Add(cuttingMaterial); 
                        }

                        // Column 8,9 = LEATHER (1)
                        var leatherMaterial = new RawMaterialModel();
                        leatherMaterial.ProductNo = productNoValue.ToString();
                        leatherMaterial.MaterialTypeId = 3;
                        leatherMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 3) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 3).Name : "";
                        leatherMaterial.ETD = dtDefault;
                        leatherMaterial.ActualDate = dtDefault;
                        var efdLeather = (excelRange.Cells[i, 8] as Excel.Range).Value2;
                        if (efdLeather != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdLeather.ToString(), out efd);
                            leatherMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualLeather = (excelRange.Cells[i, 9] as Excel.Range).Value2;
                        if (actualLeather != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualLeather.ToString(), out actual);
                            leatherMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdLeather != null || actualLeather != null)
                        {
                            upperMaterialFromExcelList.Add(leatherMaterial);
                        }

                        // Column 10, 11 = SEMIPROCESS (1)
                        var semiprocessMaterial = new RawMaterialModel();
                        semiprocessMaterial.ProductNo = productNoValue.ToString();
                        semiprocessMaterial.MaterialTypeId = 4;
                        semiprocessMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 4) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 4).Name : "";
                        semiprocessMaterial.ETD = dtDefault;
                        semiprocessMaterial.ActualDate = dtDefault;
                        var efdSemiprocess = (excelRange.Cells[i, 10] as Excel.Range).Value2;
                        if (efdSemiprocess != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdSemiprocess.ToString(), out efd);
                            semiprocessMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualSemiprocess = (excelRange.Cells[i, 11] as Excel.Range).Value2;
                        if (actualSemiprocess != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualSemiprocess.ToString(), out actual);
                            semiprocessMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdSemiprocess != null || actualSemiprocess != null)
                        {
                            upperMaterialFromExcelList.Add(semiprocessMaterial);
                        }

                        // Column 12, 13 = SEWING (1)
                        var sewingMaterial = new RawMaterialModel();
                        sewingMaterial.ProductNo = productNoValue.ToString();
                        sewingMaterial.MaterialTypeId = 5;
                        sewingMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 5) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 5).Name : "";
                        sewingMaterial.ETD = dtDefault;
                        sewingMaterial.ActualDate = dtDefault;
                        var efdSewing = (excelRange.Cells[i, 12] as Excel.Range).Value2;
                        if (efdSewing != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdSewing.ToString(), out efd);
                            sewingMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualSewing = (excelRange.Cells[i, 13] as Excel.Range).Value2;
                        if (actualSewing != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualSewing.ToString(), out actual);
                            sewingMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdSewing != null || actualSewing != null)
                        {
                            upperMaterialFromExcelList.Add(sewingMaterial);
                        }

                        // Column 14,15 = SECURITY LABEL (1)
                        var securitylabelMaterial = new RawMaterialModel();
                        securitylabelMaterial.ProductNo = productNoValue.ToString();
                        securitylabelMaterial.MaterialTypeId = 7;
                        securitylabelMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 7) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 7).Name : "";
                        securitylabelMaterial.ETD = dtDefault;
                        securitylabelMaterial.ActualDate = dtDefault;
                        var efdSecuritylabel = (excelRange.Cells[i, 14] as Excel.Range).Value2;
                        if (efdSecuritylabel != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdSecuritylabel.ToString(), out efd);
                            securitylabelMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualSecuritylabel = (excelRange.Cells[i, 15] as Excel.Range).Value2;
                        if (actualSecuritylabel != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualSecuritylabel.ToString(), out actual);
                            securitylabelMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdSecuritylabel != null || actualSecuritylabel != null)
                        {
                            upperMaterialFromExcelList.Add(securitylabelMaterial);
                        }

                        // Column 16,17 = ASSEMBLY (1)
                        var assemblyMaterial = new RawMaterialModel();
                        assemblyMaterial.ProductNo = productNoValue.ToString();
                        assemblyMaterial.MaterialTypeId = 8;
                        assemblyMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 8) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 8).Name : "";
                        assemblyMaterial.ETD = dtDefault;
                        assemblyMaterial.ActualDate = dtDefault;
                        var efdAssembly = (excelRange.Cells[i, 16] as Excel.Range).Value2;
                        if (efdAssembly != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdAssembly.ToString(), out efd);
                            assemblyMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualAssembly = (excelRange.Cells[i, 17] as Excel.Range).Value2;
                        if (actualAssembly != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualAssembly.ToString(), out actual);
                            assemblyMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdAssembly != null || actualAssembly != null)
                        {
                            upperMaterialFromExcelList.Add(assemblyMaterial);
                        }

                        // Column 18,19 = SOCKLINING (1)
                        var sockliningMaterial = new RawMaterialModel();
                        sockliningMaterial.ProductNo = productNoValue.ToString();
                        sockliningMaterial.MaterialTypeId = 9;
                        sockliningMaterial.MaterialTypeName = materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 9) != null ? materialTypeList.FirstOrDefault(w => w.MaterialTypeId == 9).Name : "";
                        sockliningMaterial.ETD = dtDefault;
                        sockliningMaterial.ActualDate = dtDefault;
                        var efdSocklining = (excelRange.Cells[i, 18] as Excel.Range).Value2;
                        if (efdSocklining != null)
                        {
                            double efd = 0;
                            Double.TryParse(efdSocklining.ToString(), out efd);
                            sockliningMaterial.ETD = DateTime.FromOADate(efd);
                        }
                        var actualSocklining = (excelRange.Cells[i, 19] as Excel.Range).Value2;
                        if (actualSocklining != null)
                        {
                            double actual = 0;
                            Double.TryParse(actualSocklining.ToString(), out actual);
                            sockliningMaterial.ActualDate = DateTime.FromOADate(actual);
                        }
                        if (efdSocklining != null || actualSocklining != null)
                        {
                            upperMaterialFromExcelList.Add(sockliningMaterial);
                        }
                        //upperMaterialFromExcelList.Add(taiwanMaterial);
                        //upperMaterialFromExcelList.Add(laminationMaterial);
                        //upperMaterialFromExcelList.Add(cuttingMaterial);
                        //upperMaterialFromExcelList.Add(leatherMaterial);
                        //upperMaterialFromExcelList.Add(semiprocessMaterial);
                        //upperMaterialFromExcelList.Add(sewingMaterial);
                        //upperMaterialFromExcelList.Add(securitylabelMaterial);
                        //upperMaterialFromExcelList.Add(assemblyMaterial);
                        //upperMaterialFromExcelList.Add(sockliningMaterial);

                        progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i - 2));
                    }
                }
            }
            catch
            {
                upperMaterialFromExcelList.Clear();
            }
            finally
            {
                excelWorkbook.Close(false, Missing.Value, Missing.Value);
                excelApplication.Quit();
            }
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            this.Cursor = null;
            lblStatus.Text = "Completed!";
            if (upperMaterialFromExcelList.Count() > 0)
            {
                dgUpperMaterialSchedule.ItemsSource = upperMaterialFromExcelList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} records!", upperMaterialFromExcelList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Excel File Error. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            var importList = dgUpperMaterialSchedule.ItemsSource.OfType<RawMaterialModel>().ToList();
            if (bwImport.IsBusy == false && importList.Count() > 0)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                lblStatus.Text = "Importing ...";
                progressBar.Value = 0;
                bwImport.RunWorkerAsync(importList);
            }
        }
        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            var importList = e.Argument as List<RawMaterialModel>;
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = importList.Count()));
            int indexPrg = 1;

            foreach (var import in importList)
            {
                var productNoRevise = new ProductNoReviseModel
                {
                    ProductNo = import.ProductNo,
                    SectionId = "WH",
                    ReviseDate = DateTime.Now,
                };

                // db excute
                RawMaterialController.InsertFromExcel(import);
                ProductNoReviseController.Insert(productNoRevise);

                Dispatcher.Invoke(new Action(() =>
                {
                    dgUpperMaterialSchedule.SelectedItem = import;
                    dgUpperMaterialSchedule.ScrollIntoView(import);
                }));
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = indexPrg));
                indexPrg++;
            }
        }
        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnImport.IsEnabled = true;
            if (e.Error == null && e.Cancelled == false)
            {
                MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                lblStatus.Text = "Finished !";
            }
            else
            {
                MessageBox.Show("An error occured ! Please try again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgUpperMaterialSchedule_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
