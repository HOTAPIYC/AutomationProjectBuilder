using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Data.Model;
using AutomationProjectBuilder.Export.Model;
using System;
using System.Linq;

namespace AutomationProjectBuilder.Export
{
    public class PlcExport
    {
        private ISettings _settings;
        private IDataService _dataservice;

        public PlcExport(IDataService dataservice)
        {
            _dataservice = dataservice;
            _settings = dataservice.Settings;

            _dataservice.ExportRequested += OnExportRequest;
        }
        
        private void OnExportRequest(object sender, EventArgs e)
        {
            CreatePlcCode();
        }

        private void CreatePlcCode()
        {
            var export = new PlcOpenXmlDoc((string)_settings["ProjectName"]);

            var pouFolder = CreatePlcFunctionBlocks(new PlcFolder((string)_settings["ProjectName"]), _dataservice.GetProjectRoot());

            export.SetProjectStructure(pouFolder);

            export.SaveXml((string)_settings["FilePathExport"]);
        }

        private PlcFolder CreatePlcFunctionBlocks(PlcFolder folder, ProjectModule module)
        {
            var functionblock = new PlcFunctionBlock(GetFunctionBlockName(module), module.Id);

            foreach(IListItem function in module.Functions)
            {
                functionblock.Methods.Add(new PlcMethod(GetMethodName(function), PlcDataType.BOOL));
            }

            folder.PlcFunctionBlocks.Add(functionblock);

            var hasSubfolderModules = folder.SubFolders.Exists(x => x.Name == "Modules");
            var hasSubfolderRecipePhases = folder.SubFolders.Exists(x => x.Name == "RecipePhases");

            var hasSubModules = module.SubModules.Where(x => x.Type != ModuleType.RecipePhase).ToList().Count > 0;
            var hasRecipePhases = module.SubModules.Where(x => x.Type == ModuleType.RecipePhase).ToList().Count > 0;

            if (!hasSubfolderModules && hasSubModules) folder.SubFolders.Add(new PlcFolder("Modules"));
            if (!hasSubfolderRecipePhases && hasRecipePhases) folder.SubFolders.Add(new PlcFolder("RecipePhases"));

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
            var name = projectModule.Name.Replace(" ", "");

            switch (projectModule.Type)
            {
                case ModuleType.CtrlModule:
                    return string.Format("{0}_{1}", "CM", name);
                case ModuleType.EquipmentModule:
                    return string.Format("{0}_{1}", "EQM", name);
                case ModuleType.ProcessCell:
                    return string.Format("{0}_{1}", "PC", name);
                default:
                    return name;
            }
        }

        private string GetMethodName(IListItem function)
        {
            return string.Format("{0}_{1}", "M", function.Name.Replace(" ", ""));
        }
    }
}
