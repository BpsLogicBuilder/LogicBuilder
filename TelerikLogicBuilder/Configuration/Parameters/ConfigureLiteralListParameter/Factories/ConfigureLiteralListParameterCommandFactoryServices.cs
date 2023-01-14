using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralListParameterCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralListParameterCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureLiteralListParameterCommandFactory, ConfigureLiteralListParameterCommandFactory>()
                .AddTransient<Func<IConfigureLiteralListParameterControl, UpdateLiteralListParameterDefaultValueCommand>>
                (
                    provider =>
                    literalListParameterControl => new UpdateLiteralListParameterDefaultValueCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListParameterControl
                    )
                )
                .AddTransient<Func<IConfigureLiteralListParameterControl, UpdateLiteralListParameterDomainCommand>>
                (
                    provider =>
                    literalListParameterControl => new UpdateLiteralListParameterDomainCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListParameterControl
                    )
                );
        }
    }
}
