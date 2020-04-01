using AutomationProjectBuilder.Gui.Dialogs;
using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.ObjectModel;
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

        public ProjectModule Module { get; set; }
      
        public ViewModelTreeItem Parent { get; set; }
        public bool IsSelected { get; set; }

        public ObservableCollection<ViewModelTreeItem> SubViewModels { get; } = new ObservableCollection<ViewModelTreeItem>();

        public string ModuleName
        {
            get => Module.Name;
            set
            {
                Module.Name = value;
                NotifyPropertChanged("ModuleName");
            }
        }

        public ModuleType ModuleType
        {
            get => Module.Type;
            set
            {
                Module.Type = value;
                NotifyPropertChanged("ModuleType");
            }
        }

        public Guid ModuleId { get => Module.Id; }

        public ICommand CmdAddSubsystem { get => _cmdAddSubsystem; }

        public ICommand CmdDeleteSubsystem  { get => _cmdDeleteSubsystem; }

        public ICommand CmdEditSubsystem { get => _cmdEditSubsystem; }

        public ViewModelTreeItem()
        {
            //
        }

        public ViewModelTreeItem(ProjectModule module, IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            Module = module;

            foreach (ProjectModule projectItem in module.SubModules)
            {
                AddSubViewModel(new ViewModelTreeItem(projectItem, dialogService, dataService));
            }           

            _cmdAddSubsystem = new DelegateCommand(x => CreateSubViewModel());
            _cmdDeleteSubsystem = new DelegateCommand(x => DeleteSubViewModel());
            _cmdEditSubsystem = new DelegateCommand(x => EditSubViewModel());
        }

        private void CreateSubViewModel()
        {
            var dialog = new ViewModelDialogTreeItem(new ProjectModule());

            var result = _dialogService.ShowDialog(dialog);         

            if(result.Value)
            {
                var newItem = new ProjectModule(dialog.ModuleNameInput, dialog.ModuleTypeSelection);

                Module.SubModules.Add(newItem);

                AddSubViewModel(new ViewModelTreeItem(newItem, _dialogService, _dataService));
            }       
        }

        private void AddSubViewModel(ViewModelTreeItem subsystem)
        {
            subsystem.Parent = this;

            SubViewModels.Add(subsystem);
        }

        private void DeleteSubViewModel()
        {
            Parent.SubViewModels.Remove(this);
        }

        public void EditSubViewModel()
        {
            var dialog = new ViewModelDialogTreeItem(Module);

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                ModuleName = dialog.ModuleNameInput;
                ModuleType = dialog.ModuleTypeSelection;
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
    }
}
