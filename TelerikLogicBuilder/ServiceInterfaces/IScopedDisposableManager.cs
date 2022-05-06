using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IScopedDisposableManager<T> : IDisposable where T : IDisposable
    {
        T ScopedService { get; }
    }
}
