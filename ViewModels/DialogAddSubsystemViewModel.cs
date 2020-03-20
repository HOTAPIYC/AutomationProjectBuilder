using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class DialogAddSubsystemViewModel : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdCreateNewComponent;
        private string _textInput;
        private ComponentType _componentTypeSelection;

        public ComponentType ComponentTypeSelection
        {
            get
            {
                return _componentTypeSelection;
            }
            set
            {
                _componentTypeSelection = value;
                NotifyPropertChanged("ComponentTypeSelection");
            }
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
