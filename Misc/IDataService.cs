using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IDataService
    {
        public ObservableCollection<ModuleFunction> GetModuleFunctions(Guid ItemId);
        public void AddModuleFunction(ModuleFunction function);

        public ProjectItem GetProjectRoot();
        public void UpdateProjectItem(ProjectItem item);

        public void SaveToFile(string filePath);
        public void ReadFromFile(string filePath);
    }
}
