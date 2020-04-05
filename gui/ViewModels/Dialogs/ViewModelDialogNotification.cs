using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDialogNotification : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdOk;

        private string _notificationText;

        public ICommand CmdOk
        {
            get => _cmdOk;
        }

        public string NotificationText
        {
            get => _notificationText;
            set { _notificationText = value; NotifyPropertChanged("NotificationText"); }
        }

        public bool? DialogResult { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogNotification(string text)
        {
            NotificationText = text;
            
            _cmdOk = new DelegateCommand(x => 
            { 
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)); 
            });
        }
    }
}
