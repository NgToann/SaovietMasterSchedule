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
//using MasterSchedule.Helpers;
using System.Windows.Media;
using System.Data.Metadata.Edm;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddRejectBySizeForMaterialWindow.xaml
    /// </summary>
    public partial class AddRejectForMaterialWindow : Window
    {
        private List<RejectModel> rejectUpperAccessoriesList;
        List<RejectModel> rejectClickedList;
        private SizeRunModel sizeRunClicked;
        public AddRejectForMaterialWindow(List<RejectModel> rejectUpperAccessoriesList, SizeRunModel sizeRunClicked)
        {
            this.rejectUpperAccessoriesList = rejectUpperAccessoriesList;
            this.sizeRunClicked = sizeRunClicked;
            rejectClickedList = new List<RejectModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sizeRunClicked != null)
            {
                txtSizeNo.Text          = sizeRunClicked.SizeNo;
                txtSizeNo.IsReadOnly    = true;
                txtQuantity.IsReadOnly  = true;
            }
            else
            {
                txtSizeNo.Focus();
            }
            Thread.Sleep(500);
            LoadListOfDefects(rejectUpperAccessoriesList);
        }
        
        private void LoadListOfDefects(List<RejectModel> rejectUpperAccessoriesList)
        {
            // binding to error to grid
            int countColumn = gridError.ColumnDefinitions.Count();
            int countRow = countRow = rejectUpperAccessoriesList.Count / countColumn;
            if (rejectUpperAccessoriesList.Count % countColumn != 0)
            {
                countRow = rejectUpperAccessoriesList.Count / countColumn + 1;
            }
            gridError.RowDefinitions.Clear();
            for (int i = 1; i <= countRow; i++)
            {
                RowDefinition rd = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star),
                };
                gridError.RowDefinitions.Add(rd);
            }

            for (int i = 0; i <= rejectUpperAccessoriesList.Count() - 1; i++)
            {
                Button button = new Button();
                var template = FindResource("ButtonDefectTemplate") as ControlTemplate;
                button.Template = template;
                button.Name = Name = string.Format("button{0}", rejectUpperAccessoriesList[i].RejectKey);
                button.Margin = new Thickness(4, 4, 0, 0);
                if (i / countColumn == 0)
                {
                    if (i != 0)
                        button.Margin = new Thickness(4, 0, 0, 0);
                    else
                        button.Margin = new Thickness(0, 0, 0, 0);
                }
                if (i % countColumn == 0 && i / countColumn != 0)
                    button.Margin = new Thickness(0, 4, 0, 0);
                button.Tag = rejectUpperAccessoriesList[i];
                button.Click += Button_Click;
                button.MaxHeight = 68;
                Border br = new Border();
                br.Name = string.Format("border{0}", rejectUpperAccessoriesList[i].RejectKey);

                Grid.SetColumn(button, i % countColumn);
                Grid.SetRow(button, i / countColumn);

                Grid grid = new Grid();
                ColumnDefinition cld1 = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Auto),
                };
                ColumnDefinition cld2 = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star),
                };
                grid.ColumnDefinitions.Add(cld1);
                grid.ColumnDefinitions.Add(cld2);

                TextBlock tblKey = new TextBlock();
                tblKey.Text = rejectUpperAccessoriesList[i].RejectKey;
                tblKey.FontSize = 20;
                tblKey.Foreground = Brushes.Blue;
                tblKey.VerticalAlignment = VerticalAlignment.Center;
                tblKey.Margin = new Thickness(4, 0, 4, 0);
                Grid.SetColumn(tblKey, 0);

                TextBlock tblReject = new TextBlock();
                tblReject.Text = string.Format("{0}\n{1}", rejectUpperAccessoriesList[i].RejectName, rejectUpperAccessoriesList[i].RejectName_1);
                tblReject.FontSize = 14;
                tblReject.TextWrapping = TextWrapping.Wrap;
                tblReject.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(tblReject, 1);

                grid.Children.Add(tblKey);
                grid.Children.Add(tblReject);

                br.Child = grid;
                button.Content = br;

                gridError.Children.Add(button);
            }
        }

        // Button Reject Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonClicked = sender as Button;
            var rejectClicked = buttonClicked.Tag as RejectModel;
            int qtyCurrent = 0;
            var qtyCurrentString = txtQuantity.Text.Trim().ToString();
            Int32.TryParse(qtyCurrentString, out qtyCurrent);
            qtyCurrent += 1;
            if (qtyCurrent > sizeRunClicked.Quantity)
            {
                MessageBox.Show(String.Format("Total reject can't be greater than #size {0} quantity", sizeRunClicked.SizeNo), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            txtQuantity.Text = qtyCurrent.ToString();
            rejectClickedList.Add(rejectClicked);
            HighLightError(rejectClicked.RejectKey);
            DisplayRejectClicked(rejectClickedList);
        }

        private void DisplayRejectClicked(List<RejectModel> rejectClickedList)
        {
            tblRejectClicked.Text = "";
            List<string> displayList = new List<string>();
            var rejectIdList = rejectClickedList.Select(s => s.RejectId).Distinct().ToList();
            if (rejectIdList.Count() > 0)
                rejectIdList = rejectIdList.OrderBy(o => o).ToList();
            foreach (var rId in rejectIdList)
            {
                var rejectById = rejectClickedList.FirstOrDefault(f => f.RejectId.Equals(rId));
                var noOfReject = rejectClickedList.Where(w => w.RejectId.Equals(rId)).Count();
                displayList.Add(String.Format("{0} - {1}: {2} pair{3}", rejectById.RejectName, rejectById.RejectName_1, noOfReject, noOfReject > 1 ? "s" : ""));
            }
            tblRejectClicked.Text = String.Join(";  ", displayList);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearQuantity_Click(object sender, RoutedEventArgs e)
        {
            txtQuantity.Text = "0";
            rejectClickedList.Clear();
            DisplayRejectClicked(rejectClickedList);
        }
        private void HighLightError(string rejectKey)
        {
            try
            {
                var childrenCount = VisualTreeHelper.GetChildrenCount(gridError);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(gridError, i);
                    if (child != null)
                    {
                        Button buttonClicked = child as Button;
                        var template = FindResource("ButtonDefectTemplate") as ControlTemplate;
                        var templateClicked = FindResource("ButtonDefectClickedTemplate") as ControlTemplate;
                        buttonClicked.Template = template;
                        if (buttonClicked.Name.Equals(String.Format("button{0}", rejectKey)))
                            buttonClicked.Template = templateClicked;
                    }
                }
            }
            catch { }
        }
    }
}
