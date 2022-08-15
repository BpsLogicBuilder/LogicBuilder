using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IMainWindow
    {
        Form Instance { get; set; }
        IConfigurationExplorer ConfigurationExplorer { get; }
        IDocumentsExplorer DocumentsExplorer { get; }
        IMDIParent MDIParent { get; }
        IMessages Messages { get; }
        IProjectExplorer ProjectExplorer { get; }
        RightToLeft RightToLeft { get; }
        IRulesExplorer RulesExplorer { get; }
    }
}
