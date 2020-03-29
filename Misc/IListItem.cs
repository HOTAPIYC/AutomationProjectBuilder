using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IListItem
    {
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }
    }
}
