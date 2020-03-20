using AutomationProjectBuilder.Misc;
using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class DialogAddSubsystemViewModel : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdCreateNewComponent;
        private string _textInput;

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
        public ICommand CmdCreateNewComponent
        {
            get { return _cmdCreateNewComponent; }
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public DialogAddSubsystemViewModel()
        {
            _cmdCreateNewComponent = new DelegateCommand(x => CreateComponent());
        }

        public void CreateComponent()
        {
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }
    }
}
