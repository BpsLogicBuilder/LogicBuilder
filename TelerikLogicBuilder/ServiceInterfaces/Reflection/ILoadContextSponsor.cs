using System;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface ILoadContextSponsor
    {
        void LoadAssembiesIfNeeded();
        Task LoadAssembiesIfNeededAsync();
        Task LoadAssembiesOnOpenProject();
        void Run(Action action);
        Task RunAsync(Action action);
        void UnloadAssembliesOnCloseProject();
    }
}
