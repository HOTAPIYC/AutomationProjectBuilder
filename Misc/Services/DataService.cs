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
        private List<ModuleFunction> _moduleFunctions;
        private ProjectModule _projectRoot;
        private FileReadWrite _fileReadWrite = new FileReadWrite();
        private IConfigService _settings;

        public DataService(IConfigService configService)
        {
            _settings = configService;

            _moduleFunctions = new List<ModuleFunction>();
            _projectRoot = new ProjectModule("Project",ModuleType.ProcessCell);
        }
        
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

        public ProjectModule GetProjectRoot()
        {
            return _projectRoot;
        }

        public void Update(ProjectModule item)
        {
            if(item.Id == _projectRoot.Id)
            {
                _projectRoot = item;
            }
            else
            {
                FindItem(item, _projectRoot);
            }
        }

        private void FindItem(ProjectModule newItem, ProjectModule root)
        {
            if(root.SubModules.Where(item => item.Id == newItem.Id).ToList().Count > 0)
            {
                root.SubModules[root.SubModules.IndexOf(root.SubModules.Where(item => item.Id == newItem.Id).ToList().FirstOrDefault())] = newItem;
            }
            else
            {
                foreach(ProjectModule subitem in root.SubModules)
                {
                    FindItem(newItem, subitem);
                }
            }
        }

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

        public ProjectModule Reset()
        {
            _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
            _moduleFunctions.Clear();

            return _projectRoot;
        }
    }
}
