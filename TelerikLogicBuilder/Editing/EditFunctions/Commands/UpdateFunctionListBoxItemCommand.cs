using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands
{
    internal class UpdateFunctionListBoxItemCommand : ClickCommandBase
    {
        private readonly IFunctionListBoxItemFactory _functionListBoxItemFactory;
        private readonly IEditFunctionsForm editFunctionsForm;

        public UpdateFunctionListBoxItemCommand(
            IFunctionListBoxItemFactory functionListBoxItemFactory,
            IEditFunctionsForm editFunctionsForm)
        {
            _functionListBoxItemFactory = functionListBoxItemFactory;
            this.editFunctionsForm = editFunctionsForm;
        }

        private IEditingControl? CurrentEditingControl => editFunctionsForm.CurrentEditingControl;
        private IRadListBoxManager<IFunctionListBoxItem> RadListBoxManager => editFunctionsForm.RadListBoxManager;

        public override void Execute()
        {
            if (CurrentEditingControl == null)
                return;

            try
            {
                CurrentEditingControl.ValidateFields();
                RadListBoxManager.Update
                (
                    _functionListBoxItemFactory.GetFunctionListBoxItem
                    (
                        CurrentEditingControl.VisibleText,
                        CurrentEditingControl.XmlResult.OuterXml,
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
