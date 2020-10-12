using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using System.ComponentModel;


namespace MasterSchedule.ViewModels
{
    public class MachineViewModel : INotifyPropertyChanged
    {
        public int MachineID { get; set; }

        private string _MachineName;
        public string MachineName
        {
            get { return _MachineName; }
            set
            {
                _MachineName = value;
                OnPropertyChanged("MachineName");
            }
        }

        //private int _PhaseID;
        //public int PhaseID
        //{
        //    get { return _PhaseID; }
        //    set
        //    {
        //        _PhaseID = value;
        //        OnPropertyChanged("PhaseID");
        //    }
        //}

        //private string _PhaseName;
        //public string PhaseName
        //{
        //    get { return _PhaseName; }
        //    set
        //    {
        //        _PhaseName = value;
        //        OnPropertyChanged("PhaseName");
        //    }
        //}

        private PhaseModel _PhaseSelected;
        public PhaseModel PhaseSelected
        {
            get { return _PhaseSelected; }
            set
            {
                _PhaseSelected = value;
                OnPropertyChanged("PhaseSelected");
            }
        }

        private bool _IsMachine;
        public bool IsMachine
        {
            get { return _IsMachine; }
            set
            {
                _IsMachine = value;
                OnPropertyChanged("IsMachine");
            }
        }

        private string _IsMachineView;
        public string IsMachineView
        {
            get { return _IsMachineView; }
            set
            {
                _IsMachineView = value;
                OnPropertyChanged("IsMachineView");
            }
        }

        private int _Available;
        public int Available
        {
            get { return _Available; }
            set
            {
                _Available = value;
                OnPropertyChanged("Available");
            }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
