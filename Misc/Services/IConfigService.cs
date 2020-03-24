using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IConfigService
    {
        public void Update(string settingName, object value);

        public object Get(string settingName);
    }

    public interface ISetting
    {
        object this[string propertyName] { get; set; }

        void Save();
    }
}
