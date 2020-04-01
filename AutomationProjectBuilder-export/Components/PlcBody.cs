using AutomationProjectBuilder.Export.Enums;
using AutomationProjectBuilder.Export.Interfaces;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Components
{
    public class PlcBodyST : IPlcBody
    {
        private string _content;
        public PlcBodyST()
        {
            _content = "";
        }

        public void AddFunctionBlockCall(string instancename, PlcFunctionBlock functionblock)
        {
            AddNewLine(string.Format("{0}{1}", instancename, "();"));
        }

        private void AddNewLine(string line)
        {
            _content = string.Format("{0}\n{1}", _content, line);
        }

        public XElement GetXml()
        {
            return new XElement(PlcOpenNamespaces.Ns + "body",
                 new XElement(PlcOpenNamespaces.Ns + PlcPouType.ST.ToString(),
                 new XElement(PlcOpenNamespaces.Xhtmlns + "xhtml", _content)));
        }
    }
}
