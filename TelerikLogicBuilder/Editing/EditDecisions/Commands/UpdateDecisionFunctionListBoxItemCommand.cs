using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class UpdateDecisionFunctionListBoxItemCommand : ClickCommandBase
    {
        private readonly IDecisionFunctionListBoxItemFactory _decisionFunctionListBoxItemFactory;
        private readonly IEditDecisionForm editDecisionForm;

        public UpdateDecisionFunctionListBoxItemCommand(
            IDecisionFunctionListBoxItemFactory decisionFunctionListBoxItemFactory,
            IEditDecisionForm editDecisionForm)
        {
            _decisionFunctionListBoxItemFactory = decisionFunctionListBoxItemFactory;
            this.editDecisionForm = editDecisionForm;
        }

        private IEditConditionFunctionControl EditConditionFunctionControl => editDecisionForm.EditConditionFunctionControl;
        private IEditingControl? CurrentEditingControl => editDecisionForm.CurrentEditingControl;
        private IRadListBoxManager<IDecisionFunctionListBoxItem> RadListBoxManager => editDecisionForm.RadListBoxManager;

        public override void Execute()
        {
            if (CurrentEditingControl == null)
                return;

            try
            {
                CurrentEditingControl.ValidateFields();
                CurrentEditingControl.RequestDocumentUpdate();
                EditConditionFunctionControl.ValidateXmlDocument();
                RadListBoxManager.Update
                (
                    _decisionFunctionListBoxItemFactory.GetDecisionFunctionListBoxItem
                    (
                        EditConditionFunctionControl.VisibleText,
                        EditConditionFunctionControl.XmlResult.OuterXml,
                        editDecisionForm
                    )
                );
            }
            catch (LogicBuilderException ex)
            {
                editDecisionForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
