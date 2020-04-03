using AutomationProjectBuilder.Data.Interfaces;
using System.Collections.Generic;

namespace AutomationProjectBuilder.Data
{
    public class ProjectSettings : ISettings
    {
        private IDictionary<string,object> settings = new Dictionary<string,object>();
        public ProjectSettings()
        {
            // Read settings from database. Temp solution with hardcoded values.

            settings["ProjectName"] = "";

            settings["FilePathLast"] = "";
            settings["FilePathConfig"] = "G:\\CustomConfig.xml";
            settings["FilePathExport"] = "";
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
