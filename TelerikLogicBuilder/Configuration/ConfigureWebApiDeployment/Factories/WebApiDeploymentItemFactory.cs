using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories
{
    internal class WebApiDeploymentItemFactory : IWebApiDeploymentItemFactory
    {
        private readonly Func<string, string, string, string, WebApiDeployment> _getWebApiDeployment;

        public WebApiDeploymentItemFactory(
            Func<string, string, string, string, WebApiDeployment> getWebApiDeployment)
        {
            _getWebApiDeployment = getWebApiDeployment;
        }

        public WebApiDeployment GetWebApiDeployment(string postFileDataUrl, string postVariablesMetaUrl, string deleteRulesUrl, string deleteAllRulesUrl)
            => _getWebApiDeployment(postFileDataUrl, postVariablesMetaUrl, deleteRulesUrl, deleteAllRulesUrl);
    }
}
