using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportMachineWindow.xaml
    /// INPUT MACHINE REPORT...
    /// </summary>
    public partial class ImportMachineWindow : Window
    {
        BackgroundWorker bwLoad;
        List<MachineModel> machineList;
        List<PhaseModel> phaseList;
        List<MachineViewModel> machineViewModelList;
        public ImportMachineWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += BwLoad_DoWork;
            bwLoad.RunWorkerCompleted += BwLoad_RunWorkerCompleted;

            machineList = new List<MachineModel>();

            machineViewModelList = new List<MachineViewModel>();
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
        private void BwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            phaseList = PhaseController.Select();
            machineList = MachineController.Select().Where(w => w.IsMachine == true).ToList();
            foreach(var machine in machineList)
            {
                var viewModel = new MachineViewModel();
                ConvertModelToViewModel(machine, viewModel);
                machineViewModelList.Add(viewModel);
            }
        }
        private void BwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RenderData(machineViewModelList, dgMachineList);
            this.Cursor = null;
        }

        private void RenderData(List<MachineViewModel> viewModelList, DataGrid dg)
        {
            if (viewModelList.Count() > 0)
                viewModelList = viewModelList.OrderBy(o => o.PhaseSelected.PhaseName).ThenBy(t => t.MachineName).ToList();
            var xList = new ObservableCollection<MachineViewModel>(viewModelList);
            dg.ItemsSource = xList;
            dg.Items.Refresh();
        }

        private void dgMachineList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var itemClicked = dgMachineList.SelectedItem as MachineViewModel;
            if (itemClicked == null)
                return;
            EditMachineWindow window = new EditMachineWindow(itemClicked);
            window.ShowDialog();
            if (window.machineUpdated != null && window.excuteMode != EExcute.None)
            {
                if (window.excuteMode == EExcute.Delete)
                {
                    machineViewModelList.RemoveAll(r => r.MachineID == window.machineUpdated.MachineID);
                    RenderData(machineViewModelList, dgMachineList);
                }
                else
                {
                    ConvertModelToViewModel(window.machineUpdated, itemClicked);
                }
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            EditMachineWindow window = new EditMachineWindow(null);
            window.ShowDialog();
            if (window.machineUpdated != null && window.excuteMode != EExcute.None)
            {
                var viewModel = new MachineViewModel();
                ConvertModelToViewModel(window.machineUpdated, viewModel);
                machineViewModelList.Add(viewModel);
                RenderData(machineViewModelList, dgMachineList);
            }
        }

        private void ConvertModelToViewModel(MachineModel model, MachineViewModel viewModel)
        {
            viewModel.MachineID     = model.MachineID;
            viewModel.MachineName   = model.MachineName;
            //viewModel.PhaseID       = model.PhaseID;
            //viewModel.PhaseName     = phaseList.FirstOrDefault(f => f.PhaseID == model.PhaseID) != null ? phaseList.FirstOrDefault(f => f.PhaseID == model.PhaseID).PhaseName : "";
            if (model.PhaseSelected != null)
                viewModel.PhaseSelected = phaseList.FirstOrDefault(f => f.PhaseID == model.PhaseSelected.PhaseID) != null ? phaseList.FirstOrDefault(f => f.PhaseID == model.PhaseSelected.PhaseID) : null;
            else
                viewModel.PhaseSelected = phaseList.FirstOrDefault(f => f.PhaseID == model.PhaseID) != null ? phaseList.FirstOrDefault(f => f.PhaseID == model.PhaseID) : null;
            viewModel.IsMachine     = model.IsMachine;
            viewModel.IsMachineView = model.IsMachine == true ? "Yes" : "No";
            viewModel.Available     = model.Available;
            viewModel.Remarks       = model.Remarks;
        }
        
        private void dgMachineList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
