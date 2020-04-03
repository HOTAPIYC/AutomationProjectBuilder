using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Gui.ViewModels;
using AutomationProjectBuilder.Gui.Views;

namespace AutomationProjectBuilder.Gui
{
    public class UserInterface
    {
        private IDataService _dataService;
        public UserInterface(IDataService dataService)
        {
            _dataService = dataService;

            ViewMain mainView = new ViewMain();

            IDialogService dialogService = new DialogService(mainView);

            dialogService.Register<ViewModelDialogTreeItem, ViewDialogTreeItem>();
            dialogService.Register<ViewModelDialogTextInput, ViewDialogTextInput>();
            dialogService.Register<ViewModelDialogConfig, ViewDialogConfig>();
            dialogService.Register<ViewModelDialogPlcExport, ViewDialogPlcExport>();
            dialogService.Register<ViewModelDialogSettings, ViewDialogSettings>();

            mainView.DataContext = new ViewModelMain(dialogService, _dataService);

            mainView.ShowDialog();
        }
    }
}
