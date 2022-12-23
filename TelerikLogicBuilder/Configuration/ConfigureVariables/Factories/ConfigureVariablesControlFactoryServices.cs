using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureVariablesFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureVariablesRootNode;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureVariablesControlFactoryServices
    {
        internal static IServiceCollection AddConfigureVariablesControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureVariablesForm, IConfigureLiteralListVariableControl>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureLiteralListVariableControl
                    (
                        provider.GetRequiredService<IConfigureLiteralListVariableCommandFactory>(),
                        provider.GetRequiredService<IConfigureVariablesStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureLiteralVariableControl>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureLiteralVariableControl
                    (
                        provider.GetRequiredService<IConfigureLiteralVariableCommandFactory>(),
                        provider.GetRequiredService<IConfigureVariablesStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureObjectListVariableControl>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureObjectListVariableControl
                    (
                        provider.GetRequiredService<IConfigureVariablesStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureObjectVariableControl>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureObjectVariableControl
                    (
                        provider.GetRequiredService<IConfigureVariablesStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureVariablesFolderControl>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesFolderControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<IConfigureVariablesRootNodeControl, ConfigureVariablesRootNodeControl>()
                .AddTransient<IConfigureVariablesControlFactory, ConfigureVariablesControlFactory>();
        }
    }
}
