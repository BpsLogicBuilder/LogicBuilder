using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class SelectRulesFormFactory : ISelectRulesFormFactory
    {
        private readonly IServiceScope _scope;
        private readonly Func<string, SelectRulesForm> _factoryMethod;
        private SelectRulesForm? _scopedService;

        public SelectRulesFormFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            _factoryMethod = _scope.ServiceProvider.GetRequiredService<Func<string, SelectRulesForm>>();
        }

        public SelectRulesForm GetScopedService(string applicationName)
        {
            _scopedService = _factoryMethod(applicationName);
            return _scopedService;
        }

        public void Dispose()
        {
            //The factory method uses ActivatorUtilities.CreateInstance (outside the container) because of the parameter
            //so we have to dispose of the service manually (_scope.Dispose() will not dispose _scopedService).
            _scopedService?.Dispose();
            _scope.Dispose();
        }
    }
}
