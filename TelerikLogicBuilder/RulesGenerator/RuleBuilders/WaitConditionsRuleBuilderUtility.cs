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
    internal class WaitConditionsRuleBuilderUtility : ShapeSetRuleBuilderUtility
    {
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IShapeHelper _shapeHelper;

        public WaitConditionsRuleBuilderUtility(
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
            _conditionsDataParser = conditionsDataParser;
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
            string conditionsXml = _shapeXmlHelper.GetXmlString(RuleShapes[0].Shape);
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
        }

        /// <summary>
        /// Generates one rule when conditions are ORed
        /// </summary>
        private void OrRule(ConditionsData conditionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllConditions(conditionsData, true, CodeBinaryOperatorType.BooleanOr));

            GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when conditions are ORed and the resultant condition is false
        /// </summary>
        private void FalseOrRule(ConditionsData conditionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllConditions(conditionsData, false, CodeBinaryOperatorType.BooleanOr));

            ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule when conditions are ANDed
        /// </summary>
        private void AndRule(ConditionsData conditionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllConditions(conditionsData, true, CodeBinaryOperatorType.BooleanAnd));

            GenerateRightHandSide();
            AssembleRule();
        }

        /// <summary>
        /// Generates one rule to call Wait function when conditions are ANDed and the resultant condition is false
        /// </summary>
        private void FalseAndRule(ConditionsData conditionsData)
        {
            RuleCount++;
            Conditions.Add(new IfCondition(CodeExpressionBuilderUtility.BuildDriverCondition(RuleShapes[0].Shape.Index, RuleShapes[0].Shape.ContainingPage.Index)));

            Conditions.Add(GetAllConditions(conditionsData, false, CodeBinaryOperatorType.BooleanAnd));

            ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONWAIT, Array.Empty<CodeExpression>())));
            AssembleRule();
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
