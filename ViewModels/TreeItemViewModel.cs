using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using AutomationProjectBuilder.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class TreeItemViewModel : ViewModelBase
    {
        private string _itemName;
        private ItemTypeISA88 _itemType;

        public Guid ItemId { get; set; }
        public TreeItemViewModel Parent { get; set; }
        public bool IsSelected { get; set; }
        public ObservableCollection<TreeItemViewModel> Subsystems { get; } = new ObservableCollection<TreeItemViewModel>();

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

        public ItemTypeISA88 ItemType
        {
            get
            {
                return _itemType;
            }
            set
            {
                _itemType = value;
                NotifyPropertChanged("ItemType");
            }
        }

        private ICommand _cmdAddSubsystem;
        private ICommand _cmdDeleteSubsystem;
        private ICommand _cmdEditSubsystem;

        private IDialogService _dialogService;

        public ICommand CmdAddSubsystem
        {
            get { return _cmdAddSubsystem; }
        }

        public ICommand CmdDeleteSubsystem
        {
            get { return _cmdDeleteSubsystem; }
        }

        public ICommand CmdEditSubsystem
        {
            get { return _cmdEditSubsystem; }
        }

        public TreeItemViewModel()
        {
            // empty constructor
        }

        public TreeItemViewModel(string name, ItemTypeISA88 itemType, IDialogService dialogService)
        {
            ItemId = Guid.NewGuid();
            ItemName = name;
            ItemType = itemType;

            _dialogService = dialogService;

            _cmdAddSubsystem = new DelegateCommand(x => CreateSubsystem());
            _cmdDeleteSubsystem = new DelegateCommand(x => DeleteSubsystem());
            _cmdEditSubsystem = new DelegateCommand(x => EditSubsystem());
        }

        public void CreateSubsystem()
        {
            var dialog = new DialogTreeItemViewModel(new TreeItemViewModel());

            var result = _dialogService.ShowDialog(dialog);         

            if(result.Value)
            {
                var subsystem = new TreeItemViewModel(
                    dialog.ItemName, 
                    dialog.ItemTypeSelection,
                    _dialogService);
                AddSubsystem(subsystem);
            }       
        }

        public void AddSubsystem(TreeItemViewModel subsystem)
        {
            subsystem.Parent = this;
            Subsystems.Add(subsystem);
        }

        public void DeleteSubsystem()
        {
            Parent.Subsystems.Remove(this);
        }

        public void EditSubsystem()
        {
            var dialog = new DialogTreeItemViewModel(this);

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                ItemName = dialog.ItemName;
                ItemType = dialog.ItemTypeSelection;
            }
        }

        public TreeItemViewModel GetSelectedItem(TreeItemViewModel treeitem)
        {          
            if(IsSelected)
            {
                treeitem = this;
            }
            else
            {
                foreach(TreeItemViewModel item in Subsystems)
                {
                    treeitem = item.GetSelectedItem(treeitem);
                }
            }
            return treeitem;
        }
    }
}
