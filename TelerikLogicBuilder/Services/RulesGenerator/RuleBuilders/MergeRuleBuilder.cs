using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class MergeRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeSetRuleBuilderHelper _shapeSetRuleBuilderHelper;

        public MergeRuleBuilder(
            IRulesGeneratorFactory rulesGeneratorFactory, 
            IShapeHelper shapeHelper,
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
            _shapeHelper = shapeHelper;
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

            _shapeSetRuleBuilderHelper.Conditions.Add(new IfCondition(_codeExpressionBuilder.BuildDriverCondition(_shapeSetRuleBuilderHelper.RuleShapes[0].Shape.Index, _shapeSetRuleBuilderHelper.RuleShapes[0].Shape.ContainingPage.Index)));
            _shapeSetRuleBuilderHelper.GenerateRightHandSide();

            return GetRules();
        }

        private IList<RuleBag> GetRules()
        {
            Rule rule = new(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.RULENAMEFORMAT, _shapeSetRuleBuilderHelper.ModuleName, _shapeSetRuleBuilderHelper.RuleCount, _shapeSetRuleBuilderHelper.RuleConnectors[0].Index, _shapeSetRuleBuilderHelper.RuleConnectors[0].ContainingPage.Index))
            {
                Condition = new RuleExpressionCondition(_codeExpressionBuilder.AggregateConditions(_shapeSetRuleBuilderHelper.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in _shapeSetRuleBuilderHelper.ThenActions)
                rule.ThenActions.Add(thenAction);

            if (_shapeSetRuleBuilderHelper.RuleConnectors[0].Master.NameU == UniversalMasterName.CONNECTOBJECT)
                return new RuleBag[] { new RuleBag(rule) };
            else
                return new RuleBag[] { new RuleBag(rule, _shapeHelper.GetApplicationList(_shapeSetRuleBuilderHelper.RuleConnectors[0], _shapeSetRuleBuilderHelper.RuleShapes[0])) };
        }
    }
}
