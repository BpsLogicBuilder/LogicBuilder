using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
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
                .AddTransient<Func<ApplicationTypeInfo, IDictionary<string, string>, string, ICodeExpressionBuilder>>
                (
                    provider =>
                    (application, resourceStrings, resourceNamePrefix) => new CodeExpressionBuilder
                    (
                        provider.GetRequiredService<IAnyParametersHelper>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IGetValidConfigurationFromData>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListParameterDataParser>(),
                        provider.GetRequiredService<ILiteralListVariableDataParser>(),
                        provider.GetRequiredService<IMetaObjectDataParser>(),
                        provider.GetRequiredService<IObjectDataParser>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListParameterDataParser>(),
                        provider.GetRequiredService<IObjectListVariableDataParser>(),
                        provider.GetRequiredService<IObjectParameterDataParser>(),
                        provider.GetRequiredService<IObjectVariableDataParser>(),
                        provider.GetRequiredService<IParameterHelper>(),
                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IVariableHelper>(),
                        application,
                        resourceStrings,
                        resourceNamePrefix
                    )
                )
                .AddTransient<Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramRulesBuilder>>
                (
                    provider =>
                    (sourceFile, document, application, progress, cancellationTokenSource) => new DiagramRulesBuilder
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IJumpDataParser>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                        provider.GetRequiredService<IShapeXmlHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        sourceFile,
                        document,
                        application,
                        progress,
                        cancellationTokenSource
                    )
                )
                .AddTransient<Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramValidator>>
                (
                    provider =>
                    (sourceFile, document, application, progress, cancellationTokenSource) => new DiagramValidator
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IJumpDataParser>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IShapeHelper>(),
                        provider.GetRequiredService<IShapeXmlHelper>(),
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                        provider.GetRequiredService<IVisioFileSourceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        sourceFile,
                        document,
                        application,
                        progress,
                        cancellationTokenSource
                    )
                )
                .AddTransient<Func<IDictionary<string, Shape>, IGetRuleShapes>>
                (
                    provider =>
                    jumpToShapes => new GetRuleShapes
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IJumpDataParser>(),
                        provider.GetRequiredService<IShapeHelper>(),
                        provider.GetRequiredService<IShapeXmlHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        jumpToShapes
                    )
                )
                .AddTransient<Func<IDictionary<string, string>, string, IResourcesManager>>
                (
                    provider =>
                    (resourceStrings, resourceNamePrefix) => new ResourcesManager
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        resourceStrings, 
                        resourceNamePrefix
                    )
                )
                .AddTransient<IRulesGeneratorFactory, RulesGeneratorFactory>()
                .AddTransient<Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilder>>
                (
                    provider =>
                    (ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings) =>
                    {
                        switch (ruleShapes[0].Shape.Master.NameU)
                        {
                            case UniversalMasterName.BEGINFLOW:
                                return new BeginFlowRuleBuilder
                                (
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.CONDITIONOBJECT:
                                return new ConditionsRuleBuilder
                                (
                                    provider.GetRequiredService<IConditionsDataParser>(),
                                    provider.GetRequiredService<IConnectorDataParser>(),
                                    provider.GetRequiredService<IExceptionHelper>(),
                                    provider.GetRequiredService<IFunctionDataParser>(),
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    provider.GetRequiredService<IShapeXmlHelper>(),
                                    provider.GetRequiredService<IXmlDocumentHelpers>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.DECISIONOBJECT:
                                return new DecisionsRuleBuilder
                                (
                                    provider.GetRequiredService<IConnectorDataParser>(),
                                    provider.GetRequiredService<IDecisionDataParser>(),
                                    provider.GetRequiredService<IDecisionsDataParser>(),
                                    provider.GetRequiredService<IExceptionHelper>(),
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    provider.GetRequiredService<IShapeXmlHelper>(),
                                    provider.GetRequiredService<IXmlDocumentHelpers>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.DIALOG:
                                IShapeXmlHelper shapeXmlHelper = provider.GetRequiredService<IShapeXmlHelper>();
                                if (string.IsNullOrEmpty(shapeXmlHelper.GetXmlString(ruleConnectors[0])))
                                {
                                    return new DialogWithoutExitsRuleBuilder
                                    (
                                        provider.GetRequiredService<IExceptionHelper>(),
                                        provider.GetRequiredService<IFunctionDataParser>(),
                                        provider.GetRequiredService<IFunctionsDataParser>(),
                                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                                        shapeXmlHelper,
                                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                                        ruleShapes,
                                        ruleConnectors,
                                        moduleName,
                                        ruleCount,
                                        application,
                                        resourceStrings
                                    );
                                }
                                else
                                {
                                    return new DialogWithExitsRuleBuilder
                                    (
                                        provider.GetRequiredService<IConnectorDataParser>(),
                                        provider.GetRequiredService<IExceptionHelper>(),
                                        provider.GetRequiredService<IFunctionDataParser>(),
                                        provider.GetRequiredService<IFunctionsDataParser>(),
                                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                                        provider.GetRequiredService<IShapeHelper>(),
                                        shapeXmlHelper,
                                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                                        ruleShapes,
                                        ruleConnectors,
                                        moduleName,
                                        ruleCount,
                                        application,
                                        resourceStrings
                                    );
                                }
                            case UniversalMasterName.MERGEOBJECT:
                                return new MergeRuleBuilder
                                (
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    provider.GetRequiredService<IShapeHelper>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.MODULEBEGIN:
                                return new ModuleBeginRuleBuilder
                                (
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.MODULE:
                                return new ModuleRuleBuilder
                                (
                                    provider.GetRequiredService<IExceptionHelper>(),
                                    provider.GetRequiredService<IModuleDataParser>(),
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    provider.GetRequiredService<IShapeHelper>(),
                                    provider.GetRequiredService<IShapeXmlHelper>(),
                                    provider.GetRequiredService<IXmlDocumentHelpers>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.WAITCONDITIONOBJECT:
                                return new WaitConditionsRuleBuilder
                                (
                                    provider.GetRequiredService<IConditionsDataParser>(),
                                    provider.GetRequiredService<IExceptionHelper>(),
                                    provider.GetRequiredService<IFunctionDataParser>(),
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    provider.GetRequiredService<IShapeHelper>(),
                                    provider.GetRequiredService<IShapeXmlHelper>(),
                                    provider.GetRequiredService<IXmlDocumentHelpers>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            case UniversalMasterName.WAITDECISIONOBJECT:
                                return new WaitDecisionsRuleBuilder
                                (
                                    provider.GetRequiredService<IDecisionDataParser>(),
                                    provider.GetRequiredService<IDecisionsDataParser>(),
                                    provider.GetRequiredService<IExceptionHelper>(),
                                    provider.GetRequiredService<IRulesGeneratorFactory>(),
                                    provider.GetRequiredService<IShapeHelper>(),
                                    provider.GetRequiredService<IShapeXmlHelper>(),
                                    provider.GetRequiredService<IXmlDocumentHelpers>(),
                                    ruleShapes,
                                    ruleConnectors,
                                    moduleName,
                                    ruleCount,
                                    application,
                                    resourceStrings
                                );
                            default:
                                throw provider.GetRequiredService<IExceptionHelper>().CriticalException("{A8CD2AD7-38CE-42C3-83F7-918DA14BE587}");
                        }
                    }
                )
                .AddTransient<Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilderHelper>>
                (
                    provider =>
                    (ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings) => new ShapeSetRuleBuilderHelper
                    (
                        provider.GetRequiredService<IAssertFunctionDataParser>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IGetValidConfigurationFromData>(),
                        provider.GetRequiredService<IModuleDataParser>(),
                        provider.GetRequiredService<IRetractFunctionDataParser>(),
                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                        provider.GetRequiredService<IShapeXmlHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IVariableValueDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        ruleShapes,
                        ruleConnectors,
                        moduleName,
                        ruleCount,
                        application,
                        resourceStrings
                    )
                )
                .AddTransient<Func<string, Page, Shape, List<ResultMessage>, ApplicationTypeInfo, IConnectorValidator>>
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
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IRulesGeneratorFactory>(),
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
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IRulesGeneratorFactory>(),
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
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IRulesGeneratorFactory>(),
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
                                provider.GetRequiredService<IShapeXmlHelper>(),
                                provider.GetRequiredService<IRulesGeneratorFactory>(),
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
                .AddTransient<ITableFileSourceFactory, TableFileSourceFactory>()
                .AddTransient<Func<DataRow, string, int, ApplicationTypeInfo, IDictionary<string, string>, ITableRowRuleBuilder>>
                (
                    provider =>
                    (dataRow, moduleName, ruleCount, application, resourceStrings) => new TableRowRuleBuilder
                    (
                        provider.GetRequiredService<IAssertFunctionDataParser>(),
                        provider.GetRequiredService<ICellXmlHelper>(),
                        provider.GetRequiredService<IConditionsDataParser>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IGetValidConfigurationFromData>(),
                        provider.GetRequiredService<IPriorityDataParser>(),
                        provider.GetRequiredService<IRetractFunctionDataParser>(),
                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IVariableValueDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataRow,
                        moduleName,
                        ruleCount,
                        application,
                        resourceStrings
                    )
                )
                .AddTransient<Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableRulesBuilder>>
                (
                    provider =>
                    (sourceFile, dataSet, application, progress, cancellationTokenSource) => new TableRulesBuilder
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IRulesGeneratorFactory>(),
                        sourceFile,
                        dataSet,
                        application,
                        progress,
                        cancellationTokenSource
                    )
                )
                .AddTransient<Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableValidator>>
                (
                    provider =>
                    (sourceFile, dataSet, application, progress, cancellationTokenSource) => new TableValidator
                    (
                        provider.GetRequiredService<ICellHelper>(),
                        provider.GetRequiredService<ICellXmlHelper>(),
                        provider.GetRequiredService<IConditionsElementValidator>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionsElementValidator>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IPriorityDataParser>(),
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<ITableFileSourceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        sourceFile,
                        dataSet,
                        application,
                        progress,
                        cancellationTokenSource)
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
                )
                .AddTransient<IVisioFileSourceFactory, VisioFileSourceFactory>();
        }
    }
}
