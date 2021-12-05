using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Reflection;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.Controllers;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpdateSpecifyOrderWindow.xaml
    /// </summary>
    public partial class UpdateSpecifyOrderWindow : Window
    {
        string filePath;
        List<OrdersModel> ordersList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        List<OrdersModel> ordersToImportList;

        List<String> updateWhatList;
        AccountModel acc;
        public UpdateSpecifyOrderWindow(AccountModel acc)
        {
            this.acc = acc;
            filePath = "";
            updateWhatList = new List<String>();

            ordersList = new List<OrdersModel>();
            ordersToImportList = new List<OrdersModel>();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load grid Update What?
            var propertyList = typeof(OrdersModel).GetProperties().ToList();

            gridUpdateWhat.Children.Clear();
            int countColumn = gridUpdateWhat.ColumnDefinitions.Count();
            int countRow = countRow = propertyList.Count / countColumn;
            if (propertyList.Count % countColumn != 0)
            {
                countRow = propertyList.Count / countColumn + 1;
            }
            gridUpdateWhat.RowDefinitions.Clear();

            for (int i = 1; i <= countRow; i++)
            {
                RowDefinition rd = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star),
                };
                gridUpdateWhat.RowDefinitions.Add(rd);
            }

            for (int i = 0; i < propertyList.Count(); i++)
            {
                CheckBox chkUpdateWhat = new CheckBox();
                chkUpdateWhat.Content = propertyList[i].Name.ToString();
                chkUpdateWhat.Margin = new Thickness(4, 0, 4, 0);
                chkUpdateWhat.Tag = propertyList[i].Name.ToString();
                chkUpdateWhat.Checked +=new RoutedEventHandler(chkUpdateWhat_Checked);
                chkUpdateWhat.Unchecked +=new RoutedEventHandler(chkUpdateWhat_Unchecked);

                if (i / countColumn > 0)
                {
                    chkUpdateWhat.Margin = new Thickness(4, 6, 4, 0);
                }
                Grid.SetColumn(chkUpdateWhat, i % countColumn);
                Grid.SetRow(chkUpdateWhat, i / countColumn);
                gridUpdateWhat.Children.Add(chkUpdateWhat);
            }

            // Load OpenfileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select Order File";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
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
                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
                for (int i = 5; i <= excelRange.Rows.Count; i++)
                {
                    var productNoValue = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                    if (productNoValue != null)
                    {
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

                        double shipDateOADate = 0;
                        Double.TryParse((excelRange.Cells[i, 10] as Excel.Range).Value2.ToString(), out shipDateOADate);
                        DateTime shipDate = DateTime.FromOADate(shipDateOADate);
                        order.ShipDate = shipDate;

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
                    }
                    else
                    {
                        i = i + 4;
                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                }
            }
            catch
            {
                ordersList.Clear();
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
            lblStatus.Text = "Read Completed!";
            if (ordersList.Count() > 0)
            {
                dgOrders.ItemsSource = ordersList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} Prod. No.!", ordersList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Excel File Error. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void chkUpdateWhat_Checked(object sender, RoutedEventArgs e)
        {
            var checkboxChecked = sender as CheckBox;
            if (checkboxChecked == null)
                return;
            checkboxChecked.Foreground = Brushes.Blue;
            checkboxChecked.FontWeight = FontWeights.Bold;

            updateWhatList.Add(checkboxChecked.Tag.ToString());
        }
        private void chkUpdateWhat_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkboxUnChecked = sender as CheckBox;
            if (checkboxUnChecked == null)
                return;
            checkboxUnChecked.Foreground = Brushes.Black;
            checkboxUnChecked.FontWeight = FontWeights.Normal;

            if (updateWhatList.Contains(checkboxUnChecked.Tag.ToString()))
                updateWhatList.Remove(checkboxUnChecked.Tag.ToString());
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                progressBar.Value = 0;
                lblStatus.Text = "";
                ordersToImportList = dgOrders.Items.OfType<OrdersModel>().ToList();
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            int prgValue = 1;
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = ordersToImportList.Count));
            foreach (var orders in ordersToImportList)
            {
                OrdersController.Update(orders, updateWhatList);
                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Updating ...";
                    progressBar.Value = prgValue;
                    dgOrders.SelectedItem = orders;
                    dgOrders.ScrollIntoView(orders);
                    prgValue++;
                }));
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
            lblStatus.Text = "Updated !";
            MessageBox.Show("Update Completed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgOrders_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
