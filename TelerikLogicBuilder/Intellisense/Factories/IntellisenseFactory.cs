using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal class IntellisenseFactory : IIntellisenseFactory
    {
        private readonly Func<IConfigureConstructorsHelperForm, IIntellisenseConstructorsFormManager> _getIntellisenseConstructorsFormManager;
        private readonly Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager> _getIntellisenseFunctionsFormManager;
        private readonly Func<IIncludesHelperForm, IIntellisenseIncludesFormManager> _getIntellisenseIncludesFormManager;
        private readonly Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager> _getIntellisenseVariablesFormManager;

        public IntellisenseFactory(
            Func<IConfigureConstructorsHelperForm, IIntellisenseConstructorsFormManager> getIntellisenseConstructorsFormManager,
            Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager> getIntellisenseFunctionsFormManager,
            Func<IIncludesHelperForm, IIntellisenseIncludesFormManager> getIntellisenseIncludesFormManager,
            Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager> getIntellisenseVariablesFormManager)
        {
            _getIntellisenseConstructorsFormManager = getIntellisenseConstructorsFormManager;
            _getIntellisenseFunctionsFormManager = getIntellisenseFunctionsFormManager;
            _getIntellisenseIncludesFormManager = getIntellisenseIncludesFormManager;
            _getIntellisenseVariablesFormManager = getIntellisenseVariablesFormManager;
        }

        public IIntellisenseConstructorsFormManager GetIntellisenseConstructorsFormManager(IConfigureConstructorsHelperForm configureConstructorsHelperForm)
            => _getIntellisenseConstructorsFormManager(configureConstructorsHelperForm);

        public IIntellisenseFunctionsFormManager GetIntellisenseFunctionsFormManager(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIntellisenseFunctionsFormManager(configuredItemHelperForm);

        public IIntellisenseIncludesFormManager GetIntellisenseIncludesFormManager(IIncludesHelperForm includesHelperForm)
            => _getIntellisenseIncludesFormManager(includesHelperForm);

        public IIntellisenseVariablesFormManager GetIntellisenseVariablesFormManager(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIntellisenseVariablesFormManager(configuredItemHelperForm);
    }
}
