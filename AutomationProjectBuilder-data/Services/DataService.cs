using AutomationProjectBuilder.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public class DataService : IDataService
    {
        private ProjectModule _projectRoot;

        private List<ModuleFunction> _moduleFunctions = new List<ModuleFunction>();
        private List<ModuleParameter> _moduleParameters = new List<ModuleParameter>();
        private List<ParameterGroup> _customConfig;

        private FileReadWrite _fileReadWrite = new FileReadWrite();
        private ProjectSettings _settings = new ProjectSettings();

        private IDictionary<Type, int> _listItemTypes = new Dictionary<Type, int>();

        public DataService()
        {
            _customConfig = _fileReadWrite.ReadConfiguration((string)_settings["ConfigFilePath"]);

            _listItemTypes.Add(typeof(ModuleFunction), 1);
            _listItemTypes.Add(typeof(ModuleParameter), 2);
        }
        
        // Module functions

        public ObservableCollection<ModuleFunction> GetFunctions(Guid ItemId)
        {
            return new ObservableCollection<ModuleFunction>(_moduleFunctions.Where(fct => fct.ModuleId == ItemId).ToList());
        }
        public void AddFunction(ModuleFunction function)
        {
            _moduleFunctions.Add(function);
        }
        public void DeleteListItem(IListItem function)
        {
            switch (_listItemTypes[function.GetType()])
            {
                case 1:
                    _moduleFunctions.Remove((ModuleFunction)function);
                    break;
                case 2:
                    _moduleParameters.Remove((ModuleParameter)function);
                    break;
            }
        }

        // Project tree

        public ProjectModule GetProjectRoot()
        {
            return _projectRoot;
        }
        public ProjectModule ResetProjectRoot()
        {
            _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
            _moduleFunctions.Clear();

            return _projectRoot;
        }
        public void UpdateModule(ProjectModule projectModule)
        {
            if(projectModule.Id == _projectRoot.Id)
            {
                _projectRoot = projectModule;
            }
            else
            {
                ReplaceSubModule(projectModule, _projectRoot);
            }
        }
        private void ReplaceSubModule(ProjectModule newModule, ProjectModule root)
        {
            if(root.SubModules.Where(module => module.Id == newModule.Id).ToList().Count > 0)
            {
                root.SubModules[root.SubModules.IndexOf(root.SubModules.Where(module => module.Id == newModule.Id).ToList().FirstOrDefault())] = newModule;
            }
            else
            {
                foreach(ProjectModule subitem in root.SubModules)
                {
                    ReplaceSubModule(newModule, subitem);
                }
            }
        }
 
        // Configuration

        public void SetParameters(Guid moduleId,List<ModuleParameter> parameters)
        {
            _moduleParameters.RemoveAll(values => values.ModuleId == moduleId);
            
            foreach(ModuleParameter parameter in parameters)
            {
                _moduleParameters.Add(parameter);
            }
        }
        public List<ModuleParameter> GetParameters(Guid moduleId)
        {
            return _moduleParameters.Where(parameter => parameter.ModuleId == moduleId).ToList();
        }
        public List<ParameterGroup> GetParameterGroups()
        {
            return _customConfig;
        }

        // File interface

        public bool Save()
        {
            if (File.Exists((string)_settings["LastFilePath"]))
            {
                _fileReadWrite.CreateFile(
                    _projectRoot,
                    _moduleFunctions,
                    _moduleParameters,
                    (string)_settings["LastFilePath"]);

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

            _fileReadWrite.CreateFile(
                _projectRoot,
                _moduleFunctions,
                _moduleParameters,
                (string)_settings["LastFilePath"]);
        }
        public void Load()
        {
            if (File.Exists((string)_settings["LastFilePath"]))
            {
                _fileReadWrite.ReadFile((string)_settings["LastFilePath"]);

                _projectRoot = _fileReadWrite.RootModule;
                _moduleFunctions = _fileReadWrite.ModuleFunctions;
                _moduleParameters = _fileReadWrite.ModuleParameters;
            }
            else
            {
                _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
                _moduleFunctions.Clear();
                _moduleParameters.Clear();
            }
        }
        public void Open(string filePath)
        {
            _settings["LastFilePath"] = filePath;

            Load();
        }
    }
}
