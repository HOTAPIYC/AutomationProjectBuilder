﻿using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDetailsEqModule : ViewModelDetailsBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdAddFunction;

        public ICommand CmdAddFunction
        {
            get => _cmdAddFunction;
        }

        public ObservableCollection<ViewModelListItem> FunctionList { get; set; } = new ObservableCollection<ViewModelListItem>();

        public ViewModelDetailsEqModule(ProjectModule module, IDialogService dialogService,IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            ModuleId = module.Id;
            ModuleType = ModuleType.EquipmentModule;
            
            foreach(ModuleFunction function in dataService.GetFunctions(module.Id))
            {
                FunctionList.Add(new ViewModelListItem(FunctionList, function, dataService));
            }

            _cmdAddFunction = new DelegateCommand(x => AddFunction());
        }

        private void AddFunction()
        {
            var dialog = new ViewModelDialogTextInput("");

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                var moduleFunction = new ModuleFunction(ModuleId, dialog.TextInput);

                _dataService.AddFunction(moduleFunction);

                FunctionList.Add(new ViewModelListItem(FunctionList, moduleFunction, _dataService));
            }
        }
    }
}