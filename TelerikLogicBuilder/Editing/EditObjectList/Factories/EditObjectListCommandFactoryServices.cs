using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditObjectListCommandFactoryServices
    {
        internal static IServiceCollection AddEditObjectListCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditParameterObjectListControl, AddObjectListBoxItemCommand>>
                (
                    provider =>
                    editObjectListControl => new AddObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        editObjectListControl
                    )
                )
                .AddTransient<IEditObjectListCommandFactory, EditObjectListCommandFactory>()
                .AddTransient<Func<IEditParameterObjectListForm, EditObjectListFormXmlCommand>>
                (
                    provider =>
                    editObjectListForm => new EditObjectListFormXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editObjectListForm
                    )
                )
                .AddTransient<Func<IEditParameterObjectListControl, UpdateObjectListBoxItemCommand>>
                (
                    provider =>
                    editObjectListControl => new UpdateObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        editObjectListControl
                    )
                );
        }
    }
}
