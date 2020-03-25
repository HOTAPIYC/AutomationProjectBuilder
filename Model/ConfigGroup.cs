using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ConfigGroup
    {
        public string Name { get; set; }
        public List<ModuleConfig> Configurations { get; set; } = new List<ModuleConfig>();

        public ConfigGroup(string name)
        {
            Name = name;
        }
    }
}
