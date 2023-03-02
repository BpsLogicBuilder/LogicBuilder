using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class LiteralParameterElementTreeNode : ParametersDataTreeNode, IParameterElementTreeNode
    {
        public LiteralParameterElementTreeNode(string parameterName, string name, Type assignedToType)
            : base(parameterName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX;
            ParameterName = parameterName;
        }

        public string ParameterName { get; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.LiteralParameter;
    }
}
