using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDetailsComplexCtrlModule : ViewModelDetailsBase
    {
        private ICommand _cmdAddFunction;
        private ICommand _cmdDeleteFunction;
        private IDialogService _dialogService;

        public ICommand CmdAddFunction
        {
            get { return _cmdAddFunction; }
        }

        public ICommand CmdDeleteFunction
        {
            get { return _cmdDeleteFunction; }
        }

        public ObservableCollection<ModuleFunction> ModuleFunctions { get; set; } = new ObservableCollection<ModuleFunction>();
                       
        public ViewModelDetailsComplexCtrlModule(IDialogService dialogService)
        {
            ViewItemType = ItemTypeISA88.ComplexCtrlModule;

            ModuleFunctions.Add(new ModuleFunction("Function 1"));
            ModuleFunctions.Add(new ModuleFunction("Function 2"));

            _cmdAddFunction = new DelegateCommand(x => AddFunction());
            _cmdDeleteFunction = new DelegateCommand(x => DeleteFunction());

            _dialogService = dialogService;
        }

        private void AddFunction()
        {
            var dialog = new ViewModelDialogTextInput("");

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                var moduleFunction = new ModuleFunction(dialog.TextInput);
                ModuleFunctions.Add(moduleFunction);
            }
        }

        private void DeleteFunction()
        {

        }
    }
}
