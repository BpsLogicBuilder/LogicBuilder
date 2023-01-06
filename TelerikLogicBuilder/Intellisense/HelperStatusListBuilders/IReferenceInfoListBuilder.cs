using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal interface IReferenceInfoListBuilder
    {
        IList<ReferenceInfo> BuildList(IList<string> referenceDefinitionArray, IList<string> referenceNameArray, IList<string> castReferenceAsArray);
    }
}
