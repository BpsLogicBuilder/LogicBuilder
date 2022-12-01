using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories
{
    internal interface IConfigureLiteralListDefaultValueCommandFactory
    {
        AddLiteralListDefaultValueItemCommand GetAddLiteralListDefaultValueItemCommand(IConfigureLiteralListDefaultValueControl configureLiteralListDefaultValueControl);
        UpdateLiteralListDefaultValueItemCommand GetUpdateLiteralListDefaultValueItemCommand(IConfigureLiteralListDefaultValueControl configureLiteralListDefaultValueControl);
    }
}
