using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Data.Model
{
    public class ParameterSet
    {
        public string Name { get; set; }
        public List<ModuleParameter> Parameters { get; set; } = new List<ModuleParameter>();

        public ParameterSet(string name)
        {
            Name = name;
        }
    }
}
