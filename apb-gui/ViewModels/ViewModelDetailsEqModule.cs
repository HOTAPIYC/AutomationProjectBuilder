using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDetailsEqModule : ViewModelDetailsBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdAddFunction;

        private ProjectModule _module;

        public ICommand CmdAddFunction { get => _cmdAddFunction; }

        public ObservableCollection<ViewModelListItem> FunctionsList { get; set; } = new ObservableCollection<ViewModelListItem>();

        public ViewModelDetailsEqModule(ProjectModule module, IDialogService dialogService,IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            _module = module;

            ModuleId = module.Id;
            ModuleType = ModuleType.EquipmentModule;
            
            foreach(ModuleFunction function in module.Functions)
            {
                FunctionsList.Add(new ViewModelListItem(function, this.FunctionsList, module.Functions));
            }

            _cmdAddFunction = new DelegateCommand(x => AddFunction());
        }

        private void AddFunction()
        {
            var function = new ModuleFunction(ModuleId, "New function");

            _module.Functions.Add(function);

            FunctionsList.Add(new ViewModelListItem(function, FunctionsList, _module.Functions));
        }
    }
}
