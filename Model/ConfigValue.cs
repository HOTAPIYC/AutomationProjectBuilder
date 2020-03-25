using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ConfigValue
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public ConfigValue(string name)
        {
            Name = name;
        }
    }
}
