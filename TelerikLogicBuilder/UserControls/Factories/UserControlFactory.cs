using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal class UserControlFactory : IUserControlFactory
    {
        public IEditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel()
        {
            return new EditXmlRichTextBoxPanel
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IRichTextBoxPanelCommandFactory>()
            );
        }

        public IMessages GetMessages()
        {
            return new Messages
            (
                Program.ServiceProvider.GetRequiredService<IDiagramErrorSourceDataParser>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IMainWindow>(),
                Program.ServiceProvider.GetRequiredService<IOpenFileOperations>(),
                Program.ServiceProvider.GetRequiredService<ITableErrorSourceDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IUiNotificationService>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>()
            );
        }

        public IProjectExplorer GetProjectExplorer()
        {
            return new ProjectExplorer
            (
                Program.ServiceProvider.GetRequiredService<IMainWindow>(),
                Program.ServiceProvider.GetRequiredService<ConfigurationExplorer>(),
                Program.ServiceProvider.GetRequiredService<DocumentsExplorer>(),
                Program.ServiceProvider.GetRequiredService<RulesExplorer>()
            );
        }

        public RichInputBoxMessagePanel GetRichInputBoxMessagePanel()
        {
            return new RichInputBoxMessagePanel
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>()
            );
        }

        public IRichTextBoxPanel GetRichTextBoxPanel()
        {
            return new RichTextBoxPanel
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IRichTextBoxPanelCommandFactory>()
            );
        }
    }
}
