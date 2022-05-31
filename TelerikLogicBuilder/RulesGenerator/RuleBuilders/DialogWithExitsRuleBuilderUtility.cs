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
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class DialogWithExitsRuleBuilderUtility : ShapeSetRuleBuilderUtility
    {
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IShapeHelper _shapeHelper;

        public DialogWithExitsRuleBuilderUtility(
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
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IVariableHelper variableHelper,
            IVariableValueDataParser variableValueDataParser) : base(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings, contextProvider, anyParametersHelper, assertFunctionDataParser, configurationService, constructorDataParser, diagramResourcesManager, enumHelper, exceptionHelper, functionDataParser, functionHelper, functionsDataParser, getValidConfigurationFromData, literalListDataParser, literalListParameterDataParser, literalListVariableDataParser, metaObjectDataParser, moduleDataParser, objectDataParser, objectListDataParser, objectListParameterDataParser, objectListVariableDataParser, objectParameterDataParser, objectVariableDataParser, parameterHelper, retractFunctionDataParser, shapeXmlHelper, xmlDocumentHelpers, typeLoadHelper, variableDataParser, variableHelper, variableValueDataParser)
        {
            _connectorDataParser = connectorDataParser;
            _shapeHelper = shapeHelper;

            string connectorXml = _shapeXmlHelper.GetXmlString(FirstConnector);
            if (string.IsNullOrEmpty(connectorXml))
                throw _exceptionHelper.CriticalException("{E2562487-C87D-4228-A8B1-89A663E1DA4E}");

            firstConnectorData = _connectorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(connectorXml)
            );
        }

        private readonly ConnectorData firstConnectorData;
        private readonly List<RuleBag> rules = new();

        private IfCondition QuestionAnsweredCondition
            => new(codeExpressionBuilderUtility.BuildSelectCondition(firstConnectorData.TextXmlNode));

        private static IfCondition QuestionNotAnsweredCondition
            => new(CodeExpressionBuilderUtility.BuildSelectCondition(string.Empty));

        internal override IList<RuleBag> Rules => rules;

        internal override void GenerateRules()
        {
            if (firstConnectorData.Index == 1)//build Question rule only for the first connector
                BuildQuestionRule();

            BuildContinueRule();
        }

        /// <summary>
        /// Generates the rule which displays the dialog
        /// </summary>
        private void BuildQuestionRule()
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));
            Conditions.Add(QuestionNotAnsweredCondition);

            ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(DirectorReference, RuleFunctionConstants.VIRTUALFUNCTIONSETBUSINESSBACKUPDATA, Array.Empty<CodeExpression>())));

            string functionsXml = _shapeXmlHelper.GetXmlString(RuleShapes[0].Shape);
            if (string.IsNullOrEmpty(functionsXml))
                throw _exceptionHelper.CriticalException("{B346D7C0-A16E-455F-89A0-0FDA60723247}");

            AddDialogFunction
            (
                _functionsDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(functionsXml))
            );

            AssembleRule();

            void AddDialogFunction(FunctionsData functionsData)
            {
                ThenActions.Add
                (
                    new RuleStatementAction
                    (
                        codeExpressionBuilderUtility.BuildFunction
                        (
                            _functionDataParser.Parse(functionsData.FunctionElements[0]),
                            _shapeHelper.GetMultipleChoiceConnectorData(RuleShapes[0].Shape)
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
        protected void BuildContinueRule()
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));
            Conditions.Add(QuestionAnsweredCondition);

            GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Assembles the rule and clears all actions and conditions
        /// </summary>
        void AssembleRule()
        {
            Rule rule = new(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.RULENAMEFORMAT, ModuleName, RuleCount, RuleConnectors[0].Index, RuleConnectors[0].ContainingPage.Index))
            {
                Condition = new RuleExpressionCondition
                (
                    CodeExpressionBuilderUtility.AggregateConditions(this.Conditions, CodeBinaryOperatorType.BooleanAnd)
                )
            };
            
            foreach (RuleAction thenAction in ThenActions)
                rule.ThenActions.Add(thenAction);

            rules.Add(new RuleBag(rule));

            Conditions.Clear();
            ThenActions.Clear();
        }
    }
}
