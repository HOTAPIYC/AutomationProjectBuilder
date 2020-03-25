using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutomationProjectBuilder.Model
{
    public enum ModuleType
    {
        [Description("Uncategorized")]
        Uncategorized,
        [Description("Basic control module")]
        BasicCtrlModule,
        [Description("Complex control module")]
        ComplexCtrlModule,
        [Description("Equipment module")]
        EquipmentModule,
        [Description("Process cell")]
        ProcessCell,
        [Description("Recipe phase")]
        RecipePhase
    }
}
