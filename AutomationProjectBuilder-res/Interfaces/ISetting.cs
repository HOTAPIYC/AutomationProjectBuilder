using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Interfaces
{
    public interface ISetting
    {
        object this[string propertyName] { get; set; }

        void Save();
    }
}
