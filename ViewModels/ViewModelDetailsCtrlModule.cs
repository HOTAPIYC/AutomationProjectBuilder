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
            get => _cmdAddFunction;
        }

        public ICommand CmdDeleteFunction
        {
            get => _cmdDeleteFunction;
        }

        public ICommand CmdEdit
        {
            get => _cmdEdit;
        }

        public ParameterGroup SelectedParameterGroup
        {
            get => Module.ParamGroup;
            set
            {
                Module.ParamGroup = value;
                NotifyPropertChanged("SelectedParameterGroup");
            }
        }

        public ParameterSet SelectedParameterSet
        {
            get => Module.ParamSet;
            set
            {
                Module.ParamSet = value;
                NotifyPropertChanged("SelectedParameterSet");
            }
        }

        public ObservableCollection<ViewModelListItem> ParameterList { get; set; } = new ObservableCollection<ViewModelListItem>();
        public ObservableCollection<ViewModelListItem> FunctionsList { get; set; } = new ObservableCollection<ViewModelListItem>();

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
           
            foreach(ModuleParameter parameter in dataService.GetParameters(ModuleId))
            {
                ParameterList.Add(new ViewModelListItem(parameter));
            }

            foreach(ModuleFunction function in dataService.GetFunctions(module.Id))
            {
                FunctionsList.Add(new ViewModelListItem(function));
            }
        }
        private void LoadParameterSet() 
        {
            var dialog = new ViewModelDialogConfig(_dataService);

            var result = _dialogService.ShowDialog(dialog);
            
            if(result.Value && dialog.SelectedParameterGroup != null && dialog.SelectedParameterSet != null)
            {
                SelectedParameterGroup = dialog.SelectedParameterGroup;
                SelectedParameterSet = dialog.SelectedParameterSet;

                ParameterList.Clear();

                var newParameterSet = SelectedParameterSet.Parameters;

                foreach (ModuleParameter parameter in newParameterSet)
                {
                    parameter.ModuleId = ModuleId;
                    
                    ParameterList.Add(new ViewModelListItem(parameter));
                }

                _dataService.SetParameters(ModuleId, newParameterSet);
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

                FunctionsList.Add(new ViewModelListItem(moduleFunction));
            }
        }

        private void DeleteFunction()
        {

        }
    }
}
