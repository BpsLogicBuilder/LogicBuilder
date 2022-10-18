using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static  class RulesGeneratorFactoryServices
    {
        internal static IServiceCollection AddRulesGeneratorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Page, Shape, List<ResultMessage>, IApplicationSpecificFlowShapeValidator>>
                (
                    provider =>
                    (sourceFile, page, shape, validationErrors) => new ApplicationSpecificFlowShapeValidator
                    (
                        provider.GetRequiredService<IResultMessageHelperFactory>(),
                        sourceFile,
                        page,
                        shape,
                        validationErrors
                    )
                )
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
                        provider.GetRequiredService<IShapeValidatorFactory>(),
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
                .AddTransient<Func<string, Page, Shape, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator>>
                (
                    provider =>
                    (sourceFile, page, shape, validationErrors, application) =>
                    {
                        return shape.Master.NameU switch
                        {
                            UniversalMasterName.APP01CONNECTOBJECT 
                                or UniversalMasterName.APP02CONNECTOBJECT 
                                or UniversalMasterName.APP03CONNECTOBJECT 
                                or UniversalMasterName.APP04CONNECTOBJECT 
                                or UniversalMasterName.APP05CONNECTOBJECT 
                                or UniversalMasterName.APP06CONNECTOBJECT 
                                or UniversalMasterName.APP07CONNECTOBJECT 
                                or UniversalMasterName.APP08CONNECTOBJECT 
                                or UniversalMasterName.APP09CONNECTOBJECT 
                                or UniversalMasterName.APP10CONNECTOBJECT 
                                or UniversalMasterName.OTHERSCONNECTOBJECT => new ApplicationConnectorValidator
                            (
                                provider.GetRequiredService<IConfigurationService>(),
                                provider.GetRequiredService<IExceptionHelper>(),
                                provider.GetRequiredService<IModuleDataParser>(),
                                provider.GetRequiredService<IPathHelper>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.CONNECTOBJECT => new RegularConnectorValidator
                            (
                                provider.GetRequiredService<IConnectorElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors,
                                application
                            ),
                            _ => throw provider.GetRequiredService<IExceptionHelper>().CriticalException("{4475E4AC-25AA-417D-9371-23877FA039F9}"),
                        };
                    }
                )
                .AddTransient<Func<string, Page, ShapeBag, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator>>
                (
                    provider =>
                    (sourceFile, page, shapeBag, validationErrors, application) =>
                    {
                        Shape shape = shapeBag.Shape;
                        return shape.Master.NameU switch
                        {
                            UniversalMasterName.ACTION => new ActionShapeValidator
                            (
                                provider.GetRequiredService<IApplicationTypeInfoManager>(),
                                provider.GetRequiredService<IFunctionsElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeValidatorFactory>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shapeBag,
                                validationErrors,
                                application
                            ),
                            UniversalMasterName.BEGINFLOW or UniversalMasterName.MODULEBEGIN => new BeginShapeValidator
                            (
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.COMMENT => new CommentShapeValidator
                            (
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.CONDITIONOBJECT => new ConditionShapeValidator
                            (
                                provider.GetRequiredService<IConditionsElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors,
                                application
                            ),
                            UniversalMasterName.DECISIONOBJECT => new DecisionShapeValidator
                            (
                                provider.GetRequiredService<IDecisionsElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors,
                                application
                            ),
                            UniversalMasterName.DIALOG => new DialogShapeValidator
                            (
                                provider.GetRequiredService<IFunctionsElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors,
                                application
                            ),
                            UniversalMasterName.ENDFLOW 
                                or UniversalMasterName.MODULEEND 
                                or UniversalMasterName.TERMINATE => new EndShapeValidator
                            (
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.JUMPOBJECT => new JumpShapeValidator
                            (
                                provider.GetRequiredService<IJumpDataParser>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.MERGEOBJECT => new MergeShapeValidator
                            (
                                provider.GetRequiredService<IConfigurationService>(),
                                provider.GetRequiredService<IExceptionHelper>(),
                                provider.GetRequiredService<IPathHelper>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.MODULE => new ModuleShapeValidator
                            (
                                provider.GetRequiredService<IModuleDataParser>(),
                                provider.GetRequiredService<IModuleNamesReader>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeValidatorFactory>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shape,
                                validationErrors
                            ),
                            UniversalMasterName.WAITCONDITIONOBJECT => new WaitConditionShapeValidator
                            (
                                provider.GetRequiredService<IApplicationTypeInfoManager>(),
                                provider.GetRequiredService<IConditionsElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeValidatorFactory>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shapeBag,
                                validationErrors,
                                application
                            ),
                            UniversalMasterName.WAITDECISIONOBJECT => new WaitDecisionShapeValidator
                            (
                                provider.GetRequiredService<IApplicationTypeInfoManager>(),
                                provider.GetRequiredService<IDecisionsElementValidator>(),
                                provider.GetRequiredService<IResultMessageHelperFactory>(),
                                provider.GetRequiredService<IShapeHelper>(),
                                provider.GetRequiredService<IShapeValidatorFactory>(),
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                sourceFile,
                                page,
                                shapeBag,
                                validationErrors,
                                application
                            ),
                            _ => throw provider.GetRequiredService<IExceptionHelper>().CriticalException("{3F24C6C4-277E-46F8-9AE2-5BCC160641B3}"),
                        };
                    }
                )
                .AddTransient<IShapeValidatorFactory, ShapeValidatorFactory>()
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
