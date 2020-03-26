using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ModuleParameter
    {
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

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
