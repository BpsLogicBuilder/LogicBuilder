using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector
{
    internal interface IConnectorObjectRichTextBoxControl : IParameterRichTextBoxValueControl
    {
        void DisableControls();
        void EnableControls();
        void SetAssignedToType(Type type);
        void SetupDefaultElement(Type type);
    }
}
