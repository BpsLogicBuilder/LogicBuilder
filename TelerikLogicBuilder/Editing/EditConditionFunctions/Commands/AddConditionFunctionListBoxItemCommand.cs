using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands
{
    internal class AddConditionFunctionListBoxItemCommand : ClickCommandBase
    {
        private readonly IConditionFunctionListBoxItemFactory _conditionFunctionListBoxItemFactory;
        private readonly IEditConditionFunctionsForm editConditionFunctionsForm;

        public AddConditionFunctionListBoxItemCommand(
            IConditionFunctionListBoxItemFactory conditionFunctionListBoxItemFactory,
            IEditConditionFunctionsForm editConditionFunctionsForm)
        {
            _conditionFunctionListBoxItemFactory = conditionFunctionListBoxItemFactory;
            this.editConditionFunctionsForm = editConditionFunctionsForm;
        }

        private IEditConditionFunctionControl EditConditionFunctionControl => editConditionFunctionsForm.EditConditionFunctionControl;
        private IEditingControl? CurrentEditingControl => editConditionFunctionsForm.CurrentEditingControl;
        private IRadListBoxManager<IConditionFunctionListBoxItem> RadListBoxManager => editConditionFunctionsForm.RadListBoxManager;

        public override void Execute()
        {
            if (CurrentEditingControl == null)
                return;

            try
            {
                CurrentEditingControl.ValidateFields();
                CurrentEditingControl.RequestDocumentUpdate();
                EditConditionFunctionControl.ValidateXmlDocument();
                RadListBoxManager.Add
                (
                    _conditionFunctionListBoxItemFactory.GetConditionFunctionListBoxItem
                    (
                        EditConditionFunctionControl.VisibleText,
                        EditConditionFunctionControl.XmlResult.OuterXml,
                       editConditionFunctionsForm
                    )
                );
            }
            catch (LogicBuilderException ex)
            {
                editConditionFunctionsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
