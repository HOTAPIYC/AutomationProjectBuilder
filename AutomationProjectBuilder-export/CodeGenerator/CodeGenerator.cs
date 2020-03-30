using AutomationProjectBuilder.Export.Components;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Export.CodeGenerator
{
    public static class CodeGenerator
    {
        public static void CreatePlcCode(ProjectModule projectModule, IDictionary<string,object> exportsettings)
        {
            var fb = new PlcFunctionBlock(projectModule.Name);

            var export = new PlcOpenXmlDoc((string)exportsettings["ProjectName"]);
            export.AddFunctionBlock(fb);
            export.SaveXml((string)exportsettings["FilePath"]);
        }
    }
}
