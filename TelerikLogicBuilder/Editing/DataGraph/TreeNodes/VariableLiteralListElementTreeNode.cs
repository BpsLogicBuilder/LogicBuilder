using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class VariableLiteralListElementTreeNode : ParametersDataTreeNode
    {
        public VariableLiteralListElementTreeNode(string text, string name, Type assignedToType, LiteralListVariableElementInfo listInfo)
            : base(text, name, assignedToType)
        {
            ImageIndex = ImageIndexes.LITERALLISTCONSTRUCTORIMAGEINDEX;
            ListInfo = listInfo;
        }

        public LiteralListVariableElementInfo ListInfo { get; set; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.VariableLiteralList;
    }
}
