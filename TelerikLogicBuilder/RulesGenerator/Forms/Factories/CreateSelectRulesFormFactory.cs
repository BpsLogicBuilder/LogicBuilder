using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal class CreateSelectRulesFormFactory : ICreateSelectRulesFormFactory
    {
        private readonly Func<string, ISelectRulesForm> _getSelectRulesForm;
        private readonly Func<string, ISelectRulesResourcesPairForm> _getSelectRulesResourcesPairForm;

        public CreateSelectRulesFormFactory(
            Func<string, ISelectRulesForm> getSelectRulesForm,
            Func<string, ISelectRulesResourcesPairForm> getSelectRulesResourcesPairForm)
        {
            _getSelectRulesForm = getSelectRulesForm;
            _getSelectRulesResourcesPairForm = getSelectRulesResourcesPairForm;
        }

        public ISelectRulesForm GetSelectRulesForm(string applicationName)
            => _getSelectRulesForm(applicationName);

        public ISelectRulesResourcesPairForm GetSelectRulesResourcesPairForm(string applicationName)
            => _getSelectRulesResourcesPairForm(applicationName);
    }
}
