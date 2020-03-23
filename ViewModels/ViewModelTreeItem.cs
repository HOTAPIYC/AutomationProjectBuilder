using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using AutomationProjectBuilder.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelTreeItem : ViewModelBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdAddSubsystem;
        private ICommand _cmdDeleteSubsystem;
        private ICommand _cmdEditSubsystem;

        private ProjectItem _item;
      
        public ViewModelTreeItem Parent { get; set; }
        public bool IsSelected { get; set; }

        public ObservableCollection<ViewModelTreeItem> SubViewModels { get; } = new ObservableCollection<ViewModelTreeItem>();

        public string ItemName
        {
            get
            {
                return _item.Name;
            }
            set
            {
                _item.Name = value;
                NotifyPropertChanged("ItemName");
            }
        }

        public ItemTypeISA88 ItemType
        {
            get
            {
                return _item.Type;
            }
            set
            {
                _item.Type = value;
                NotifyPropertChanged("ItemType");
            }
        }

        public Guid ItemId
        {
            get
            {
                return _item.Id;
            }
        }

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

        public ViewModelTreeItem()
        {
            //
        }

        public ViewModelTreeItem(ProjectItem item, IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            _item = item;

            foreach (ProjectItem projectItem in item.SubItems)
            {
                SubViewModels.Add(new ViewModelTreeItem(projectItem, dialogService, dataService));
            }           

            _cmdAddSubsystem = new DelegateCommand(x => CreateSubViewModel());
            _cmdDeleteSubsystem = new DelegateCommand(x => DeleteSubViewModel());
            _cmdEditSubsystem = new DelegateCommand(x => EditSubViewModel());

            PropertyChanged += UpdateItem;
            SubViewModels.CollectionChanged += UpdateItem;
        }

        public void CreateSubViewModel()
        {
            var dialog = new ViewModelDialogTreeItem(new ProjectItem());

            var result = _dialogService.ShowDialog(dialog);         

            if(result.Value)
            {
                var newItem = new ProjectItem(dialog.ItemNameInput, dialog.ItemTypeSelection);

                _item.SubItems.Add(newItem);

                AddSubViewModel(new ViewModelTreeItem(newItem, _dialogService, _dataService));
            }       
        }

        public void AddSubViewModel(ViewModelTreeItem subsystem)
        {
            subsystem.Parent = this;

            SubViewModels.Add(subsystem);
        }

        public void DeleteSubViewModel()
        {
            Parent.SubViewModels.Remove(this);
        }

        public void EditSubViewModel()
        {
            var dialog = new ViewModelDialogTreeItem(_item);

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                ItemName = dialog.ItemNameInput;
                ItemType = dialog.ItemTypeSelection;
            }
        }

        public ViewModelTreeItem GetSelectedViewModel(ViewModelTreeItem viewModel)
        {          
            if(IsSelected)
            {
                viewModel = this;
            }
            else
            {
                foreach(ViewModelTreeItem item in SubViewModels)
                {
                    viewModel = item.GetSelectedViewModel(viewModel);
                }
            }
            return viewModel;
        }

        private void UpdateItem(object sender, EventArgs e)
        {         
            _dataService.UpdateProjectItem(_item);
        }
    }
}
