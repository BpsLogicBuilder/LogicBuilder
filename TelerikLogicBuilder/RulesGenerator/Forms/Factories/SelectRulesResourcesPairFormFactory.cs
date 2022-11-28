using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal class SelectRulesResourcesPairFormFactory : ISelectRulesResourcesPairFormFactory
    {
        private readonly IServiceScope _scope;
        private readonly ICreateSelectRulesFormFactory _createSelectRulesFormFactory;
        private ISelectRulesResourcesPairForm? _scopedService;

        public SelectRulesResourcesPairFormFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            _createSelectRulesFormFactory = _scope.ServiceProvider.GetRequiredService<ICreateSelectRulesFormFactory>();
        }

        public ISelectRulesResourcesPairForm GetScopedService(string applicationName)
        {
            _scopedService = _createSelectRulesFormFactory.GetSelectRulesResourcesPairForm(applicationName);
            return _scopedService;
        }

        public void Dispose()
        {
            //The factory method uses new SelectRulesResourcesPairForm() (outside the container) because of the parameter
            //so we have to dispose of the service manually (_scope.Dispose() will not dispose _scopedService).
            _scopedService?.Dispose();
            _scope.Dispose();
        }

    }
}
