using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.ViewModels;
using AutomationProjectBuilder.Views;

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

            IConfigService settings = new ConfigService(new ProjectSettings());
            IDialogService dialogService = new DialogService(MainWindow);
            IDataService dataService = new DataService(settings);

            dialogService.Register<ViewModelDialogTreeItem, ViewDialogTreeItem>();
            dialogService.Register<ViewModelDialogTextInput, ViewDialogTextInput>();
            dialogService.Register<ViewModelDialogConfig, ViewDialogConfig>();

            ViewMain mainView = new ViewMain() { DataContext = new ViewModelMain(dialogService, dataService) };

            mainView.Show();
        }
    }
}
