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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDialogService dialogService = new DialogService(MainWindow);
            IDataService dataService = new DataService();
            ICodeGenService plcCodeService = new CodeGenService(dataService);

            dialogService.Register<ViewModelDialogTreeItem, ViewDialogTreeItem>();
            dialogService.Register<ViewModelDialogTextInput, ViewDialogTextInput>();
            dialogService.Register<ViewModelDialogConfig, ViewDialogConfig>();
            dialogService.Register<ViewModelDialogPlcExport, ViewDialogPlcExport>();

            ViewMain mainView = new ViewMain() { DataContext = new ViewModelMain(dialogService, dataService, plcCodeService) };

            mainView.Show();
        }
    }
}
