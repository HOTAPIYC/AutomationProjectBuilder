using AutomationProjectBuilder.Data.Interfaces;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDialogPlcExport : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdTarget;
        private ICommand _cmdExport;
        private ICommand _cmdCancel;

        private string _filePath;

        public IDataService _dataservice;

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; NotifyPropertChanged("FilePath"); }
        }

        public string ProjectName
        {
            get => (string)_dataservice.Settings["ProjectName"];
            set { _dataservice.Settings["ProjectName"] = value; NotifyPropertChanged("ProjectName"); }
        }

        public ICommand CmdTarget 
        { 
            get => _cmdTarget; 
        }
        public ICommand CmdExport 
        { 
            get => _cmdExport; 
        }
        public ICommand CmdCancel 
        { 
            get => _cmdCancel; 
        }

        public bool? DialogResult { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogPlcExport(IDataService dataservice)
        {
            _dataservice = dataservice;

            if ((string)_dataservice.Settings["FilePathExport"] != "")
                FilePath = (string)_dataservice.Settings["FilePathExport"];
            else
                FilePath = "Please choose a file path.";

            _cmdTarget = new DelegateCommand(x => 
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "xml files (*.xml)|*.xml";

                var result = dialog.ShowDialog();

                if (result.Value)
                {
                    FilePath = dialog.FileName;

                    _dataservice.Settings["FilePathExport"] = dialog.FileName;
                }
            });         
            _cmdExport = new DelegateCommand(x => 
            { 
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)); 
            });
            _cmdCancel = new DelegateCommand(x => 
            { 
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)); 
            });
        }
    }
}
