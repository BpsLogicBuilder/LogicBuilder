using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal interface ILiteralParameter
    {
        string Name { get; }
        bool IsOptional { get; }
        string Comments { get; }
        LiteralParameterType LiteralType { get; }
        LiteralParameterInputStyle Control { get; }
        string PropertySource { get; }
        string PropertySourceParameter { get; }
        List<string> Domain { get; }
    }
}
