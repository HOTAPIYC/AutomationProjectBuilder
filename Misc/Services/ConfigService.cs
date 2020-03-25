using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public class ConfigService : IConfigService
    {
        protected ISetting _settings;

        public ConfigService(ISetting settings)
        {
            _settings = settings;
        }

        public void Update(string settingName, object value)
        {
            if (String.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("Setting name must be provided");

            var setting = _settings[settingName];

            if (setting == null)
            {
                throw new ArgumentException("Setting " + settingName + " not found.");
            }
            else if (setting.GetType() != value.GetType())
            {
                throw new InvalidCastException("Unable to cast value to " + setting.GetType());
            }
            else
            {
                _settings[settingName] = value;
                _settings.Save();
            }

        }

        public object Get(string SettingName)
        {
            if (String.IsNullOrEmpty(SettingName))
                throw new ArgumentNullException("Setting name must be provided");

            return _settings[SettingName];
        }
    }
}
