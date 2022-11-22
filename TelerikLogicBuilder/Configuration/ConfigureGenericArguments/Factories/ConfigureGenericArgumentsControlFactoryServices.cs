using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericObjectArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericObjectListArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureGenericArgumentsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureGenericArgumentsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureGenericArgumentsControlFactory, ConfigureGenericArgumentsControlFactory>()
                .AddTransient<Func<IConfigureGenericArgumentsForm, ConfigureGenericLiteralArgumentControl>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ConfigureGenericLiteralArgumentControl
                    (
                        provider.GetRequiredService<IConfigureGenericLiteralArgumentCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, ConfigureGenericLiteralListArgumentControl>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ConfigureGenericLiteralListArgumentControl
                    (
                        provider.GetRequiredService<IConfigureGenericLiteralListArgumentCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, ConfigureGenericObjectArgumentControl>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ConfigureGenericObjectArgumentControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, ConfigureGenericObjectListArgumentControl>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ConfigureGenericObjectListArgumentControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureGenericArgumentsForm
                    )
                );
        }
    }
}
