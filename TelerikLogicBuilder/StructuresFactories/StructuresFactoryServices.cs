using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class StructuresFactoryServices
    {
        internal static IServiceCollection AddStructuresFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Page, Shape, List<ResultMessage>, IDiagramResultMessageHelper>>
                (
                    provider =>
                    (sourceFile, page, shape, resultMessages) => new DiagramResultMessageHelper
                    (
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<IStructuresFactory>(),
                        sourceFile,
                        page,
                        shape,
                        resultMessages
                    )
                )
                .AddTransient<IStructuresFactory, StructuresFactory>()
                .AddTransient<Func<string, int, int, TableFileSource>>
                (
                    provider =>
                    (sourceFileFullname, row, column) => new TableFileSource
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        sourceFileFullname,
                        row,
                        column
                    )
                )
                .AddTransient<Func<string, int, short, string, int, int, VisioFileSource>>
                (
                    provider =>
                    (sourceFileFullname, pageId, pageIndex, shapeMasterName, shapeId, shapeIndex) => new VisioFileSource
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        sourceFileFullname,
                        pageId,
                        pageIndex,
                        shapeMasterName,
                        shapeId,
                        shapeIndex
                    )
                );
        }
    }
}
