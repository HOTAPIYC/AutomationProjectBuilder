using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace AutomationProjectBuilder.Gui
{
    public class DialogService : ViewModelBase, IDialogService
    {
        private ViewModelBase _dialogContent;
        private Visibility _dialogVisibility = Visibility.Collapsed;
        private bool _isOpen = false;
        private TaskCompletionSource<bool> _taskCompletionSource = null;

        public ViewModelBase DialogContent
        {
            get => _dialogContent;
            set { _dialogContent = value; NotifyPropertChanged("DialogContent"); }
        }

        public Visibility DialogVisibility
        {
            get => _dialogVisibility;
            set { _dialogVisibility = value; NotifyPropertChanged("DialogVisibility"); }
        }

        public bool IsOpen
        {
            get => _isOpen;
            set { _isOpen = value; NotifyPropertChanged("IsOpen"); }
        }

        public async Task<bool?> ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase, IDialogRequestClose
        {
            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            _taskCompletionSource = new TaskCompletionSource<bool>();

            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    viewModel.DialogResult = e.DialogResult;
                }

                IsOpen = false;

                _taskCompletionSource?.TrySetResult(true);
            };

            viewModel.CloseRequested += handler;

            DialogContent = viewModel;
            DialogVisibility = Visibility.Visible;

            IsOpen = true;

            await _taskCompletionSource.Task;

            return viewModel.DialogResult;
        }
    }
}
