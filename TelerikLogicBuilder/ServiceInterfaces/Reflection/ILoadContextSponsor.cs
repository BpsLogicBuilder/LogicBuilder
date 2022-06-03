using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface ILoadContextSponsor
    {
        void LoadAssembiesIfNeeded(IProgress<ProgressMessage>? progress = null);
        Task LoadAssembiesIfNeededAsync(IProgress<ProgressMessage>? progress = null);
        Task LoadAssembiesOnOpenProject();
        void Run(Action action, IProgress<ProgressMessage> progress);
        Task RunAsync(Func<Task> func, IProgress<ProgressMessage> progress);
        Task<T> RunAsync<T>(Func<Task<T>> func, IProgress<ProgressMessage> progress);
        void UnloadAssembliesOnCloseProject();
    }
}
