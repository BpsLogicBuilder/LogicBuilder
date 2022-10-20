using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class DialogWithoutExitsRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DialogWithoutExitsRuleBuilder(
            IExceptionHelper exceptionHelper, 
            IFunctionDataParser functionDataParser, 
            IFunctionsDataParser functionsDataParser,
            IRuleBuilderFactory ruleBuilderFactory,
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
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        private IfCondition QuestionNotAnsweredCondition
            => new(_codeExpressionBuilder.BuildSelectCondition(string.Empty));

        public IList<RuleBag> GenerateRules()
        {
            BuildQuestionRule();

            return _shapeSetRuleBuilderHelper.GetRules();
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

            void AddDialogFunction(FunctionsData functionsData)
            {
                _shapeSetRuleBuilderHelper.ThenActions.Add
                (
                    new RuleStatementAction
                    (
                        _codeExpressionBuilder.BuildFunction
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
