using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories
{
    internal class EditDecisionFormCommandFactory : IEditDecisionFormCommandFactory
    {
        public AddDecisionFunctionListBoxItemCommand GetAddDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IDecisionFunctionListBoxItemFactory>(),
                editDecisionForm
            );

        public EditDecisionFormCopyXmlCommand GetEditDecisionFormCopyXmlCommand(IEditDecisionForm editDecisionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionForm
            );

        public EditDecisionFormEditXmlCommand GetEditDecisionFormEditXmlCommand(IEditDecisionForm editDecisionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDecisionForm
            );

        public UpdateDecisionFunctionListBoxItemCommand GetUpdateDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IDecisionFunctionListBoxItemFactory>(),
                editDecisionForm
            );
    }
}
