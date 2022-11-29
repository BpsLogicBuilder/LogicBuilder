using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DocumentsExplorerCommandServices
    {
        internal static IServiceCollection AddDocumentsExplorerCommands(this IServiceCollection services) 
            => services.AddTransient<AddNewFileForm>()
                .AddTransient<AddExistingFileCommand>()
                .AddTransient<AddNewFileCommand>()
                .AddTransient<CloseProjectCommand>()
                .AddTransient<CreateDirectoryCommand>()
                .AddTransient<CutCommand>()
                .AddTransient<DeleteCommand>()
                .AddTransient<OpenFileCommand>()
                .AddTransient<PasteCommand>()
                .AddTransient<RefreshDocumentsExplorerCommand>()
                .AddTransient<RenameCommand>()
                .AddTransient<IOpenFileOperations, OpenFileOperations>()//must be transient because it uses a scoped disposable
                .AddTransient<IAddNewFileOperations, AddNewFileOperations>()//must be transient because it uses a scoped disposable
                .AddTransient<IInputBoxForm, InputBoxForm>()
                .AddSingleton<IDeleteOperations, DeleteOperations>()
                .AddSingleton<IMoveFileOperations, MoveFileOperations>()
                .AddTransient<TextViewer>();
    }
}
