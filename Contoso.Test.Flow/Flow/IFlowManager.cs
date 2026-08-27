using Contoso.Test.Flow.Cache;
using LogicBuilder.RulesDirector;
using System;

namespace Contoso.Test.Flow
{
    public interface IFlowManager
    {
        ICustomActions CustomActions { get; }
        ICustomDialogs CustomDialogs { get; }
        DirectorBase Director { get; }
        IFlowActivity FlowActivity { get; }
        FlowDataCache FlowDataCache { get; }
        Progress Progress { get; }
        IServiceProvider ServiceProvider { get; }

        void Start(string module);
        void SetCurrentBusinessBackupData();
        void FlowComplete();
        void Terminate();
    }
}