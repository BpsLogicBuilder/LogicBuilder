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
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class DialogWithoutExitsRuleBuilderUtility : ShapeSetRuleBuilderUtility
    {
        public DialogWithoutExitsRuleBuilderUtility(
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
        }

        private static IfCondition QuestionNotAnsweredCondition
            => new(CodeExpressionBuilderUtility.BuildSelectCondition(string.Empty));

        internal override IList<RuleBag> Rules => base.GetRules();

        internal override void GenerateRules()
        {
            BuildQuestionRule();
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

            void AddDialogFunction(FunctionsData functionsData)
            {
                ThenActions.Add
                (
                    new RuleStatementAction
                    (
                        codeExpressionBuilderUtility.BuildFunction
                        (
                            _functionDataParser.Parse(functionsData.FunctionElements[0]),
                            null
                        )
                    )
                );
            }
        }
    }
}
