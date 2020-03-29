using AutomationProjectBuilder.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ModuleFunction : IListItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get; set; }

        public ModuleFunction(Guid moduleId, string name)
        {          
            ModuleId = moduleId;
            Name = name;
            Description = "";
        }

        public ModuleFunction(Guid moduleId, string name, string description)
        {
            ModuleId = moduleId;
            Name = name;
            Description = description;
        }
    }
}
