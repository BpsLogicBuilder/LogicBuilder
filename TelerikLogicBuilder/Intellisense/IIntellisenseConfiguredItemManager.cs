using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal interface IIntellisenseConfiguredItemManager
    {
        string CastReferenceAs { get; }
        HelperStatus HelperStatus { get; }
        ReferenceCategories ReferenceCategory { get; }
        string ReferenceDefinition { get; }
        string ReferenceName { get; }
        string TypeName { get; }

        void AddChildren(VariableTreeNode treeNode);
        void ApplicationChanged();
        void BeforeCollapse(BaseTreeNode treeNode);
        void BeforeExpand(BaseTreeNode treeNode);
        void BuildTreeView(string classFullName);
        void ClearTreeView();
        void CmbClassTextChanged();
        void CmbReferenceCategorySelectedIndexChanged();

        void Initialize();

        void UpdateSelectedVariableConfiguration(CustomVariableConfiguration customVariableConfiguration);
        void UpdateSelection(HelperStatus? helperStatus);
    }
}
