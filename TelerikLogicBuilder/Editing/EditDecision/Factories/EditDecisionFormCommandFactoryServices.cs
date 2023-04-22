using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDecisionFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditDecisionFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditDecisionForm, AddDecisionFunctionListBoxItemCommand>>
                (
                    provider =>
                    editDecisionForm => new AddDecisionFunctionListBoxItemCommand
                    (
                        provider.GetRequiredService<IDecisionFunctionListBoxItemFactory>(),
                        editDecisionForm
                    )
                )
                .AddTransient<IEditDecisionFormCommandFactory, EditDecisionFormCommandFactory>()
                .AddTransient<Func<IEditDecisionForm, EditDecisionFormCopyXmlCommand>>
                (
                    provider =>
                    editDecisionForm => new EditDecisionFormCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionForm
                    )
                )
                .AddTransient<Func<IEditDecisionForm, EditDecisionFormEditXmlCommand>>
                (
                    provider =>
                    editDecisionForm => new EditDecisionFormEditXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionForm
                    )
                )
                .AddTransient<Func<IEditDecisionForm, UpdateDecisionFunctionListBoxItemCommand>>
                (
                    provider =>
                    editDecisionForm => new UpdateDecisionFunctionListBoxItemCommand
                    (
                        provider.GetRequiredService<IDecisionFunctionListBoxItemFactory>(),
                        editDecisionForm
                    )
                );
        }
    }
}
