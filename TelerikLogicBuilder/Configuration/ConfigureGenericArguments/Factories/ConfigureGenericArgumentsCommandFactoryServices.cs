using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureGenericArgumentsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureGenericArgumentsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureGenericArgumentsCommandFactory, ConfigureGenericArgumentsCommandFactory>()
                .AddTransient<Func<IConfigureGenericArgumentsForm, ReplaceWithListOfLiteralsParameterCommand>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ReplaceWithListOfLiteralsParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGenericConfigManager>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, ReplaceWithListOfObjectsParameterCommand>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ReplaceWithListOfObjectsParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGenericConfigManager>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, ReplaceWithLiteralParameterCommand>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ReplaceWithLiteralParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGenericConfigManager>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, ReplaceWithObjectParameterCommand>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ReplaceWithObjectParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGenericConfigManager>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureGenericArgumentsForm
                    )
                );
        }
    }
}
