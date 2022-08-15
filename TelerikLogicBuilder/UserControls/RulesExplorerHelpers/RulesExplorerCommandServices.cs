using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers.Forms;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesExplorerCommandServices
    {
        internal static IServiceCollection AddRulesExplorerCommands(this IServiceCollection services)
        {
            return services
                .AddTransient<RadRuleSetDialog>()
                .AddTransient<DeleteAllRulesCommand>()
                .AddTransient<DeleteRulesExplorerFileCommand>()
                .AddTransient<RefreshRulesExplorerCommand>()
                .AddTransient<ValidateCommand>()
                .AddTransient<ViewCommand>();
        }
    }
}
