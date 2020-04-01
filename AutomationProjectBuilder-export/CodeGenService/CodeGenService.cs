using AutomationProjectBuilder.Export.Components;
using AutomationProjectBuilder.Interfaces;
using AutomationProjectBuilder.Model;
using System.Linq;

namespace AutomationProjectBuilder.Export.CodeGenerator
{
    public class CodeGenService : ICodeGenService
    {
        private ISettings _settings;
        private IDataService _dataservice;

        public CodeGenService(IDataService dataservice)
        {
            _dataservice = dataservice;
            _settings = dataservice.Settings;

        }
        
        public void CreatePlcCode()
        {
            var export = new PlcOpenXmlDoc((string)_settings["ProjectName"]);

            var plcFolderStruct = CreatePlcFunctionBlocks(new PlcFolderStructure((string)_settings["ProjectName"]), _dataservice.GetProjectRoot());

            export.SetProjectStructure(plcFolderStruct);

            export.SaveXml((string)_settings["ExportFilePath"]);
        }

        private PlcFolderStructure CreatePlcFunctionBlocks(PlcFolderStructure folder, ProjectModule module)
        {
            var functionblock = new PlcFunctionBlock(GetFunctionBlockName(module), module.Id);

            folder.PlcFunctionBlocks.Add(functionblock);

            var hasSubfolderModules = folder.SubFolders.Exists(x => x.Name == "Modules");
            var hasSubfolderRecipePhases = folder.SubFolders.Exists(x => x.Name == "RecipePhases");

            var hasSubModules = module.SubModules.Where(x => x.Type != ModuleType.RecipePhase).ToList().Count > 0;
            var hasRecipePhases = module.SubModules.Where(x => x.Type == ModuleType.RecipePhase).ToList().Count > 0;

            if (!hasSubfolderModules && hasSubModules) folder.SubFolders.Add(new PlcFolderStructure("Modules"));
            if (!hasSubfolderRecipePhases && hasRecipePhases) folder.SubFolders.Add(new PlcFolderStructure("RecipePhases"));

            foreach (ProjectModule submodule in module.SubModules.Where(x => x.Type != ModuleType.RecipePhase).ToList())
            {
                CreatePlcFunctionBlocks(folder.SubFolders.Where(x => x.Name == "Modules").FirstOrDefault(), submodule);
            }

            foreach (ProjectModule submodule in module.SubModules.Where(x => x.Type == ModuleType.RecipePhase).ToList())
            {
                CreatePlcFunctionBlocks(folder.SubFolders.Where(x => x.Name == "RecipePhases").FirstOrDefault(), submodule);
            }

            return folder;
        }

        private string GetFunctionBlockName(ProjectModule projectModule)
        {
            switch (projectModule.Type)
            {
                case ModuleType.CtrlModule:
                    return string.Format("{0}_{1}", "CM", projectModule.Name);
                case ModuleType.EquipmentModule:
                    return string.Format("{0}_{1}", "EQM", projectModule.Name);
                case ModuleType.ProcessCell:
                    return string.Format("{0}_{1}", "PC", projectModule.Name);
                default:
                    return projectModule.Name;
            }
        }
    }
}
