namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories
{
    internal interface IConfigureLoadAssemblyPathsControlFactory
    {
        IConfigureLoadAssemblyPathsControl GetLoadAssemblyPathsControl(IConfigureLoadAssemblyPathsForm configureLoadAssemblyPathsForm);
    }
}
