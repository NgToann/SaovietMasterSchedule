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

using MasterSchedule.Models;
using MasterSchedule.ViewModels;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ConfirmColumnPrintWindow.xaml
    /// </summary>
    public partial class ConfirmColumnPrintWindow : Window
    {
        public EPrintSchedule printWhat;
        object[] contentPrint = new object[] { };
        List<String> columnNeedPrintList;
        public ConfirmColumnPrintWindow(EPrintSchedule printWhat, object[] contentPrint)
        {
            this.printWhat = printWhat;
            this.contentPrint = contentPrint;
            columnNeedPrintList = new List<string>();
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (printWhat == EPrintSchedule.CutprepMaster)
            {
                tblTitle.Text = "Confirm Column Print For Cutprep Schedule";

                var propertyList = typeof(CutprepMasterExportViewModel).GetProperties().ToList();
                var propertyNameList = new List<String>();
                if (propertyList.Count() > 0)
                    propertyNameList = propertyList.OrderBy(o => o.Name).Select(s => s.Name).ToList();

                gridUpdateWhat.Children.Clear();
                int countColumn = gridUpdateWhat.ColumnDefinitions.Count();
                int countRow = countRow = propertyNameList.Count / countColumn;
                if (propertyNameList.Count % countColumn != 0)
                {
                    countRow = propertyList.Count / countColumn + 1;
                }
                gridUpdateWhat.RowDefinitions.Clear();

                for (int i = 1; i <= countRow; i++)
                {
                    RowDefinition rd = new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Auto),
                    };
                    gridUpdateWhat.RowDefinitions.Add(rd);
                }

                for (int i = 0; i < propertyNameList.Count(); i++)
                {
                    CheckBox chkUpdateWhat = new CheckBox();
                    chkUpdateWhat.Content = propertyNameList[i].ToString();
                    chkUpdateWhat.Margin = new Thickness(5, 10, 5, 0);
                    chkUpdateWhat.Tag = propertyNameList[i].ToString();
                    chkUpdateWhat.IsChecked = true;
                    chkUpdateWhat.FontWeight = FontWeights.Bold;
                    chkUpdateWhat.FontStyle = FontStyles.Italic;
                    chkUpdateWhat.Checked += new RoutedEventHandler(chkUpdateWhat_Checked);
                    chkUpdateWhat.Unchecked += new RoutedEventHandler(chkUpdateWhat_Unchecked);
                    columnNeedPrintList.Add(chkUpdateWhat.Tag.ToString());

                    if (i / countColumn > 0)
                    {
                        chkUpdateWhat.Margin = new Thickness(5, 10, 5, 0);
                    }
                    Grid.SetColumn(chkUpdateWhat, i % countColumn);
                    Grid.SetRow(chkUpdateWhat, i / countColumn);
                    gridUpdateWhat.Children.Add(chkUpdateWhat);
                }
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (printWhat == EPrintSchedule.CutprepMaster)
            {
                var cutprepMasterExportViewReportList = contentPrint[0] as List<CutprepMasterExportViewModel>;
                var lineName = contentPrint[1] as String;

                CutprepMasterReportWindow window = new CutprepMasterReportWindow(cutprepMasterExportViewReportList, lineName, columnNeedPrintList);
                window.Show();
                //this.Close();
            }
        }

        private void chkUpdateWhat_Checked(object sender, RoutedEventArgs e)
        {
            var checkboxChecked = sender as CheckBox;
            if (checkboxChecked == null)
                return;
            checkboxChecked.Foreground  = Brushes.OrangeRed;
            checkboxChecked.FontStyle   = FontStyles.Italic;
            checkboxChecked.FontWeight  = FontWeights.Bold;

            columnNeedPrintList.Add(checkboxChecked.Tag.ToString());
        }
        private void chkUpdateWhat_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkboxUnChecked = sender as CheckBox;
            if (checkboxUnChecked == null)
                return;
            checkboxUnChecked.Foreground    = Brushes.Black;
            checkboxUnChecked.FontWeight    = FontWeights.Normal;
            checkboxUnChecked.FontStyle     = FontStyles.Normal;

            if (columnNeedPrintList.Contains(checkboxUnChecked.Tag.ToString()))
                columnNeedPrintList.Remove(checkboxUnChecked.Tag.ToString());
        }

    }
}
