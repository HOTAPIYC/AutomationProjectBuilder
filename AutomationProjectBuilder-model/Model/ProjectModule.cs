using AutomationProjectBuilder.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ProjectModule
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ModuleType Type { get; set; }
        public string NameParamGrp { get; set; } = "";
        public string NameParamSet { get; set; } = "";
        public List<ProjectModule> SubModules { get; set; } = new List<ProjectModule>();
        public List<IListItem> Functions { get; set; } = new List<IListItem>();
        public List<IListItem> Parameters { get; set; } = new List<IListItem>();

        public ProjectModule()
        {
            Id = Guid.NewGuid();
            Name = "";
            Type = ModuleType.Uncategorized;
        }
        
        public ProjectModule(string name, ModuleType type)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
        }
    }
}
