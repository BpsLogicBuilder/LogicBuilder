using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IProjectExplorer
    {
        IConfigurationExplorer ConfigurationExplorer { get; }
        DockStyle Dock { set; }
        IDocumentsExplorer DocumentsExplorer { get; }
        IRulesExplorer RulesExplorer { get; }
        bool Visible { set; }
        void ClearProfile();
        void CreateProfile();
    }
}
