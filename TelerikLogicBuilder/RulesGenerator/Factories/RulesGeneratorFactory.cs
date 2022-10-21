using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal class RulesGeneratorFactory : IRulesGeneratorFactory
    {
        private readonly Func<string, Page, Shape, List<ResultMessage>, IApplicationSpecificFlowShapeValidator> _getApplicationSpecificFlowShapeValidator;
        private readonly Func<ApplicationTypeInfo, IDictionary<string, string>, string, ICodeExpressionBuilder> _getCodeExpressionBuilder;
        private readonly Func<string, Page, Shape, List<ResultMessage>, ApplicationTypeInfo, IConnectorValidator> _getConnectorValidator;
        private readonly Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramRulesBuilder> _getDiagramRulesBuilder;
        private readonly Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramValidator> _getDiagramValidator;
        private readonly Func<IDictionary<string, Shape>, IGetRuleShapes> _getGetRuleShapes;
        private readonly Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilderHelper> _getShapeSetRuleBuilderHelper;
        private readonly Func<IDictionary<string, string>, string, IResourcesManager> _getResourcesManager;
        private readonly Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilder> _getShapeSetRuleBuilder;
        private readonly Func<string, Page, ShapeBag, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator> _getShapeValidator;
        private readonly Func<DataRow, string, int, ApplicationTypeInfo, IDictionary<string, string>, ITableRowRuleBuilder> _getTableRowRuleBuilder;
        private readonly Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableRulesBuilder> _getTableRulesBuilder;
        private readonly Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableValidator> _getTableValidator;

        public RulesGeneratorFactory(
            Func<string, Page, Shape, List<ResultMessage>, IApplicationSpecificFlowShapeValidator> getApplicationSpecificFlowShapeValidator,
            Func<ApplicationTypeInfo, IDictionary<string, string>, string, ICodeExpressionBuilder> getCodeExpressionBuilder,
            Func<string, Page, Shape, List<ResultMessage>, ApplicationTypeInfo, IConnectorValidator> getConnectorValidator,
            Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramRulesBuilder> getDiagramRulesBuilder,
            Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramValidator> getDiagramValidator,
            Func<IDictionary<string, Shape>, IGetRuleShapes> getGetRuleShapes,
            Func<IDictionary<string, string>, string, IResourcesManager> getResourcesManager,
            Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilder> getShapeSetRuleBuilder,
            Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilderHelper> getShapeSetRuleBuilderHelper,
            Func<string, Page, ShapeBag, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator> getShapeValidator,
            Func<DataRow, string, int, ApplicationTypeInfo, IDictionary<string, string>, ITableRowRuleBuilder> getTableRowRuleBuilder,
            Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableRulesBuilder> getTableRulesBuilder,
            Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableValidator> getTableValidator)
        {
            _getApplicationSpecificFlowShapeValidator = getApplicationSpecificFlowShapeValidator;
            _getCodeExpressionBuilder = getCodeExpressionBuilder;
            _getConnectorValidator = getConnectorValidator;
            _getDiagramRulesBuilder = getDiagramRulesBuilder;
            _getDiagramValidator = getDiagramValidator;
            _getGetRuleShapes = getGetRuleShapes;
            _getResourcesManager = getResourcesManager;
            _getShapeSetRuleBuilder = getShapeSetRuleBuilder;
            _getShapeSetRuleBuilderHelper = getShapeSetRuleBuilderHelper;
            _getShapeValidator = getShapeValidator;
            _getTableRowRuleBuilder = getTableRowRuleBuilder;
            _getTableRulesBuilder = getTableRulesBuilder;
            _getTableValidator = getTableValidator;
        }

        public IApplicationSpecificFlowShapeValidator GetApplicationSpecificFlowShapeValidator(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
            => _getApplicationSpecificFlowShapeValidator(sourceFile, page, shape, validationErrors);

        public ICodeExpressionBuilder GetCodeExpressionBuilder(ApplicationTypeInfo application, IDictionary<string, string> resourceStrings, string resourceNamePrefix)
            => _getCodeExpressionBuilder(application, resourceStrings, resourceNamePrefix);

        public IConnectorValidator GetConnectorValidator(string sourceFile, Page page, Shape connector, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
            => _getConnectorValidator(sourceFile, page, connector, validationErrors, application);

        public IDiagramRulesBuilder GetDiagramRulesBuilder(string sourceFile, Document document, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getDiagramRulesBuilder(sourceFile, document, application, progress, cancellationTokenSource);

        public IDiagramValidator GetDiagramValidator(string sourceFile, Document document, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getDiagramValidator(sourceFile, document, application, progress, cancellationTokenSource);

        public IGetRuleShapes GetGetRuleShapes(IDictionary<string, Shape> jumpToShapes)
            => _getGetRuleShapes(jumpToShapes);

        public IResourcesManager GetResourcesManager(IDictionary<string, string> resourceStrings, string resourceNamePrefix)
            => _getResourcesManager(resourceStrings, resourceNamePrefix);

        public IShapeSetRuleBuilder GetShapeSetRuleBuilder(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
            => _getShapeSetRuleBuilder(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings);

        public IShapeSetRuleBuilderHelper GetShapeSetRuleBuilderHelper(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
            => _getShapeSetRuleBuilderHelper(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings);

        public IShapeValidator GetShapeValidator(string sourceFile, Page page, ShapeBag shapeBag, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
            => _getShapeValidator(sourceFile, page, shapeBag, validationErrors, application);

        public ITableRowRuleBuilder GetTableRowRuleBuilder(DataRow dataRow, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
            => _getTableRowRuleBuilder(dataRow, moduleName, ruleCount, application, resourceStrings);

        public ITableRulesBuilder GetTableRulesBuilder(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getTableRulesBuilder(sourceFile, dataSet, application, progress, cancellationTokenSource);

        public ITableValidator GetTableValidator(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getTableValidator(sourceFile, dataSet, application, progress, cancellationTokenSource);
    }
}
