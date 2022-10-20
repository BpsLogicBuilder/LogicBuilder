using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class ModuleRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ModuleRuleBuilder(
            IExceptionHelper exceptionHelper,
            IModuleDataParser moduleDataParser,
            IRuleBuilderFactory ruleBuilderFactory,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings)
        {
            _codeExpressionBuilder = ruleBuilderFactory.GetCodeExpressionBuilder
            (
                application,
                resourceStrings,
                moduleName
            );
            _exceptionHelper = exceptionHelper;
            _moduleDataParser = moduleDataParser;
            _shapeHelper = shapeHelper;
            _shapeSetRuleBuilderHelper = ruleBuilderFactory.GetShapeSetRuleBuilderHelper
            (
                ruleShapes,
                ruleConnectors,
                moduleName,
                ruleCount,
                application,
                resourceStrings
            );
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IList<RuleBag> GenerateRules()
        {
            _shapeSetRuleBuilderHelper.RuleCount++;

            string externalModuleShapeXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(externalModuleShapeXml))
                throw _exceptionHelper.CriticalException("{ECF31808-B4AF-4BCA-A722-50762A34B9CC}");

            string externalModule = _moduleDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(externalModuleShapeXml)
            );

            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDirectorPropertyCondition(DirectorProperties.MODULEENDPROPERTY, new CodePrimitiveExpression(externalModule))));
            _shapeSetRuleBuilderHelper.GenerateRightHandSide();

            return GetRules();
        }

        private IList<RuleBag> GetRules()
        {
            Rule rule = new(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.RULENAMEFORMAT, _shapeSetRuleBuilderHelper.ModuleName, _shapeSetRuleBuilderHelper.RuleCount, _shapeSetRuleBuilderHelper.RuleConnectors[0].Index, _shapeSetRuleBuilderHelper.RuleConnectors[0].ContainingPage.Index))
            {
                Condition = new RuleExpressionCondition(_codeExpressionBuilder.AggregateConditions(_shapeSetRuleBuilderHelper.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in _shapeSetRuleBuilderHelper.ThenActions)
                rule.ThenActions.Add(thenAction);

            if (_shapeSetRuleBuilderHelper.RuleConnectors[0].Master.NameU == UniversalMasterName.CONNECTOBJECT)
                return new RuleBag[] { new RuleBag(rule) };
            else
                return new RuleBag[] { new RuleBag(rule, _shapeHelper.GetApplicationList(_shapeSetRuleBuilderHelper.RuleConnectors[0], _shapeSetRuleBuilderHelper.RuleShapes[0])) };
        }
    }
}
