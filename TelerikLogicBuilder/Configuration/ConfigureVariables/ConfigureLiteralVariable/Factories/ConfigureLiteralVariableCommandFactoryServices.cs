using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralVariableCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralVariableCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureLiteralVariableCommandFactory, ConfigureLiteralVariableCommandFactory>()
                .AddTransient<Func<IConfigureLiteralVariableControl, UpdateLiteralVariableDomainCommand>>
                (
                    provider =>
                    literalVariableControl => new UpdateLiteralVariableDomainCommand
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalVariableControl
                    )
                );
        }
    }
}
