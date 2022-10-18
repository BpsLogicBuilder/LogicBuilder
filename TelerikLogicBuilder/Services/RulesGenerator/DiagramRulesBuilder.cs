using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class DiagramRulesBuilder : IDiagramRulesBuilder
    {
        private readonly IContextProvider _contextProvider;
        private readonly IDiagramValidatorFactory _diagramValidatorFactory;
        private readonly IGetRuleShapes _getRuleShapes;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeSetRuleBuilder _shapeSetRuleBuilder;
        private readonly IShapeXmlHelper _shapeXmlHelper;

        public DiagramRulesBuilder(
            IContextProvider contextProvider,
            IDiagramValidatorFactory diagramValidatorFactory,
            IGetRuleShapes getRuleShapes,
            IJumpDataParser jumpDataParser,
            IShapeSetRuleBuilder shapeSetRuleBuilder,
            IShapeXmlHelper shapeXmlHelper)
        {
            _contextProvider = contextProvider;
            _diagramValidatorFactory = diagramValidatorFactory;
            _getRuleShapes = getRuleShapes;
            _jumpDataParser = jumpDataParser;
            _shapeSetRuleBuilder = shapeSetRuleBuilder;
            _shapeXmlHelper = shapeXmlHelper;
        }

        public Task<BuildRulesResult> BuildRules(string sourceFile, Document document, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource) 
            => new DiagramRulesBuilderUtility
            (
                sourceFile,
                document,
                application,
                progress,
                cancellationTokenSource,
                _contextProvider,
                _diagramValidatorFactory,
                _getRuleShapes,
                _jumpDataParser,
                _shapeSetRuleBuilder,
                _shapeXmlHelper
            ).BuildRules();
    }
}
