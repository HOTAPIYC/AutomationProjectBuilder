using AutomationProjectBuilder.Export.Enums;
using AutomationProjectBuilder.Export.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Components
{
    public class PlcMethod
    {
        public string Name { get; set; }
        public Guid ObjectId { get; set; }
        public PlcDataType ReturnType { get; set; }
        public string DerivedReturnType { get; set; }
        public List<PlcVariable> Variables { get; set; }
        public IPlcBody Body { get; set; } = new PlcBodyST();

        public PlcMethod(string name, PlcDataType returnType)
        {
            Name = name;
            ReturnType = returnType;
            ObjectId = Guid.NewGuid();
        }

        public PlcMethod(string name, string derivedReturnType)
        {
            Name = name;
            ReturnType = PlcDataType.DERIVED;
            DerivedReturnType = derivedReturnType;
            ObjectId = Guid.NewGuid();
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

        public XElement GetXml()
        {
            var meth = new XElement(PlcOpenNamespaces.Ns + "data",
                new XAttribute("name", "http://www.3s-software.com/plcopenxml/method"),
                new XAttribute("handleUnknown", "implementation"),
                new XElement(PlcOpenNamespaces.Ns + "Method",
                    new XAttribute("name",Name),
                    new XAttribute("ObjectId",ObjectId.ToString()))
                );

            var returnVar = new XElement(PlcOpenNamespaces.Ns + "returnType");

            if (ReturnType == PlcDataType.DERIVED)
                returnVar.Add(new XElement(PlcOpenNamespaces.Ns + DerivedReturnType));
            else
                returnVar.Add(new XElement(PlcOpenNamespaces.Ns + ReturnType.ToString()));
            

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

            pouInterface.Add(returnVar);

            if (inputVars.HasElements) { pouInterface.Add(inputVars); }
            if (outputVars.HasElements) { pouInterface.Add(outputVars); }
            if (localVars.HasElements) { pouInterface.Add(localVars); }

            meth.Element(PlcOpenNamespaces.Ns + "Method").Add(pouInterface);

            var body = Body.GetXml();

            meth.Element(PlcOpenNamespaces.Ns + "Method").Add(body);

            return meth;
        }
    }
}
