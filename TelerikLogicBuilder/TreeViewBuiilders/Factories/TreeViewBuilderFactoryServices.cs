using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static  class TreeViewBuilderFactoryServices
    {
        internal static IServiceCollection AddTreeViewBuiilderFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsForm, IConfigureConstructorsTreeViewBuilder>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsTreeViewBuilder
                    (
                        provider.GetRequiredService<IConfigureConstructorsStateImageSetter>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, IConfigureFunctionsTreeViewBuilder>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsTreeViewBuilder
                    (
                        provider.GetRequiredService<IConfigureFunctionsStateImageSetter>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureVariablesTreeViewBuilder>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesTreeViewBuilder
                    (
                        provider.GetRequiredService<IConfigureVariablesStateImageSetter>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder>>
                (
                    provider =>
                    (documentNames, documentProfileErrors, expandedNodes) => new DocumentsExplorerTreeViewBuilder
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        documentNames,
                        documentProfileErrors,
                        expandedNodes
                    )
                )
                .AddTransient<Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder>>
                (
                    provider =>
                    expandedNodes => new RulesExplorerTreeViewBuilder
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        expandedNodes
                    )
                )
                .AddTransient<ITreeViewBuilderFactory, TreeViewBuilderFactory>();
        }
    }
}
