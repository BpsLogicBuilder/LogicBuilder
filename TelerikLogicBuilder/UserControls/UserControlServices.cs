using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class UserControlServices
    {
        internal static IServiceCollection AddUserControls(this IServiceCollection services)
        {
            return services
                .AddSingleton<IRadCheckBoxHelper, RadCheckBoxHelper>()
                .AddSingleton<IRadDropDownListHelper, RadDropDownListHelper>()

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
