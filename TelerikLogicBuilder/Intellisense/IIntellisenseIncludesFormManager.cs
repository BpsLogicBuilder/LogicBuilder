using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal interface IIntellisenseIncludesFormManager
    {
        string ReferenceDefinition { get; }
        string ReferenceName { get; }

        void AddChildren(VariableTreeNode treeNode);
        void ApplicationChanged();
        void BeforeCollapse(BaseTreeNode treeNode);
        void BeforeExpand(BaseTreeNode treeNode);
        void BuildTreeView(string classFullName);
        void ClearTreeView();
        void CmbClassTextChanged();
        void Initialize();
    }
}
