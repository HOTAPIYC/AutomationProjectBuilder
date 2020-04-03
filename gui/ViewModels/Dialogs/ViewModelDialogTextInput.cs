using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDialogTextInput : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdSave;
        private ICommand _cmdCancel;

        private string _textInput;

        public ICommand CmdSave
        {
            get { return _cmdSave; }
        }

        public ICommand CmdCancel
        {
            get { return _cmdCancel; }
        }

        public string TextInput
        {
            get
            {
                return _textInput;
            }
            set
            {
                _textInput = value;
                NotifyPropertChanged("TextInput");
            }
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogTextInput(string text)
        {
            TextInput = text;
            
            _cmdSave = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
        }
    }
}
