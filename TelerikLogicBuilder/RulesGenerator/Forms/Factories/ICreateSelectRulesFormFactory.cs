namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal interface ICreateSelectRulesFormFactory
    {
        SelectRulesForm GetSelectRulesForm(string applicationName);
        SelectRulesResourcesPairForm GetSelectRulesResourcesPairForm(string applicationName);
    }
}
