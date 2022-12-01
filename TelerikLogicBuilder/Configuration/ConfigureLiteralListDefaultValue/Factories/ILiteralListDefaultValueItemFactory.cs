using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories
{
    internal interface ILiteralListDefaultValueItemFactory
    {
        LiteralListDefaultValueItem GetLiteralListDefaultValueItem(string item, Type type);
    }
}
