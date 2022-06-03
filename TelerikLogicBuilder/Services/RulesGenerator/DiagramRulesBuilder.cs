using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
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
        private readonly IDiagramValidator _diagramValidator;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeSetRuleBuilder _shapeSetRuleBuilder;
        private readonly IShapeXmlHelper _shapeXmlHelper;

        public DiagramRulesBuilder(
            IContextProvider contextProvider,
            IDiagramValidator diagramValidator,
            IJumpDataParser jumpDataParser,
            IShapeHelper shapeHelper,
            IShapeSetRuleBuilder shapeSetRuleBuilder,
            IShapeXmlHelper shapeXmlHelper)
        {
            _contextProvider = contextProvider;
            _diagramValidator = diagramValidator;
            _jumpDataParser = jumpDataParser;
            _shapeHelper = shapeHelper;
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
                _diagramValidator,
                _jumpDataParser,
                _shapeHelper,
                _shapeSetRuleBuilder,
                _shapeXmlHelper
            ).BuildRules();
    }
}
