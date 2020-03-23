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

        private string _itemNameInput;
        private ItemTypeISA88 _itemTypeSelection;

        public ICommand CmdSave
        {
            get { return _cmdSave; }
        }

        public ICommand CmdCancel
        {
            get { return _cmdCancel; }
        }

        public string ItemNameInput
        {
            get
            {
                return _itemNameInput;
            }
            set
            {
                _itemNameInput = value;
                NotifyPropertChanged("ItemName");
            }
        }

        public ItemTypeISA88 ItemTypeSelection
        {
            get
            {
                return _itemTypeSelection;
            }
            set
            {
                _itemTypeSelection = value;
                NotifyPropertChanged("ItemTypeSelection");
            }
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogTreeItem(ProjectItem item)
        {
            ItemNameInput = item.Name;
            ItemTypeSelection = item.Type;
            
            _cmdSave = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
        }
    }
}
