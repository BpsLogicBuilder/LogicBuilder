using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ObjectVariableElementTreeNode : ParametersDataTreeNode, IVariableElementTreeNode
    {
        public ObjectVariableElementTreeNode(string variableName, string name, Type assignedToType)
            : base(variableName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX;
        }

        public string VariableName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.ObjectVariable;
    }
}
