using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditFunctionsCommandFactoryServices
    {
        internal static IServiceCollection AddEditFunctionsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditFunctionsForm, AddFunctionListBoxItemCommand>>
                (
                    provider =>
                    editFunctionsForm => new AddFunctionListBoxItemCommand
                    (
                        provider.GetRequiredService<IFunctionListBoxItemFactory>(),
                        editFunctionsForm
                    )
                )
                .AddTransient<IEditFunctionsCommandFactory, EditFunctionsCommandFactory>()
                .AddTransient<Func<IEditFunctionsForm, EditFunctionsFormCopyXmlCommand>>
                (
                    provider =>
                    editFunctionsForm => new EditFunctionsFormCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editFunctionsForm
                    )
                )
                .AddTransient<Func<IEditFunctionsForm, EditFunctionsFormEditXmlCommand>>
                (
                    provider =>
                    editFunctionsForm => new EditFunctionsFormEditXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editFunctionsForm
                    )
                )
                .AddTransient<Func<IEditFunctionsForm, UpdateFunctionListBoxItemCommand>>
                (
                    provider =>
                    editFunctionsForm => new UpdateFunctionListBoxItemCommand
                    (
                        provider.GetRequiredService<IFunctionListBoxItemFactory>(),
                        editFunctionsForm
                    )
                );
        }
    }
}
