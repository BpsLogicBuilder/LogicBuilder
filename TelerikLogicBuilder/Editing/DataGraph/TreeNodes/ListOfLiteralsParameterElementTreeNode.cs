using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ListOfLiteralsParameterElementTreeNode : ParametersDataTreeNode, IParameterElementTreeNode
    {
        public ListOfLiteralsParameterElementTreeNode(string parameterName, string name, Type assignedToType)
            : base(parameterName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX;
        }

        public string ParameterName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.ListOfLiteralsParameter;
    }
}
