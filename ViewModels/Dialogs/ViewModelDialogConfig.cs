using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDialogConfig : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdApply;
        private ICommand _cmdCancel;

        private ConfigGroup _selectedConfigGroup;
        private ModuleConfig _selectedModuleConfig;

        public ICommand CmdApply
        {
            get { return _cmdApply; }
        }

        public ICommand CmdCancel
        {
            get { return _cmdCancel; }
        }

        public ConfigGroup SelectedConfigGroup
        {
            get
            {
                return _selectedConfigGroup;
            }
            set
            {
                _selectedConfigGroup = value;
                NotifyPropertChanged("SelectedConfigGroup");
            }
        }

        public ModuleConfig SelectedModuleConfig
        {
            get
            {
                return _selectedModuleConfig;
            }
            set
            {
                _selectedModuleConfig = value;
                NotifyPropertChanged("SelectedModuleConfig");
            }
        }

        public ObservableCollection<ConfigGroup> ConfigGroups { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogConfig(IDataService dataService)
        {
            _cmdApply = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

            ConfigGroups = new ObservableCollection<ConfigGroup>(dataService.GetLoadedConfigs());
        }
    }
}
