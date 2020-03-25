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
        public ProjectModule Reset();
        public void Update(ProjectModule item);

        public void Save();
        public void SaveAs();

        public void Load();
        public void Open();
    }
}
