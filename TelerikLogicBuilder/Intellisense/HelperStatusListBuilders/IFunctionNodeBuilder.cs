using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal interface IFunctionNodeBuilder
    {
        BaseTreeNode? Build(FunctionCategories functionCategory, VariableTreeNode? parentNode, Type parentType, string methodName, IList<ParameterBase> configuredParameters, BindingFlagCategory bindingFlagCategory);
    }
}
