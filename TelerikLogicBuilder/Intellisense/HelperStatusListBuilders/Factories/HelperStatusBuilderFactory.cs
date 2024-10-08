﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories
{
    internal class HelperStatusBuilderFactory : IHelperStatusBuilderFactory
    {
        private readonly Func<IConfigureConstructorsForm, IConstructorHelperStatusBuilder> _getConstructorHelperStatusBuilder;
        private readonly Func<IConfigureFunctionsForm, IFunctionHelperStatusBuilder> _getFunctionHelperStatusBuilder;
        private readonly Func<IConfigurationForm, IReferenceNodeListBuilder> _getReferenceNodeListBuilder;
        private readonly Func<IConfigureVariablesForm, IVariableHelperStatusBuilder> _getVariableHelperStatusBuilder;
        private readonly Func<IConfigurationForm, IVariableNodeBuilder> _getVariableNodeBuilder;

        public HelperStatusBuilderFactory(
            Func<IConfigureConstructorsForm, IConstructorHelperStatusBuilder> getConstructorHelperStatusBuilder,
            Func<IConfigureFunctionsForm, IFunctionHelperStatusBuilder> getFunctionHelperStatusBuilder,
            Func<IConfigurationForm, IReferenceNodeListBuilder> getReferenceNodeListBuilder,
            Func<IConfigureVariablesForm, IVariableHelperStatusBuilder> getVariableHelperStatusBuilder,
            Func<IConfigurationForm, IVariableNodeBuilder> getVariableNodeBuilder)
        {
            _getConstructorHelperStatusBuilder = getConstructorHelperStatusBuilder;
            _getFunctionHelperStatusBuilder = getFunctionHelperStatusBuilder;
            _getReferenceNodeListBuilder = getReferenceNodeListBuilder;
            _getVariableHelperStatusBuilder = getVariableHelperStatusBuilder;
            _getVariableNodeBuilder = getVariableNodeBuilder;
        }

        public IConstructorHelperStatusBuilder GetConstructorHelperStatusBuilder(IConfigureConstructorsForm configureConstructorsForm)
            => _getConstructorHelperStatusBuilder(configureConstructorsForm);

        public IFunctionHelperStatusBuilder GetFunctionHelperStatusBuilder(IConfigureFunctionsForm configureFunctionsForm)
            => _getFunctionHelperStatusBuilder(configureFunctionsForm);

        public IReferenceNodeListBuilder GetReferenceNodeListBuilder(IConfigurationForm configurationForm)
            => _getReferenceNodeListBuilder(configurationForm);

        public IVariableHelperStatusBuilder GetVariableHelperStatusBuilder(IConfigureVariablesForm configureVariablesForm)
            => _getVariableHelperStatusBuilder(configureVariablesForm);

        public IVariableNodeBuilder GetVariableNodeBuilder(IConfigurationForm configurationForm)
            => _getVariableNodeBuilder(configurationForm);
    }
}
