using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public class DataService : IDataService
    {
        private List<ModuleFunction> _moduleFunctions;
        private ProjectItem _projectRoot;
        private FileReadWrite _fileReadWrite;

        public DataService()
        {
            _fileReadWrite = new FileReadWrite();
            
            _moduleFunctions = new List<ModuleFunction>();
            _projectRoot = new ProjectItem("Project",ItemTypeISA88.ProcessCell);

            var item1 = new ProjectItem("Item 1", ItemTypeISA88.EquipmentModule);
            var item2 = new ProjectItem("Item 2", ItemTypeISA88.EquipmentModule);
            var item3 = new ProjectItem("Item 3", ItemTypeISA88.ComplexCtrlModule);

            item2.SubItems.Add(item3);

            _projectRoot.SubItems.Add(item1);
            _projectRoot.SubItems.Add(item2);

            _moduleFunctions.Add(new ModuleFunction(item3.Id, "Move forward"));
            _moduleFunctions.Add(new ModuleFunction(item3.Id, "Move up"));
        }
        
        public ObservableCollection<ModuleFunction> GetModuleFunctions(Guid ItemId)
        {
            return new ObservableCollection<ModuleFunction>(_moduleFunctions.Where(fct => fct.ModuleId == ItemId).ToList());
        }

        public void AddModuleFunction(ModuleFunction moduleFunction)
        {
            _moduleFunctions.Add(moduleFunction);
        }

        public ProjectItem GetProjectRoot()
        {
            return _projectRoot;
        }

        public void UpdateProjectItem(ProjectItem item)
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

        private void FindItem(ProjectItem newItem, ProjectItem root)
        {
            if(root.SubItems.Where(item => item.Id == newItem.Id).ToList().Count > 0)
            {
                root.SubItems[root.SubItems.IndexOf(root.SubItems.Where(item => item.Id == newItem.Id).ToList().FirstOrDefault())] = newItem;
            }
            else
            {
                foreach(ProjectItem subitem in root.SubItems)
                {
                    FindItem(newItem, subitem);
                }
            }
        }

        public void SaveToFile(string filePath)
        {
            _fileReadWrite.CreateFile(_projectRoot, _moduleFunctions, filePath);
        }

        public void ReadFromFile(string filePath)
        {
            _fileReadWrite.ReadFile(filePath);

            _projectRoot = _fileReadWrite.RootModule;
            _moduleFunctions = _fileReadWrite.ModuleFunctions;
        }
    }
}
