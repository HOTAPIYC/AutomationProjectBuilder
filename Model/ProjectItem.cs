using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public class ProjectItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemTypeISA88 Type { get; set; }
        public ObservableCollection<ProjectItem> SubItems { get; set; }

        public ProjectItem(string name, ItemTypeISA88 type)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;

            SubItems = new ObservableCollection<ProjectItem>();
        }
    }
}
