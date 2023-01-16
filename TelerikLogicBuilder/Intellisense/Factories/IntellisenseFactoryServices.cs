using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseFactoryServices
    {
        internal static IServiceCollection AddIntellisenseFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IDictionary<string, Constructor>, ConstructorHelperStatus?, string?, IConfigureConstructorsHelperForm>>
                (
                    provider =>
                    (existingConstructors, helperStatus, constructorToUpdate) => new ConfigureConstructorsHelperForm
                    (
                        provider.GetRequiredService<IChildConstructorFinderFactory>(),
                        provider.GetRequiredService<IConstructorManager>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IExistingConstructorFinder>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IIntellisenseFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        existingConstructors,
                        helperStatus,
                        constructorToUpdate
                    )
                )
                .AddTransient<Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureClassVariablesHelperForm>>
                (
                    provider =>
                    (existingVariables, helperStatus) => new ConfigureClassVariablesHelperForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                        provider.GetRequiredService<IIntellisenseFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IVariablesManager>(),
                        existingVariables,
                        helperStatus
                    )
                )
                .AddTransient<Func< IDictionary<string, Constructor>, IDictionary<string, VariableBase>, HelperStatus?, IConfigureFunctionsHelperForm>>
                (
                    provider =>
                    (existingConstructors, existingVariables, helperStatus) => new ConfigureFunctionsHelperForm
                    (
                        provider.GetRequiredService<IChildConstructorFinderFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionManager>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                        provider.GetRequiredService<IIntellisenseFactory>(),
                        provider.GetRequiredService<IMultipleChoiceParameterValidator>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        existingConstructors,
                        existingVariables,
                        helperStatus
                    )
                )
                .AddTransient<Func<IDictionary<string, VariableBase>, HelperStatus?, IConfigureVariablesHelperForm>>
                (
                    provider =>
                    (existingVariables, helperStatus) => new ConfigureVariablesHelperForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationControlFactory>(),
                        provider.GetRequiredService<IIntellisenseFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IVariablesManager>(),
                        existingVariables,
                        helperStatus
                    )
                )
                .AddTransient<Func<IConfigureConstructorsHelperForm, IIntellisenseConstructorsFormManager>>
                (
                    provider =>
                    configureConstructorsHelperForm => new IntellisenseConstructorsFormManager
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configureConstructorsHelperForm
                    )
                )
                .AddTransient<IIntellisenseFactory, IntellisenseFactory>()
                .AddTransient<IIntellisenseFormFactory, IntellisenseFormFactory>()
                .AddTransient<Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager>>
                (
                    provider =>
                    configuredItemHelperForm => new IntellisenseFunctionsFormManager
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                )
                .AddTransient<Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager>>
                (
                    provider =>
                    configuredItemHelperForm => new IntellisenseVariablesFormManager
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                );
        }
    }
}
