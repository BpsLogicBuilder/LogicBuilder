namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal interface IObjectParameter
    {
        string Name { get; }
        bool IsOptional { get; }
        string Comments { get; }
        string ObjectType { get; }
    }
}
