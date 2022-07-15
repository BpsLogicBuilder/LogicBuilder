using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DocumentsExplorerCommandServices
    {
        internal static IServiceCollection AddDocumentsExplorerCommands(this IServiceCollection services) 
            => services.AddTransient<AddNewFileForm>()
                .AddTransient<Func<IDocumentsExplorer, AddExistingFileCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<AddExistingFileCommand>
                    (
                        provider,
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, AddNewFileCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<AddNewFileCommand>
                    (
                        provider,
                        provider.GetRequiredService<IAddNewFileOperations>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IMessageBoxOptionsHelper>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<CloseProjectCommand>>
                (
                    provider =>
                    () => ActivatorUtilities.CreateInstance<CloseProjectCommand>
                    (
                        provider
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, CreateDirectoryCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<CreateDirectoryCommand>
                    (
                        provider,
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IMessageBoxOptionsHelper>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, CutDocumentCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<CutDocumentCommand>
                    (
                        provider,
                        provider.GetRequiredService<ITreeViewService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, DeleteCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<DeleteCommand>
                    (
                        provider,
                        provider.GetRequiredService<IDeleteOperations>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, OpenFileCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<OpenFileCommand>
                    (
                        provider,
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, PasteCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<PasteCommand>
                    (
                        provider,
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IMoveFileOperations>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, RefreshDocumentsExplorerCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<RefreshDocumentsExplorerCommand>
                    (
                        provider,
                        documentExplorer
                    )
                )
                .AddTransient<Func<IDocumentsExplorer, RenameCommand>>
                (
                    provider =>
                    documentExplorer => ActivatorUtilities.CreateInstance<RenameCommand>
                    (
                        provider,
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IMessageBoxOptionsHelper>(),
                        provider.GetRequiredService<IMoveFileOperations>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        documentExplorer
                    )
                )
                .AddTransient<IOpenFileOperations, OpenFileOperations>()//must be transient because it uses a scoped disposable
                .AddTransient<IAddNewFileOperations, AddNewFileOperations>()//must be transient because it uses a scoped disposable
                .AddTransient<InputBoxForm>()
                .AddSingleton<IDeleteOperations, DeleteOperations>()
                .AddSingleton<IMoveFileOperations, MoveFileOperations>()
                .AddTransient<TextViewer>();
    }
}
