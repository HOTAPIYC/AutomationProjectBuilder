using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Misc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels.Dialogs
{
    public class ViewModelDialogSettings : ViewModelBase, IDialogRequestClose
    {
        private ICommand _cmdSetConfigSource;
        private ICommand _cmdClose;

        private string _filePathConfig;

        public IDataService _dataservice;

        public string FilePathConfig
        {
            get => _filePathConfig;
            set
            {
                _filePathConfig = value;
                NotifyPropertChanged("FilePathConfig");
            }
        }
        public ICommand CmdSetConfigSource { get => _cmdSetConfigSource; }
        public ICommand CmdClose { get => _cmdClose; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogSettings(IDataService dataservice)
        {
            _dataservice = dataservice;

            FilePathConfig = (string)_dataservice.Settings["FilePathConfig"];

            _cmdSetConfigSource = new DelegateCommand(x => ChooseFilePath());
            _cmdClose = new DelegateCommand(x => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
        }

        private void ChooseFilePath()
        {
            var dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                FilePathConfig = dialog.FileName;

                _dataservice.Settings["FilePathConfig"] = dialog.FileName;
            }
        }
    }
}
