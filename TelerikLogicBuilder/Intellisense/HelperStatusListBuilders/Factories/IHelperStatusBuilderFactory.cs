﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories
{
    internal interface IHelperStatusBuilderFactory
    {
        IConstructorHelperStatusBuilder GetConstructorHelperStatusBuilder(IConfigureConstructorsForm configureConstructorsForm);
        IFunctionHelperStatusBuilder GetFunctionHelperStatusBuilder(IConfigureFunctionsForm configureFunctionsForm);
        IReferenceNodeListBuilder GetReferenceNodeListBuilder(IConfigurationForm configurationForm);
        IVariableHelperStatusBuilder GetVariableHelperStatusBuilder(IConfigureVariablesForm configureVariablesForm);
        IVariableNodeBuilder GetVariableNodeBuilder(IConfigurationForm configurationForm);
    }
}
