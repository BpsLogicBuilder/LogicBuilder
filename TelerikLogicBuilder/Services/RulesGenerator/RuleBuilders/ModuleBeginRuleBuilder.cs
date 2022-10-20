using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class ModuleBeginRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;

        public ModuleBeginRuleBuilder(
            IRuleBuilderFactory ruleBuilderFactory,
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
            _shapeSetRuleBuilderHelper = ruleBuilderFactory.GetShapeSetRuleBuilderHelper
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
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDirectorPropertyCondition(DirectorProperties.MODULEBEGINPROPERTY, new CodePrimitiveExpression(_shapeSetRuleBuilderHelper.ModuleName))));
            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDirectorPropertyCondition(DirectorProperties.DRIVERPROPERTY, new CodePrimitiveExpression(string.Empty))));
            _shapeSetRuleBuilderHelper.GenerateRightHandSide();

            return _shapeSetRuleBuilderHelper.GetRules();
        }
    }
}
