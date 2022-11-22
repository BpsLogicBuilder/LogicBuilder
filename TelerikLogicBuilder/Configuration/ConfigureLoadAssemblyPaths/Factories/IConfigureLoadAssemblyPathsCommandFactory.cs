using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories
{
    internal interface IConfigureLoadAssemblyPathsCommandFactory
    {
        AddAssemblyPathListBoxItemCommand GetAddAssemblyPathListBoxItemCommand(IConfigureLoadAssemblyPathsControl loadAssemblyPathsControl);
        UpdateAssemblyPathListBoxItemCommand GetUpdateAssemblyPathListBoxItemCommand(IConfigureLoadAssemblyPathsControl loadAssemblyPathsControl);
    }
}
