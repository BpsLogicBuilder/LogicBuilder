using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
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
        private readonly Func<ApplicationTypeInfo, IDictionary<string, string>, string, ICodeExpressionBuilder> _getCodeExpressionBuilder;
        private readonly Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramRulesBuilder> _getDiagramRulesBuilder;
        private readonly Func<IDictionary<string, Shape>, IGetRuleShapes> _getGetRuleShapes;
        private readonly Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilderHelper> _getShapeSetRuleBuilderHelper;
        private readonly Func<IDictionary<string, string>, string, IResourcesManager> _getResourcesManager;
        private readonly Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilder> _getShapeSetRuleBuilder;
        private readonly Func<DataRow, string, int, ApplicationTypeInfo, IDictionary<string, string>, ITableRowRuleBuilder> _getTableRowRuleBuilder;
        private readonly Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableRulesBuilder> _getTableRulesBuilder;

        public RulesGeneratorFactory(Func<ApplicationTypeInfo, IDictionary<string, string>, string, ICodeExpressionBuilder> getCodeExpressionBuilder,
            Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramRulesBuilder> getDiagramRulesBuilder,
            Func<IDictionary<string, Shape>, IGetRuleShapes> getGetRuleShapes,
            Func<IDictionary<string, string>, string, IResourcesManager> getResourcesManager,
            Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilder> getShapeSetRuleBuilder,
            Func<IList<ShapeBag>, IList<Shape>, string, int, ApplicationTypeInfo, IDictionary<string, string>, IShapeSetRuleBuilderHelper> getShapeSetRuleBuilderHelper,
            Func<DataRow, string, int, ApplicationTypeInfo, IDictionary<string, string>, ITableRowRuleBuilder> getTableRowRuleBuilder,
            Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableRulesBuilder> getTableRulesBuilder)
        {
            _getCodeExpressionBuilder = getCodeExpressionBuilder;
            _getDiagramRulesBuilder = getDiagramRulesBuilder;
            _getGetRuleShapes = getGetRuleShapes;
            _getResourcesManager = getResourcesManager;
            _getShapeSetRuleBuilder = getShapeSetRuleBuilder;
            _getShapeSetRuleBuilderHelper = getShapeSetRuleBuilderHelper;
            _getTableRowRuleBuilder = getTableRowRuleBuilder;
            _getTableRulesBuilder = getTableRulesBuilder;
        }

        public ICodeExpressionBuilder GetCodeExpressionBuilder(ApplicationTypeInfo application, IDictionary<string, string> resourceStrings, string resourceNamePrefix)
            => _getCodeExpressionBuilder(application, resourceStrings, resourceNamePrefix);

        public IDiagramRulesBuilder GetDiagramRulesBuilder(string sourceFile, Document document, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getDiagramRulesBuilder(sourceFile, document, application, progress, cancellationTokenSource);

        public IGetRuleShapes GetGetRuleShapes(IDictionary<string, Shape> jumpToShapes)
            => _getGetRuleShapes(jumpToShapes);

        public IResourcesManager GetResourcesManager(IDictionary<string, string> resourceStrings, string resourceNamePrefix)
            => _getResourcesManager(resourceStrings, resourceNamePrefix);

        public IShapeSetRuleBuilder GetShapeSetRuleBuilder(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
            => _getShapeSetRuleBuilder(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings);

        public IShapeSetRuleBuilderHelper GetShapeSetRuleBuilderHelper(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
            => _getShapeSetRuleBuilderHelper(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings);

        public ITableRowRuleBuilder GetTableRowRuleBuilder(DataRow dataRow, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
            => _getTableRowRuleBuilder(dataRow, moduleName, ruleCount, application, resourceStrings);

        public ITableRulesBuilder GetTableRulesBuilder(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getTableRulesBuilder(sourceFile, dataSet, application, progress, cancellationTokenSource);
    }
}
