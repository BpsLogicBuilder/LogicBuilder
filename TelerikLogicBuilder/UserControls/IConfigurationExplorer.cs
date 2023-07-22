using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IConfigurationExplorer
    {
        DockStyle Dock { set; }
        RadTreeView TreeView { get; }

        void ClearProfile();
        void CreateProfile();
        void RefreshTreeView();
    }
}
