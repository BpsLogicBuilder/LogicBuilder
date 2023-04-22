using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDecisionsFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditDecisionsFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditDecisionsForm, AddDecisionListBoxItemCommand>>
                (
                    provider =>
                    editDecisionsForm => new AddDecisionListBoxItemCommand
                    (
                        provider.GetRequiredService<IDecisionDataParser>(),
                        provider.GetRequiredService<IDecisionListBoxItemFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionsForm
                    )
                )
                .AddTransient<Func<IEditDecisionsForm, EditDecisionCommand>>
                (
                    provider =>
                    editDecisionsForm => new EditDecisionCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionsForm
                    )
                )
                .AddTransient<IEditDecisionsFormCommandFactory, EditDecisionsFormCommandFactory>()
                .AddTransient<Func<IEditDecisionsForm, EditDecisionsFormCopyXmlCommand>>
                (
                    provider =>
                    editDecisionsForm => new EditDecisionsFormCopyXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionsForm
                    )
                )
                .AddTransient<Func<IEditDecisionsForm, EditDecisionsFormEditXmlCommand>>
                (
                    provider =>
                    editDecisionsForm => new EditDecisionsFormEditXmlCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionsForm
                    )
                )
                .AddTransient<Func<IEditDecisionsForm, UpdateDecisionListBoxItemCommand>>
                (
                    provider =>
                    editDecisionsForm => new UpdateDecisionListBoxItemCommand
                    (
                        provider.GetRequiredService<IDecisionDataParser>(),
                        provider.GetRequiredService<IDecisionListBoxItemFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDecisionsForm
                    )
                );
        }
    }
}
