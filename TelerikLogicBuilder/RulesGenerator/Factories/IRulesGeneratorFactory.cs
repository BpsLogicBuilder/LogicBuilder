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
    internal interface IRulesGeneratorFactory
    {
        ICodeExpressionBuilder GetCodeExpressionBuilder(
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings,
            string resourceNamePrefix);

        IDiagramRulesBuilder GetDiagramRulesBuilder(string sourceFile,
            Document document,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource);

        IGetRuleShapes GetGetRuleShapes(IDictionary<string, Shape> jumpToShapes);

        IResourcesManager GetResourcesManager(IDictionary<string, string> resourceStrings,
            string resourceNamePrefix);

        IShapeSetRuleBuilder GetShapeSetRuleBuilder(IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings);

        IShapeSetRuleBuilderHelper GetShapeSetRuleBuilderHelper(IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings);

        ITableRowRuleBuilder GetTableRowRuleBuilder(DataRow dataRow,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings);

        ITableRulesBuilder GetTableRulesBuilder(string sourceFile,
            DataSet dataSet,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource);
    }
}
