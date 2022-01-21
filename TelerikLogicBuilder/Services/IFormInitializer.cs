using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IFormInitializer
    {
        Icon GetLogicBuilderIcon();
        void SetCenterScreen(Form form);
    }
}
