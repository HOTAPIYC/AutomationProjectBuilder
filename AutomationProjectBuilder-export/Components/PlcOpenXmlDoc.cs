using System;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Components
{
    public class PlcOpenXmlDoc
    {
        private XElement _project;
        private XElement _pous;
        private XElement _dataTypes;
        private XElement _types;
        private XElement _instances;
        
        public PlcOpenXmlDoc(string name)
        {
            // Create root
            _project = new XElement(PlcOpenNamespaces.Ns + "project",
                new XElement(PlcOpenNamespaces.Ns + "fileHeader",
                    new XAttribute("companyName", "Me"),
                    new XAttribute("productName", "AutomationProjectBuilder"),
                    new XAttribute("productVersion", "0.0.1"),
                    new XAttribute("creationDateTime", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"))),
                new XElement(PlcOpenNamespaces.Ns + "contentHeader",
                    new XAttribute("name", name),
                    new XAttribute("modificationDateTime", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss")),
                    new XElement(PlcOpenNamespaces.Ns + "coordinateInfo",
                        new XElement(PlcOpenNamespaces.Ns + "fbd",
                            new XElement(PlcOpenNamespaces.Ns + "scaling",
                                new XAttribute("x", "1"),
                                new XAttribute("y", "1"))),
                        new XElement(PlcOpenNamespaces.Ns + "ld",
                            new XElement(PlcOpenNamespaces.Ns + "scaling",
                                new XAttribute("x", "1"),
                                new XAttribute("y", "1"))),
                        new XElement(PlcOpenNamespaces.Ns + "sfc",
                            new XElement(PlcOpenNamespaces.Ns + "scaling",
                                new XAttribute("x", "1"),
                                new XAttribute("y", "1")))
                                )
                            )
                        );

            // Create sections
            _types = new XElement(PlcOpenNamespaces.Ns + "types");
            _dataTypes = new XElement(PlcOpenNamespaces.Ns + "dataTypes");
            _pous = new XElement(PlcOpenNamespaces.Ns + "pous");
            _instances = new XElement(PlcOpenNamespaces.Ns + "instances", 
                new XElement(PlcOpenNamespaces.Ns + "configurations"));
        }

        public void AddFunctionBlock(PlcFunctionBlock functionblock)
        {
            _pous.Add(functionblock.GetXml());
        }

        public void SaveXml(string filepath)
        {
            XDocument doc = new XDocument();

            _types.Add(_dataTypes);
            _types.Add(_pous);

            _project.Add(_types);

            _project.Add(_instances);

            doc.Add(_project);
            doc.Save(filepath);
        }
    }
}
