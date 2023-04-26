using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories
{
    internal class EditDialogConnectorFieldControlFactory : IEditDialogConnectorFieldControlFactory
    {
        private readonly Func<IEditingControl, IConnectorObjectRichTextBoxControl> _getConnectorObjectRichTextBoxControl;
        private readonly Func<IEditingControl, IConnectorTextRichInputBoxControl> _getConnectorTextRichInputBoxControl;

        public EditDialogConnectorFieldControlFactory(
            Func<IEditingControl, IConnectorObjectRichTextBoxControl> getConnectorObjectRichTextBoxControl,
            Func<IEditingControl, IConnectorTextRichInputBoxControl> getConnectorTextRichInputBoxControl)
        {
            _getConnectorObjectRichTextBoxControl = getConnectorObjectRichTextBoxControl;
            _getConnectorTextRichInputBoxControl = getConnectorTextRichInputBoxControl;
        }

        public IConnectorObjectRichTextBoxControl GetConnectorObjectRichTextBoxControl(IEditingControl editingControl)
            => _getConnectorObjectRichTextBoxControl(editingControl);

        public IConnectorTextRichInputBoxControl GetConnectorTextRichInputBoxControl(IEditingControl editingControl)
            => _getConnectorTextRichInputBoxControl(editingControl);
    }
}
