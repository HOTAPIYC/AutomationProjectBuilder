using AutomationProjectBuilder.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ModuleParameter : IListItem
    {
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ModuleParameter(string name)
        {
            Name = name;
        }

        public ModuleParameter(Guid moduleId, string name, object value)
        {
            ModuleId = moduleId;
            Name = name;
            Value = value;
        }
    }
}
