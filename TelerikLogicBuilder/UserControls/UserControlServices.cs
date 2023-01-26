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
                .AddTransient<IDialogFormMessageControl, DialogFormMessageControl>()
                .AddTransient<DocumentsExplorer>()
                .AddTransient<IMessages, Messages>()
                .AddTransient<IProjectExplorer, ProjectExplorer>()
                .AddSingleton<IRadDropDownListHelper, RadDropDownListHelper>()
                .AddTransient<RichInputBoxMessagePanel>()
                .AddTransient<RichTextBoxPanel>()
                .AddTransient<RulesExplorer>()

                //ConfigurationExplorerHelpers
                .AddConfiguratioExplorerCommands()

                //DialogFormMessageControlHelpers
                .AddDialogFormMessageControlCommands()

                //DocumentsExplorerHelpers
                .AddDocumentsExplorerCommands()

                //RulesExplorerHelpers
                .AddRulesExplorerCommands();
        }
    }
}
