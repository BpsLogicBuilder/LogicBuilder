using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal interface ISelectRulesResourcesPairFormFactory : IDisposable
    {
        SelectRulesResourcesPairForm GetScopedService(string applicationName);
    }
}
