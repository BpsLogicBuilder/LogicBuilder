using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class ConditionsRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConditionsRuleBuilder(
            IConditionsDataParser conditionsDataParser,
            IConnectorDataParser connectorDataParser,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IRulesGeneratorFactory rulesGeneratorFactory,
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
            _conditionsDataParser = conditionsDataParser;
            _connectorDataParser = connectorDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _shapeSetRuleBuilderHelper = rulesGeneratorFactory.GetShapeSetRuleBuilderHelper
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
            string conditionsXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(conditionsXml))
                throw _exceptionHelper.CriticalException("{3C274E76-0F7A-47DE-9CEF-4469BE712F32}");

            string connectorXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.FirstConnector);
            if (string.IsNullOrEmpty(connectorXml))
                throw _exceptionHelper.CriticalException("{C6B44DA3-1A7C-4C8A-A6E8-D67AAC3EFDDD}");

            ConditionsData conditionsData = _conditionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(conditionsXml)
            );
            ConnectorData firstConnectorData = _connectorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(connectorXml)
            );

            switch (conditionsData.FirstChildElementName)
            {
                case XmlDataConstants.NOTELEMENT:
                case XmlDataConstants.FUNCTIONELEMENT:
                case XmlDataConstants.ANDELEMENT:
                    AndRule(conditionsData, firstConnectorData);
                    break;
                case XmlDataConstants.ORELEMENT:
                    OrRule(conditionsData, firstConnectorData);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{6E643796-6C30-4520-AB2E-F40D2595E7CD}");
            }

            return _shapeSetRuleBuilderHelper.GetRules();
        }

        /// <summary>
        /// generates one rule when decisions are ORed
        /// </summary>
        private void OrRule(ConditionsData conditionsData, ConnectorData firstConnectorData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            //all conditions into one rule
            _shapeSetRuleBuilderHelper.Conditions.Add
            (
                GetAllConditions
                (
                    conditionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanOr
                )
            );

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
        }

        /// <summary>
        /// generates one rule when decisions are ANDed
        /// </summary>
        private void AndRule(ConditionsData conditionsData, ConnectorData firstConnectorData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            //all conditions into one rule
            _shapeSetRuleBuilderHelper.Conditions.Add
            (
                GetAllConditions
                (
                    conditionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanAnd
                )
            );

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
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
