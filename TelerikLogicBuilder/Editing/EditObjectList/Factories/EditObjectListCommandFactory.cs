using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class EditObjectListCommandFactory : IEditObjectListCommandFactory
    {
        public AddParameterObjectListBoxItemCommand GetAddParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IObjectListBoxItemFactory>(),
                editParameterObjectListControl
            );

        public AddVariableObjectListBoxItemCommand GetAddVariableObjectListBoxItemCommand(IEditVariableObjectListControl editVariableObjectListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IObjectListBoxItemFactory>(),
                editVariableObjectListControl
            );

        public EditParameterObjectListFormXmlCommand GetEditParameterObjectListFormXmlCommand(IEditParameterObjectListForm editParameterObjectListForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editParameterObjectListForm
            );

        public EditVariableObjectListFormXmlCommand GetEditVariableObjectListFormXmlCommand(IEditVariableObjectListForm editVariableObjectListForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editVariableObjectListForm
            );

        public UpdateParameterObjectListBoxItemCommand GetUpdateParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IObjectListBoxItemFactory>(),
                editParameterObjectListControl
            );

        public UpdateVariableObjectListBoxItemCommand GetUpdateVariableObjectListBoxItemCommand(IEditVariableObjectListControl editVariableObjectListControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IObjectListBoxItemFactory>(),
                editVariableObjectListControl
            );
    }
}
