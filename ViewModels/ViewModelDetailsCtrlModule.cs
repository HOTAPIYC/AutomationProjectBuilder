using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDetailsCtrlModule : ViewModelDetailsBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdAddFunction;
        private ICommand _cmdDeleteFunction;
        private ICommand _cmdEdit;
        
        public ProjectModule Module { get; set; }
        
        public ICommand CmdAddFunction
        {
            get { return _cmdAddFunction; }
        }

        public ICommand CmdDeleteFunction
        {
            get { return _cmdDeleteFunction; }
        }

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
        public ObservableCollection<ModuleFunction> ModuleFunctions { get; set; } = new ObservableCollection<ModuleFunction>();

        public ViewModelDetailsCtrlModule(ProjectModule module, IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            Module = module;

            ModuleId = module.Id;
            ModuleType = ModuleType.CtrlModule;

            _cmdAddFunction = new DelegateCommand(x => AddFunction());
            _cmdDeleteFunction = new DelegateCommand(x => DeleteFunction());
            _cmdEdit = new DelegateCommand(x => LoadParameterSet());
           
            Parameters = new ObservableCollection<ModuleParameter>(_dataService.GetParameters(ModuleId));
            ModuleFunctions = dataService.GetFunctions(module.Id);
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

        private void AddFunction()
        {
            var dialog = new ViewModelDialogTextInput("");

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                var moduleFunction = new ModuleFunction(ModuleId, dialog.TextInput);

                _dataService.AddFunction(moduleFunction);

                ModuleFunctions.Add(moduleFunction);
            }
        }

        private void DeleteFunction()
        {

        }
    }
}
