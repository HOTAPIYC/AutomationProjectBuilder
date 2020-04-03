using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Data.Model;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelMain : ViewModelBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ViewModelDetailsBase _detailsPage;
        private ViewModelTreeItem _selectedItem;

        private ICommand _cmdSelectedItem;
        private ICommand _cmdSaveFile;
        private ICommand _cmdOpenFile;
        private ICommand _cmdSaveAsFile;
        private ICommand _cmdNewFile;
        private ICommand _cmdExport;
        private ICommand _cmdSettings;

        public ViewModelDetailsBase DetailsPage
        {
            get => _detailsPage;
            set
            {
                _detailsPage = value;
                NotifyPropertChanged("DetailsPage");
            }
        }
        
        public ICommand CmdSelectedItem { get => _cmdSelectedItem; }
        public ICommand CmdSaveFile { get => _cmdSaveFile; }       
        public ICommand CmdOpenFile { get => _cmdOpenFile; }
        public ICommand CmdSaveAsFile { get => _cmdSaveAsFile; }
        public ICommand CmdNewFile { get => _cmdNewFile; }
        public ICommand CmdExport { get => _cmdExport; }
        public ICommand CmdSettings { get => _cmdSettings; }

        public ObservableCollection<ViewModelTreeItem> ProjectStructure { get; } = new ObservableCollection<ViewModelTreeItem>();

        public ViewModelMain(IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            _cmdSelectedItem = new DelegateCommand(x => GetSelectedItem());
            _cmdSaveFile = new DelegateCommand(x => SaveFile());
            _cmdSaveAsFile = new DelegateCommand(x => SaveFileAs());
            _cmdOpenFile = new DelegateCommand(x => OpenFile());
            _cmdNewFile = new DelegateCommand(x => New());
            _cmdExport = new DelegateCommand(x => Export());
            _cmdSettings = new DelegateCommand(x => ShowAppSettings());

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
            if(DetailsPage.ModuleType != _selectedItem.ModuleType || DetailsPage.ModuleId != _selectedItem.ModuleId)
            {
                LoadSelectedModulePage();
            }
        }

        private void LoadSelectedModulePage()
        {
            switch (_selectedItem.ModuleType)
            {
                case ModuleType.EquipmentModule:
                    DetailsPage = new ViewModelDetailsEqModule(
                        _selectedItem.Module, 
                        _dialogService, 
                        _dataService);
                    break;
                case ModuleType.CtrlModule:
                    DetailsPage = new ViewModelDetailsCtrlModule(
                        _selectedItem.Module,
                        _dialogService,
                        _dataService);
                    break;
                default:
                    DetailsPage = new ViewModelDetailsBlank();
                    DetailsPage.ModuleType = _selectedItem.ModuleType;
                    break;
            }
        }

        private void OpenFile()
        {
            var dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                _dataService.Open(dialog.FileName);

                GetProjectStructure();
            }                                          
        }

        private void SaveFile()
        {
            if (!_dataService.Save())
            {
                SaveFileAs();
            }                  
        }

        private void SaveFileAs()
        {
            var dialog = new SaveFileDialog();

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                _dataService.SaveAs(dialog.FileName);
            }
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

        private void Export()
        {
            var dialog = new ViewModelDialogPlcExport(_dataService);

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value) _dataService.Export();
        }

        private void ShowAppSettings()
        {
            var dialog = new ViewModelDialogSettings(_dataService);

            _dialogService.ShowDialog(dialog);
        }
    }
}
