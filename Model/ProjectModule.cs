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
        public ObservableCollection<ProjectModule> SubModules { get; set; } = new ObservableCollection<ProjectModule>();

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
