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
    internal class WaitDecisionsRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly List<RuleBag> rules = new();

        public WaitDecisionsRuleBuilder(
            IDecisionDataParser decisionDataParser,
            IDecisionsDataParser decisionsDataParser,
            IExceptionHelper exceptionHelper,
            IRulesGeneratorFactory rulesGeneratorFactory,
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
            _codeExpressionBuilder = rulesGeneratorFactory.GetCodeExpressionBuilder
            (
                application,
                resourceStrings,
                moduleName
            );
            _decisionDataParser = decisionDataParser;
            _decisionsDataParser = decisionsDataParser;
            _exceptionHelper = exceptionHelper;
            _shapeSetRuleBuilderHelper = rulesGeneratorFactory.GetShapeSetRuleBuilderHelper
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
            string decisionsXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(decisionsXml))
                throw _exceptionHelper.CriticalException("{49316E0F-38CE-43F7-AC16-5EB3843AE8C3}");

            DecisionsData decisionsData = _decisionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(decisionsXml)
            );

            switch (decisionsData.FirstChildElementName)
            {
                case XmlDataConstants.NOTELEMENT:
                case XmlDataConstants.DECISIONELEMENT:
                case XmlDataConstants.ANDELEMENT:
                    AndRule(decisionsData);
                    FalseAndRule(decisionsData);
                    break;
                case XmlDataConstants.ORELEMENT:
                    OrRule(decisionsData);
                    FalseOrRule(decisionsData);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{DE02AAFC-F135-44A6-AD5A-7AA819F1778B}");
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
        /// Generates one rule when decisions are ORed
        /// </summary>
        private void OrRule(DecisionsData decisionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllDecisions(decisionsData, true, CodeBinaryOperatorType.BooleanOr));

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when decisions are ORed and the condition resultant is false
        /// </summary>
        private void FalseOrRule(DecisionsData decisionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllDecisions(decisionsData, false, CodeBinaryOperatorType.BooleanOr));

            _shapeSetRuleBuilderHelper.ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule when decisions are "ANDED"
        /// </summary>
        private void AndRule(DecisionsData decisionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllDecisions(decisionsData, true, CodeBinaryOperatorType.BooleanAnd));

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when decisions are "ANDED" and the condition resultant is false
        /// </summary>
        private void FalseAndRule(DecisionsData decisionsData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add(GetAllDecisions(decisionsData, false, CodeBinaryOperatorType.BooleanAnd));

            _shapeSetRuleBuilderHelper.ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        private IfCondition GetAllDecisions(DecisionsData decisionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator)
        {
            return new IfCondition
            (
                _codeExpressionBuilder.AggregateConditions
                (
                    decisionsData
                        .DecisionElements
                        .Select(e => _decisionDataParser.Parse(e))
                        .Select(dData => _codeExpressionBuilder.BuildIfCondition(dData)),
                    binaryOperator
                ),
                !isYesPath//if yes Path the Not is false
            );
        }
    }
}
