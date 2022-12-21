using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralListVariableCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralListVariableCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureLiteralListVariableCommandFactory, ConfigureLiteralListVariableCommandFactory>()
                .AddTransient<Func<IConfigureLiteralListVariableControl, UpdateLiteralListVariableDefaultValueCommand>>
                (
                    provider =>
                    literalListVariableControl => new UpdateLiteralListVariableDefaultValueCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListVariableControl
                    )
                )
                .AddTransient<Func<IConfigureLiteralListVariableControl, UpdateLiteralListVariableDomainCommand>>
                (
                    provider =>
                    literalListVariableControl => new UpdateLiteralListVariableDomainCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListVariableControl
                    )
                );
        }
    }
}
