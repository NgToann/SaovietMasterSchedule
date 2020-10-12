using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.ViewModels;
using MasterSchedule.Helpers;

using System.Globalization;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleDeliveryStatusWindow.xaml
    /// </summary>
    public partial class CompletionStatusWindow : Window
    {
        BackgroundWorker threadLoad;
        List<OrdersModel> orderList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<AssemblyMasterModel> assemblyMasterList;

        DateTime dtDefault;
        DateTime dtNothing;
        DateTime etdStart, etdEnd;

        List<CompletionStatusViewModel> completionStatusViewList;
        List<OrderExtraModel> orderExtraList;
        public CompletionStatusWindow()
        {
            InitializeComponent();
            threadLoad = new BackgroundWorker();
            threadLoad.WorkerSupportsCancellation = true;
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);

            
            orderList = new List<OrdersModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();

            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            completionStatusViewList = new List<CompletionStatusViewModel>();

            orderExtraList = new List<OrderExtraModel>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpETDStart.SelectedDate = DateTime.Now;
            dpETDEnd.SelectedDate = DateTime.Now;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (threadLoad.IsBusy == false)
            {
                completionStatusViewList = new List<CompletionStatusViewModel>();

                etdStart = dpETDStart.SelectedDate.Value;
                etdEnd = dpETDEnd.SelectedDate.Value;

                this.Cursor = Cursors.Wait;
                btnLoad.IsEnabled = false;
                btnView.IsEnabled = false;
                dgMain.ItemsSource = null;

                threadLoad.RunWorkerAsync();
            }
        }

        void threadLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.SelectFull(etdStart, etdEnd);
            sewingMasterList = SewingMasterController.SelectFull(etdStart, etdEnd);
            outsoleMasterList = OutsoleMasterController.SelectFull(etdStart, etdEnd);
            assemblyMasterList = AssemblyMasterController.SelectFull(etdStart, etdEnd);
            orderExtraList = OrderExtraController.SelectFull(etdStart, etdEnd);

            List<String> productNoList = sewingMasterList.Select(r => r.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                var order = orderList.FirstOrDefault(f => f.ProductNo == productNo);

                //var order1 = orderList.Where(w => w.ProductNo == productNo).FirstOrDefault();

                var sewingMaster = sewingMasterList.FirstOrDefault(f => f.ProductNo == productNo);
                var outsoleMaster = outsoleMasterList.FirstOrDefault(f => f.ProductNo == productNo);
                var assemblyMaster = assemblyMasterList.FirstOrDefault(f => f.ProductNo == productNo);
                var orderExtra = orderExtraList.FirstOrDefault(f => f.ProductNo == productNo);

                CompletionStatusViewModel completionStatusView = new CompletionStatusViewModel
                {
                    ProductNo = order.ProductNo,
                    Country = order.Country,
                    ShoeName = order.ShoeName,
                    ArticleNo = order.ArticleNo,
                    OutsoleCode = order.OutsoleCode,
                    ETD = order.ETD,
                    Quantity = order.Quantity,
                };

                if (orderExtra != null)
                {
                    DateTime loadingDate = dtDefault;
                    if (DateTime.TryParse(orderExtra.LoadingDate, out loadingDate) == true)
                    {
                        completionStatusView.LoadingDate = string.Format(new CultureInfo("en-US"), "{0:dd-MMM}", loadingDate);
                    }
                    else
                    {
                        completionStatusView.LoadingDate = "";
                    }
                }

                if (sewingMaster != null)
                {
                    completionStatusView.SewingLine = sewingMaster.SewingLine;
                    completionStatusView.CutAFinishDate = sewingMaster.CutBBalance;
                    completionStatusView.CutAActualFinishDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualFinishDate);
                    completionStatusView.SewingFinishDate = sewingMaster.SewingActualFinishDate;

                    if (string.IsNullOrEmpty(sewingMaster.SewingActualFinishDate.Trim()) == true)
                    {
                        completionStatusView.SewingFinishDate = sewingMaster.SewingBalance;
                    }
                }
                else
                {
                    completionStatusView.SewingLine = "";
                    completionStatusView.CutAFinishDate = "";
                    completionStatusView.CutAActualFinishDate = "";
                    completionStatusView.SewingFinishDate = "";
                }

                if (outsoleMaster != null)
                {
                    completionStatusView.OutsoleLine = outsoleMaster.OutsoleLine;
                    completionStatusView.OutsoleFinishDate = outsoleMaster.OutsoleActualFinishDate;
                    if (string.IsNullOrEmpty(outsoleMaster.OutsoleActualFinishDate.Trim()) == true)
                    {
                        completionStatusView.OutsoleFinishDate = outsoleMaster.OutsoleBalance;
                    }
                }
                else
                {
                    completionStatusView.OutsoleLine = "";
                    completionStatusView.OutsoleFinishDate = "";
                }

                completionStatusView.IsFinished = false;
                if (assemblyMaster != null)
                {
                    completionStatusView.AssemblyLine = assemblyMaster.AssemblyLine;
                    completionStatusView.AssemblyFinishDate = 
                        assemblyMaster.AssemblyActualFinishDate.Contains("/")? TimeHelper.ConvertDateToView(assemblyMaster.AssemblyActualFinishDate)
                        : assemblyMaster.AssemblyActualFinishDate;

                    if (string.IsNullOrEmpty(assemblyMaster.AssemblyActualFinishDate.Trim()) == true)
                    {
                        completionStatusView.AssemblyFinishDate = assemblyMaster.AssemblyBalance;
                    }

                    if (string.IsNullOrEmpty(assemblyMaster.AssemblyActualFinishDate.Trim()) == false || assemblyMaster.AssemblyBalance.ToLower() == "OK".ToLower())
                    {
                        completionStatusView.IsFinished = true;
                    }
                }
                else
                {
                    completionStatusView.AssemblyLine = "";
                    completionStatusView.AssemblyFinishDate = "";
                }
                completionStatusViewList.Add(completionStatusView);
            }
        }

        void threadLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnLoad.IsEnabled = true;
            btnView.IsEnabled = true;
            this.Cursor = null;
        }

        private void dgMain_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            List<CompletionStatusViewModel> completionStatusViewFilterList = completionStatusViewList;

            string shoeName = txtShoeName.Text;
            if (string.IsNullOrEmpty(shoeName) == false)
            {
                completionStatusViewFilterList = completionStatusViewFilterList.Where(o => o.ShoeName.ToLower().Contains(shoeName.ToLower()) == true).ToList();
            }

            string article = txtArticleNo.Text;
            if (string.IsNullOrEmpty(article) == false)
            {
                completionStatusViewFilterList = completionStatusViewFilterList.Where(o => o.ArticleNo.ToLower().Contains(article.ToLower()) == true).ToList();
            }

            if (chboFinished.IsChecked.Value == true || chboUnfinished.IsChecked.Value == true)
            {
                completionStatusViewFilterList = completionStatusViewFilterList.Where(o => o.IsFinished == chboFinished.IsChecked.Value || !o.IsFinished == chboUnfinished.IsChecked.Value).ToList();
            }
            else
            {
                completionStatusViewFilterList = null;
            }

            dgMain.ItemsSource = null;
            dgMain.ItemsSource = completionStatusViewFilterList;
        }
    }
}
