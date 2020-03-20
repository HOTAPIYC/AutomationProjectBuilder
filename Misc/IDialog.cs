using System;
using System.Windows;

namespace AutomationProjectBuilder.Misc
{
    public interface IDialog
    {
        public object DataContext { get; set; }
        public bool? DialogResult { get; set; }
        public Window Owner { get; set; }
        public void Close();
        public bool? ShowDialog();
    }

    public interface IDialogService
    {
        public void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                           where TView : IDialog;

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
    }

    public interface IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
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
