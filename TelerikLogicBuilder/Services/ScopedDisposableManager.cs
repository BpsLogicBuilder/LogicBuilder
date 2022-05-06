using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ScopedDisposableManager<T> : IScopedDisposableManager<T> where T : IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly T _scopedService;

        public ScopedDisposableManager(IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            _scopedService = _scope.ServiceProvider.GetRequiredService<T>();
        }

        public T ScopedService => _scopedService;

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
