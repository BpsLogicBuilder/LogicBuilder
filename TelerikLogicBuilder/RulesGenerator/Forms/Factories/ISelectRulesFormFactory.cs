using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal interface ISelectRulesFormFactory : IDisposable
    {
        ISelectRulesForm GetScopedService(string applicationName);
    }
}
