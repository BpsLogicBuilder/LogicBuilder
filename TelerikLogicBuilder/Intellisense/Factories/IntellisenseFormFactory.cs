using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal class IntellisenseFormFactory : IIntellisenseFormFactory
    {
        public IConfigureClassFunctionsHelperForm GetConfigureClassFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            return new ConfigureClassFunctionsHelperForm
            (
                Program.ServiceProvider.GetRequiredService<IChildConstructorFinderFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionManager>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseFactory>(),
                Program.ServiceProvider.GetRequiredService<IMemberAttributeReader>(),
                Program.ServiceProvider.GetRequiredService<IMultipleChoiceParameterValidator>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                existingConstructors,
                existingVariables,
                helperStatus
            );
        }

        public IConfigureClassVariablesHelperForm GetConfigureClassVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            return new ConfigureClassVariablesHelperForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IVariablesManager>(),
                existingVariables,
                helperStatus
            );
        }

        public IConfigureConstructorsHelperForm GetConfigureConstructorsHelperForm(IDictionary<string, Constructor> existingConstructors, ConstructorHelperStatus? helperStatus, string? constructorToUpdate = null)
        {
            return new ConfigureConstructorsHelperForm
            (
                Program.ServiceProvider.GetRequiredService<IChildConstructorFinderFactory>(),
                Program.ServiceProvider.GetRequiredService<IConstructorManager>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IExistingConstructorFinder>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IStringHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                existingConstructors,
                helperStatus,
                constructorToUpdate
            );
        }

        public IConfigureFunctionsHelperForm GetConfigureFunctionsHelperForm(IDictionary<string, Constructor> existingConstructors, IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            return new ConfigureFunctionsHelperForm
            (
                Program.ServiceProvider.GetRequiredService<IChildConstructorFinderFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionManager>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseFactory>(),
                Program.ServiceProvider.GetRequiredService<IMultipleChoiceParameterValidator>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                existingConstructors,
                existingVariables,
                helperStatus
            );
        }

        public IConfigureVariablesHelperForm GetConfigureVariablesHelperForm(IDictionary<string, VariableBase> existingVariables, HelperStatus? helperStatus)
        {
            return new ConfigureVariablesHelperForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IVariablesManager>(),
                existingVariables,
                helperStatus
            );
        }

        public IIncludesHelperForm GetIncludesHelperForm(string className)
        {
            return new IncludesHelperForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IIntellisenseFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IStringHelper>(),
                className
            );
        }
    }
}
