using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class DecisionsRuleBuilderUtility : ShapeSetRuleBuilderUtility
    {
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;

        public DecisionsRuleBuilderUtility(
            IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings,
            IContextProvider contextProvider,
            IAnyParametersHelper anyParametersHelper,
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IConnectorDataParser connectorDataParser,
            IConstructorDataParser constructorDataParser,
            IDecisionDataParser decisionDataParser,
            IDecisionsDataParser decisionsDataParser,
            IDiagramResourcesManager diagramResourcesManager,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionHelper functionHelper,
            IFunctionsDataParser functionsDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterDataParser literalListParameterDataParser,
            ILiteralListVariableDataParser literalListVariableDataParser,
            IMetaObjectDataParser metaObjectDataParser,
            IModuleDataParser moduleDataParser,
            IObjectDataParser objectDataParser,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterDataParser objectListParameterDataParser,
            IObjectListVariableDataParser objectListVariableDataParser,
            IObjectParameterDataParser objectParameterDataParser,
            IObjectVariableDataParser objectVariableDataParser,
            IParameterHelper parameterHelper,
            IRetractFunctionDataParser retractFunctionDataParser,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IVariableHelper variableHelper,
            IVariableValueDataParser variableValueDataParser) : base(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings, contextProvider, anyParametersHelper, assertFunctionDataParser, configurationService, constructorDataParser, diagramResourcesManager, enumHelper, exceptionHelper, functionDataParser, functionHelper, functionsDataParser, getValidConfigurationFromData, literalListDataParser, literalListParameterDataParser, literalListVariableDataParser, metaObjectDataParser, moduleDataParser, objectDataParser, objectListDataParser, objectListParameterDataParser, objectListVariableDataParser, objectParameterDataParser, objectVariableDataParser, parameterHelper, retractFunctionDataParser, shapeXmlHelper, xmlDocumentHelpers, typeLoadHelper, variableDataParser, variableHelper, variableValueDataParser)
        {
            _connectorDataParser = connectorDataParser;
            _decisionDataParser = decisionDataParser;
            _decisionsDataParser = decisionsDataParser;
        }

        internal override IList<RuleBag> Rules => base.GetRules();

        internal override void GenerateRules()
        {
            string decisionsXml = _shapeXmlHelper.GetXmlString(RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(decisionsXml))
                throw _exceptionHelper.CriticalException("{49316E0F-38CE-43F7-AC16-5EB3843AE8C3}");

            string connectorXml = _shapeXmlHelper.GetXmlString(FirstConnector);
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
        }

        /// <summary>
        /// generates one rule when decisions are ORed
        /// </summary>
        private void OrRules(DecisionsData decisionsData, ConnectorData firstConnectorData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add
            (
                GetAllDecisions
                (
                    decisionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanOr
                )
            );

            GenerateRightHandSide();
        }

        /// <summary>
        /// generates one rule when decisions are "ANDED"
        /// </summary>
        private void AndRules(DecisionsData decisionsData, ConnectorData firstConnectorData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add
            (
                GetAllDecisions
                (
                    decisionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanAnd
                )
            );

            GenerateRightHandSide();
        }

        private IfCondition GetAllDecisions(DecisionsData decisionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator) 
            => new
            (
                CodeExpressionBuilderUtility.AggregateConditions
                (
                    decisionsData
                        .DecisionElements
                        .Select(e => _decisionDataParser.Parse(e))
                        .Select(dData => codeExpressionBuilderUtility.BuildIfCondition(dData)),
                    binaryOperator
                ),
                !isYesPath//if yes Path the Not is false
            );
    }
}
