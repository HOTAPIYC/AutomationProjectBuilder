using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.ObjectModel;
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
        private ICommand _cmdSaveAsFile;
        private ICommand _cmdNewFile;
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

        public ICommand CmdSaveAsFile
        {
            get { return _cmdSaveAsFile; }
        }

        public ICommand CmdNewFile
        {
            get { return _cmdNewFile; }
        }

        public ObservableCollection<ViewModelTreeItem> ProjectStructure { get; } = new ObservableCollection<ViewModelTreeItem>();

        public ViewModelMain(IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            _cmdSelectedItem = new DelegateCommand(x => GetSelectedItem());
            _cmdSaveFile = new DelegateCommand(x => _dataService.Save());
            _cmdSaveAsFile = new DelegateCommand(x => _dataService.SaveAs());
            _cmdOpenFile = new DelegateCommand(x => OpenFile());
            _cmdNewFile = new DelegateCommand(x => New());

            DetailsPage = new ViewModelDetailsBlank();

            _dataService.Load();

            GetProjectStructure();
        }

        private void GetSelectedItem()
        {
            if (ProjectStructure.Count > 0)
            {
                _selectedItem = ProjectStructure[0].GetSelectedViewModel(new ViewModelTreeItem());

                _selectedItem.PropertyChanged += HandleListChangeEvent;

                LoadSelectedModulePage();
            }
        }

        private void HandleListChangeEvent(object sender, EventArgs e)
        {
            if(DetailsPage.ViewModuleType != _selectedItem.ModuleType || DetailsPage.ViewModuleId != _selectedItem.ModuleId)
            {
                LoadSelectedModulePage();
            }
        }

        private void LoadSelectedModulePage()
        {
            switch (_selectedItem.ModuleType)
            {
                case ModuleType.ComplexCtrlModule:
                    DetailsPage = new ViewModelDetailsComplexCtrlModule(
                        _selectedItem.ModuleId, 
                        _dialogService, 
                        _dataService);
                    break;
                case ModuleType.BasicCtrlModule:
                    DetailsPage = new ViewModelDetailsBasicCtrlModule(
                        _selectedItem.ModuleId,
                        _dialogService,
                        _dataService);
                    break;
                default:
                    DetailsPage = new ViewModelDetailsBlank();
                    DetailsPage.ViewModuleType = _selectedItem.ModuleType;
                    break;
            }
        }

        private void OpenFile()
        {
            _dataService.Open();

            GetProjectStructure();         
        }

        private void GetProjectStructure()
        {        
            ProjectStructure.Clear();

            ProjectStructure.Add(
                new ViewModelTreeItem(
                    _dataService.GetProjectRoot(), 
                    _dialogService, 
                    _dataService));
        }

        private void New()
        {
            ProjectStructure.Clear();

            ProjectStructure.Add(
                new ViewModelTreeItem(
                    _dataService.ResetProjectRoot(),
                    _dialogService,
                    _dataService));
        }
    }
}
