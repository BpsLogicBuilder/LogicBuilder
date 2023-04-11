using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class AssertFunctionElementTreeNode : ParametersDataTreeNode
    {
        public AssertFunctionElementTreeNode(string functionName, string name)
            : base(functionName, name, typeof(object))
        {
            ImageIndex = ImageIndexes.METHODIMAGEINDEX;
        }

        public string FunctionName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.AssertFunction;
    }
}
