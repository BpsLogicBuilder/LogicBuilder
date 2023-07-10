using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal class EditConditionFunctionsFormCommandFactory : IEditConditionFunctionsFormCommandFactory
    {
        public AddConditionFunctionListBoxItemCommand GetAddConditionFunctionListBoxItemCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConditionFunctionListBoxItemFactory>(),
                editConditionFunctionsForm
            );

        public EditConditionFunctionsFormCopyXmlCommand GetEditConditionFunctionsFormCopyXmlCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editConditionFunctionsForm
            );

        public EditConditionFunctionsFormEditXmlCommand GetEditConditionFunctionsFormEditXmlCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editConditionFunctionsForm
            );

        public UpdateConditionFunctionListBoxItemCommand GetUpdateConditionFunctionListBoxItemCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConditionFunctionListBoxItemFactory>(),
                editConditionFunctionsForm
            );
    }
}
