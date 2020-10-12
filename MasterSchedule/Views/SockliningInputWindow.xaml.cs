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
using MasterSchedule.Helpers;
using System.Data;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SockliningInputWindow.xaml
    /// </summary>
    public partial class SockliningInputWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwInsert;
        string productNo;

        List<SizeRunModel> sizeRunList;

        List<SockliningInputModel> sockliningInputList;
        List<SockliningInputModel> sockliningInputInsertList;

        DataTable dt;
        DateTime nowDate;
        List<OffDayModel> offDayList;

        public string resultString;
        public SockliningInputWindow(string productNo)
        {
            this.productNo = productNo;
            // 108a-2332
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork +=new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            sizeRunList = new List<SizeRunModel>();
            sockliningInputList = new List<SockliningInputModel>();
            sockliningInputInsertList = new List<SockliningInputModel>();

            dt = new DataTable();
            offDayList = new List<OffDayModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                this.Title = String.Format("{0} for : {1}", this.Title, productNo);

                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            sizeRunList = SizeRunController.Select(productNo);
            sockliningInputList = SockliningInputController.SelectByPO(productNo);
            offDayList = OffDayController.Select();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = String.Format("{0}\n({1})", sizeRun.SizeNo, sizeRun.Quantity);
                column.MinWidth = 40;
                column.Binding = new Binding(String.Format("Column{0}", i));

                Style styleColumn = new Style();
                Setter setterColumnForecolor = new Setter();
                setterColumnForecolor.Property = DataGridCell.ForegroundProperty;
                setterColumnForecolor.Value = new Binding(String.Format("Column{0}Foreground", i));
                styleColumn.Setters.Add(setterColumnForecolor);

                DataColumn columnForeground = new DataColumn(String.Format("Column{0}Foreground", i), typeof(SolidColorBrush));
                columnForeground.DefaultValue = Brushes.Black;

                column.CellStyle = styleColumn;
                dt.Columns.Add(columnForeground);
                dgSockliningInput.Columns.Add(column);
            }
            colCompleted.DisplayIndex = dgSockliningInput.Columns.Count - 1;

            DataRow dr = dt.NewRow();
            for (int j = 0; j <= sizeRunList.Count - 1; j++)
            {
                SizeRunModel sizeRun = sizeRunList[j];
                int qtyPerSize = sockliningInputList.Where(s => s.SizeNo == sizeRun.SizeNo).Sum(s => s.Quantity);
                dr[String.Format("Column{0}", j)] = qtyPerSize;
                if (qtyPerSize == sizeRun.Quantity)
                {
                    dr[String.Format("Column{0}Foreground", j)] = Brushes.Blue;
                }
                else
                {
                    dr[String.Format("Column{0}Foreground", j)] = Brushes.Red;
                }
            }

            dt.Rows.Add(dr);

            dgSockliningInput.ItemsSource = dt.AsDataView();

            lblQtyTotal.Text = sockliningInputList.Sum(s => s.Quantity).ToString();
            lblQtyOrder.Text = String.Format("/{0}", sizeRunList.Sum(s => s.Quantity));

            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            // get date
            nowDate = DateTime.Now.Date.AddDays(-1);
            while (offDayList.Select(s => s.Date).ToList().Contains(nowDate))
            {
                nowDate = nowDate.AddDays(-1);
            }
            dt = ((DataView)dgSockliningInput.ItemsSource).ToTable();
            foreach (DataRow dr in dt.Rows)
            {
                int qtyActual = 0;
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    string sizeNo = sizeRunList[i].SizeNo;
                    int quantity = (Int32)dr[String.Format("Column{0}", i)];
                    qtyActual += quantity;
                    if (quantity >= 0)
                    {
                        var model = new SockliningInputModel
                        {
                            ProductNo = productNo,
                            SizeNo = sizeNo,
                            Quantity = quantity,
                        };
                        sockliningInputInsertList.Add(model);
                    }
                }
            }

            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var insertModel in sockliningInputInsertList)
            {
                SockliningInputController.Insert(insertModel);
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);

            int qtyBalance = sizeRunList.Sum(s => s.Quantity) - sockliningInputInsertList.Sum(s => s.Quantity);
            //if (qtyBalance > 0)
            //{
            //    resultString = qtyBalance.ToString();
            //    if (qtyBalance == sizeRunList.Sum(s => s.Quantity))
            //    {
            //        resultString = "";
            //    }
            //}
            if (qtyBalance != 0)
            {
                resultString = qtyBalance.ToString();
                if (qtyBalance == sizeRunList.Sum(s => s.Quantity))
                    resultString = "";
            }

            else
            {
                resultString = String.Format("{0:M/d}", nowDate);
            }

            this.DialogResult = true;
        }

        private void dgSockliningInput_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
            {
                return;
            }
            if (e.Column.GetValue(TagProperty) == null)
            {
                return;
            }
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }
            int qtyOld = sockliningInputList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                int qtyInput = 0;
                qtyInput = (qtyOld + qtyNew);
                if (qtyOld + qtyNew < 0 || qtyOld + qtyNew > qtyOrder)
                {
                    qtyInput = qtyOld;
                }
                txtCurrent.Text = qtyInput.ToString();
                int qtyTotal = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        if (sizeNo == sizeRunList[i].SizeNo)
                        {
                            dr[String.Format("Column{0}", i)] = qtyInput;
                            if (qtyInput == qtyOrder)
                            {
                                dr[String.Format("Column{0}Foreground", i)] = Brushes.Blue;
                            }
                            else
                            {
                                dr[String.Format("Column{0}Foreground", i)] = Brushes.Red;
                            }
                        }
                        int qtyCurrent = (Int32)(dr[String.Format("Column{0}", i)]);
                        qtyTotal += qtyCurrent;
                    }
                }
                lblQtyTotal.Text = qtyTotal.ToString();
            }
        }

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ((DataRowView)dgSockliningInput.CurrentItem).Row;
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
                dr[String.Format("Column{0}Foreground", i)] = Brushes.Blue;
            }
            dgSockliningInput.ItemsSource = null;
            dgSockliningInput.ItemsSource = dt.AsDataView();

            lblQtyTotal.Text = sizeRunList.Sum(s => s.Quantity).ToString();
        }
    }
}
