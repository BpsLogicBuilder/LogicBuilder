using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IMainWindow
    {
        IWin32Window Instance { get; set; }
        RightToLeft RightToLeft { get; }
    }
}
