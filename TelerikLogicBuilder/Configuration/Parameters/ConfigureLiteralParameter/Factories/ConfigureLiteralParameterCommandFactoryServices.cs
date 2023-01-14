using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Command;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralParameterCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralParameterCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureLiteralParameterCommandFactory, ConfigureLiteralParameterCommandFactory>()
                .AddTransient<Func<IConfigureLiteralParameterControl, UpdateLiteralParameterDomainCommand>>
                (
                    provider =>
                    literalParameterControl => new UpdateLiteralParameterDomainCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalParameterControl
                    )
                );
        }
    }
}
