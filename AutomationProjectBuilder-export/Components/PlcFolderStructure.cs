using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Export.Components
{
    public class PlcFolderStructure
    {
        public string Name { get; set; }
        public List<PlcFunctionBlock> PlcFunctionBlocks { get; set; } = new List<PlcFunctionBlock>();
        public List<PlcFolderStructure> SubFolders { get; set; } = new List<PlcFolderStructure>();

        public PlcFolderStructure(string name)
        {
            Name = name;
        }
    }
}
