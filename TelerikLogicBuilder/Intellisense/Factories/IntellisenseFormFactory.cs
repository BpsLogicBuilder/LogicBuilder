using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal class IntellisenseFormFactory : IIntellisenseFormFactory
    {
        private IDisposable? _scopedService;

        private readonly Func<IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureFunctionsHelperForm> _getConfigureFunctionsHelperForm;
        private readonly Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureVariablesHelperForm> _getConfigureVariablesHelperForm;

        public IntellisenseFormFactory(
            Func<IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureFunctionsHelperForm> getConfigureFunctionsHelperForm,
            Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureVariablesHelperForm> getConfigureVariablesHelperForm)
        {
            _getConfigureFunctionsHelperForm = getConfigureFunctionsHelperForm;
            _getConfigureVariablesHelperForm = getConfigureVariablesHelperForm;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }

        public IConfigureFunctionsHelperForm GetConfigureFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            _scopedService = _getConfigureFunctionsHelperForm(existingConstructors, existingVariables, helperStatus);
            return (IConfigureFunctionsHelperForm)_scopedService;
        }

        public IConfigureVariablesHelperForm GetConfigureVariablesHelperForm(IDictionary<string, VariableBase> existingVariable, HelperStatus? helperStatus)
        {
            _scopedService = _getConfigureVariablesHelperForm(existingVariable, helperStatus);
            return (IConfigureVariablesHelperForm)_scopedService;
        }
    }
}
