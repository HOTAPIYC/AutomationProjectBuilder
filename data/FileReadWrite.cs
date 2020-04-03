using AutomationProjectBuilder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Data
{
    public static class FileReadWrite
    {
        public static void CreateFile(ProjectModule projectModule, string filePath)
        {
            var doc = new XDocument();

            doc.Add(CreateXmlModules(projectModule));

            doc.Save(filePath);
        }

        private static XElement CreateXmlModules(ProjectModule projectModule)
        {
            var module = new XElement("Module", 
                new XAttribute("Name", projectModule.Name),
                new XAttribute("Type", projectModule.Type.ToString()));

            var submodules = new XElement("SubModules");

            foreach(ProjectModule subitem in projectModule.SubModules)
            {
                submodules.Add(CreateXmlModules(subitem));
            }

            var functions = new XElement("Functions");

            foreach(ModuleFunction function in projectModule.Functions)
            {
                functions.Add(new XElement("Function",
                    new XAttribute("Name", function.Name),
                    new XAttribute("Description", function.Description)));
            }

            var parameters = new XElement("Parameters",
                new XAttribute("Group", projectModule.NameParamGrp),
                new XAttribute("Set", projectModule.NameParamSet));

            foreach(ModuleParameter value in projectModule.Parameters)
            {
                parameters.Add(new XElement("Parameter",
                    new XAttribute("Name", value.Name), value.Value));
            }

            module.Add(submodules);
            module.Add(functions);
            module.Add(parameters);

            return module;
        }

        public static ProjectModule ReadFile(string filePath)
        {
            var doc = XDocument.Load(filePath);

            return ReadXmlModules(doc.Elements("Module").FirstOrDefault());
        }

        private static ProjectModule ReadXmlModules(XElement xmlModule)
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
                projectModule.Functions.Add(new ModuleFunction(projectModule.Id, 
                    (string)xmlFunction.Attribute("Name"), 
                    (string)xmlFunction.Attribute("Description")));
            }

            var xmlParameters = xmlModule.Element("Parameters");

            projectModule.NameParamGrp = (string)xmlParameters.Attribute("Group");
            projectModule.NameParamSet = (string)xmlParameters.Attribute("Set");

            foreach(XElement xmlParameter in xmlParameters.Elements("Parameter").ToList())
            {
                projectModule.Parameters.Add(new ModuleParameter(projectModule.Id,
                    (string)xmlParameter.Attribute("Name"),
                    (object)xmlParameter.Value));
            }

            return projectModule;
        }

        public static List<ParameterGroup> ReadConfiguration(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var content = doc.Element("CustomConfiguration");

            var customconfig = new List<ParameterGroup>();

            foreach(XElement group in content.Elements("ConfigurationGroup"))
            {
                var customgroup = new ParameterGroup((string)group.Attribute("Name"));

                foreach(XElement config in group.Elements("Configuration"))
                {
                    var customconfiguration = new ParameterSet((string)config.Attribute("Name"));

                    foreach(XElement param in config.Element("Parameters").Elements("Parameter"))
                    {
                        customconfiguration.Parameters.Add(new ModuleParameter((string)param.Attribute("Name")));
                    }

                    customgroup.Configurations.Add(customconfiguration);
                }

                customconfig.Add(customgroup);
            }

            return customconfig;
        }
    }
}
