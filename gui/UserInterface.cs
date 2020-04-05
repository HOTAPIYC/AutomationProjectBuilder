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

            IDialogService dialogService = new DialogService();

            mainView.DataContext = new ViewModelMain(dialogService, _dataService);

            mainView.ShowDialog();
        }
    }
}
