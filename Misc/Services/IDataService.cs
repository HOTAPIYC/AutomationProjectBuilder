using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IDataService
    {
        public ObservableCollection<ModuleFunction> GetFunctions(Guid ItemId);
        public void AddFunction(ModuleFunction function);
        public void UpdateFunction(ModuleFunction function);

        public ProjectModule GetProjectRoot();
        public ProjectModule ResetProjectRoot();
        public void UpdateModule(ProjectModule item);

        public void SetCustomParameters(Guid ModuleId, List<ConfigValue> parameters);
        public List<ConfigValue> GetCustomParameters(Guid ItemId);
        public List<ConfigGroup> GetLoadedConfigs();

        public void Save();
        public void SaveAs();

        public void Load();
        public void Open();
    }
}
