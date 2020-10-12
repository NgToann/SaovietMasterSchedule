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
using System.ComponentModel;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportMachineRequestWindow.xaml
    /// </summary>
    public partial class ImportMachineRequestWindow : Window
    {
        BackgroundWorker bwLoad;
        List<PhaseModel> phaseList;
        List<MachineModel> machineList;
        PhaseModel phaseSelected;
        List<MachineModel> machineList_PhaseSelected;
        public ImportMachineRequestWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            phaseSelected = new PhaseModel();
            phaseList = new List<PhaseModel>();
            machineList = new List<MachineModel>();
            machineList_PhaseSelected = new List<MachineModel>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string computerName = System.Environment.MachineName;
            string osVersion = System.Environment.OSVersion.Version.ToString();
            string userName = System.Environment.UserDomainName.ToString();
            string userName1 = System.Environment.UserName.ToString();

            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            phaseList = PhaseController.Select();
            machineList = MachineController.Select();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cbPhase.ItemsSource = phaseList;
            cbPhase.SelectedValue = phaseList.FirstOrDefault();
            this.Cursor = null;
        }

        private void cbPhase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            phaseSelected = cbPhase.SelectedItem as PhaseModel;
            machineList_PhaseSelected = machineList.Where(w => w.PhaseID == phaseSelected.PhaseID).ToList();

            // show machine
            var machineListActList = machineList_PhaseSelected.Where(w => w.IsMachine == true).ToList();
            grbMachine.Header = String.Format("Input Machine Request for: {0}", phaseSelected.PhaseName);

            gridMachine.Children.Clear();
            int countColumn = gridMachine.ColumnDefinitions.Count();
            int countRow = countRow = machineListActList.Count / countColumn;
            if (machineListActList.Count % countColumn != 0)
            {
                countRow = machineListActList.Count / countColumn + 1;
            }
            gridMachine.RowDefinitions.Clear();

            for (int i = 1; i <= countRow; i++)
            {
                RowDefinition rd = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star),
                };
                gridMachine.RowDefinitions.Add(rd);
            }

            for (int i = 0; i <= machineListActList.Count() - 1; i++)
            {
                MachineModel machine = machineListActList[i];
                StackPanel stkMachine = new StackPanel();
                stkMachine.Orientation = Orientation.Vertical;

                stkMachine.Margin = new Thickness(4, 0, 4, 0);
                if (i / countColumn > 0)
                {
                    stkMachine.Margin = new Thickness(4, 4, 4, 0);
                }

                TextBlock txtMachineName = new TextBlock();
                txtMachineName.Text = machine.MachineName;
                txtMachineName.FontWeight = FontWeights.SemiBold;

                TextBox txtMachineID = new TextBox();
                txtMachineID.Tag = machine;
                txtMachineID.BorderBrush = Brushes.Black;
                txtMachineID.Foreground = Brushes.Blue;
                txtMachineID.VerticalContentAlignment = VerticalAlignment.Center;
                //txtMachineID.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txtMachineID_LostKeyboardFocus);

                stkMachine.Children.Add(txtMachineName);
                stkMachine.Children.Add(txtMachineID);
                Grid.SetColumn(stkMachine, i % countColumn);
                Grid.SetRow(stkMachine, i / countColumn);

                gridMachine.Children.Add(stkMachine);
            }
            // show worker
            var machineIsWorkerListAct = machineList_PhaseSelected.Where(w => w.PhaseID == phaseSelected.PhaseID && w.IsMachine == false).ToList();
            for (int i = 0; i <= machineIsWorkerListAct.Count() - 1; i++)
            {
                var machine = machineIsWorkerListAct[i];
            }
        }

        //private void txtMachineID_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    var txtMachineLostFocus = sender as TextBox;
        //    if (txtMachineLostFocus == null)
        //        return;
        //    var machine = txtMachineLostFocus.Tag as MachineModel;
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Get Machine List
            // 
            // Stack Machine
            List<MachineModel> machineListFromTextBox = new List<MachineModel>();

            var stackMachineCount = VisualTreeHelper.GetChildrenCount(gridMachine);
            for (int i = 0; i < stackMachineCount; i++)
            {
                var child = VisualTreeHelper.GetChild(gridMachine, i);
                var stackMachine = child as StackPanel;
                if (stackMachine != null)
                {
                    var childrenCount = VisualTreeHelper.GetChildrenCount(stackMachine);
                    for(int j = 0; j < childrenCount; j++)
                    {
                        var child_1 = VisualTreeHelper.GetChild(stackMachine, j);
                        if (child_1 != null)
                        {
                            var txtMachine = child_1 as TextBox;
                            if (txtMachine != null)
                            {
                                machineListFromTextBox.Add(txtMachine.Tag as MachineModel);
                            }
                        }
                    }
                }
            }
        }

        private void txtArticleNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            RenderData();
        }
        private void RenderData()
        {
            string articleNo = txtArticleNo.Text.Trim().ToUpper().ToString();
        }
    }
}
