﻿using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomationProjectBuilder.Data
{
    public class DataService : IDataService
    {
        private ProjectModule _projectRoot;

        private List<ParameterGroup> _customConfig = new List<ParameterGroup>();

        public bool Export()
        {
            OnExportRequest(new EventArgs());

            return true;
        }

        public event EventHandler ExportRequested;

        protected virtual void OnExportRequest(EventArgs e)
        {
            ExportRequested?.Invoke(this, e);
        }

        public ISettings Settings { get; } = new ProjectSettings();

        public DataService()
        {
            LoadParameterGroups();
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
            if (File.Exists((string)Settings["FilePathLast"]))
            {
                FileReadWrite.CreateFile(_projectRoot, (string)Settings["FilePathLast"]);

                return true;
            }
            else
            {
                return false;
            }
        }
        public void SaveAs(string filePath)
        {
            Settings["FilePathLast"] = filePath;

            FileReadWrite.CreateFile(_projectRoot, (string)Settings["FilePathLast"]);
        }
        public void Load()
        {
            if (File.Exists((string)Settings["FilePathLast"]))
            {
                _projectRoot = FileReadWrite.ReadFile((string)Settings["FilePathLast"]);
            }
            else
            {
                _projectRoot = new ProjectModule("Empty project", ModuleType.Uncategorized);
            }
        }
        public void Open(string filePath)
        {
            Settings["FilePathLast"] = filePath;

            Load();
        }

        public void LoadParameterGroups()
        {
            if((string)Settings["FilePathConfig"] != "")
                _customConfig = FileReadWrite.ReadConfiguration((string)Settings["FilePathConfig"]);
        }
    }
}
