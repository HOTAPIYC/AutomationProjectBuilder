using System.Windows;
using AutomationProjectBuilder.Data.Services;
using AutomationProjectBuilder.Export.CodeGenerator;
using AutomationProjectBuilder.Gui.Dialogs;
using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.ViewModels;
using AutomationProjectBuilder.ViewModels.Dialogs;
using AutomationProjectBuilder.Views;
using AutomationProjectBuilder.Views.Dialogs;

namespace AutomationProjectBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDataService _dataService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<ViewModelDialogTreeItem, ViewDialogTreeItem>();
            dialogService.Register<ViewModelDialogTextInput, ViewDialogTextInput>();
            dialogService.Register<ViewModelDialogConfig, ViewDialogConfig>();
            dialogService.Register<ViewModelDialogPlcExport, ViewDialogPlcExport>();
            dialogService.Register<ViewModelDialogSettings, ViewDialogSettings>();

            ViewMain mainView = new ViewMain() { DataContext = new ViewModelMain(dialogService, _dataService) };

            mainView.Show();
        }

        public void SetDataService(IDataService dataService)
        {
            _dataService = dataService;
        }
    }
}
