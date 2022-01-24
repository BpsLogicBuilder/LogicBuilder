using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal interface IComparableParameter
    {
        string Name { get; }
        bool UseForEquality { get; }
        bool UseForHashCode { get; }
        bool UseForToString { get; }
        ParameterCategory ParameterCategory { get; }
    }
}
