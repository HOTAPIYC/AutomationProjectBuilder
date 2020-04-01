using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Model;
using System.Collections.Generic;
using System.IO;

namespace AutomationProjectBuilder.Data.Services
{
    public class DataService : IDataService
    {
        private ProjectModule _projectRoot;

        private List<ParameterGroup> _customConfig = new List<ParameterGroup>();

        public ISettings Settings { get; } = new ProjectSettings();

        public DataService()
        {
            _customConfig = FileReadWrite.ReadConfiguration((string)Settings["ConfigFilePath"]);
        }
        
        public ProjectModule GetProjectRoot()
        {
            return _projectRoot;
        }
        public ProjectModule ResetProjectRoot()
        {
            _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);

            return _projectRoot;
        }
  
        public List<ParameterGroup> GetParameterGroups()
        {
            return _customConfig;
        }

        public bool Save()
        {
            if (File.Exists((string)Settings["LastFilePath"]))
            {
                FileReadWrite.CreateFile(_projectRoot, (string)Settings["LastFilePath"]);

                return true;
            }
            else
            {
                return false;
            }
        }
        public void SaveAs(string filePath)
        {
            Settings["LastFilePath"] = filePath;

            FileReadWrite.CreateFile(_projectRoot, (string)Settings["LastFilePath"]);
        }
        public void Load()
        {
            if (File.Exists((string)Settings["LastFilePath"]))
            {
                _projectRoot = FileReadWrite.ReadFile((string)Settings["LastFilePath"]);
            }
            else
            {
                _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
            }
        }
        public void Open(string filePath)
        {
            Settings["LastFilePath"] = filePath;

            Load();
        }
    }
}
