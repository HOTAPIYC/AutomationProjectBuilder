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
        private IDataService _dataService;

        private ViewModelDetailsBase _detailsPage;
        private ICommand _cmdSelectedItem;
        private ICommand _cmdSaveFile;
        private ICommand _cmdOpenFile;
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

        public ICommand CmdSaveFile
        {
            get { return _cmdSaveFile; }
        }

        public ICommand CmdOpenFile
        {
            get { return _cmdOpenFile; }
        }

        public ObservableCollection<ViewModelTreeItem> ProjectStructure { get; } = new ObservableCollection<ViewModelTreeItem>();

        public ViewModelMain(IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            _cmdSelectedItem = new DelegateCommand(x => GetSelectedItem());
            _cmdSaveFile = new DelegateCommand(x => SaveData());
            _cmdOpenFile = new DelegateCommand(x => LoadData());

            DetailsPage = new ViewModelDetailsBlank();

            LoadLatestConfig();
        }

        private void LoadLatestConfig()
        {
            var projectRoot = _dataService.GetProjectRoot();

            ProjectStructure.Add(new ViewModelTreeItem(projectRoot, _dialogService,_dataService));
        }

        private void GetSelectedItem()
        {
            _selectedItem = ProjectStructure[0].GetSelectedViewModel(new ViewModelTreeItem());

            _selectedItem.PropertyChanged += HandleListChangeEvent;

            LoadSelectedItemPage();
        }

        private void HandleListChangeEvent(object sender, EventArgs e)
        {
            if(DetailsPage.ViewItemType != _selectedItem.ItemType || DetailsPage.ViewItemId != _selectedItem.ItemId)
            {
                LoadSelectedItemPage();
            }
        }

        private void LoadSelectedItemPage()
        {
            switch (_selectedItem.ItemType)
            {
                case ItemTypeISA88.ComplexCtrlModule:
                    DetailsPage = new ViewModelDetailsComplexCtrlModule(_selectedItem.ItemId,_dialogService, _dataService);
                    break;
                default:
                    DetailsPage = new ViewModelDetailsBlank();
                    DetailsPage.ViewItemType = _selectedItem.ItemType;
                    break;
            }
        }

        private void LoadData()
        {
            var settings = new FileDialogSettings();

            var result = _dialogService.ShowOpenFileDialog(settings);
        }

        private void SaveData()
        {
            var settings = new FileDialogSettings();

            var result = _dialogService.ShowSaveFileDialog(settings);
        }
    }
}
