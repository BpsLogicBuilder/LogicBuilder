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
                .AddTransient<IMessages, Messages>()
                .AddTransient<IProjectExplorer, ProjectExplorer>()
                .AddSingleton<IRadCheckBoxHelper, RadCheckBoxHelper>()
                .AddSingleton<IRadDropDownListHelper, RadDropDownListHelper>()
                .AddTransient<RichInputBoxMessagePanel>()
                .AddTransient<RulesExplorer>()

                //ConfigurationExplorerHelpers
                .AddConfiguratioExplorerCommands()

                //DialogFormMessageControlHelpers
                .AddDialogFormMessageControlCommands()
                .AddDialogFormMessageControlFactories()

                //DocumentsExplorerHelpers
                .AddDocumentsExplorerCommands()

                .AddRichTextBoxPanelCommandFactories()

                //RulesExplorerHelpers
                .AddRulesExplorerCommands()
                
                .AddUserControlFactories();
        }
    }
}
