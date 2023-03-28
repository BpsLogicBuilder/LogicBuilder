using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class RetractFunctionElementTreeNode : ParametersDataTreeNode
    {
        public RetractFunctionElementTreeNode(string functionName, string name, Type assignedToType)
            : base(functionName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.METHODIMAGEINDEX;
        }

        public string FunctionName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.RetractFunction;
    }
}
