using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories
{
    internal class ConfigureLoadAssemblyPathsCommandFactory : IConfigureLoadAssemblyPathsCommandFactory
    {
        private readonly Func<IConfigureLoadAssemblyPathsControl, AddAssemblyPathListBoxItemCommand> _getAddAssemblyPathListBoxItemCommand;
        private readonly Func<IConfigureLoadAssemblyPathsControl, UpdateAssemblyPathListBoxItemCommand> _getUpdateAssemblyPathListBoxItemCommand;

        public ConfigureLoadAssemblyPathsCommandFactory(
            Func<IConfigureLoadAssemblyPathsControl, AddAssemblyPathListBoxItemCommand> getAddAssemblyPathListBoxItemCommand,
            Func<IConfigureLoadAssemblyPathsControl, UpdateAssemblyPathListBoxItemCommand> getUpdateAssemblyPathListBoxItemCommand)
        {
            _getAddAssemblyPathListBoxItemCommand = getAddAssemblyPathListBoxItemCommand;
            _getUpdateAssemblyPathListBoxItemCommand = getUpdateAssemblyPathListBoxItemCommand;
        }

        public AddAssemblyPathListBoxItemCommand GetAddAssemblyPathListBoxItemCommand(IConfigureLoadAssemblyPathsControl loadAssemblyPathsControl)
            => _getAddAssemblyPathListBoxItemCommand(loadAssemblyPathsControl);

        public UpdateAssemblyPathListBoxItemCommand GetUpdateAssemblyPathListBoxItemCommand(IConfigureLoadAssemblyPathsControl loadAssemblyPathsControl)
            => _getUpdateAssemblyPathListBoxItemCommand(loadAssemblyPathsControl);
    }
}
