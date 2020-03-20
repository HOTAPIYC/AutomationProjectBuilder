using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using AutomationProjectBuilder.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class TreeItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TreeItemViewModel Parent { get; set; }
        public ComponentType ItemType { get; set; }
        public bool IsSelected { get; set; }
        public ObservableCollection<TreeItemViewModel> Subsystems { get; } = new ObservableCollection<TreeItemViewModel>();

        private ICommand _cmdAddSubsystem;
        private ICommand _cmdDeleteSubsystem;

        private IDialogService _dialogService;

        public ICommand CmdAddSubsystem
        {
            get { return _cmdAddSubsystem; }
        }
        public ICommand CmdDeleteSubsystem
        {
            get { return _cmdDeleteSubsystem; }
        }

        public TreeItemViewModel()
        {
            // empty constructor
        }

        public TreeItemViewModel(string name, IDialogService dialogService)
        {
            Id = Guid.NewGuid();
            Name = name;

            _dialogService = dialogService;

            _cmdAddSubsystem = new DelegateCommand(x => CreateSubsystem());
            _cmdDeleteSubsystem = new DelegateCommand(x => DeleteSubsystem());
        }

        public void CreateSubsystem()
        {
            var dialog = new DialogAddSubsystemViewModel();

            _dialogService.ShowDialog(dialog);         

            if(dialog.TextInput != "")
            {
                var subsystem = new TreeItemViewModel(dialog.TextInput, _dialogService);
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
