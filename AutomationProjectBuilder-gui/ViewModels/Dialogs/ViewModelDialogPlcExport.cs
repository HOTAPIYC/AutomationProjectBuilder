using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Misc;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels.Dialogs
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
            set
            {
                _filePath = value;
                NotifyPropertChanged("FilePath");
            }
        }

        public string ProjectName
        {
            get => (string)_dataservice.Settings["ProjectName"];
            set
            {
                _dataservice.Settings["ProjectName"] = value;
                NotifyPropertChanged("ProjectName");
            }
        }

        public ICommand CmdTarget { get => _cmdTarget; }
        public ICommand CmdExport { get => _cmdExport; }
        public ICommand CmdCancel { get => _cmdCancel; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogPlcExport(IDataService dataservice)
        {
            FilePath = "Choose a file path";

            _dataservice = dataservice;

            _cmdTarget = new DelegateCommand(x => ChooseFilePath());         
            _cmdExport = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            _cmdCancel = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
        }

        private void ChooseFilePath()
        {
            var dialog = new SaveFileDialog();

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                FilePath = dialog.FileName;

                _dataservice.Settings["ExportFilePath"] = dialog.FileName;
            }
        }
    }
}
