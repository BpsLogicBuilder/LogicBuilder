namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories
{
    internal interface IIntellisenseFactory
    {
        IIntellisenseFunctionsFormManager GetIntellisenseFunctionsFormManager(IConfiguredItemHelperForm configuredItemHelperForm);
        IIntellisenseVariablesFormManager GetIntellisenseVariablesFormManager(IConfiguredItemHelperForm configuredItemHelperForm);
    }
}
