namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories
{
    internal interface ILoadAssemblyPathsItemFactory
    {
        AssemblyPath GetAssemblyPath(string path);
    }
}
