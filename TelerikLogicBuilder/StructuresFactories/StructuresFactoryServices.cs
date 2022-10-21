using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Structures;
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
                        provider.GetRequiredService<IVisioFileSourceFactory>(),
                        sourceFile,
                        page,
                        shape,
                        resultMessages
                    )
                )
                .AddTransient<IStructuresFactory, StructuresFactory>();
        }
    }
}
