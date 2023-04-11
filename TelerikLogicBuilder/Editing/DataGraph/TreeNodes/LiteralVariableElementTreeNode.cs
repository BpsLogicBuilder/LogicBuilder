using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class LiteralVariableElementTreeNode : ParametersDataTreeNode, IVariableElementTreeNode
    {
        public LiteralVariableElementTreeNode(string variableName, string name, Type assignedToType)
            : base(variableName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX;
        }

        public string VariableName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.LiteralVariable;
    }
}
