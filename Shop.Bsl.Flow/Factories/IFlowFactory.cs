using LogicBuilder.RulesDirector;
using Shop.Bsl.Flow.Interfaces;

namespace Shop.Bsl.Flow.Factories
{
    public interface IFlowFactory
    {
        DirectorBase GetDirector(IFlowManager flowManager);
        IFlowActivity GetFlowActivity(IFlowManager flowManager);
    }
}
