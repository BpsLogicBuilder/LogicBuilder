namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories
{
    internal interface IEditDialogConnectorFieldControlFactory
    {
        IConnectorObjectRichTextBoxControl GetConnectorObjectRichTextBoxControl(IEditingControl editingControl);
        IConnectorTextRichInputBoxControl GetConnectorTextRichInputBoxControl(IEditingControl editingControl);
    }
}
