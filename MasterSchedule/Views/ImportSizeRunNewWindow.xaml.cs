using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

using Excel = Microsoft.Office.Interop.Excel;

using MasterSchedule.Controllers;
using MasterSchedule.Models;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportSizeRunNewWindow.xaml
    /// </summary>
    public partial class ImportSizeRunNewWindow : Window
    {
        string filePath;
        List<SizeRunModel> sizeRunList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        BackgroundWorker bwImportOrders;
        List<SizeRunModel> sizeRunToImportList;
        List<OrdersModel> ordersToImportList;
        List<OrdersModel> ordersList;
        AccountModel acc;
        PrivateDefineModel privateDef;
        public ImportSizeRunNewWindow(AccountModel acc)
        {
            this.acc = acc;
            filePath = "";
            sizeRunList = new List<SizeRunModel>();
            ordersToImportList = new List<OrdersModel>();
            ordersList = new List<OrdersModel>();
            privateDef = new PrivateDefineModel();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);

            bwImportOrders = new BackgroundWorker();
            bwImportOrders.DoWork += BwImportOrders_DoWork; 
            bwImportOrders.RunWorkerCompleted += BwImportOrders_RunWorkerCompleted;

            sizeRunToImportList = new List<SizeRunModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Orders + SizeRun";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    sizeRunList.Clear();
                    ordersList.Clear();
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

            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
            //excelApplication.Visible = true;
            Excel.Worksheet excelWorksheet;
            Excel.Range excelRange;
            try
            {
                privateDef = PrivateDefineController.GetDefine();
                var regex = new Regex("[a-z]|[A-Z]");

                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
                int noOfPO = 0;
                for (int i = 5; i <= excelRange.Rows.Count; i++)
                {
                    var productNoValue = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                    if (productNoValue != null)
                    {
                        noOfPO++;
                        string productNo = productNoValue.ToString();
                        // Order
                        var order = new OrdersModel();
                        string UCustomerCode = "";
                        var uCustomerCodeValue = (excelRange.Cells[i, 2] as Excel.Range).Value2;
                        if (uCustomerCodeValue != null)
                        {
                            UCustomerCode = uCustomerCodeValue.ToString();
                        }
                        order.UCustomerCode = UCustomerCode;

                        string GTNPONo = "";
                        var GTNPONoValue = (excelRange.Cells[i, 3] as Excel.Range).Value2;
                        if (GTNPONoValue != null)
                        {
                            GTNPONo = GTNPONoValue.ToString();
                        }
                        order.GTNPONo = GTNPONo;
                        
                        order.ProductNo = productNo;

                        double csdOADate = 0;
                        Double.TryParse((excelRange.Cells[i, 5] as Excel.Range).Value2.ToString(), out csdOADate);
                        DateTime csd = DateTime.FromOADate(csdOADate);
                        order.ETD = csd.AddDays(-10);

                        string articleNo = (excelRange.Cells[i, 6] as Excel.Range).Value2.ToString();
                        order.ArticleNo = articleNo;

                        string shoeName = (excelRange.Cells[i, 7] as Excel.Range).Value2.ToString();
                        order.ShoeName = shoeName;

                        int quantity = 0;
                        int.TryParse((excelRange.Cells[i, 8] as Excel.Range).Value2.ToString(), out quantity);
                        order.Quantity = quantity;

                        string patternNo = (excelRange.Cells[i, 12] as Excel.Range).Value2.ToString();
                        order.PatternNo = patternNo;

                        var midsoleCodeValue = (excelRange.Cells[i, 13] as Excel.Range).Value2;
                        string midsoleCode = "";
                        if (midsoleCodeValue != null)
                        {
                            midsoleCode = midsoleCodeValue.ToString();
                        }
                        order.MidsoleCode = midsoleCode;

                        var outsoleCodeValue = (excelRange.Cells[i, 14] as Excel.Range).Value2;
                        string outsoleCode = "";
                        if (outsoleCodeValue != null)
                        {
                            outsoleCode = outsoleCodeValue.ToString();
                        }
                        order.OutsoleCode = outsoleCode;

                        var lastCodeValue = (excelRange.Cells[i, 15] as Excel.Range).Value2;
                        string lastCode = "";
                        if (lastCodeValue != null)
                        {
                            lastCode = lastCodeValue.ToString();
                        }
                        order.LastCode = lastCode;

                        var countryValue = (excelRange.Cells[i, 16] as Excel.Range).Value2;
                        string country = "";
                        if (countryValue != null)
                        {
                            country = countryValue.ToString();
                        }
                        order.Country = country;
                        order.Reviser = acc.UserName;

                        ordersList.Add(order);

                        //for (int j = 17; j <= 65; j++)
                        // Size Run
                        int noOfColumnLoad = privateDef != null ? privateDef.NoOfColumnOrderExcelFile : 100;
                        for (int j = 18; j <= privateDef.NoOfColumnOrderExcelFile; j++)
                        {
                            var qtyValue        = (excelRange.Cells[i, j] as Excel.Range).Value2;
                            var sizeNoValue     = (excelRange.Cells[i - noOfPO, j] as Excel.Range).Value2;
                            var osSizeValue     = (excelRange.Cells[i - noOfPO - 1, j] as Excel.Range).Value2;
                            var midSizeValue    = (excelRange.Cells[i - noOfPO - 2, j] as Excel.Range).Value2;
                            var lastSizeValue   = (excelRange.Cells[i - noOfPO - 3, j] as Excel.Range).Value2;
                            if (qtyValue != null && sizeNoValue != null)
                            {
                                int qty = 0;
                                int.TryParse(qtyValue.ToString(), out qty);
                                //string sizeNo = (excelRange.Cells[1, j] as Excel.Range).Value2.ToString();
                                string sizeNo       = sizeNoValue.ToString();
                                string outsoleSize  = osSizeValue != null ? osSizeValue.ToString() : sizeNo;
                                string midsoleSize  = midSizeValue != null ? midSizeValue.ToString() : sizeNo;
                                string lastSize     = lastSizeValue != null ? lastSizeValue.ToString() : sizeNo;

                                SizeRunModel sizeRun = new SizeRunModel
                                {
                                    ProductNo   = productNo,
                                    SizeNo      = regex.IsMatch(sizeNo) ? regex.Replace(sizeNo,"") : sizeNo,
                                    OutsoleSize = regex.IsMatch(outsoleSize) ? regex.Replace(outsoleSize, "") : outsoleSize,
                                    MidsoleSize = regex.IsMatch(midsoleSize) ? regex.Replace(midsoleSize, "") : midsoleSize,
                                    LastSize    = regex.IsMatch(lastSize) ? regex.Replace(lastSize, "") : lastSize,
                                    Quantity    = qty,
                                };
                                sizeRunList.Add(sizeRun);
                            }
                        }
                    }
                    else
                    {
                        i = i + 4;
                        noOfPO = 0;
                    }

                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                }
            }
            catch
            {
                sizeRunList.Clear();
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
            lblStatus.Text = "Load Completed!";
            if (sizeRunList.Count() > 0)
            {
                dgSizeRun.ItemsSource = sizeRunList;
                dgOrders.ItemsSource = ordersList;
                btnImport.IsEnabled = true;
                btnImportOrders.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} Size Run!\n{1} Order Records", sizeRunList.Count(), ordersList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Excel File Error. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Confirm Import SizeRunList"), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                sizeRunToImportList = dgSizeRun.Items.OfType<SizeRunModel>().ToList();
                progressBar.Value = 0;
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            // Insert SizeRun
            int i = 1;
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = sizeRunToImportList.Count));
            foreach (SizeRunModel sizeRun in sizeRunToImportList)
            {
                SizeRunController.InsertNew(sizeRun);
                dgSizeRun.Dispatcher.Invoke((Action)(() =>
                {
                    dgSizeRun.SelectedItem = sizeRun;
                    dgSizeRun.ScrollIntoView(sizeRun);
                }));
                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Importing SizeRun ...";
                    progressBar.Value = i;
                }));
                i++;
            }
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnImport.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lblStatus.Text = "Finished !";
            progressBar.Value = 0;
            MessageBox.Show("Insert SizeRun List Completed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgSizeRun_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dgOrders_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btnImportOrders_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Confirm Import OrdersList"), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            if (bwImportOrders.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImportOrders.IsEnabled = false;
                ordersToImportList = dgOrders.Items.OfType<OrdersModel>().ToList();
                bwImportOrders.RunWorkerAsync();
            }
        }
        private void BwImportOrders_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = sizeRunToImportList.Count));
            foreach (var order in ordersToImportList)
            {
                OrdersController.Insert(order);
                dgOrders.Dispatcher.Invoke((Action)(() =>
                {
                    dgOrders.SelectedItem = order;
                    dgOrders.ScrollIntoView(order);
                }));
                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Importing Order List ...";
                    progressBar.Value = i;
                }));
                i++;
            }
        }
        private void BwImportOrders_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnImportOrders.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lblStatus.Text = "Finished !";
            progressBar.Value = 0;
            MessageBox.Show("Insert OrdersList Completed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
