using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class AssertFunctionElementTreeNode : ParametersDataTreeNode
    {
        public AssertFunctionElementTreeNode(string functionName, string name, Type assignedToType)
            : base(functionName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.METHODIMAGEINDEX;
        }

        public string FunctionName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.AssertFunction;
    }
}
