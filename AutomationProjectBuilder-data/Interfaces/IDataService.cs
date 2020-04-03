using AutomationProjectBuilder.Data.Model;
using System;
using System.Collections.Generic;

namespace AutomationProjectBuilder.Data.Interfaces
{
    public interface IDataService
    {
        public bool Export();    
        
        public event EventHandler ExportRequested;
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
