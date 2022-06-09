using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ISelectRulesFormFactory : IDisposable
    {
        SelectRulesForm GetScopedService(string applicationName);
    }
}
