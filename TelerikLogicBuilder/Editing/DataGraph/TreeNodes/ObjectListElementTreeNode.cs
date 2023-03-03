using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ObjectListElementTreeNode : ParametersDataTreeNode
    {
        public ObjectListElementTreeNode(string text, string name, Type assignedToType, ObjectListParameterElementInfo listElementInfo)
            : base(text, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTLISTCONSTRUCTORIMAGEINDEX;
            ListInfo = listElementInfo;
        }

        internal ObjectListParameterElementInfo ListInfo { get; set; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.ObjectList;
    }
}
