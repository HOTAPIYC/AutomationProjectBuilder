using AutomationProjectBuilder.Export.Enums;
using AutomationProjectBuilder.Export.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Components
{    
    public class PlcFunctionBlock
    {
        public string Name { get; set; }
        public List<PlcVariable> Variables { get; set; } = new List<PlcVariable>();
        public List<PlcMethod> Methods { get; set; } = new List<PlcMethod>();
        public IPlcBody Body { get; set; } = new PlcBodyST();

        private Guid _objectId;

        public PlcFunctionBlock(string name, Guid objectId)
        {
            Name = name;
            _objectId = objectId;
        }

        public void SetImplementationType(PlcPouType var)
        {
            switch (var)
            {
                case PlcPouType.ST:
                    Body = new PlcBodyST();
                    break;
            }
        }

        public XElement GetPouXml()
        {
            var fb = new XElement(PlcOpenNamespaces.Ns + "pou",
                    new XAttribute("name",Name),
                    new XAttribute("pouType","functionBlock")
                    );

            var inputVars = new XElement(PlcOpenNamespaces.Ns + "inputVars");
            var outputVars = new XElement(PlcOpenNamespaces.Ns + "outputVars");
            var localVars = new XElement(PlcOpenNamespaces.Ns + "localVars");

            foreach (PlcVariable var in Variables)
            {
                var newVar = new XElement(PlcOpenNamespaces.Ns + "variable",
                    new XAttribute("name", var.Name));

                if (var.DataType != PlcDataType.DERIVED)
                    newVar.Add(new XElement(PlcOpenNamespaces.Ns + "type",
                        new XElement(PlcOpenNamespaces.Ns + var.DataType.ToString())));
                else
                    newVar.Add(new XElement(PlcOpenNamespaces.Ns + "type",
                        new XElement(PlcOpenNamespaces.Ns + "derived",
                            new XAttribute("name", var.DerivedType))));

                newVar.Add(new XElement(PlcOpenNamespaces.Ns + "documentation",
                    new XElement(PlcOpenNamespaces.Xhtmlns + "xhtml", " " + var.Comment)));

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

            var pouInterface = new XElement(PlcOpenNamespaces.Ns + "interface");

            if (inputVars.HasElements) { pouInterface.Add(inputVars); }
            if (outputVars.HasElements) { pouInterface.Add(outputVars); }
            if (localVars.HasElements) { pouInterface.Add(localVars); }

            var addData = new XElement(PlcOpenNamespaces.Ns + "addData");

            foreach(PlcMethod method in Methods)
            {
                addData.Add(method.GetXml());
            }

            var objectId = new XElement(PlcOpenNamespaces.Ns + "data",
                new XAttribute("name", "http://www.3s-software.com/plcopenxml/objectid"),
                new XAttribute("handleUnknown","discard"),
                new XElement(PlcOpenNamespaces.Ns + "ObjectId",_objectId.ToString()));
          
            addData.Add(objectId);

            var body = Body.GetXml();

            fb.Add(pouInterface);
            fb.Add(body);
            fb.Add(addData);

            return fb;
        }

        public XElement GetProjectStructureXml()
        {
            var projectStructureObject = new XElement(PlcOpenNamespaces.Ns + "Object",
                new XAttribute("Name", Name),
                new XAttribute("ObjectId", _objectId.ToString()));

            foreach(PlcMethod method in Methods)
            {
                projectStructureObject.Add(new XElement(PlcOpenNamespaces.Ns + "Object",
                    new XAttribute("Name", method.Name),
                    new XAttribute("ObjectId", method.ObjectId.ToString())));
            }

            return projectStructureObject;
        }
    }
}
