using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands
{
    internal class AddFunctionListBoxItemCommand : ClickCommandBase
    {
        private readonly IFunctionListBoxItemFactory _functionListBoxItemFactory;
        private readonly IEditFunctionsForm editFunctionsForm;

        public AddFunctionListBoxItemCommand(
            IFunctionListBoxItemFactory functionListBoxItemFactory,
            IEditFunctionsForm editFunctionsForm)
        {
            _functionListBoxItemFactory = functionListBoxItemFactory;
            this.editFunctionsForm = editFunctionsForm;
        }

        private IEditVoidFunctionControl EditVoidFunctionControl => editFunctionsForm.EditVoidFunctionControl;
        private IEditingControl? CurrentEditingControl => editFunctionsForm.CurrentEditingControl;
        private IRadListBoxManager<IFunctionListBoxItem> RadListBoxManager => editFunctionsForm.RadListBoxManager;

        public override void Execute()
        {
            if (CurrentEditingControl == null)
                return;

            try
            {
                CurrentEditingControl.ValidateFields();
                CurrentEditingControl.RequestDocumentUpdate();
                EditVoidFunctionControl.ValidateXmlDocument();
                RadListBoxManager.Add
                (
                    _functionListBoxItemFactory.GetFunctionListBoxItem
                    (
                        EditVoidFunctionControl.VisibleText,
                        EditVoidFunctionControl.XmlResult.OuterXml,
                        typeof(object),
                        editFunctionsForm
                    )
                );
            }
            catch (LogicBuilderException ex)
            {
                editFunctionsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
