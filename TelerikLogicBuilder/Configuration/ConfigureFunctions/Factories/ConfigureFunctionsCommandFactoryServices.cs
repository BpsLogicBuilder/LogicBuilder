using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFunctionsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureFunctionsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddBinaryOperatorCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddBinaryOperatorCommand
                    (
                        provider.GetRequiredService<IFunctionFactory>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddClassMembersCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddClassMembersCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddDialogFunctionCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddDialogFunctionCommand
                    (
                        provider.GetRequiredService<IFunctionFactory>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddFolderCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddFolderCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddGenericParameterCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddGenericParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfGenericsParameterCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddListOfGenericsParameterCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfLiteralsParameterCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddListOfLiteralsParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfObjectsParameterCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddListOfObjectsParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddLiteralParameterCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddLiteralParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddObjectParameterCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddObjectParameterCommand
                    (
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsAddStandardFunctionCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsAddStandardFunctionCommand
                    (
                        provider.GetRequiredService<IFunctionFactory>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<IConfigureFunctionsCommandFactory, ConfigureFunctionsCommandFactory>()
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsCopyXmlCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsCutCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsCutCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsDeleteCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsDeleteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsHelperCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsHelperCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsImportCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsImportCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ILoadFunctions>(),
                        provider.GetRequiredService<IPathHelper>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsPasteCommand>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsPasteCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                );
        }
    }
}
