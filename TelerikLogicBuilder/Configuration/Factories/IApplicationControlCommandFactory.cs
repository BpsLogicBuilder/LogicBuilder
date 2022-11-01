using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IApplicationControlCommandFactory
    {
        EditActivityAssemblyCommand GetEditActivityAssemblyCommand(IApplicationControl applicationControl);
        EditActivityAssemblyPathCommand GetEditActivityAssemblyPathCommand(IApplicationControl applicationControl);
        EditExcludedModulesCommand GetEditExcludedModulesCommand(IApplicationControl applicationControl);
        EditLoadAssemblyPathsCommand GetEditLoadAssemblyPathsCommand(IApplicationControl applicationControl);
        EditResourceFilesDeploymentCommand GetEditResourceFilesDeploymentCommand(IApplicationControl applicationControl);
        EditRulesDeploymentCommand GetEditRulesDeploymentCommand(IApplicationControl applicationControl);
        EditWebApiDeploymentCommand GetEditWebApiDeploymentCommand(IApplicationControl applicationControl);
    }
}
