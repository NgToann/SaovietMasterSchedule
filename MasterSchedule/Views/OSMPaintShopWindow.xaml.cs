using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using MasterSchedule.Models;
using Microsoft.Reporting.WinForms;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OSMPaintShopWindow.xaml
    /// </summary>
    public partial class OSMPaintShopWindow : Window
    {
        BackgroundWorker bwLoad;
        public OSMPaintShopWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!bwLoad.IsBusy)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    DataTable dt = new OSMPaintShopDataset().Tables["PaintShopTable"];
                    var sources = ReportController.GetPaintShop();

                    foreach (var item in sources)
                    {
                        var dr = dt.NewRow();

                        dr["ProductNo"] = item.ProductNo;
                        dr["Name"] = item.Name;
                        dr["QtyOrder"] = item.QtyOrder > 0 ? item.QtyOrder.ToString() : "";
                        dr["QtySend"] = item.QtySend > 0 ? item.QtySend.ToString() : "";
                        dr["QtyDelivery"] = item.QtyDelivery > 0 ? item.QtyDelivery.ToString() : "";
                        dr["FirstDate"] = item.FirstDate.ToShortDateString();
                        dr["LastDate"] = item.LastDate.ToShortDateString();
                        dr["Balance"] = item.Balance != 0 ? item.Balance.ToString() : "";
                        dt.Rows.Add(dr);
                    }


                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "OSMPaintShopDetail";
                    rds.Value = dt;

                    reportViewer.LocalReport.ReportPath = @"Reports\OSMPaintShopReport.rdlc";
                    //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.DataSources.Add(rds);
                    reportViewer.RefreshReport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }));
        }
        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

    }
}
