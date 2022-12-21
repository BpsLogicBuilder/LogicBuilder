using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
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
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, AddLiteralVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddLiteralVariableCommand
                    (
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, AddObjectListVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddObjectListVariableCommand
                    (
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, AddObjectVariableCommand>>
                (
                    provider =>
                    configureVariablesForm => new AddObjectVariableCommand
                    (
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesAddFolderCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesAddFolderCommand
                    (
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
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesCutCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesCutCommand
                    (
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesDeleteCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesDeleteCommand
                    (
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
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesPasteCommand>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesPasteCommand
                    (
                        configureVariablesForm
                    )
                );
        }
    }
}
