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
                .AddTransient<Func<IEditParameterObjectListControl, AddParameterObjectListBoxItemCommand>>
                (
                    provider =>
                    editParameterObjectListControl => new AddParameterObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        editParameterObjectListControl
                    )
                )
                .AddTransient<IEditObjectListCommandFactory, EditObjectListCommandFactory>()
                .AddTransient<Func<IEditParameterObjectListForm, EditParameterObjectListFormXmlCommand>>
                (
                    provider =>
                    editParameterObjectListForm => new EditParameterObjectListFormXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editParameterObjectListForm
                    )
                )
                .AddTransient<Func<IEditParameterObjectListControl, UpdateParameterObjectListBoxItemCommand>>
                (
                    provider =>
                    editParameterObjectListControl => new UpdateParameterObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        editParameterObjectListControl
                    )
                );
        }
    }
}
