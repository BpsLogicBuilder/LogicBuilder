using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class RetractFunctionElementTreeNode : ParametersDataTreeNode
    {
        public RetractFunctionElementTreeNode(string functionName, string name)
            : base(functionName, name, typeof(object))
        {
            ImageIndex = ImageIndexes.METHODIMAGEINDEX;
        }

        public string FunctionName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.RetractFunction;
    }
}
