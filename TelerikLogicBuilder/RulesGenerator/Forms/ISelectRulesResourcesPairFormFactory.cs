using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ISelectRulesResourcesPairFormFactory : IDisposable
    {
        SelectRulesResourcesPairForm GetScopedService(string applicationName);
    }
}
