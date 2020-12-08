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

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddRejectBySizeForMaterialWindow.xaml
    /// </summary>
    public partial class AddRejectForMaterialWindow : Window
    {
        private List<RejectModel> rejectUpperAccessoriesList;
        public AddRejectForMaterialWindow(List<RejectModel> rejectUpperAccessoriesList)
        {
            this.rejectUpperAccessoriesList = rejectUpperAccessoriesList;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                button.Margin = new Thickness(4, 4, 0, 0);
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

                TextBlock txtKey = new TextBlock();
                txtKey.Text = rejectUpperAccessoriesList[i].RejectKey;
                txtKey.FontSize = 28;
                txtKey.Foreground = Brushes.Blue;
                txtKey.VerticalAlignment = VerticalAlignment.Center;
                txtKey.Margin = new Thickness(4, 0, 4, 0);
                Grid.SetColumn(txtKey, 0);

                TextBlock txtErrorName = new TextBlock();
                txtErrorName.Text = string.Format("{0}\n{1}", rejectUpperAccessoriesList[i].RejectName, rejectUpperAccessoriesList[i].RejectName_1);
                txtErrorName.FontSize = 15;
                txtErrorName.TextWrapping = TextWrapping.Wrap;
                txtErrorName.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(txtErrorName, 1);

                grid.Children.Add(txtKey);
                grid.Children.Add(txtErrorName);

                br.Child = grid;
                button.Content = br;

                gridError.Children.Add(button);
            }
        }
    }
}
