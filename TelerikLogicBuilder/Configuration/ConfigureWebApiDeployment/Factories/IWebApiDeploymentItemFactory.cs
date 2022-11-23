namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories
{
    internal interface IWebApiDeploymentItemFactory
    {
        WebApiDeployment GetWebApiDeployment(string postFileDataUrl, string postVariablesMetaUrl, string deleteRulesUrl, string deleteAllRulesUrl);
    }
}
