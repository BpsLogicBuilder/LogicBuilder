using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IMessageBoxOptionsHelper
    {
        IWin32Window MainWindow { get; set; }
        RightToLeft MessageBoxOptions { get; set; }
    }
}
