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

        private PlcFolder _pouFolders;

        public PlcOpenXmlDoc(string name)
        {
            _project = new XElement(PlcOpenNamespaces.Ns + "project",
                new XElement(PlcOpenNamespaces.Ns + "fileHeader",
                    new XAttribute("companyName", "OpenSource"),
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

            _types = new XElement(PlcOpenNamespaces.Ns + "types");
            _dataTypes = new XElement(PlcOpenNamespaces.Ns + "dataTypes");
            _pous = new XElement(PlcOpenNamespaces.Ns + "pous");
            _instances = new XElement(PlcOpenNamespaces.Ns + "instances", 
                new XElement(PlcOpenNamespaces.Ns + "configurations"));
            _addData = new XElement(PlcOpenNamespaces.Ns + "addData");
        }

        public void AddFunctionBlock(PlcFunctionBlock functionblock)
        {
            _pous.Add(functionblock.GetPouXml());
        }

        public void SetProjectStructure(PlcFolder folders)
        {
            _pouFolders = folders;
        }
        private void FolderContentToXml(PlcFolder folders)
        {
            foreach(PlcFunctionBlock functionblock in folders.PlcFunctionBlocks)
            {
                AddFunctionBlock(functionblock);
            }

            foreach(PlcFolder subfolder in folders.SubFolders)
            {
                FolderContentToXml(subfolder);
            }
        }

        public void SaveXml(string filepath)
        {
            XDocument doc = new XDocument();

            FolderContentToXml(_pouFolders);

            _addData.Add(FolderStructureToXml(_pouFolders));

            _types.Add(_dataTypes);
            _types.Add(_pous);

            _project.Add(_types);

            _project.Add(_instances);

            _project.Add(_addData);

            doc.Add(_project);
            doc.Save(filepath);
        }

        private XElement FolderStructureToXml(PlcFolder folder)
        {
            return new XElement(PlcOpenNamespaces.Ns + "data",
                new XAttribute("name", "http://www.3s-software.com/plcopenxml/projectstructure"),
                new XAttribute("handleUnknown","discard"),
                AddFolder(new XElement(PlcOpenNamespaces.Ns + "ProjectStructure"), folder));
        }

        private XElement AddFolder(XElement folderNode, PlcFolder folder)
        {
            folderNode.Add(new XElement(PlcOpenNamespaces.Ns + "Folder",
                new XAttribute("Name", folder.Name)));

            foreach (PlcFunctionBlock functionblock in folder.PlcFunctionBlocks)
            {                   
                folderNode.Element(PlcOpenNamespaces.Ns + "Folder").Add(functionblock.GetProjectStructureXml());
            }
            
            foreach(PlcFolder subfolder in folder.SubFolders)
            {
                AddFolder(folderNode.Element(PlcOpenNamespaces.Ns + "Folder"), subfolder);
            }

            return folderNode;
        }
    }
}
