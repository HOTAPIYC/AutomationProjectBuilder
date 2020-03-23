using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IDataService
    {
        public ObservableCollection<ModuleFunction> GetItemFunctions(Guid ItemId);
        public void AddModuleFunction(ModuleFunction function);
        public ProjectItem GetProjectRoot();
        public void UpdateProjectItem(ProjectItem item);
    }
}
