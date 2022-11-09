using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class LoadAssemblyPathsCommandFactory : ILoadAssemblyPathsCommandFactory
    {
        private readonly Func<ILoadAssemblyPathsControl, AddAssemblyPathListBoxItemCommand> _getAddAssemblyPathListBoxItemCommand;
        private readonly Func<ILoadAssemblyPathsControl, UpdateAssemblyPathListBoxItemCommand> _getUpdateAssemblyPathListBoxItemCommand;

        public LoadAssemblyPathsCommandFactory(
            Func<ILoadAssemblyPathsControl, AddAssemblyPathListBoxItemCommand> getAddAssemblyPathListBoxItemCommand,
            Func<ILoadAssemblyPathsControl, UpdateAssemblyPathListBoxItemCommand> getUpdateAssemblyPathListBoxItemCommand)
        {
            _getAddAssemblyPathListBoxItemCommand = getAddAssemblyPathListBoxItemCommand;
            _getUpdateAssemblyPathListBoxItemCommand = getUpdateAssemblyPathListBoxItemCommand;
        }

        public AddAssemblyPathListBoxItemCommand GetAddAssemblyPathListBoxItemCommand(ILoadAssemblyPathsControl loadAssemblyPathsControl)
            => _getAddAssemblyPathListBoxItemCommand(loadAssemblyPathsControl);

        public UpdateAssemblyPathListBoxItemCommand GetUpdateAssemblyPathListBoxItemCommand(ILoadAssemblyPathsControl loadAssemblyPathsControl)
            => _getUpdateAssemblyPathListBoxItemCommand(loadAssemblyPathsControl);
    }
}
