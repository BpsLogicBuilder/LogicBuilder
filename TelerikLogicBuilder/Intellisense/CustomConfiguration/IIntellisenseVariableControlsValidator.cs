using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal interface IIntellisenseVariableControlsValidator
    {
        void ValidateCastAs(VariableTreeNode treeNode);
    }
}
