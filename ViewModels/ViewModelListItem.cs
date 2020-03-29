using AutomationProjectBuilder.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelListItem : ViewModelBase
    {     
        private IListItem _listItem;

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

        public ViewModelListItem(IListItem listItem)
        {
            _listItem = listItem;
        }
    }
}
