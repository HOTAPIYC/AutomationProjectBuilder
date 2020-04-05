using System;
using System.Threading.Tasks;
using System.Windows;

namespace AutomationProjectBuilder.Gui
{
    public interface IDialogService
    {
        public ViewModelBase DialogContent { get; set; }

        public Visibility DialogVisibility { get; set; }

        public Task<bool?> ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase, IDialogRequestClose;
    }

    public interface IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public bool? DialogResult { get; set; }
    }

    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; }
    }
}
