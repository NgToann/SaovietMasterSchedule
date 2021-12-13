using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using LiveCharts;
using LiveCharts.Wpf;

namespace MasterSchedule.Views
{
    public partial class MasterStatisticsWindow : Window
    {
        BackgroundWorker bwLoad;
        List<ChartRawMaterialModel> rawMats;
        public MasterStatisticsWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            rawMats = new List<ChartRawMaterialModel>();
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string macAddress = "30-0E-D5-0F-BA-C8";
            //byte[] binaryData = Encoding.ASCII.GetBytes(macAddress);            
            //string strHex = BitConverter.ToString(binaryData);
            //Guid id = new Guid(binaryData);
            if (!bwLoad.IsBusy)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            var months = rawMats.Select(s => s.POETD.Month).Distinct().ToList();
            months = months.OrderBy(o => o).ToList();

            var poList              = new double[12];
            var upperOKList         = new double[12];
            var upperNotOKList      = new double[12];
            var upperRemarksList    = new double[12];

            int index = 0;
            DateTime dtDefault = new DateTime(2000, 01, 01);
            foreach (var month in months)
            {
                var rawMatsPerMonth = rawMats.Where(w => w.POETD.Month == month).ToList();
                var poPerMonth = rawMatsPerMonth.Select(s => s.ProductNo).Distinct().ToList().Count();
                poList[index] = poPerMonth;

                var poHasNotActualDate = rawMatsPerMonth.Where(w => w.ActualDate == dtDefault
                                                        && w.GroupName.Equals("UPPER")).Select(s => s.ProductNo).Distinct().ToList();
                var upperOKPerMonth = rawMatsPerMonth.Where(w => !poHasNotActualDate.Contains(w.ProductNo)).Select(s => s.ProductNo).Distinct().ToList().Count();
                var upperNotOKPerMonth = poPerMonth - upperOKPerMonth;
                var remarksPerMonth = rawMatsPerMonth.Where(w => !String.IsNullOrEmpty(w.Remarks)
                                                    && w.GroupName.Equals("UPPER")).Select(s => s.ProductNo).Distinct().ToList().Count();

                upperOKList[index] = upperOKPerMonth;
                upperNotOKList[index] = upperNotOKPerMonth;
                upperRemarksList[index] = remarksPerMonth;

                index++;
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Upper OK",
                    Values = new ChartValues<double>(upperOKList),
                    PointGeometry = null,
                    PointForeground = Brushes.Green
                },
                new LineSeries
                {
                    Title = "Upper ETD",
                    Values = new ChartValues<double>(upperNotOKList),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Upper Remarks",
                    LineSmoothness = 0,
                    Values = new ChartValues<double>(upperRemarksList),
                    PointGeometry = DefaultGeometries.Square,
                    PointForeground = Brushes.Yellow,
                },
                
                //new LineSeries
                //{
                //    Title = "Series 2",
                //    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                //    PointGeometry = null
                //},
                //new LineSeries
                //{
                //    Title = "Series 3",
                //    Values = new ChartValues<double> { 4,2,7,2,7 },
                //    PointGeometry = DefaultGeometries.Square,
                //    PointGeometrySize = 15
                //}
            };
            
            var rowsDisplay = 1509;
            for (int i = 0; i < rowsDisplay; i++)
            {

            }

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };
            //YFormatter = value => value.ToString("C");
            YFormatter = value => value.ToString();

            //modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            SeriesCollection[0].Values.Add(5d);
            DataContext = this;
            this.Cursor = null;
        }

        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            rawMats = ChartController.GetRawMaterial();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
    }
}
