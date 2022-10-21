using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class BeginFlowRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;

        public BeginFlowRuleBuilder(
            IRulesGeneratorFactory rulesGeneratorFactory,
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
            _shapeSetRuleBuilderHelper = rulesGeneratorFactory.GetShapeSetRuleBuilderHelper
            (
                ruleShapes,
                ruleConnectors,
                moduleName,
                ruleCount,
                application,
                resourceStrings
            );
        }

        public IList<RuleBag> GenerateRules()
        {
            _shapeSetRuleBuilderHelper.RuleCount++;
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDirectorPropertyCondition(DirectorProperties.DRIVERPROPERTY, new CodePrimitiveExpression(string.Empty))));
            _shapeSetRuleBuilderHelper.ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(_shapeSetRuleBuilderHelper.DirectorReference, RuleFunctionConstants.SETMODULENAMEFUNCTIONNAME, new CodeExpression[] { new CodePrimitiveExpression(_shapeSetRuleBuilderHelper.ModuleName) })));

            _shapeSetRuleBuilderHelper.GenerateRightHandSide();
            return _shapeSetRuleBuilderHelper.GetRules();
        }
    }
}
