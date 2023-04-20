using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConditionFunctionsFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditConditionFunctionsFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditConditionFunctionsForm, AddConditionFunctionListBoxItemCommand>>
                (
                    provider =>
                    editConditionFunctionsForm => new AddConditionFunctionListBoxItemCommand
                    (
                        provider.GetRequiredService<IConditionFunctionListBoxItemFactory>(),
                        editConditionFunctionsForm
                    )
                )
                .AddTransient<IEditConditionFunctionsFormCommandFactory, EditConditionFunctionsFormCommandFactory>()
                .AddTransient<Func<IEditConditionFunctionsForm, EditConditionFunctionsFormCopyXmlCommand>>
                (
                    provider =>
                    editConditionFunctionsForm => new EditConditionFunctionsFormCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editConditionFunctionsForm
                    )
                )
                .AddTransient<Func<IEditConditionFunctionsForm, EditConditionFunctionsFormEditXmlCommand>>
                (
                    provider =>
                    editConditionFunctionsForm => new EditConditionFunctionsFormEditXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editConditionFunctionsForm
                    )
                )
                .AddTransient<Func<IEditConditionFunctionsForm, UpdateConditionFunctionListBoxItemCommand>>
                (
                    provider =>
                    editConditionFunctionsForm => new UpdateConditionFunctionListBoxItemCommand
                    (
                        provider.GetRequiredService<IConditionFunctionListBoxItemFactory>(),
                        editConditionFunctionsForm
                    )
                );
        }
    }
}
