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
        public ProjectModule RootModule { get; set; }
        public List<ModuleFunction> ModuleFunctions { get; set; } = new List<ModuleFunction>();

        // Project file

        public void CreateFile(ProjectModule project, List<ModuleFunction> moduleFunctions, string filePath)
        {
            var doc = new XDocument();

            doc.Add(CreateXmlModules(project, moduleFunctions));

            doc.Save(filePath);
        }

        private XElement CreateXmlModules(ProjectModule item, List<ModuleFunction> moduleFunctions)
        {
            var module = new XElement("Module", 
                new XAttribute("Name", item.Name),
                new XAttribute("Type", item.Type.ToString()));

            var submodules = new XElement("SubModules");

            foreach(ProjectModule subitem in item.SubModules)
            {
                submodules.Add(CreateXmlModules(subitem, moduleFunctions));
            }

            var functions = new XElement("Functions");

            foreach(ModuleFunction function in moduleFunctions)
            {
                if(function.ModuleId == item.Id)
                {
                    functions.Add(new XElement("Function", 
                        new XAttribute("Name",function.Name), 
                        new XAttribute("Description",function.Description)));
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

        private ProjectModule ReadXmlModules(XElement xmlModule)
        {
            ModuleType moduleType;
            Enum.TryParse((string)xmlModule.Attribute("Type"), out moduleType);

            var projectModule = new ProjectModule((string)xmlModule.Attribute("Name"), moduleType);

            var xmlSubModules = xmlModule.Element("SubModules");

            foreach(XElement xmlSubModule in xmlSubModules.Elements("Module").ToList())
            {
                projectModule.SubModules.Add(ReadXmlModules(xmlSubModule));
            }

            var xmlFunctions = xmlModule.Element("Functions");

            foreach(XElement xmlFunction in xmlFunctions.Elements("Function").ToList())
            {
                ModuleFunctions.Add(new ModuleFunction(projectModule.Id, 
                    (string)xmlFunction.Attribute("Name"), 
                    (string)xmlFunction.Attribute("Description")));
            }

            return projectModule;
        }

        // Custom configuration

        public List<ConfigGroup> ReadConfiguration(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var content = doc.Element("CustomConfiguration");

            var customconfig = new List<ConfigGroup>();

            foreach(XElement group in content.Elements("ConfigurationGroup"))
            {
                var customgroup = new ConfigGroup((string)group.Attribute("Name"));

                foreach(XElement config in group.Elements("Configuration"))
                {
                    var customconfiguration = new ModuleConfig((string)config.Attribute("Name"));

                    foreach(XElement param in config.Elements("Parameter"))
                    {
                        customconfiguration.Parameters.Add(new ConfigValue((string)param.Attribute("Name")));
                    }

                    customgroup.Configurations.Add(customconfiguration);
                }

                customconfig.Add(customgroup);
            }

            return customconfig;
        }
    }
}
