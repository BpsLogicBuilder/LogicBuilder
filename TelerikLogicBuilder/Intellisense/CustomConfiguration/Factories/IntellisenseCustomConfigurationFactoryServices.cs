using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseCustomConfigurationFactoryServices
    {
        internal static IServiceCollection AddIntellisenseCustomConfigurationFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfiguredItemHelperForm, IArrayIndexerConfigurationControl>>
                (
                    provider =>
                    configuredItemHelperForm => new ArrayIndexerConfigurationControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationValidatorFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                )
                .AddTransient<Func<IConfiguredItemHelperForm, IFieldConfigurationControl>>
                (
                    provider =>
                    configuredItemHelperForm => new FieldConfigurationControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationValidatorFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                )
                .AddTransient<IFunctionConfigurationControl, FunctionConfigurationControl>()
                .AddTransient<Func<IConfiguredItemHelperForm, IIndexerConfigurationControl>>
                (
                    provider =>
                    configuredItemHelperForm => new IndexerConfigurationControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationValidatorFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                )
                .AddTransient<IIntellisenseCustomConfigurationControlFactory, IntellisenseCustomConfigurationControlFactory>()
                .AddTransient<IIntellisenseCustomConfigurationValidatorFactory, IntellisenseCustomConfigurationValidatorFactory>()
                .AddTransient<Func<IIntellisenseVariableConfigurationControl, IIntellisenseVariableControlsValidator>>
                (
                    provider =>
                    variableConfigurationControl => new IntellisenseVariableControlsValidator
                    (
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        variableConfigurationControl
                    )
                )
                .AddTransient<Func<IConfiguredItemHelperForm, IPropertyConfigurationControl>>
                (
                    provider =>
                    configuredItemHelperForm => new PropertyConfigurationControl
                    (
                       provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IIntellisenseCustomConfigurationValidatorFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                );
        }
    }
}
