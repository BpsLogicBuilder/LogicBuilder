using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal interface IConstructorNodeBuilder
    {
        ConstructorTreeNode? Build(Type declaringType, IList<ParameterBase> configuredParameters);
    }
}
