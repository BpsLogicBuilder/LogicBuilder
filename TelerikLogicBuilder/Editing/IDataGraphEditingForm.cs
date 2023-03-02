using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDataGraphEditingForm : IEditingForm
    {
        RadPanel RadPanelFields { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }
    }
}
