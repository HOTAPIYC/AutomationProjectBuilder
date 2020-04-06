using AutomationProjectBuilder.Data.Interfaces;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
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
            set { _filePathConfig = value; NotifyPropertChanged("FilePathConfig"); }
        }
        public ICommand CmdSetConfigSource { get => _cmdSetConfigSource; }
        public ICommand CmdClose { get => _cmdClose; }

        public bool? DialogResult { get; set; }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public ViewModelDialogSettings(IDataService dataservice)
        {
            _dataservice = dataservice;

            FilePathConfig = (string)_dataservice.Settings["FilePathConfig"];

            _cmdSetConfigSource = new DelegateCommand(x => 
            {
                var dialog = new OpenFileDialog();

                var result = dialog.ShowDialog();

                if (result.Value)
                {
                    FilePathConfig = dialog.FileName;

                    _dataservice.Settings["FilePathConfig"] = dialog.FileName;
                }
            });
            _cmdClose = new DelegateCommand(x => 
            {
                _dataservice.LoadParameterGroups();
                
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)); 
            });
        }
    }
}
