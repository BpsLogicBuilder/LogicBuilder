namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal interface ITableFileSourceFactory
    {
        TableFileSource GetTableFileSource(string sourceFileFullname, int row, int column);
    }
}
