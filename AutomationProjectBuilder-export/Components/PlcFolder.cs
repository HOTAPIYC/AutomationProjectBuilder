using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Export.Components
{
    public class PlcFolder
    {
        public string Name { get; set; }
        public List<PlcFunctionBlock> PlcFunctionBlocks { get; set; } = new List<PlcFunctionBlock>();
        public List<PlcFolder> SubFolders { get; set; } = new List<PlcFolder>();

        public PlcFolder(string name)
        {
            Name = name;
        }
    }
}
