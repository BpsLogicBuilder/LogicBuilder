using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using LogicBuilder.Workflow.Activities.Rules;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class SaveDiagramRules : ISaveDiagramRules
    {
        private readonly ISaveRules _saveRules;

        public SaveDiagramRules(ISaveRules saveRules)
        {
            _saveRules = saveRules;
        }

        public void Save(string sourceFile, IList<RuleBag> rules)
        {
            _saveRules.Save
            (
                sourceFile,
                rules,
                ProjectPropertiesConstants.DIAGRAMFOLDER,
                RuleChainingBehavior.Full
            );
        }
    }
}
