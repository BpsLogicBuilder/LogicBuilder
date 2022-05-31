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
    internal class WaitDecisionsRuleBuilderUtility : ShapeSetRuleBuilderUtility
    {
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IShapeHelper _shapeHelper;

        public WaitDecisionsRuleBuilderUtility(
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
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IVariableHelper variableHelper,
            IVariableValueDataParser variableValueDataParser) : base(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings, contextProvider, anyParametersHelper, assertFunctionDataParser, configurationService, constructorDataParser, diagramResourcesManager, enumHelper, exceptionHelper, functionDataParser, functionHelper, functionsDataParser, getValidConfigurationFromData, literalListDataParser, literalListParameterDataParser, literalListVariableDataParser, metaObjectDataParser, moduleDataParser, objectDataParser, objectListDataParser, objectListParameterDataParser, objectListVariableDataParser, objectParameterDataParser, objectVariableDataParser, parameterHelper, retractFunctionDataParser, shapeXmlHelper, xmlDocumentHelpers, typeLoadHelper, variableDataParser, variableHelper, variableValueDataParser)
        {
            _decisionDataParser = decisionDataParser;
            _decisionsDataParser = decisionsDataParser;
            _shapeHelper = shapeHelper;
        }

        private readonly List<RuleBag> rules = new();

        internal override IList<RuleBag> Rules => rules;

        /// <summary>
        /// Assembles the entire rule and clears all conditions
        /// </summary>
        private void AssembleRule()
        {
            Rule rule = new(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.RULENAMEFORMAT, ModuleName, RuleCount, RuleConnectors[0].Index, RuleConnectors[0].ContainingPage.Index))
            {
                Condition = new RuleExpressionCondition(CodeExpressionBuilderUtility.AggregateConditions(this.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in ThenActions)
                rule.ThenActions.Add(thenAction);

            if (RuleConnectors[0].Master.NameU == UniversalMasterName.CONNECTOBJECT)
                rules.Add(new RuleBag(rule));
            else
                rules.Add(new RuleBag(rule, _shapeHelper.GetApplicationList(RuleConnectors[0], RuleShapes[0])));

            Conditions.Clear();
            ThenActions.Clear();
        }

        internal override void GenerateRules()
        {
            string decisionsXml = _shapeXmlHelper.GetXmlString(RuleShapes[0].Shape);
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
        }

        /// <summary>
        /// Generates one rule when decisions are ORed
        /// </summary>
        private void OrRule(DecisionsData decisionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllDecisions(decisionsData, true, CodeBinaryOperatorType.BooleanOr));

            GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when decisions are ORed and the condition resultant is false
        /// </summary>
        private void FalseOrRule(DecisionsData decisionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllDecisions(decisionsData, false, CodeBinaryOperatorType.BooleanOr));

            ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule when decisions are "ANDED"
        /// </summary>
        private void AndRule(DecisionsData decisionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllDecisions(decisionsData, true, CodeBinaryOperatorType.BooleanAnd));

            GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when decisions are "ANDED" and the condition resultant is false
        /// </summary>
        private void FalseAndRule(DecisionsData decisionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllDecisions(decisionsData, false, CodeBinaryOperatorType.BooleanAnd));

            ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        private IfCondition GetAllDecisions(DecisionsData decisionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator)
        {
            return new IfCondition
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
}
