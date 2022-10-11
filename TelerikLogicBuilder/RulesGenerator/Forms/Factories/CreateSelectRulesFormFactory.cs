using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal class CreateSelectRulesFormFactory : ICreateSelectRulesFormFactory
    {
        private readonly Func<string, SelectRulesForm> _getSelectRulesForm;
        private readonly Func<string, SelectRulesResourcesPairForm> _getSelectRulesResourcesPairForm;

        public CreateSelectRulesFormFactory(
            Func<string, SelectRulesForm> getSelectRulesForm,
            Func<string, SelectRulesResourcesPairForm> getSelectRulesResourcesPairForm)
        {
            _getSelectRulesForm = getSelectRulesForm;
            _getSelectRulesResourcesPairForm = getSelectRulesResourcesPairForm;
        }

        public SelectRulesForm GetSelectRulesForm(string applicationName)
            => _getSelectRulesForm(applicationName);

        public SelectRulesResourcesPairForm GetSelectRulesResourcesPairForm(string applicationName)
            => _getSelectRulesResourcesPairForm(applicationName);
    }
}
