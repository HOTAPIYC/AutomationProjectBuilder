using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Model;
using System.Collections.Generic;

namespace AutomationProjectBuilder.Interfaces
{
    public interface IDataService
    {
        public ISettings Settings { get; }
        public ProjectModule GetProjectRoot();

        public ProjectModule ResetProjectRoot();

        public List<ParameterGroup> GetParameterGroups();

        public bool Save();

        public void SaveAs(string filePath);

        public void Load();

        public void Open(string filePath);
    }
}
