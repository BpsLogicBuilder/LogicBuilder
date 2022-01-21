namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class WebApiDeployment
    {
        internal WebApiDeployment(string postFileDataUrl, string postVariablesMetaUrl, string deleteRulesUrl, string deleteAllRulesUrl)
        {
            this.PostFileDataUrl = postFileDataUrl;
            this.PostVariablesMetaUrl = postVariablesMetaUrl;
            this.DeleteRulesUrl = deleteRulesUrl;
            this.DeleteAllRulesUrl = deleteAllRulesUrl;
        }

        public string PostFileDataUrl { get; private set; }
        public string PostVariablesMetaUrl { get; private set; }
        public string DeleteRulesUrl { get; private set; }
        public string DeleteAllRulesUrl { get; private set; }
    }
}
