using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Factories
{
    internal class ComponentFactory : IComponentFactory
    {
        public IFileSystemTreeView GetFileSystemTreeView()
        {
            return new FileSystemTreeView
            (
                Program.ServiceProvider.GetRequiredService<IFileSystemDragDropHandler>()
            ); 
        }

        public IObjectRichTextBox GetObjectRichTextBox()
        {
            return new ObjectRichTextBox();
        }

        public IRichInputBox GetRichInputBox()
        {
            return new RichInputBox
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IPathHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>()
            );
        }
    }
}
