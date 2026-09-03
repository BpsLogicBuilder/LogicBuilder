using LogicBuilder.App.Bsl.Flow.Interfaces;
using LogicBuilder.RulesDirector;
using System;

namespace Shop.Bsl.Flow.Interfaces
{
    public interface IFlowManager
    {
        ICustomActions CustomActions { get; }
        DirectorBase Director { get; }
        IFlowActivity FlowActivity { get; }
        IFlowDataCache FlowDataCache { get; }
        Progress Progress { get; }
        IRulesCache RulesCache { get; }
        IServiceProvider ServiceProvider { get; }

        void Start(string module);
        void SetCurrentBusinessBackupData();
        void FlowComplete();
        void Terminate();
    }
}
