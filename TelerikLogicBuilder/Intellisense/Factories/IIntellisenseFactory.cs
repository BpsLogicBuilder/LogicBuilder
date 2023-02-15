using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal interface IIntellisenseFactory
    {
        IIntellisenseConstructorsFormManager GetIntellisenseConstructorsFormManager(IConfigureConstructorsHelperForm configureConstructorsHelperForm);
        IIntellisenseFunctionsFormManager GetIntellisenseFunctionsFormManager(IConfiguredItemHelperForm configuredItemHelperForm);
        IIntellisenseIncludesFormManager GetIntellisenseIncludesFormManager(IIncludesHelperForm includesHelperForm);
        IIntellisenseVariablesFormManager GetIntellisenseVariablesFormManager(IConfiguredItemHelperForm configuredItemHelperForm);
    }
}
