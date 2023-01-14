using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConstructorControlCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureConstructorControlCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureConstructorControlCommandFactory, ConfigureConstructorControlCommandFactory>()
                .AddTransient<Func<IConfigureConstructorControl, EditConstructorTypeNameCommand>>
                (
                    provider =>
                    configureConstructorControl => new EditConstructorTypeNameCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorControl
                    )
                )
                .AddTransient<Func<IConfigureConstructorControl, EditGenericArgumentsCommand>>
                (
                    provider =>
                    configureConstructorControl => new EditGenericArgumentsCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorControl
                    )
                );
        }
    }
}
