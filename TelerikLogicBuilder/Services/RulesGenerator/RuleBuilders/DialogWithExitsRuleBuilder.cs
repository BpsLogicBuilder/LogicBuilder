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
    internal class DialogWithExitsRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly ConnectorData firstConnectorData;
        private readonly List<RuleBag> rules = new();

        public DialogWithExitsRuleBuilder(
            IConnectorDataParser connectorDataParser, 
            IExceptionHelper exceptionHelper, 
            IFunctionDataParser functionDataParser,
            IFunctionsDataParser functionsDataParser,
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
            _connectorDataParser = connectorDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionsDataParser = functionsDataParser;
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

            string connectorXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.FirstConnector);
            if (string.IsNullOrEmpty(connectorXml))
                throw _exceptionHelper.CriticalException("{E2562487-C87D-4228-A8B1-89A663E1DA4E}");

            firstConnectorData = _connectorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(connectorXml)
            );
        }

        private IfCondition QuestionAnsweredCondition
            => new(_codeExpressionBuilder.BuildSelectCondition(firstConnectorData.TextXmlNode));

        private IfCondition QuestionNotAnsweredCondition
            => new(_codeExpressionBuilder.BuildSelectCondition(string.Empty));

        public IList<RuleBag> GenerateRules()
        {
            if (firstConnectorData.Index == 1)//build Question rule only for the first connector
                BuildQuestionRule();

            BuildContinueRule();

            return rules;
        }

        /// <summary>
        /// Generates the rule which displays the dialog
        /// </summary>
        private void BuildQuestionRule()
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));
            _shapeSetRuleBuilderHelper.Conditions.Add(QuestionNotAnsweredCondition);

            _shapeSetRuleBuilderHelper.ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(_shapeSetRuleBuilderHelper.DirectorReference, RuleFunctionConstants.VIRTUALFUNCTIONSETBUSINESSBACKUPDATA, Array.Empty<CodeExpression>())));

            string functionsXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(functionsXml))
                throw _exceptionHelper.CriticalException("{B346D7C0-A16E-455F-89A0-0FDA60723247}");

            AddDialogFunction
            (
                _functionsDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(functionsXml))
            );

            AssembleRule();

            void AddDialogFunction(FunctionsData functionsData)
            {
                _shapeSetRuleBuilderHelper.ThenActions.Add
                (
                    new RuleStatementAction
                    (
                        _codeExpressionBuilder.BuildFunction
                        (
                            _functionDataParser.Parse(functionsData.FunctionElements[0]),
                            _shapeHelper.GetMultipleChoiceConnectorData(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape)
                                .OrderBy(cd => cd.Index)
                                .ToList()
                        )
                    )
                );
            }
        }

        /// <summary>
        /// generates the rule for proceeding to the next shape
        /// </summary>
        private void BuildContinueRule()
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));
            _shapeSetRuleBuilderHelper.Conditions.Add(QuestionAnsweredCondition);

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Assembles the rule and clears all actions and conditions
        /// </summary>
        void AssembleRule()
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
                Condition = new RuleExpressionCondition
                (
                    _codeExpressionBuilder.AggregateConditions(_shapeSetRuleBuilderHelper.Conditions, CodeBinaryOperatorType.BooleanAnd)
                )
            };

            foreach (RuleAction thenAction in _shapeSetRuleBuilderHelper.ThenActions)
                rule.ThenActions.Add(thenAction);

            rules.Add(new RuleBag(rule));

            _shapeSetRuleBuilderHelper.Conditions.Clear();
            _shapeSetRuleBuilderHelper.ThenActions.Clear();
        }
    }
}
