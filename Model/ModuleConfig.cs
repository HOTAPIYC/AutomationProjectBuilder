using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ModuleConfig
    {
        public string Name { get; set; }
        public List<ConfigValue> Parameters { get; set; } = new List<ConfigValue>();

        public ModuleConfig(string name)
        {
            Name = name;
        }
    }
}
