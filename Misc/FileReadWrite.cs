using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AutomationProjectBuilder.Misc
{
    public class FileReadWrite
    {
        public ProjectModule RootModule { get; set; }
        public List<ModuleFunction> ModuleFunctions { get; set; } = new List<ModuleFunction>();
        public List<ModuleParameter> ModuleParameters { get; set; } = new List<ModuleParameter>();

        // Project file

        public void CreateFile(
            ProjectModule projectModule, 
            List<ModuleFunction> moduleFunctions, 
            List<ModuleParameter> moduleParameters, string filePath)
        {
            var doc = new XDocument();

            doc.Add(CreateXmlModules(projectModule, moduleFunctions, moduleParameters));

            doc.Save(filePath);
        }

        private XElement CreateXmlModules(
            ProjectModule projectModule, 
            List<ModuleFunction> moduleFunctions, 
            List<ModuleParameter> moduleParameters)
        {
            var module = new XElement("Module", 
                new XAttribute("Name", projectModule.Name),
                new XAttribute("Type", projectModule.Type.ToString()));

            var submodules = new XElement("SubModules");

            foreach(ProjectModule subitem in projectModule.SubModules)
            {
                submodules.Add(CreateXmlModules(subitem, moduleFunctions, moduleParameters));
            }

            var functions = new XElement("Functions");

            foreach(ModuleFunction function in moduleFunctions)
            {
                if(function.ModuleId == projectModule.Id)
                {
                    functions.Add(new XElement("Function", 
                        new XAttribute("Name",function.Name), 
                        new XAttribute("Description",function.Description)));
                }
            }

            var parameters = new XElement("Parameters",
                new XAttribute("Group", projectModule.ParamGroup.Name),
                new XAttribute("Set", projectModule.ParamSet.Name));

            foreach(ModuleParameter value in moduleParameters)
            {
                if(value.ModuleId == projectModule.Id)
                {
                    parameters.Add(new XElement("Parameter",
                        new XAttribute("Name", value.Name), value.Value));
                }
            }

            module.Add(submodules);
            module.Add(functions);
            module.Add(parameters);

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

            var xmlParameters = xmlModule.Element("Parameters");

            projectModule.ParamGroup = new ParameterGroup((string)xmlParameters.Attribute("Group"));
            projectModule.ParamSet = new ParameterSet((string)xmlParameters.Attribute("Set"));

            foreach(XElement xmlParameter in xmlParameters.Elements("Parameter").ToList())
            {
                ModuleParameters.Add(new ModuleParameter(projectModule.Id,
                    (string)xmlParameter.Attribute("Name"),
                    (object)xmlParameter.Value));
            }

            return projectModule;
        }

        // Custom configuration

        public List<ParameterGroup> ReadConfiguration(string filePath)
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

                    foreach(XElement param in config.Elements("Parameter"))
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
