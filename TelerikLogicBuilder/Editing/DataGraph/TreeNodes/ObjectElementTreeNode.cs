using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ObjectElementTreeNode : ParametersDataTreeNode
    {
        public ObjectElementTreeNode(string text, string name, Type assignedToType, int nodeIndex)
            : base(text, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX;
            NodeIndex = nodeIndex;
        }

        public int NodeIndex { get; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.Object;
    }
}
