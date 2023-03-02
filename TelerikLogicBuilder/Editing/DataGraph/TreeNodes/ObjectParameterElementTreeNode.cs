using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ObjectParameterElementTreeNode : ParametersDataTreeNode, IParameterElementTreeNode
    {
        public ObjectParameterElementTreeNode(string parameterName, string name, Type assignedToType)
            : base(parameterName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX;
            ParameterName = parameterName;
        }

        public string ParameterName { get; }

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.ObjectParameter;
    }
}
