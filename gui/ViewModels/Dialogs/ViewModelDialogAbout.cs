using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDialogAbout : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdClose;
        
        public ICommand CmdClose
        {
            get => _cmdClose;
        }

        public bool? DialogResult { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogAbout()
        {
            _cmdClose = new DelegateCommand(x =>
            {
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
            });
        }
    }
}
