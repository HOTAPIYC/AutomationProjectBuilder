using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDialogTreeItem : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdSave;
        private ICommand _cmdCancel;

        private string _itemName;
        private ItemTypeISA88 _itemTypeSelection;

        public ICommand CmdSave
        {
            get { return _cmdSave; }
        }

        public ICommand CmdCancel
        {
            get { return _cmdCancel; }
        }

        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                _itemName = value;
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

        public ViewModelDialogTreeItem(ViewModelTreeItem item)
        {
            ItemName = item.ItemName;
            ItemTypeSelection = item.ItemType;
            
            _cmdSave = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
        }
    }
}
