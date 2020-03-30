using AutomationProjectBuilder.Export.Enums;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Components
{    
    public class PlcFunctionBlock
    {
        public string Name { get; set; }

        private List<PlcVariable> _variables;
        private XElement _body;

        public PlcFunctionBlock(string name)
        {
            Name = name;

            _variables = new List<PlcVariable>();
            _body = new XElement(PlcOpenNamespaces.Ns + "body");
        }

        public void AddVar(PlcVarType varType, string name, PlcDataType dataType, string comment)
        {
            _variables.Add(new PlcVariable(varType, name, dataType, comment));
        }

        public void SetBody(string body)
        {
           _body.Add(new XElement(PlcOpenNamespaces.Ns + PlcPouType.ST.ToString(),
                     new XElement(PlcOpenNamespaces.Xhtmlns + "xhtml",body)));
        }

        public XElement GetXml()
        {
            XElement fb = new XElement(PlcOpenNamespaces.Ns + "pou",
                    new XAttribute("name",Name),
                    new XAttribute("pouType","functionBlock")
                    );

            XElement inputVars = new XElement(PlcOpenNamespaces.Ns + "inputVars");
            XElement outputVars = new XElement(PlcOpenNamespaces.Ns + "outputVars");
            XElement localVars = new XElement(PlcOpenNamespaces.Ns + "localVars");

            foreach (PlcVariable var in _variables)
            {
                XElement newVar = new XElement(PlcOpenNamespaces.Ns + "variable",
                    new XAttribute("name",var.Name),
                    new XElement(PlcOpenNamespaces.Ns + "type",
                        new XElement(PlcOpenNamespaces.Ns + var.DataType.ToString())),
                        new XElement(PlcOpenNamespaces.Ns + "documentation",
                            new XElement(PlcOpenNamespaces.Xhtmlns + "xhtml", " " + var.Comment))
                        );

                switch (var.VarType)
                {
                    case PlcVarType.INPUT:
                        inputVars.Add(newVar);
                        break;
                    case PlcVarType.OUTPUT:
                        outputVars.Add(newVar);
                        break;
                    case PlcVarType.LOCAL:
                        localVars.Add(newVar);
                        break;
                }
            }

            XElement pouInterface = new XElement(PlcOpenNamespaces.Ns + "interface");

            if (inputVars.HasElements) { pouInterface.Add(inputVars); }
            if (outputVars.HasElements) { pouInterface.Add(outputVars); }
            if (localVars.HasElements) { pouInterface.Add(localVars); }

            fb.Add(pouInterface);
            fb.Add(_body);

            return fb;
        }
    }
}
