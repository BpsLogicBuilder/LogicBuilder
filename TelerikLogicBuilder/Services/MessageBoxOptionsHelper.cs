using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class MessageBoxOptionsHelper : IMessageBoxOptionsHelper
    {
        public RightToLeft MessageBoxOptions { get ; set ; }
    }
}
