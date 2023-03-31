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
                .AddTransient<EditXmlRichTextBoxPanel>()
                .AddTransient<IMessages, Messages>()
                .AddTransient<IProjectExplorer, ProjectExplorer>()
                .AddSingleton<IRadCheckBoxHelper, RadCheckBoxHelper>()
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

                .AddRichTextBoxPanelCommandFactories()

                //RulesExplorerHelpers
                .AddRulesExplorerCommands();
        }
    }
}
