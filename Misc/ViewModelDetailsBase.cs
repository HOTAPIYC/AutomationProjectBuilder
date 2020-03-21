using AutomationProjectBuilder.Model;
using System.ComponentModel;

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
        public ItemTypeISA88 ViewItemType { get; set; }
    }
}
