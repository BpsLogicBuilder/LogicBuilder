using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class LiteralListElementTreeNode : ParametersDataTreeNode
    {
        public LiteralListElementTreeNode(string text, string name, Type assignedToType, LiteralListParameterElementInfo listInfo)
            : base(text, name, assignedToType)
        {
            ImageIndex = ImageIndexes.LITERALLISTCONSTRUCTORIMAGEINDEX;
            ListInfo = listInfo;
        }

        public LiteralListParameterElementInfo ListInfo { get; set; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.LiteralList;
    }
}
