﻿using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditLiteralListCommandFactoryServices
    {
        internal static IServiceCollection AddEditLiteralListCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditLiteralListControl, AddLiteralListBoxItemCommand>>
                (
                    provider =>
                    editLiteralListControl => new AddLiteralListBoxItemCommand
                    (
                        provider.GetRequiredService<ILiteralListBoxItemFactory>(),
                        editLiteralListControl
                    )
                )
                .AddTransient<IEditLiteralListCommandFactory, EditLiteralListCommandFactory>()
                .AddTransient<Func<IEditLiteralListForm, EditLiteralListFormXmlCommand>>
                (
                    provider =>
                    editLiteralListControl => new EditLiteralListFormXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editLiteralListControl
                    )
                )
                .AddTransient<Func<IEditLiteralListControl, UpdateLiteralListBoxItemCommand>>
                (
                    provider =>
                    editLiteralListControl => new UpdateLiteralListBoxItemCommand
                    (
                        provider.GetRequiredService<ILiteralListBoxItemFactory>(),
                        editLiteralListControl
                    )
                );
        }
    }
}
