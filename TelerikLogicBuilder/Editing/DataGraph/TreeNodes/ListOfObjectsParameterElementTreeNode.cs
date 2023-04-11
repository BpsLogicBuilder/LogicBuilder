using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ListOfObjectsParameterElementTreeNode : ParametersDataTreeNode, IParameterElementTreeNode
    {
        public ListOfObjectsParameterElementTreeNode(string parameterName, string name, Type assignedToType)
            : base(parameterName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX;
        }

        public string ParameterName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.ListOfObjectsParameter;
    }
}
