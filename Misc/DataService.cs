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
        private ObservableCollection<ModuleFunction> _moduleFunctions;
        private ProjectItem _projectRoot;

        public DataService()
        {
            _moduleFunctions = new ObservableCollection<ModuleFunction>();
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
        
        public ObservableCollection<ModuleFunction> GetItemFunctions(Guid ItemId)
        {
            return new ObservableCollection<ModuleFunction>(_moduleFunctions.Where(w => w.ModuleId == ItemId).ToList());
        }

        public void AddModuleFunction(ModuleFunction function)
        {
            _moduleFunctions.Add(function);
        }

        public ProjectItem GetProject()
        {
            return _projectRoot;
        }
    }
}
