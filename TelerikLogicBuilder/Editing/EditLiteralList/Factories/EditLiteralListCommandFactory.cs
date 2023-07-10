using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class EditLiteralListCommandFactory : IEditLiteralListCommandFactory
    {
        public AddParameterLiteralListBoxItemCommand GetAddParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<ILiteralListBoxItemFactory>(),
                editParameterLiteralListControl
            );

        public AddVariableLiteralListBoxItemCommand GetAddVariableLiteralListBoxItemCommand(IEditVariableLiteralListControl editVariableLiteralListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<ILiteralListBoxItemFactory>(),
                editVariableLiteralListControl
            );

        public EditParameterLiteralListFormXmlCommand GetEditParameterLiteralListFormXmlCommand(IEditParameterLiteralListForm editParameterLiteralListForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editParameterLiteralListForm
            );

        public EditVariableLiteralListFormXmlCommand GetEditVariableLiteralListFormXmlCommand(IEditVariableLiteralListForm editVariableLiteralListForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editVariableLiteralListForm
            );

        public UpdateParameterLiteralListBoxItemCommand GetUpdateParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<ILiteralListBoxItemFactory>(),
                editParameterLiteralListControl
            );

        public UpdateVariableLiteralListBoxItemCommand GetUpdateVariableLiteralListBoxItemCommand(IEditVariableLiteralListControl editVariableLiteralListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<ILiteralListBoxItemFactory>(),
                editVariableLiteralListControl
            );
    }
}
