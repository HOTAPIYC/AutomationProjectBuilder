using AutomationProjectBuilder.Data.Services;
using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Misc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels.Dialogs
{
    public class ViewModelDialogPlcExport : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdTarget;
        private ICommand _cmdExport;
        private ICommand _cmdCancel;

        private string _filePath;

        public ISetting ExportSettings { get; set; } = new ExportSettings();

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
            get => (string)ExportSettings["ProjectName"];
            set
            {
                ExportSettings["ProjectName"] = value;
                NotifyPropertChanged("ProjectName");
            }
        }

        public ICommand CmdTarget { get => _cmdTarget; }
        public ICommand CmdExport { get => _cmdExport; }
        public ICommand CmdCancel { get => _cmdCancel; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogPlcExport()
        {
            FilePath = "Choose a file path";

            ExportSettings["FilePath"] = "";
            ExportSettings["ProjectName"] = "Enter a name";

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

                ExportSettings["FilePath"] = dialog.FileName;
            }
        }
    }
}
