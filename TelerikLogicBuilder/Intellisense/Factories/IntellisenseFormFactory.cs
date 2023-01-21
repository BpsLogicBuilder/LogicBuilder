using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
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

        private readonly Func<IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureClassFunctionsHelperForm> _getConfigureClassFunctionsHelperForm;
        private readonly Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureClassVariablesHelperForm> _getConfigureClassVariablesHelperForm;
        private readonly Func<IDictionary<string, Constructor>, ConstructorHelperStatus?, string?, IConfigureConstructorsHelperForm> _getConfigureConstructorsHelperForm;
        private readonly Func<IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureFunctionsHelperForm> _getConfigureFunctionsHelperForm;
        private readonly Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureVariablesHelperForm> _getConfigureVariablesHelperForm;

        public IntellisenseFormFactory(
            Func<IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureClassFunctionsHelperForm> getConfigureClassFunctionsHelperForm,
            Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureClassVariablesHelperForm> getConfigureClassVariablesHelperForm,
            Func<IDictionary<string, Constructor>, ConstructorHelperStatus?, string?, IConfigureConstructorsHelperForm> getConfigureConstructorsHelperForm,
            Func<IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureFunctionsHelperForm> getConfigureFunctionsHelperForm,
            Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureVariablesHelperForm> getConfigureVariablesHelperForm)
        {
            _getConfigureClassFunctionsHelperForm = getConfigureClassFunctionsHelperForm;
            _getConfigureClassVariablesHelperForm = getConfigureClassVariablesHelperForm;
            _getConfigureConstructorsHelperForm = getConfigureConstructorsHelperForm;
            _getConfigureFunctionsHelperForm = getConfigureFunctionsHelperForm;
            _getConfigureVariablesHelperForm = getConfigureVariablesHelperForm;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }

        public IConfigureClassFunctionsHelperForm GetConfigureClassFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            _scopedService = _getConfigureClassFunctionsHelperForm(existingConstructors, existingVariables, helperStatus);
            return (IConfigureClassFunctionsHelperForm)_scopedService;
        }

        public IConfigureClassVariablesHelperForm GetConfigureClassVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            _scopedService = _getConfigureClassVariablesHelperForm(existingVariables, helperStatus);
            return (IConfigureClassVariablesHelperForm)_scopedService;
        }

        public IConfigureConstructorsHelperForm GetConfigureConstructorsHelperForm(IDictionary<string, Constructor> existingConstructors, ConstructorHelperStatus? helperStatus, string? constructorToUpdate = null)
        {
            _scopedService = _getConfigureConstructorsHelperForm(existingConstructors, helperStatus, constructorToUpdate);
            return (IConfigureConstructorsHelperForm)_scopedService;
        }

        public IConfigureFunctionsHelperForm GetConfigureFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            _scopedService = _getConfigureFunctionsHelperForm(existingConstructors, existingVariables, helperStatus);
            return (IConfigureFunctionsHelperForm)_scopedService;
        }

        public IConfigureVariablesHelperForm GetConfigureVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            _scopedService = _getConfigureVariablesHelperForm(existingVariables, helperStatus);
            return (IConfigureVariablesHelperForm)_scopedService;
        }
    }
}
