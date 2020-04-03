using AutomationProjectBuilder.Export.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Interfaces
{
    public interface IPlcBody
    {
        public void AddFunctionBlockCall(string instancename, PlcFunctionBlock functionblock);
        public XElement GetXml();
    }
}
