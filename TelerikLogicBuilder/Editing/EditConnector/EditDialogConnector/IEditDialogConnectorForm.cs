using ABIS.LogicBuilder.FlowBuilder.Structures;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector
{
    internal interface IEditDialogConnectorForm : IEditConnectorForm, ISetDialogMessages, IApplicationHostControl
    {
        RadButton RadButtonOk { get; }
    }
}
