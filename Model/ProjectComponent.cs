using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.Model
{
    public enum ComponentType
    {
        Function,
        FunctionBlock,
        BasicCtrlM,
        ComplexCtrlM,
        EqM,
        ProcessCell,
        RecipePhase
    }

    public class ProjectComponent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProjectComponent Parent { get; set; }
        public bool IsSelected { get; set; }
        public ObservableCollection<ProjectComponent> Subsystems { get; } = new ObservableCollection<ProjectComponent>();

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

        public ProjectComponent()
        {
            // empty constructor
        }

        public ProjectComponent(string name, IDialogService dialogService)
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

            var result = _dialogService.ShowDialog(dialog);         

            if(dialog.TextInput != "")
            {
                var subsystem = new ProjectComponent(dialog.TextInput, _dialogService);
                AddSubsystem(subsystem);
            }       
        }

        public void AddSubsystem(ProjectComponent subsystem)
        {
            subsystem.Parent = this;
            Subsystems.Add(subsystem);
        }

        public void DeleteSubsystem()
        {
            Parent.Subsystems.Remove(this);
        }

        public ProjectComponent GetSelectedItem(ProjectComponent treeitem)
        {          
            if(IsSelected)
            {
                treeitem = this;
            }
            else
            {
                foreach(ProjectComponent item in Subsystems)
                {
                    treeitem = item.GetSelectedItem(treeitem);
                }
            }
            return treeitem;
        }
    }
}
