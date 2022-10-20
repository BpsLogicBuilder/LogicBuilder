using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System.CodeDom;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface IShapeSetRuleBuilderHelper
    {
        ApplicationTypeInfo Application { get; }
        IList<IfCondition> Conditions { get; }
        CodePropertyReferenceExpression DirectorReference { get; }
        CodePropertyReferenceExpression DriverReference { get; }
        Shape FirstConnector { get; }
        IList<RuleBag> GetRules();
        void GenerateRightHandSide();
        CodePropertyReferenceExpression ModuleBeginReference { get; }
        CodePropertyReferenceExpression ModuleEndReference { get; }
        string ModuleName { get; }
        Page Page { get; }
        IDictionary<string, string> ResourceStrings { get; }
        int RuleCount { get; set; }
        IList<Shape> RuleConnectors { get; }
        IList<ShapeBag> RuleShapes { get; }
        CodePropertyReferenceExpression SelectionReference { get; }
        IList<RuleAction> ThenActions { get; }
        CodeThisReferenceExpression ThisReference { get; }
    }
}
