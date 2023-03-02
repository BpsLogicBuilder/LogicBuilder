using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class VariableElementTreeNode : ParametersDataTreeNode
    {
        public VariableElementTreeNode(string variableName, string name, Type assignedToType)
            : base(variableName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.VARIABLEIMAGEINDEX;
            VariableName = variableName;
        }

        public string VariableName { get; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.Variable;
    }
}
