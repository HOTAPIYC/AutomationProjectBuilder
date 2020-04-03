using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Data.Model
{
    public class ParameterGroup
    {
        public string Name { get; set; }
        public List<ParameterSet> Configurations { get; set; } = new List<ParameterSet>();

        public ParameterGroup(string name)
        {
            Name = name;
        }
    }
}
