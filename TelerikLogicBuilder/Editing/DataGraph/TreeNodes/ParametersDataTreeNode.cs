using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal abstract class ParametersDataTreeNode : RadTreeNode
    {
        protected ParametersDataTreeNode(string text, string name, Type assignedToType)
        {
            Text = text;
            Name = name;
            AssignedToType = assignedToType;
        }

        public Type AssignedToType { get; }
        public abstract ParametersDataElementType XmlElementType { get; }
        public new ParametersDataTreeNode Parent => (ParametersDataTreeNode)base.Parent;
    }
}
