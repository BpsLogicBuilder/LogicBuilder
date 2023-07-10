using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal class EditDecisionsFormCommandFactory : IEditDecisionsFormCommandFactory
    {
        public AddDecisionListBoxItemCommand GetAddDecisionListBoxItemCommand(IEditDecisionsForm editDecisionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IDecisionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDecisionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionsForm
            );

        public EditDecisionCommand GetEditDecisionCommand(IEditDecisionsForm editDecisionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionsForm
            );

        public EditDecisionsFormCopyXmlCommand GetEditDecisionsFormCopyXmlCommand(IEditDecisionsForm editDecisionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionsForm
            );

        public EditDecisionsFormEditXmlCommand GetEditDecisionsFormEditXmlCommand(IEditDecisionsForm editDecisionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionsForm
            );

        public UpdateDecisionListBoxItemCommand GetUpdateDecisionListBoxItemCommand(IEditDecisionsForm editDecisionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IDecisionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDecisionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionsForm
            );
    }
}
