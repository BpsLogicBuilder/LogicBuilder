using Contoso.Test.Flow.Cache;
using LogicBuilder.RulesDirector;

namespace Contoso.Test.Flow
{
    public interface IFlowManager
    {
        ICustomActions CustomActions { get; }
        DirectorBase Director { get; }
        IFlowActivity FlowActivity { get; }
        FlowDataCache FlowDataCache { get; }
        Progress Progress { get; }

        void Start(string module);
        void SetCurrentBusinessBackupData();
        void FlowComplete();
        void Terminate();
    }
}