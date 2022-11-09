using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface ILoadAssemblyPathsCommandFactory
    {
        AddAssemblyPathListBoxItemCommand GetAddAssemblyPathListBoxItemCommand(ILoadAssemblyPathsControl loadAssemblyPathsControl);
        UpdateAssemblyPathListBoxItemCommand GetUpdateAssemblyPathListBoxItemCommand(ILoadAssemblyPathsControl loadAssemblyPathsControl);
    }
}
