using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IGetPromptForLiteralDomainUpdate
    {
        string Get(Type domainType);
    }
}
