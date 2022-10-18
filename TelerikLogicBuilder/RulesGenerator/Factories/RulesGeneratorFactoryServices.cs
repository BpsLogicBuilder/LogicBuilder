using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static  class RulesGeneratorFactoryServices
    {
        internal static IServiceCollection AddRulesGeneratorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramValidator>>
                (
                    provider =>
                    (sourceFile, document, application, progress, cancellationTokenSource) => new DiagramValidator
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGetRuleShapes>(),
                        provider.GetRequiredService<IJumpDataParser>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IShapeHelper>(),
                        provider.GetRequiredService<IShapeXmlHelper>(),
                        provider.GetRequiredService<IShapeValidator>(),
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<IVisioFileSourceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        sourceFile,
                        document,
                        application,
                        progress,
                        cancellationTokenSource)
                )
                .AddTransient<IDiagramValidatorFactory, DiagramValidatorFactory>()
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
                )
                .AddTransient<IVisioFileSourceFactory, VisioFileSourceFactory>(); ;
        }
    }
}
