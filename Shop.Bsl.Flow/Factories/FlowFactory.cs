using LogicBuilder.RulesDirector;
using Shop.Bsl.Flow.Interfaces;
using System;

namespace Shop.Bsl.Flow.Factories
{
    public class FlowFactory(Func<IFlowManager, DirectorBase> getDirector, Func<IFlowManager, IFlowActivity> getFlowActivity) : IFlowFactory
    {
        private readonly Func<IFlowManager, DirectorBase> _getDirector = getDirector;
        private readonly Func<IFlowManager, IFlowActivity> _getFlowActivity = getFlowActivity;

        public DirectorBase GetDirector(IFlowManager flowManager)
            => _getDirector(flowManager);

        public IFlowActivity GetFlowActivity(IFlowManager flowManager)
            => _getFlowActivity(flowManager);
    }
}
