using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureVariablesCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureVariablesCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureVariablesForm, AddLiteralListVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddLiteralListVariableCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, AddLiteralVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddLiteralVariableCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, AddObjectListVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddObjectListVariableCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, AddObjectVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddObjectVariableCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesAddFolderCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesAddFolderCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesAddMembersCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesAddMembersCommand
                    (
                        configureVariablesForm
                    )
                )
                .AddTransient<IConfigureVariablesCommandFactory, ConfigureVariablesCommandFactory>()
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesCopyXmlCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesCutCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesCutCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesDeleteCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesDeleteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesHelperCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesHelperCommand
                    (
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesImportCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesImportCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ILoadVariables>(),
                        provider.GetRequiredService<IPathHelper>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesPasteCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesPasteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                );
        }
    }
}
