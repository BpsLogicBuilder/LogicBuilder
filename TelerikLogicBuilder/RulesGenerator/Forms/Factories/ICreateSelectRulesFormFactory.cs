namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal interface ICreateSelectRulesFormFactory
    {
        ISelectRulesForm GetSelectRulesForm(string applicationName);
        SelectRulesResourcesPairForm GetSelectRulesResourcesPairForm(string applicationName);
    }
}
