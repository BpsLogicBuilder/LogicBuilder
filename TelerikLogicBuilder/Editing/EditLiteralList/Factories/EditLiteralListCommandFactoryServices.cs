using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
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
                .AddTransient<Func<IEditParameterLiteralListControl, AddParameterLiteralListBoxItemCommand>>
                (
                    provider =>
                    editParameterLiteralListControl => new AddParameterLiteralListBoxItemCommand
                    (
                        provider.GetRequiredService<ILiteralListBoxItemFactory>(),
                        editParameterLiteralListControl
                    )
                )
                .AddTransient<IEditLiteralListCommandFactory, EditLiteralListCommandFactory>()
                .AddTransient<Func<IEditParameterLiteralListForm, EditParameterLiteralListFormXmlCommand>>
                (
                    provider =>
                    editParameterLiteralListForm => new EditParameterLiteralListFormXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editParameterLiteralListForm
                    )
                )
                .AddTransient<Func<IEditParameterLiteralListControl, UpdateParameterLiteralListBoxItemCommand>>
                (
                    provider =>
                    editParameterLiteralListControl => new UpdateParameterLiteralListBoxItemCommand
                    (
                        provider.GetRequiredService<ILiteralListBoxItemFactory>(),
                        editParameterLiteralListControl
                    )
                );
        }
    }
}
