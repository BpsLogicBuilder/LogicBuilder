using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper
{
    internal interface IConfigureClassVariablesHelperForm : IConfiguredItemHelperForm
    {
        IList<VariableBase> Variables { get; }
    }
}
