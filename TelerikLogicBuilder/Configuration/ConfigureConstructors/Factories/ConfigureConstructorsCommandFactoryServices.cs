using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConstructorsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureConstructorsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddConstructorCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddConstructorCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddFolderCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddFolderCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddGenericParameterCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddGenericParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfGenericsParameterCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddListOfGenericsParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfLiteralsParameterCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddListOfLiteralsParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfObjectsParameterCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddListOfObjectsParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddLiteralParameterCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddLiteralParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsAddObjectParameterCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsAddObjectParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<IConfigureConstructorsCommandFactory, ConfigureConstructorsCommandFactory>()
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsCopyXmlCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsCutCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsCutCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsDeleteCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsDeleteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsHelperCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsHelperCommand
                    (
                        provider.GetRequiredService<IConstructorXmlParser>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsImportCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsImportCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ILoadConstructors>(),
                        provider.GetRequiredService<IPathHelper>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsPasteCommand>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsPasteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                );
        }
    }
}
