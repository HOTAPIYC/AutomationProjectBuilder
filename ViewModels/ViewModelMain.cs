using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelMain : ViewModelBase
    {
        private IDialogService _dialogService;
        private ViewModelDetailsBase _detailsPage;
        private ICommand _cmdSelectedItem;
        private ViewModelTreeItem _selectedItem;

        public ViewModelDetailsBase DetailsPage
        {
            get
            {
                return _detailsPage;
            }
            set
            {
                _detailsPage = value;
                NotifyPropertChanged("DetailsPage");
            }
        }
        public ICommand CmdSelectedItem
        {
            get { return _cmdSelectedItem; }
        }

        public ObservableCollection<ViewModelTreeItem> ProjectStructure { get; } = new ObservableCollection<ViewModelTreeItem>();

        public ViewModelMain(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _cmdSelectedItem = new DelegateCommand(x => GetSelectedItem(x));

            DetailsPage = new ViewModelDetails("No item selected");

            LoadLatestConfig();
        }

        private void LoadLatestConfig()
        {
            var root = new ViewModelTreeItem("Project", ItemTypeISA88.ProcessCell, _dialogService);
            
            var subsys1 = new ViewModelTreeItem("Subsystem 1", ItemTypeISA88.EquipmentModule, _dialogService);
            var subsys2 = new ViewModelTreeItem("Subsystem 2", ItemTypeISA88.EquipmentModule, _dialogService);
            var subsys3 = new ViewModelTreeItem("Subsystem 3", ItemTypeISA88.EquipmentModule, _dialogService);

            subsys2.AddSubsystem(subsys3);

            root.AddSubsystem(subsys1);
            root.AddSubsystem(subsys2);

            ProjectStructure.Add(root);
        }

        private void GetSelectedItem(object x)
        {
            _selectedItem = ProjectStructure[0].GetSelectedItem(new ViewModelTreeItem());

            _selectedItem.PropertyChanged += HandleListChangeEvent;

            DetailsPage = new ViewModelDetails(_selectedItem.ItemName);
        }

        private void HandleListChangeEvent(object sender, EventArgs e)
        {
            if(DetailsPage.Name != _selectedItem.ItemName)
            {
                DetailsPage = new ViewModelDetails(_selectedItem.ItemName);
            }
        }
    }
}
