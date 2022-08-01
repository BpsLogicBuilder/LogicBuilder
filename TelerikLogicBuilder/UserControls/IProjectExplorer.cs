using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IProjectExplorer
    {
        DockStyle Dock { set; }
        bool Visible { set; }
        void ClearProfile();
        void CreateProfile();
    }
}
