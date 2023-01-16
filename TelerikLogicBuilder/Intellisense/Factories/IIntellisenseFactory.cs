using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal interface IIntellisenseFactory
    {
        IIntellisenseConstructorsFormManager GetIntellisenseConstructorsFormManager(IConfigureConstructorsHelperForm configureConstructorsHelperForm);
        IIntellisenseFunctionsFormManager GetIntellisenseFunctionsFormManager(IConfiguredItemHelperForm configuredItemHelperForm);
        IIntellisenseVariablesFormManager GetIntellisenseVariablesFormManager(IConfiguredItemHelperForm configuredItemHelperForm);
    }
}
