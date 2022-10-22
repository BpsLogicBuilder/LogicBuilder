﻿using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static  class TreeViewBuiilderFactoryServices
    {
        internal static IServiceCollection AddTreeViewBuiilderFactories(this IServiceCollection services)
        {
            return services
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
                .AddTransient<ITreeViewBuiilderFactory, TreeViewBuiilderFactory>();
        }
    }
}