using AutomationProjectBuilder.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace AutomationProjectBuilder.Data.Services
{
    public class ProjectSettings : ISettings
    {
        private IDictionary<string,object> settings = new Dictionary<string,object>();
        public ProjectSettings()
        {
            // Read settings from database. Temp solution with hardcoded values.
            
            settings["LastFilePath"] = "";
            settings["ConfigFilePath"] = "G:\\CustomConfig.xml";
            settings["ProjectName"] = "Enter a name";
            settings["ExportFilePath"] = "Choose a path";
        }
        
        public object this[string propertyName]
        {
            get
            {
                return settings[propertyName];
            }
            set
            {
                settings[propertyName] = value;
            }
        }

        public void Save()
        {
            // Some database access
        }
    }
}
