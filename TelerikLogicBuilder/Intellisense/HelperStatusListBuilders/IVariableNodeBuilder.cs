using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal interface IVariableNodeBuilder
    {
        BaseTreeNode? Build(VariableCategory variableCategory, VariableTreeNode? parentNode, Type parentType, string memberName, string castVariableAs, BindingFlagCategory bindingFlagCategory);
    }
}
