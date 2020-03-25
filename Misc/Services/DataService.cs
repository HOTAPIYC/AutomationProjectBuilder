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
        private List<ModuleFunction> _moduleFunctions = new List<ModuleFunction>();
        private List<ConfigValue> _customParameters = new List<ConfigValue>();
        private List<ConfigGroup> _customConfig;
        private ProjectModule _projectRoot;
        private FileReadWrite _fileReadWrite = new FileReadWrite();
        private IConfigService _settings;

        public DataService(IConfigService configService)
        {
            _settings = configService;

            _customConfig = _fileReadWrite.ReadConfiguration("G:\\CustomConfig.xml");
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

        public void UpdateFunction(ModuleFunction function)
        {
            var index = _moduleFunctions.FindIndex(fct => fct.ModuleId == function.ModuleId);

            _moduleFunctions[index] = function;
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

        public void UpdateModule(ProjectModule item)
        {
            if(item.Id == _projectRoot.Id)
            {
                _projectRoot = item;
            }
            else
            {
                ReplaceSubModule(item, _projectRoot);
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

        public void SetCustomParameters(Guid ModuleId,List<ConfigValue> parameters)
        {
            _customParameters.RemoveAll(values => values.ModuleId == ModuleId);
            
            foreach(ConfigValue parameter in parameters)
            {
                _customParameters.Add(parameter);
            }
        }

        public List<ConfigValue> GetCustomParameters(Guid ItemId)
        {
            return _customParameters.Where(parameter => parameter.ModuleId == ItemId).ToList();
        }

        public List<ConfigGroup> GetLoadedConfigs()
        {
            return _customConfig;
        }

        // File interface

        public void Save()
        {
            if (File.Exists((string)_settings.Get("LastFilePath")))
            {
                _fileReadWrite.CreateFile(
                    _projectRoot, 
                    _moduleFunctions,
                    (string)_settings.Get("LastFilePath"));
            }
            else
            {
                SaveAs();
            }
        }

        public void SaveAs()
        {
            var dialog = new SaveFileDialog();

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                _settings.Update("LastFilePath", dialog.FileName);

                _fileReadWrite.CreateFile(
                    _projectRoot,
                    _moduleFunctions,
                    (string)_settings.Get("LastFilePath"));
            }
        }

        public void Load()
        {
            if (File.Exists((string)_settings.Get("LastFilePath")))
            {
                _fileReadWrite.ReadFile((string)_settings.Get("LastFilePath"));

                _projectRoot = _fileReadWrite.RootModule;
                _moduleFunctions = _fileReadWrite.ModuleFunctions;
            }
            else
            {
                _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
                _moduleFunctions.Clear();
            }
        }

        public void Open()
        {
            var dialog = new OpenFileDialog();

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                _settings.Update("LastFilePath", dialog.FileName);

                Load();
            }
        }

    }
}
