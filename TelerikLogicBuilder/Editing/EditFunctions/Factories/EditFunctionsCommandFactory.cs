using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class EditFunctionsCommandFactory : IEditFunctionsCommandFactory
    {
        public AddFunctionListBoxItemCommand GetAddFunctionListBoxItemCommand(IEditFunctionsForm editFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFunctionListBoxItemFactory>(),
                editFunctionsForm
            );

        public EditFunctionsFormCopyXmlCommand GetEditFFunctionsFormCopyXmlCommand(IEditFunctionsForm editFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editFunctionsForm
            );

        public EditFunctionsFormEditXmlCommand GetEditFunctionsFormXmlCommand(IEditFunctionsForm editFunctionsForm)
            => new 
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editFunctionsForm
            );

        public UpdateFunctionListBoxItemCommand GetUpdateFunctionListBoxItemCommand(IEditFunctionsForm editFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFunctionListBoxItemFactory>(),
                editFunctionsForm
            );
    }
}
