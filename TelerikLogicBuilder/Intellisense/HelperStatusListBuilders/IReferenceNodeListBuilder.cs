using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal interface IReferenceNodeListBuilder
    {
        List<BaseTreeNode> Build(string className, IList<ReferenceInfo> referenceInfoList, BindingFlagCategory rootReferenceBindingFlagCategory);
    }
}
