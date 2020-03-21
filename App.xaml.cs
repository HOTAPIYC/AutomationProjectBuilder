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

            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<ViewModelDialogTreeItem, ViewDialogTreeItem>();

            ViewMain mainView = new ViewMain() { DataContext = new ViewModelMain(dialogService) };

            mainView.Show();
        }
    }
}
