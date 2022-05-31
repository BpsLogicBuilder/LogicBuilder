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
    internal class ConditionsRuleBuilderUtility : ShapeSetRuleBuilderUtility
    {
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IConnectorDataParser _connectorDataParser;

        public ConditionsRuleBuilderUtility(
            IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings,
            IContextProvider contextProvider,
            IAnyParametersHelper anyParametersHelper,
            IAssertFunctionDataParser assertFunctionDataParser,
            IConditionsDataParser conditionsDataParser,
            IConfigurationService configurationService,
            IConnectorDataParser connectorDataParser,
            IConstructorDataParser constructorDataParser,
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
            _conditionsDataParser = conditionsDataParser;
            _connectorDataParser = connectorDataParser;
        }

        internal override IList<RuleBag> Rules => base.GetRules();

        internal override void GenerateRules()
        {
            string conditionsXml = _shapeXmlHelper.GetXmlString(RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(conditionsXml))
                throw _exceptionHelper.CriticalException("{3C274E76-0F7A-47DE-9CEF-4469BE712F32}");

            string connectorXml = _shapeXmlHelper.GetXmlString(FirstConnector);
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
        }

        /// <summary>
        /// generates one rule when decisions are ORed
        /// </summary>
        private void OrRule(ConditionsData conditionsData, ConnectorData firstConnectorData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            //all conditions into one rule
            Conditions.Add
            (
                GetAllConditions
                (
                    conditionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanOr
                )
            );

            GenerateRightHandSide();
        }

        /// <summary>
        /// generates one rule when decisions are ANDed
        /// </summary>
        private void AndRule(ConditionsData conditionsData, ConnectorData firstConnectorData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            //all conditions into one rule
            Conditions.Add
            (
                GetAllConditions
                (
                    conditionsData,
                    firstConnectorData.Index == 1,
                    CodeBinaryOperatorType.BooleanAnd
                )
            );

            GenerateRightHandSide();
        }

        private IfCondition GetAllConditions(ConditionsData conditionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator) 
            => new
            (
                CodeExpressionBuilderUtility.AggregateConditions
                (
                    conditionsData
                        .FunctionElements
                        .Select(e => _functionDataParser.Parse(e))
                        .Select(fData => codeExpressionBuilderUtility.BuildIfCondition(fData)),
                    binaryOperator
                ),
                !isYesPath//if yes Path the Not is false
            );
    }
}
