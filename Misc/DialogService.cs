using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AutomationProjectBuilder.Misc
{
    public class DialogService : IDialogService
    {
        private readonly Window owner;

        public DialogService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        public IDictionary<Type, Type> Mappings { get; }

        public void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                                  where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];

            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);

            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            };

            viewModel.CloseRequested += handler;

            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            return dialog.ShowDialog();
        }

        public bool? ShowOpenFileDialog(FileDialogSettings settings)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = settings.Filter;
            dialog.CheckFileExists = settings.CheckFileExists;
            dialog.CheckPathExists = settings.CheckPathExists;

            var result = dialog.ShowDialog();

            settings.FileName = dialog.FileName;

            return result;
        }

        public bool? ShowSaveFileDialog(FileDialogSettings settings)
        {
            var dialog = new SaveFileDialog();

            dialog.FileName = settings.FileName;
            dialog.Filter = settings.Filter;
            dialog.CheckFileExists = settings.CheckFileExists;
            dialog.CheckPathExists = settings.CheckPathExists;

            var result = dialog.ShowDialog();

            settings.FileName = dialog.FileName;

            return result;
        }
    }
}
