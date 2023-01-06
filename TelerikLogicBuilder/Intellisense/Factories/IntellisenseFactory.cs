using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal class IntellisenseFactory : IIntellisenseFactory
    {
        private readonly Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager> _getIntellisenseFunctionsFormManager;
        private readonly Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager> _getIntellisenseVariablesFormManager;

        public IntellisenseFactory(
            Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager> getIntellisenseFunctionsFormManager,
            Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager> getIntellisenseVariablesFormManager)
        {
            _getIntellisenseFunctionsFormManager = getIntellisenseFunctionsFormManager;
            _getIntellisenseVariablesFormManager = getIntellisenseVariablesFormManager;
        }

        public IIntellisenseFunctionsFormManager GetIntellisenseFunctionsFormManager(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIntellisenseFunctionsFormManager(configuredItemHelperForm);

        public IIntellisenseVariablesFormManager GetIntellisenseVariablesFormManager(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIntellisenseVariablesFormManager(configuredItemHelperForm);
    }
}
