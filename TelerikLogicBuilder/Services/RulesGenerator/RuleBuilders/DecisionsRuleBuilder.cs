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
    internal class DecisionsRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionsRuleBuilder(
            IConnectorDataParser connectorDataParser,
            IDecisionDataParser decisionDataParser,
            IDecisionsDataParser decisionsDataParser,
            IExceptionHelper exceptionHelper,
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
            _connectorDataParser = connectorDataParser;
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
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IList<RuleBag> GenerateRules()
        {
            string decisionsXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(decisionsXml))
                throw _exceptionHelper.CriticalException("{49316E0F-38CE-43F7-AC16-5EB3843AE8C3}");

            string connectorXml = _shapeXmlHelper.GetXmlString(_shapeSetRuleBuilderHelper.FirstConnector);
            if (string.IsNullOrEmpty(connectorXml))
                throw _exceptionHelper.CriticalException("{91B06789-302C-465F-ACE6-38BD7DAB4BFC}");

            DecisionsData decisionsData = _decisionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(decisionsXml)
            );

            ConnectorData firstConnectorData = _connectorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(connectorXml)
            );

            switch (decisionsData.FirstChildElementName)
            {
                case XmlDataConstants.NOTELEMENT:
                case XmlDataConstants.DECISIONELEMENT:
                case XmlDataConstants.ANDELEMENT:
                    AndRules(decisionsData, firstConnectorData);
                    break;
                case XmlDataConstants.ORELEMENT:
                    OrRules(decisionsData, firstConnectorData);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{5B5B2793-D8D5-4194-840E-855023967FA7}");
            }

            return _shapeSetRuleBuilderHelper.GetRules();
        }

        /// <summary>
        /// generates one rule when decisions are ORed
        /// </summary>
        private void OrRules(DecisionsData decisionsData, ConnectorData firstConnectorData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add
            (
                GetAllDecisions
                (
                    decisionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanOr
                )
            );

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
        }

        /// <summary>
        /// generates one rule when decisions are "ANDED"
        /// </summary>
        private void AndRules(DecisionsData decisionsData, ConnectorData firstConnectorData)
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));

            _shapeSetRuleBuilderHelper.Conditions.Add
            (
                GetAllDecisions
                (
                    decisionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanAnd
                )
            );

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
        }

        private IfCondition GetAllDecisions(DecisionsData decisionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator)
            => new
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
