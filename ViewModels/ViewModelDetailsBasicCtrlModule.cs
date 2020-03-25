using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDetailsBasicCtrlModule : ViewModelDetailsBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdEdit;

        private ConfigGroup _selectedConfigGroup;
        private ModuleConfig _selectedModuleConfig;

        public ICommand CmdEdit
        {
            get { return _cmdEdit; }
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

        public ObservableCollection<ConfigValue> Parameters { get; set; } = new ObservableCollection<ConfigValue>();

        public ViewModelDetailsBasicCtrlModule(Guid moduleId, IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            ViewModuleId = moduleId;
            ViewModuleType = ModuleType.ComplexCtrlModule;

            _cmdEdit = new DelegateCommand(x => LoadConfigParam());

            Parameters = new ObservableCollection<ConfigValue>(_dataService.GetCustomParameters(ViewModuleId));
        }

        private void LoadConfigParam() 
        {
            var dialog = new ViewModelDialogConfig(_dataService);

            var result = _dialogService.ShowDialog(dialog);
            
            if(result.Value && dialog.SelectedConfigGroup != null && dialog.SelectedModuleConfig != null)
            {
                SelectedConfigGroup = dialog.SelectedConfigGroup;
                SelectedModuleConfig = dialog.SelectedModuleConfig;

                Parameters.Clear();

                foreach(ConfigValue param in SelectedModuleConfig.Parameters)
                {
                    param.ModuleId = ViewModuleId;
                    
                    Parameters.Add(param);
                }

                _dataService.SetCustomParameters(ViewModuleId, new List<ConfigValue>(Parameters));
            }
        }
    }
}
