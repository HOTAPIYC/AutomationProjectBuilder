using AutomationProjectBuilder.Export.Enums;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Components
{
    public class PlcMethod
    {
        public string Name { get; set; }
        public PlcDataType PlcReturnType { get; set; }
        public List<PlcVariable> Variables { get; set; }

        public PlcMethod(string name, PlcDataType plcReturnType)
        {
            Name = name;
            PlcReturnType = plcReturnType;
        }

        public void AddVariable(PlcVariable var)
        {
            Variables.Add(var);
        }

        public XElement GetXml()
        {
            var meth = new XElement("data",
                new XAttribute("name", "http://www.3s-software.com/plcopenxml/method"),
                new XAttribute("handleUnkown", "implementation")
                );

            return meth;
        }
    }
}
