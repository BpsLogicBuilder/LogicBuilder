using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Factories;
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
                    literalListGenericArhumentControl => new UpdateGenericLiteralListDefaultValueCommand
                    (
                        literalListGenericArhumentControl
                    )
                )
                .AddTransient<Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDomainCommand>>
                (
                    provider =>
                    literalListGenericArhumentControl => new UpdateGenericLiteralListDomainCommand
                    (
                        literalListGenericArhumentControl
                    )
                );
        }
    }
}
