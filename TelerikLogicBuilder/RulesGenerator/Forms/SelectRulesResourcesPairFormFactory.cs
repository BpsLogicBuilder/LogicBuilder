using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class SelectRulesResourcesPairFormFactory : ISelectRulesResourcesPairFormFactory
    {
        private readonly IServiceScope _scope;
        private readonly Func<string, SelectRulesResourcesPairForm> _factoryMethod;
        private SelectRulesResourcesPairForm? _scopedService;

        public SelectRulesResourcesPairFormFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            _factoryMethod = _scope.ServiceProvider.GetRequiredService<Func<string, SelectRulesResourcesPairForm>>();
        }

        public SelectRulesResourcesPairForm GetScopedService(string applicationName)
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
