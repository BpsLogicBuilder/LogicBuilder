using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal interface IVariableTreeNode
    {
        string CastAs { get; }
        string CastReferenceDefinition { get; }
        string ReferenceDefinition { get; }
        string ReferenceName { get; }
        RadTreeNodeCollection Nodes { get; }
    }
}
