namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class RulesResourcesPair
    {
        public RulesResourcesPair(string rulesFile, string resourcesFile)
        {
            this.RulesFile = rulesFile;
            this.ResourcesFile = resourcesFile;
        }

        public string RulesFile { get; private set; }
        public string ResourcesFile { get; private set; }
    }
}
