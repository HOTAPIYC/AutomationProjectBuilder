using AutomationProjectBuilder.Data.Model;
using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDialogTreeItem : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdSave;
        private ICommand _cmdCancel;

        private string _moduleNameInput;
        private ModuleType _moduleTypeSelection;

        public ICommand CmdSave
        {
            get => _cmdSave;
        }

        public ICommand CmdCancel
        {
            get => _cmdCancel;
        }

        public string ModuleNameInput
        {
            get => _moduleNameInput;
            set { _moduleNameInput = value; NotifyPropertChanged("ModuleName"); }
        }

        public ModuleType ModuleTypeSelection
        {
            get => _moduleTypeSelection;
            set { _moduleTypeSelection = value; NotifyPropertChanged("ModuleTypeSelection"); }
        }

        public bool? DialogResult { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogTreeItem(ProjectModule item)
        {
            ModuleNameInput = item.Name;
            ModuleTypeSelection = item.Type;
            
            _cmdSave = new DelegateCommand(x => 
            { 
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)); 
            });
            _cmdCancel = new DelegateCommand(x => 
            { 
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)); 
            });
        }
    }
}
