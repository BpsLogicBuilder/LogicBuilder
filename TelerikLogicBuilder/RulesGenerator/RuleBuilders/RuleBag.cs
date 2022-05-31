using LogicBuilder.Workflow.Activities.Rules;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class RuleBag
    {
        internal RuleBag(Rule rule, IList<string> applications)
        {
            this.Rule = rule;
            this.Applications = applications;
        }

        internal RuleBag(Rule rule)
        {
            this.Rule = rule;
        }

        #region Variables
        #endregion Variables

        #region Properties
        /// <summary>
        /// The Rule
        /// </summary>
        internal Rule Rule { get; }

        /// <summary>
        /// Indicates which application the rule applies to.  Applications null means all applications are valid
        /// </summary>
        internal IList<string>? Applications { get; }
        #endregion Properties
    }
}
