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
        private XElement _addData;

        private PlcFolderStructure _plcFolders { get; set; }

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
            _addData = new XElement(PlcOpenNamespaces.Ns + "addData");
        }

        public void AddFunctionBlock(PlcFunctionBlock functionblock)
        {
            _pous.Add(functionblock.GetXml());
        }

        public void SetProjectStructure(PlcFolderStructure folders)
        {
            _plcFolders = folders;
        }
        private void AddFunctionBlocks(PlcFolderStructure folders)
        {
            foreach(PlcFunctionBlock functionblock in folders.PlcFunctionBlocks)
            {
                AddFunctionBlock(functionblock);
            }

            foreach(PlcFolderStructure subfolder in folders.SubFolders)
            {
                AddFunctionBlocks(subfolder);
            }
        }

        public void SaveXml(string filepath)
        {
            XDocument doc = new XDocument();

            AddFunctionBlocks(_plcFolders);
            CreateProjectStructure(_plcFolders);

            _types.Add(_dataTypes);
            _types.Add(_pous);

            _project.Add(_types);

            _project.Add(_instances);

            _project.Add(_addData);

            doc.Add(_project);
            doc.Save(filepath);
        }

        private void CreateProjectStructure(PlcFolderStructure plcStructure)
        {
            _addData.Add(new XElement(PlcOpenNamespaces.Ns + "data",
                new XAttribute("name", "http://www.3s-software.com/plcopenxml/projectstructure"),
                new XAttribute("handleUnknown","discard"),
                AddFolder(new XElement(PlcOpenNamespaces.Ns + "ProjectStructure"), plcStructure)));
        }

        private XElement AddFolder(XElement folderNode, PlcFolderStructure folder)
        {
            folderNode.Add(new XElement(PlcOpenNamespaces.Ns + "Folder",
                new XAttribute("Name", folder.Name)));

            var test = folderNode.Element(PlcOpenNamespaces.Ns + "Folder");

            foreach (PlcFunctionBlock functionblock in folder.PlcFunctionBlocks)
            {                   
                folderNode.Element(PlcOpenNamespaces.Ns + "Folder").Add(new XElement(PlcOpenNamespaces.Ns + "Object",
                    new XAttribute("Name", functionblock.Name),
                    new XAttribute("ObjectId", functionblock.ObjectId)));
            }
            
            foreach(PlcFolderStructure subfolder in folder.SubFolders)
            {
                AddFolder(folderNode.Element(PlcOpenNamespaces.Ns + "Folder"), subfolder);
            }

            return folderNode;
        }
    }
}
