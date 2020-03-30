using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IDataService
    {
        public ProjectModule GetProjectRoot();

        public ProjectModule ResetProjectRoot();

        public List<ParameterGroup> GetParameterGroups();

        public bool Save();

        public void SaveAs(string filePath);

        public void Load();

        public void Open(string filePath);

        public void CreatePlcCode(IDictionary<string, object> settings);
    }
}
