﻿using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Factories
{
    internal class ComponentFactory : IComponentFactory
    {
        public FileSystemTreeView GetFileSystemTreeView()
        {
            return new FileSystemTreeView
            (
                Program.ServiceProvider.GetRequiredService<IFileSystemDragDropHandler>()
            ); 
        }

        public ObjectRichTextBox GetObjectRichTextBox()
        {
            return new ObjectRichTextBox();
        }

        public RichInputBox GetRichInputBox()
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