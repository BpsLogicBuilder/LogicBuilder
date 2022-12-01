using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories
{
    internal interface ILiteralDomainItemFactory
    {
        LiteralDomainItem GetLiteralDomainItem(string item, Type type);
    }
}
