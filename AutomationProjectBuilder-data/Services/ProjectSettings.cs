using AutomationProjectBuilder.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace AutomationProjectBuilder.Misc
{
    public class ProjectSettings : ISetting
    {
        private IDictionary<string,object> settings = new Dictionary<string,object>();
        public ProjectSettings()
        {
            // Read settings from database. Temp solution with hardcoded values.
            
            settings["LastFilePath"] = "";
            settings["ConfigFilePath"] = "G:\\CustomConfig.xml";
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
