using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal interface IConfiguredItemHelperForm : IApplicationForm
    {
        RadButton BtnOk { get; }
        ApplicationDropDownList CmbApplication { get; }
        AutoCompleteRadDropDownList CmbClass { get; }
        RadDropDownList CmbReferenceCategory { get; }
        IDictionary<string, VariableBase> ExistingVariables { get; }
        ReferenceCategories ReferenceCategory { get; }
        VariableTreeNode ReferenceTreeNode { get; }
        RadTreeView TreeView { get; }

        void ClearTreeView();
        void UpdateSelectedVariableConfiguration(CustomVariableConfiguration customVariableConfiguration);
        void ValidateOk();
    }
}
