using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Data.Interfaces
{
    public interface ISettings
    {
        object this[string propertyName] { get; set; }

        void Save();
    }
}
