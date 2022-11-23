using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal class ApplicationControlCommandFactory : IApplicationControlCommandFactory
    {
        private readonly Func<IApplicationControl, EditActivityAssemblyCommand> _getEditActivityAssemblyCommand;
        private readonly Func<IApplicationControl, EditActivityAssemblyPathCommand> _getEditActivityAssemblyPathCommand;
        private readonly Func<IApplicationControl, EditExcludedModulesCommand> _getEditExcludedModulesCommand;
        private readonly Func<IApplicationControl, EditLoadAssemblyPathsCommand> _getEditLoadAssemblyPathsCommand;
        private readonly Func<IApplicationControl, EditResourceFilesDeploymentCommand> _getEditResourceFilesDeploymentCommand;
        private readonly Func<IApplicationControl, EditRulesDeploymentCommand> _getEditRulesDeploymentCommand;
        private readonly Func<IApplicationControl, EditWebApiDeploymentCommand> _getEditWebApiDeploymentCommand;

        public ApplicationControlCommandFactory(
            Func<IApplicationControl, EditActivityAssemblyCommand> getEditActivityAssemblyCommand,
            Func<IApplicationControl, EditActivityAssemblyPathCommand> getEditActivityAssemblyPathCommand,
            Func<IApplicationControl, EditExcludedModulesCommand> getEditExcludedModulesCommand,
            Func<IApplicationControl, EditLoadAssemblyPathsCommand> getEditLoadAssemblyPathsCommand,
            Func<IApplicationControl, EditResourceFilesDeploymentCommand> getEditResourceFilesDeploymentCommand,
            Func<IApplicationControl, EditRulesDeploymentCommand> getEditRulesDeploymentCommand,
            Func<IApplicationControl, EditWebApiDeploymentCommand> getEditWebApiDeploymentCommand)
        {
            _getEditActivityAssemblyCommand = getEditActivityAssemblyCommand;
            _getEditActivityAssemblyPathCommand = getEditActivityAssemblyPathCommand;
            _getEditExcludedModulesCommand = getEditExcludedModulesCommand;
            _getEditLoadAssemblyPathsCommand = getEditLoadAssemblyPathsCommand;
            _getEditResourceFilesDeploymentCommand = getEditResourceFilesDeploymentCommand;
            _getEditRulesDeploymentCommand = getEditRulesDeploymentCommand;
            _getEditWebApiDeploymentCommand = getEditWebApiDeploymentCommand;
        }

        public EditActivityAssemblyCommand GetEditActivityAssemblyCommand(IApplicationControl applicationControl)
            => _getEditActivityAssemblyCommand(applicationControl);

        public EditActivityAssemblyPathCommand GetEditActivityAssemblyPathCommand(IApplicationControl applicationControl)
            => _getEditActivityAssemblyPathCommand(applicationControl);

        public EditExcludedModulesCommand GetEditExcludedModulesCommand(IApplicationControl applicationControl)
            => _getEditExcludedModulesCommand(applicationControl);

        public EditLoadAssemblyPathsCommand GetEditLoadAssemblyPathsCommand(IApplicationControl applicationControl)
            => _getEditLoadAssemblyPathsCommand(applicationControl);

        public EditResourceFilesDeploymentCommand GetEditResourceFilesDeploymentCommand(IApplicationControl applicationControl)
            => _getEditResourceFilesDeploymentCommand(applicationControl);

        public EditRulesDeploymentCommand GetEditRulesDeploymentCommand(IApplicationControl applicationControl)
            => _getEditRulesDeploymentCommand(applicationControl);

        public EditWebApiDeploymentCommand GetEditWebApiDeploymentCommand(IApplicationControl applicationControl)
            => _getEditWebApiDeploymentCommand(applicationControl);
    }
}
