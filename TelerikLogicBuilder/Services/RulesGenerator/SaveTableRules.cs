using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class SaveTableRules : ISaveTableRules
    {
        private readonly ISaveRules _saveRules;

        public SaveTableRules(ISaveRules saveRules)
        {
            _saveRules = saveRules;
        }

        public void Save(string sourceFile, DataSet dataSet, IList<RuleBag> rules)
        {
            _saveRules.Save
            (
                sourceFile,
                rules,
                ProjectPropertiesConstants.TABLEFOLDER,
                GetChainingBehavior(dataSet)
            );
        }

        private static RuleChainingBehavior GetChainingBehavior(DataSet dataSet)
        {
            if (dataSet.Tables[TableName.RULESETTABLE]!.Rows.Count == 0)
                return RuleChainingBehavior.Full;

            if (dataSet.Tables[TableName.RULESETTABLE]!.Rows[0][TableColumns.CHAININGCOLUMNINDEX] == null)
                return RuleChainingBehavior.Full;

            string chainingBehaviour = dataSet.Tables[TableName.RULESETTABLE]!.Rows[0][TableColumns.CHAININGCOLUMNINDEX].ToString()!;

            return Enum.IsDefined(typeof(RuleChainingBehavior), chainingBehaviour)
                ? (RuleChainingBehavior)Enum.Parse(typeof(RuleChainingBehavior), chainingBehaviour)
                : RuleChainingBehavior.Full;
        }
    }
}
