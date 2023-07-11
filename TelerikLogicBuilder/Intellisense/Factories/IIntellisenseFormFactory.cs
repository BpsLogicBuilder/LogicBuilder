using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal interface IIntellisenseFormFactory
    {
        IConfigureClassFunctionsHelperForm GetConfigureClassFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
        IConfigureClassVariablesHelperForm GetConfigureClassVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
        IConfigureConstructorsHelperForm GetConfigureConstructorsHelperForm(IDictionary<string, Constructor> existingConstructors, ConstructorHelperStatus? helperStatus, string? constructorToUpdate = null);
        IConfigureFunctionsHelperForm GetConfigureFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
        IConfigureVariablesHelperForm GetConfigureVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
        IIncludesHelperForm GetIncludesHelperForm(string className);
    }
}
