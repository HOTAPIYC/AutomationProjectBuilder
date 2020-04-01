using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDialogTreeItem : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdSave;
        private ICommand _cmdCancel;

        private string _moduleNameInput;
        private ModuleType _moduleTypeSelection;

        public ICommand CmdSave
        {
            get { return _cmdSave; }
        }

        public ICommand CmdCancel
        {
            get { return _cmdCancel; }
        }

        public string ModuleNameInput
        {
            get
            {
                return _moduleNameInput;
            }
            set
            {
                _moduleNameInput = value;
                NotifyPropertChanged("ModuleName");
            }
        }

        public ModuleType ModuleTypeSelection
        {
            get
            {
                return _moduleTypeSelection;
            }
            set
            {
                _moduleTypeSelection = value;
                NotifyPropertChanged("ModuleTypeSelection");
            }
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogTreeItem(ProjectModule item)
        {
            ModuleNameInput = item.Name;
            ModuleTypeSelection = item.Type;
            
            _cmdSave = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
        }
    }
}
