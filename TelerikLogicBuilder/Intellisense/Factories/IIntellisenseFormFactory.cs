﻿using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal interface IIntellisenseFormFactory : IDisposable
    {
        IConfigureConstructorsHelperForm GetConfigureConstructorsHelperForm(IDictionary<string, Constructor> existingConstructors, ConstructorHelperStatus? helperStatus, string? constructorToUpdate = null);
        IConfigureClassVariablesHelperForm GetConfigureClassVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
        IConfigureFunctionsHelperForm GetConfigureFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
        IConfigureVariablesHelperForm GetConfigureVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus);
    }
}