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
        
        public ICommand CmdAddFunction
        {
            get { return _cmdAddFunction; }
        }

        public ICommand CmdDeleteFunction
        {
            get { return _cmdDeleteFunction; }
        }

        public ObservableCollection<ModuleFunction> ModuleFunctions { get; set; } = new ObservableCollection<ModuleFunction>();
                       
        public ViewModelDetailsComplexCtrlModule()
        {
            ViewItemType = ItemTypeISA88.ComplexCtrlModule;

            ModuleFunctions.Add(new ModuleFunction("Function 1"));
            ModuleFunctions.Add(new ModuleFunction("Function 2"));

            _cmdAddFunction = new DelegateCommand(x => AddFunction());
            _cmdDeleteFunction = new DelegateCommand(x => DeleteFunction());
        }

        private void AddFunction()
        {

        }

        private void DeleteFunction()
        {

        }
    }
}
