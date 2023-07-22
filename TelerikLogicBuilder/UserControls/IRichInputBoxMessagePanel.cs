using ABIS.LogicBuilder.FlowBuilder.Components;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IRichInputBoxMessagePanel
    {
        DockStyle Dock { set; }
        IRichInputBox RichInputBox { get; }
    }
}
