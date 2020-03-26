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

        public ProjectModule Module;

        public ICommand CmdEdit
        {
            get { return _cmdEdit; }
        }

        public ParameterGroup SelectedParameterGroup
        {
            get
            {
                return Module.ParamGroup;
            }
            set
            {
                Module.ParamGroup = value;
                NotifyPropertChanged("SelectedParameterGroup");
            }
        }

        public ParameterSet SelectedParameterSet
        {
            get
            {
                return Module.ParamSet;
            }
            set
            {
                Module.ParamSet = value;
                NotifyPropertChanged("SelectedParameterSet");
            }
        }

        public ObservableCollection<ModuleParameter> Parameters { get; set; } = new ObservableCollection<ModuleParameter>();

        public ViewModelDetailsBasicCtrlModule(ProjectModule module, IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            Module = module;

            ModuleId = module.Id;
            ModuleType = ModuleType.ComplexCtrlModule;

            _cmdEdit = new DelegateCommand(x => LoadParameterSet());

            Parameters = new ObservableCollection<ModuleParameter>(_dataService.GetParameters(ModuleId));
        }

        private void LoadParameterSet() 
        {
            var dialog = new ViewModelDialogConfig(_dataService);

            var result = _dialogService.ShowDialog(dialog);
            
            if(result.Value && dialog.SelectedParameterGroup != null && dialog.SelectedParameterSet != null)
            {
                SelectedParameterGroup = dialog.SelectedParameterGroup;
                SelectedParameterSet = dialog.SelectedParameterSet;

                Parameters.Clear();

                foreach(ModuleParameter param in SelectedParameterSet.Parameters)
                {
                    param.ModuleId = ModuleId;
                    
                    Parameters.Add(param);
                }

                _dataService.SetParameters(ModuleId, new List<ModuleParameter>(Parameters));
            }
        }
    }
}
