using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal class SelectRulesFormFactory : ISelectRulesFormFactory
    {
        private readonly IServiceScope _scope;
        private readonly ICreateSelectRulesFormFactory _createSelectRulesFormFactory;
        private ISelectRulesForm? _scopedService;

        public SelectRulesFormFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            _createSelectRulesFormFactory = _scope.ServiceProvider.GetRequiredService<ICreateSelectRulesFormFactory>();
        }

        public ISelectRulesForm GetScopedService(string applicationName)
        {
            _scopedService = _createSelectRulesFormFactory.GetSelectRulesForm(applicationName);
            return _scopedService;
        }

        public void Dispose()
        {
            //The factory method uses new SelectRulesForm() (outside the container) because of the parameter
            //so we have to dispose of the service manually (_scope.Dispose() will not dispose _scopedService).
            _scopedService?.Dispose();
            _scope.Dispose();
        }
    }
}
