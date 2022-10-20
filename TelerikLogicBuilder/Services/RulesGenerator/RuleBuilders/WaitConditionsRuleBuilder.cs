using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
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
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class WaitConditionsRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly List<RuleBag> rules = new();

        public WaitConditionsRuleBuilder(
            IConditionsDataParser conditionsDataParser,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
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
            _conditionsDataParser = conditionsDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _shapeSetRuleBuilderHelper = ruleBuilderFactory.GetShapeSetRuleBuilderHelper
            (
                ruleShapes,
                ruleConnectors,
                moduleName,
                ruleCount,
                application,
                resourceStrings
            );
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IList<RuleBag> GenerateRules()
        {
            string conditionsXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(conditionsXml))
                throw _exceptionHelper.CriticalException("{F18ECA27-4DF5-46E2-94EA-38B870611D75}");

            ConditionsData conditionsData = _conditionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(conditionsXml)
            );

            switch (conditionsData.FirstChildElementName)
            {
                case XmlDataConstants.NOTELEMENT:
                case XmlDataConstants.FUNCTIONELEMENT:
                case XmlDataConstants.ANDELEMENT:
                    AndRule(conditionsData);
                    FalseAndRule(conditionsData);
                    break;
                case XmlDataConstants.ORELEMENT:
                    OrRule(conditionsData);
                    FalseOrRule(conditionsData);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{B40A37EC-EAFE-445B-B92C-C5E1A87558AB}");
            }

            return rules;
        }

        /// <summary>
        /// Assembles the entire rule and clears all conditions
        /// </summary>
        private void AssembleRule()
        {
            Rule rule = new
            (
                string.Format
                (
                    CultureInfo.InvariantCulture,
                    RuleDefinitionConstants.RULENAMEFORMAT,
                    _shapeSetRuleBuilderHelper.ModuleName,
                    _shapeSetRuleBuilderHelper.RuleCount,
                    _shapeSetRuleBuilderHelper.RuleConnectors[0].Index,
                    _shapeSetRuleBuilderHelper.RuleConnectors[0].ContainingPage.Index
                )
            )
            {
                Condition = new RuleExpressionCondition(_codeExpressionBuilder.AggregateConditions(_shapeSetRuleBuilderHelper.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in _shapeSetRuleBuilderHelper.ThenActions)
                rule.ThenActions.Add(thenAction);

            if (_shapeSetRuleBuilderHelper.RuleConnectors[0].Master.NameU == UniversalMasterName.CONNECTOBJECT)
                rules.Add(new RuleBag(rule));
            else
                rules.Add(new RuleBag(rule, _shapeHelper.GetApplicationList(_shapeSetRuleBuilderHelper.RuleConnectors[0], _shapeSetRuleBuilderHelper.RuleShapes[0])));

            _shapeSetRuleBuilderHelper.Conditions.Clear();
            _shapeSetRuleBuilderHelper.ThenActions.Clear();
        }

        /// <summary>
        /// Generates one rule when conditions are ORed
        /// </summary>
        private void OrRule(ConditionsData conditionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllConditions(conditionsData, true, CodeBinaryOperatorType.BooleanOr));

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when conditions are ORed and the resultant condition is false
        /// </summary>
        private void FalseOrRule(ConditionsData conditionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllConditions(conditionsData, false, CodeBinaryOperatorType.BooleanOr));

            _shapeSetRuleBuilderHelper.ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule when conditions are ANDed
        /// </summary>
        private void AndRule(ConditionsData conditionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllConditions(conditionsData, true, CodeBinaryOperatorType.BooleanAnd));

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when conditions are ANDed and the resultant condition is false
        /// </summary>
        private void FalseAndRule(ConditionsData conditionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllConditions(conditionsData, false, CodeBinaryOperatorType.BooleanAnd));

            _shapeSetRuleBuilderHelper.ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        private IfCondition GetAllConditions(ConditionsData conditionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator)
            => new
            (
                _codeExpressionBuilder.AggregateConditions
                (
                    conditionsData
                        .FunctionElements
                        .Select(e => _functionDataParser.Parse(e))
                        .Select(fData => _codeExpressionBuilder.BuildIfCondition(fData)),
                    binaryOperator
                ),
                !isYesPath//if yes Path the Not is false
            );
    }
}
