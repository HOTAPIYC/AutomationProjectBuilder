using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Misc;
using System.Collections.Generic;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelListItem : ViewModelBase
    {     
        private IListItem _listItem;

        private bool _isSelected = false;
        private bool _isEditMode = false;

        private ICommand _cmdEdit;
        private ICommand _cmdDelete;
        public string Name 
        { 
            get => _listItem.Name; 
            set
            {
                _listItem.Name = value;
                NotifyPropertChanged("Name");
            }
        }

        public object Value
        {
            get => _listItem.Value;
            set
            {
                _listItem.Value = value;
                NotifyPropertChanged("Value");
            }
        }

        public string Description
        {
            get => _listItem.Description;
            set
            {
                _listItem.Description = value;
                NotifyPropertChanged("Description");
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set 
            {
                _isSelected = value;

                if (!value) IsEditMode = false;

                NotifyPropertChanged("IsSelected");
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                NotifyPropertChanged("IsEditMode");
            }
        }

        public ICommand CmdEdit { get => _cmdEdit; }
        public ICommand CmdDelete { get => _cmdDelete; }

        public ViewModelListItem(IListItem listItem, ICollection<ViewModelListItem> parentvm, ICollection<IListItem> parent)
        {
            _listItem = listItem;

            _cmdEdit = new DelegateCommand(x => { IsEditMode = !IsEditMode; });
            _cmdDelete = new DelegateCommand(x => { parent.Remove(listItem); parentvm.Remove(this); });
        }
    }
}
