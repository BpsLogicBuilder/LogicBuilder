using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFragmentsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureFragmentsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsAddFolderCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsAddFolderCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsAddFragmentCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsAddFragmentCommand
                    (
                        provider.GetRequiredService<IFragmentItemFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<IConfigureFragmentsCommandFactory, ConfigureFragmentsCommandFactory>()
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsCopyXmlCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsCutCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsCutCommand
                    (
                        provider.GetRequiredService<IConfigureFragmentsCutImageHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsDeleteCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsDeleteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsImportCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsImportCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ILoadFragments>(),
                        provider.GetRequiredService<IPathHelper>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsPasteCommand>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsPasteCommand
                    (
                        provider.GetRequiredService<IConfigureFragmentsCutImageHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFragmentsForm
                    )
                );
        }
    }
}
