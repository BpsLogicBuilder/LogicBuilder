using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal interface ISelectRulesFormFactory : IDisposable
    {
        SelectRulesForm GetScopedService(string applicationName);
    }
}
