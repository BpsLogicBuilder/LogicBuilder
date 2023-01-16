using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal class IntellisenseFactory : IIntellisenseFactory
    {
        private readonly Func<IConfigureConstructorsHelperForm, IIntellisenseConstructorsFormManager> _getIntellisenseConstructorsFormManager;
        private readonly Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager> _getIntellisenseFunctionsFormManager;
        private readonly Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager> _getIntellisenseVariablesFormManager;

        public IntellisenseFactory(
            Func<IConfigureConstructorsHelperForm, IIntellisenseConstructorsFormManager> getIntellisenseConstructorsFormManager,
            Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager> getIntellisenseFunctionsFormManager,
            Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager> getIntellisenseVariablesFormManager)
        {
            _getIntellisenseConstructorsFormManager = getIntellisenseConstructorsFormManager;
            _getIntellisenseFunctionsFormManager = getIntellisenseFunctionsFormManager;
            _getIntellisenseVariablesFormManager = getIntellisenseVariablesFormManager;
        }

        public IIntellisenseConstructorsFormManager GetIntellisenseConstructorsFormManager(IConfigureConstructorsHelperForm configureConstructorsHelperForm)
            => _getIntellisenseConstructorsFormManager(configureConstructorsHelperForm);

        public IIntellisenseFunctionsFormManager GetIntellisenseFunctionsFormManager(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIntellisenseFunctionsFormManager(configuredItemHelperForm);

        public IIntellisenseVariablesFormManager GetIntellisenseVariablesFormManager(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIntellisenseVariablesFormManager(configuredItemHelperForm);
    }
}
