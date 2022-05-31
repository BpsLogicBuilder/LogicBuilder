using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface IWaitConditionsRuleBuilder
    {
        IList<RuleBag> GenerateRules(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings);
    }
}
