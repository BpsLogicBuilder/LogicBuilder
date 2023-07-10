using ABIS.LogicBuilder.FlowBuilder.Editing.EditCell;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditShape;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class DocumentEditorFactory : IDocumentEditorFactory
    {
        public TableControl GetTableControl(string tableSourceFile, bool openedAsReadOnly)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IMainWindow>(),
                Program.ServiceProvider.GetRequiredService<IPathHelper>(),
                Program.ServiceProvider.GetRequiredService<ITableEditor>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IUiNotificationService>(),
                tableSourceFile,
                openedAsReadOnly
            );

        public VisioControl GetVisioControl(string visioSourceFile, bool openedAsReadOnly)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IDiagramEditor>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IMainWindow>(),
                Program.ServiceProvider.GetRequiredService<IPathHelper>(),
                Program.ServiceProvider.GetRequiredService<IUiNotificationService>(),
                visioSourceFile,
                openedAsReadOnly
            );
    }
}
