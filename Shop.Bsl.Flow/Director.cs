using LogicBuilder.RulesDirector;
using Shop.Bsl.Flow.Interfaces;

namespace Shop.Bsl.Flow
{
    public class Director(IFlowManager flowManager) : AppDirectorBase
    {
        private readonly IFlowManager _flowManager = flowManager;

        protected override IRulesCache RulesCache => _flowManager.RulesCache;
        protected override IFlowActivity FlowActivity => _flowManager.FlowActivity;
        protected override Progress Progress => _flowManager.Progress;

        public override void SetCurrentBusinessBackupData() => _flowManager.SetCurrentBusinessBackupData();
    }
}
