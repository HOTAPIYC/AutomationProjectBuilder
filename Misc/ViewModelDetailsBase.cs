using AutomationProjectBuilder.Model;
using System;

namespace AutomationProjectBuilder.Misc
{
    public abstract class ViewModelDetailsBase : ViewModelBase
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
        public ModuleType ModuleType { get; set; }
        public Guid ModuleId { get; set; }
    }
}
