using AutomationProjectBuilder.Export.CodeGenerator;
using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Model;
using System.Collections.Generic;
using System.IO;

namespace AutomationProjectBuilder.Misc
{
    public class DataService : IDataService
    {
        private ProjectModule _projectRoot;

        private List<ParameterGroup> _customConfig = new List<ParameterGroup>();

        private ProjectSettings _settings = new ProjectSettings();

        public DataService()
        {
            _customConfig = FileReadWrite.ReadConfiguration((string)_settings["ConfigFilePath"]);
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
            if (File.Exists((string)_settings["LastFilePath"]))
            {
                FileReadWrite.CreateFile(_projectRoot, (string)_settings["LastFilePath"]);

                return true;
            }
            else
            {
                return false;
            }
        }
        public void SaveAs(string filePath)
        {
            _settings["LastFilePath"] = filePath;

            FileReadWrite.CreateFile(_projectRoot, (string)_settings["LastFilePath"]);
        }
        public void Load()
        {
            if (File.Exists((string)_settings["LastFilePath"]))
            {
                _projectRoot = FileReadWrite.ReadFile((string)_settings["LastFilePath"]);
            }
            else
            {
                _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
            }
        }
        public void Open(string filePath)
        {
            _settings["LastFilePath"] = filePath;

            Load();
        }

        public void CreatePlcCode(ISetting settings)
        {
            var codegenerator = new CodeGenerator(settings);

            codegenerator.CreatePlcCode(_projectRoot);
        }
    }
}
