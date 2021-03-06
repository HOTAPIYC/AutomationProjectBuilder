﻿

namespace AutomationProjectBuilder.Export.Model
{
    public class PlcVariable
    {
        public string Name { get; set; }
        public PlcDataType DataType { get; set; }
        public string DerivedType { get; set; }
        public PlcVarType VarType { get; set; }
        public string InitVal { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public long LocalId { get; set; }

        public PlcVariable(PlcVarType varType, string name, PlcDataType dataType, string comment)
        {
            VarType = varType;
            Name = name;
            DataType = dataType;
            Comment = comment;
        }

        public PlcVariable(PlcVarType varType, string name, string derivedType, string comment)
        {
            VarType = varType;
            Name = name;
            DataType = PlcDataType.DERIVED;
            DerivedType = derivedType;
            Comment = comment;
        }
    }
}
