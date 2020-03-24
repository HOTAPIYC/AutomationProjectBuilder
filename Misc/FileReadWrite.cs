using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Misc
{
    public class FileReadWrite
    {
        public ProjectItem RootModule { get; set; }
        public List<ModuleFunction> ModuleFunctions { get; set; } = new List<ModuleFunction>();

        public void CreateFile(ProjectItem project, List<ModuleFunction> moduleFunctions, string filePath)
        {
            var doc = new XDocument();

            doc.Add(CreateXmlModules(project, moduleFunctions));

            doc.Save(filePath);
        }

        private XElement CreateXmlModules(ProjectItem item, List<ModuleFunction> moduleFunctions)
        {
            var module = new XElement("Module", 
                new XAttribute("Name", item.Name),
                new XAttribute("Type", item.Type.ToString()));

            var submodules = new XElement("SubModules");

            foreach(ProjectItem subitem in item.SubItems)
            {
                submodules.Add(CreateXmlModules(subitem, moduleFunctions));
            }

            var functions = new XElement("Functions");

            foreach(ModuleFunction function in moduleFunctions)
            {
                if(function.ModuleId == item.Id)
                {
                    functions.Add(new XElement("Function", function.Name));
                }
            }

            module.Add(submodules);
            module.Add(functions);

            return module;
        }

        public void ReadFile(string filePath)
        {
            var doc = XDocument.Load(filePath);

            RootModule = ReadXmlModules(doc.Elements("Module").FirstOrDefault());
        }

        private ProjectItem ReadXmlModules(XElement xmlModule)
        {
            ItemTypeISA88 moduleType;
            Enum.TryParse((string)xmlModule.Attribute("Type"), out moduleType);

            var projectModule = new ProjectItem((string)xmlModule.Attribute("Name"), moduleType);

            var xmlSubModules = xmlModule.Element("SubModules");

            foreach(XElement xmlSubModule in xmlSubModules.Elements("Module").ToList())
            {
                projectModule.SubItems.Add(ReadXmlModules(xmlSubModule));
            }

            var xmlFunctions = xmlModule.Element("Functions");

            foreach(XElement xmlFunction in xmlFunctions.Elements("Function").ToList())
            {
                ModuleFunctions.Add(new ModuleFunction(projectModule.Id, (string)xmlFunction.Value));
            }

            return projectModule;
        }
    }
}
