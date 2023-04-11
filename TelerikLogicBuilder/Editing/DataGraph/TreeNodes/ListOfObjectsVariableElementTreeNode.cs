using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ListOfObjectsVariableElementTreeNode : ParametersDataTreeNode, IVariableElementTreeNode
    {
        public ListOfObjectsVariableElementTreeNode(string variableName, string name, Type assignedToType)
            : base(variableName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX;
        }

        public string VariableName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.ListOfObjectsVariable;
    }
}
