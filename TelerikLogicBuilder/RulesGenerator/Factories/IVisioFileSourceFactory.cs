namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal interface IVisioFileSourceFactory
    {
        VisioFileSource GetVisioFileSource(string sourceFileFullname, int pageId, short pageIndex, string shapeMasterName, int shapeId, int shapeIndex);
    }
}
