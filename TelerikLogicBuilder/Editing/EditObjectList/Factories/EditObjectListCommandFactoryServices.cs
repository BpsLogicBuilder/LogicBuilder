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
                .AddTransient<Func<IEditVariableObjectListControl, AddVariableObjectListBoxItemCommand>>
                (
                    provider =>
                    editVariableObjectListControl => new AddVariableObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        editVariableObjectListControl
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
                .AddTransient<Func<IEditVariableObjectListForm, EditVariableObjectListFormXmlCommand>>
                (
                    provider =>
                    editVariableObjectListForm => new EditVariableObjectListFormXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editVariableObjectListForm
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
                )
                .AddTransient<Func<IEditVariableObjectListControl, UpdateVariableObjectListBoxItemCommand>>
                (
                    provider =>
                    editVariableObjectListControl => new UpdateVariableObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        editVariableObjectListControl
                    )
                );
        }
    }
}
