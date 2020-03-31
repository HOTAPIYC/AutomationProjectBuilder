using AutomationProjectBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Data.Services
{
    public class ExportSettings : ISetting
    {
        private IDictionary<string, object> settings = new Dictionary<string, object>();
        public ExportSettings()
        {
            // Read settings from database. Temp solution with hardcoded values.

            settings["FilePath"] = "";
            settings["ProjectName"] = "";
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
