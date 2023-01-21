using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
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
                        provider.GetRequiredService<IConstructorXmlParser>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorControl
                    )
                )
                .AddTransient<Func<IConfigureConstructorControl, EditGenericArgumentsCommand>>
                (
                    provider =>
                    configureConstructorControl => new EditGenericArgumentsCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorControl
                    )
                );
        }
    }
}
