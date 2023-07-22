using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal class UserControlFactory : IUserControlFactory
    {
        public IConfigurationExplorer GetConfigurationExplorer()
        {
            return new ConfigurationExplorer
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationExplorerTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<EditConfigurationCommand>(),
                Program.ServiceProvider.GetRequiredService<RefreshConfigurationExplorerCommand>()
            );
        }

        public IDocumentsExplorer GetDocumentsExplorer()
        {
            return new DocumentsExplorer
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IMainWindow>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUiNotificationService>(),
                Program.ServiceProvider.GetRequiredService<AddExistingFileCommand>(),
                Program.ServiceProvider.GetRequiredService<AddNewFileCommand>(),
                Program.ServiceProvider.GetRequiredService<CloseProjectCommand>(),
                Program.ServiceProvider.GetRequiredService<CreateDirectoryCommand>(),
                Program.ServiceProvider.GetRequiredService<CutCommand>(),
                Program.ServiceProvider.GetRequiredService<DeleteCommand>(),
                Program.ServiceProvider.GetRequiredService<OpenFileCommand>(),
                Program.ServiceProvider.GetRequiredService<PasteCommand>(),
                Program.ServiceProvider.GetRequiredService<RefreshDocumentsExplorerCommand>(),
                Program.ServiceProvider.GetRequiredService<RenameCommand>()
            );
        }

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
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>()
            );
        }

        public IRichInputBoxMessagePanel GetRichInputBoxMessagePanel()
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

        public IRulesExplorer GetRulesExplorer()
        {
            return new RulesExplorer
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IMainWindow>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUiNotificationService>(),
                Program.ServiceProvider.GetRequiredService<DeleteAllRulesCommand>(),
                Program.ServiceProvider.GetRequiredService<DeleteRulesExplorerFileCommand>(),
                Program.ServiceProvider.GetRequiredService<ValidateCommand>(),
                Program.ServiceProvider.GetRequiredService<ViewCommand>(),
                Program.ServiceProvider.GetRequiredService<RefreshRulesExplorerCommand>()
            );
        }
    }
}
