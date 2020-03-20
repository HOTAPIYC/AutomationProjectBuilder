using AutomationProjectBuilder.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.ViewModels
{
    class DetailsViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertChanged("Name");
            }
        }
        
        public DetailsViewModel(string name)
        {
            Name = name;
        }
    }
}
