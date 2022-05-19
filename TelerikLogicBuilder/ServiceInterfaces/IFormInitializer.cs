using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IFormInitializer
    {
        Icon GetLogicBuilderIcon();
        void SetCenterScreen(Form form);
        void SetFormDefaults(Form form, int minHeight);
        void SetProgressFormDefaults(Form form, int minHeight);
    }
}
