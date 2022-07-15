using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IMessageBoxOptionsHelper
    {
        IWin32Window Instance { get; set; }
        RightToLeft RightToLeft { get; set; }
    }
}
