using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class UserControlServices
    {
        internal static IServiceCollection AddUserControls(this IServiceCollection services)
        {
            return services
                .AddTransient<ConfigurationExplorer>()
                .AddTransient<DocumentsExplorer>()
                .AddTransient<Messages>()
                .AddTransient<ProjectExplorer>()
                .AddSingleton<IRadDropDownListHelper, RadDropDownListHelper>()
                .AddTransient<RichInputBoxMessagePanel>()
                .AddTransient<RulesExplorer>()

                //DocumentsExplorerHelpers
                .AddDocumentsExplorerCommands()

                //RulesExplorerHelpers
                .AddRulesExplorerCommands();
        }
    }
}
