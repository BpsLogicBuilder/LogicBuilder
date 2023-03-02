using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class LiteralElementTreeNode : ParametersDataTreeNode
    {
        public LiteralElementTreeNode(string text, string name, Type assignedToType, int nodeIndex)
            : base(text, name, assignedToType)
        {
            ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX;
            NodeIndex = nodeIndex;
        }

        public int NodeIndex { get; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.Literal;
    }
}
