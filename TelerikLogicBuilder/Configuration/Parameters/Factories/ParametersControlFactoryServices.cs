using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ParametersControlFactoryServices
    {
        internal static IServiceCollection AddParametersControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigurationForm, IConfigureGenericListParameterControl>>
                (
                    provider =>
                    configurationForm => new ConfigureGenericListParameterControl
                    (
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterControlValidatorFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm
                    )
                )
                .AddTransient<Func<IConfigurationForm, IConfigureGenericParameterControl>>
                (
                    provider =>
                    configurationForm => new ConfigureGenericParameterControl
                    (
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterControlValidatorFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm
                    )
                )
                .AddTransient<Func<IConfigurationForm, IConfigureLiteralListParameterControl>>
                (
                    provider =>
                    configurationForm => new ConfigureLiteralListParameterControl
                    (
                        provider.GetRequiredService<IConfigureLiteralListParameterCommandFactory>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterControlValidatorFactory>(),
                        provider.GetRequiredService<IParametersXmlParser>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm
                    )
                )
                .AddTransient<Func<IConfigurationForm, IConfigureLiteralParameterControl>>
                (
                    provider =>
                    configurationForm => new ConfigureLiteralParameterControl
                    (
                        provider.GetRequiredService<IConfigureLiteralParameterCommandFactory>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterControlValidatorFactory>(),
                        provider.GetRequiredService<IParametersXmlParser>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm
                    )
                )
                .AddTransient<Func<IConfigurationForm, IConfigureObjectListParameterControl>>
                (
                    provider =>
                    configurationForm => new ConfigureObjectListParameterControl
                    (
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterControlValidatorFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm
                    )
                )
                .AddTransient<Func<IConfigurationForm, IConfigureObjectParameterControl>>
                (
                    provider =>
                    configurationForm => new ConfigureObjectParameterControl
                    (
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterControlValidatorFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm
                    )
                )
                .AddTransient<IParametersControlFactory, ParametersControlFactory>();
        }
    }
}
