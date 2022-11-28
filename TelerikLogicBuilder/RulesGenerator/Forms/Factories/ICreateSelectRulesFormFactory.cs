namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories
{
    internal interface ICreateSelectRulesFormFactory
    {
        ISelectRulesForm GetSelectRulesForm(string applicationName);
        ISelectRulesResourcesPairForm GetSelectRulesResourcesPairForm(string applicationName);
    }
}
