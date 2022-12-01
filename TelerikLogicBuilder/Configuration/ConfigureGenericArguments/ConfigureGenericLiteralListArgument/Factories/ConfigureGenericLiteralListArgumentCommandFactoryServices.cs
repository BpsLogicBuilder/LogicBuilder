using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureGenericLiteralListArgumentCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureGenericLiteralListArgumentCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureGenericLiteralListArgumentCommandFactory, ConfigureGenericLiteralListArgumentCommandFactory>()
                .AddTransient<Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDefaultValueCommand>>
                (
                    provider =>
                    literalListGenericArgumentControl => new UpdateGenericLiteralListDefaultValueCommand
                    (
                        literalListGenericArgumentControl
                    )
                )
                .AddTransient<Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDomainCommand>>
                (
                    provider =>
                    literalListGenericArgumentControl => new UpdateGenericLiteralListDomainCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListGenericArgumentControl
                    )
                );
        }
    }
}
