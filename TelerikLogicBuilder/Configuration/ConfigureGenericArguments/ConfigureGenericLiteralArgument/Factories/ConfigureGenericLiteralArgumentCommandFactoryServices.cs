using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureGenericLiteralArgumentCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureGenericLiteralArgumentCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureGenericLiteralArgumentCommandFactory, ConfigureGenericLiteralArgumentCommandFactory>()
                .AddTransient<Func<IConfigureGenericLiteralArgumentControl, UpdateGenericLiteralDomainCommand>>
                (
                    provider =>
                    literalGenericArgumentControl => new UpdateGenericLiteralDomainCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalGenericArgumentControl
                    )
                );
        }
    }
}
