using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Data.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDialogConfig : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdApply;
        private ICommand _cmdCancel;

        private ParameterGroup _selectedParameterGroup;
        private ParameterSet _selectedParameterSet;

        public ICommand CmdApply
        {
            get { return _cmdApply; }
        }

        public ICommand CmdCancel
        {
            get { return _cmdCancel; }
        }

        public ParameterGroup SelectedParameterGroup
        {
            get
            {
                return _selectedParameterGroup;
            }
            set
            {
                _selectedParameterGroup = value;
                NotifyPropertChanged("SelectedParameterGroup");
            }
        }

        public ParameterSet SelectedParameterSet
        {
            get
            {
                return _selectedParameterSet;
            }
            set
            {
                _selectedParameterSet = value;
                NotifyPropertChanged("SelectedParameterSet");
            }
        }

        public ObservableCollection<ParameterGroup> ParameterGroups { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogConfig(IDataService dataService)
        {
            _cmdApply = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

            ParameterGroups = new ObservableCollection<ParameterGroup>(dataService.GetParameterGroups());
        }
    }
}
