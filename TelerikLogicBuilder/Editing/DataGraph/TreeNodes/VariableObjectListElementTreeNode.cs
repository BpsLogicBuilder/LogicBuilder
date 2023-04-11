using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class VariableObjectListElementTreeNode : ParametersDataTreeNode
    {
        public VariableObjectListElementTreeNode(string text, string name, Type assignedToType, ObjectListVariableElementInfo listElementInfo)
            : base(text, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTLISTCONSTRUCTORIMAGEINDEX;
            ListInfo = listElementInfo;
        }

        internal ObjectListVariableElementInfo ListInfo { get; set; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.VariableObjectList;
    }
}
