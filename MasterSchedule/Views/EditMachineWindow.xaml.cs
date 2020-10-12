using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpdateMachineWindow.xaml
    /// </summary>
    public partial class EditMachineWindow : Window
    {
        MachineViewModel machineClicked;
        List<PhaseModel> phaseList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwDo;
        List<MachineModel> machineList;
        public EExcute excuteMode = EExcute.None;
        public MachineModel machineUpdated;
        
        public EditMachineWindow(MachineViewModel machineClicked)
        {
            this.machineClicked = machineClicked;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwDo = new BackgroundWorker();
            bwDo.DoWork += BwDo_DoWork;
            bwDo.RunWorkerCompleted += BwDo_RunWorkerCompleted;

            phaseList = new List<PhaseModel>();
            machineList = new List<MachineModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
        
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                phaseList = PhaseController.Select();
                machineList = MachineController.Select();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(ex.Message);
                }));
            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cbPhase.ItemsSource = phaseList;
            if (machineClicked != null)
            {
                var model = new MachineModel();
                ConvertViewModelToModel(machineClicked, model);
                // dont understand the reason why it not get the phaseselected from machineclicked viewmodel ?
                model.PhaseSelected = phaseList.FirstOrDefault(f => f.PhaseID == machineClicked.PhaseSelected.PhaseID);
                gridMain.DataContext = model;
                this.Title = "Master Schedule - Update Machine";
                btnUpdate.Content = "Update";
                excuteMode = EExcute.Update;
            }
            else
            {
                var newModel = new MachineModel();
                newModel.IsMachine = true;
                newModel.PhaseSelected = phaseList.FirstOrDefault();
                newModel.MachineID = machineList.Max(m => m.MachineID) + 1;
                gridMain.DataContext = newModel;
                this.Title = "Master Schedule - Add New Machine";
                btnUpdate.Content = "Add";
                excuteMode = EExcute.AddNew;
            }
            this.Cursor = null;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var updateModel = gridMain.DataContext as MachineModel;
            if (String.IsNullOrEmpty(updateModel.MachineName))
            {
                MessageBox.Show("Machine name is empty !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (updateModel.Available == 0)
            {
                MessageBox.Show("Available quantity = 0 !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (bwDo.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwDo.RunWorkerAsync(updateModel);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteModel = gridMain.DataContext as MachineModel;
            if (excuteMode == EExcute.AddNew)
                return;
            if (MessageBox.Show(string.Format("Confirm delete {0} ?", deleteModel.MachineName), this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            
            if (bwDo.IsBusy == false)
            {
                excuteMode = EExcute.Delete;
                this.Cursor = Cursors.Wait;
                bwDo.RunWorkerAsync(deleteModel);
            }
        }

        private void BwDo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var model = e.Argument as MachineModel;
                if (excuteMode == EExcute.Delete)
                    MachineController.Delete(model.MachineID);
                else
                    MachineController.Insert(model);
                machineUpdated = model;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => {
                    MessageBox.Show(ex.Message);
                }));
            }
        }
        
        private void BwDo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled == true)
                return;
            if (excuteMode == EExcute.Delete)
                MessageBox.Show("Deleted !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.Cursor = null;
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void ConvertViewModelToModel(MachineViewModel viewModel, MachineModel model)
        {
            model.PhaseID       = viewModel.PhaseSelected.PhaseID;
            model.PhaseSelected = viewModel.PhaseSelected;
            model.MachineID     = viewModel.MachineID;
            model.MachineName   = viewModel.MachineName;
            model.IsMachine     = viewModel.IsMachine;
            model.Available     = viewModel.Available;
            model.Remarks       = viewModel.Remarks;
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.SelectAll();
        }
    }
}
