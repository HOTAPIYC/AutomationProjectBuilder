using AutomationProjectBuilder.ViewModels;
using System.Windows;

namespace AutomationProjectBuilder.Views
{
    public partial class MainView : Window
    {
        private MainViewModel _viewModel;

        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;
        }
    }
}
