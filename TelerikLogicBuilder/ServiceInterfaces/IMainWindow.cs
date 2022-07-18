using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IMainWindow
    {
        Form Instance { get; set; }
        RightToLeft RightToLeft { get; }
    }
}
