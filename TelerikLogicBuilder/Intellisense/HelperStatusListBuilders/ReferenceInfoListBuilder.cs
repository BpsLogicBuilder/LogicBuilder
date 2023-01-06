using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class ReferenceInfoListBuilder : IReferenceInfoListBuilder
    {
        public IList<ReferenceInfo> BuildList(IList<string> referenceDefinitionArray, IList<string> referenceNameArray, IList<string> castReferenceAsArray)
        {
            List<ReferenceInfo> list = new();
            for (int i = 0; i < referenceDefinitionArray.Count; i++)
            {
                list.Add
                (
                    new ReferenceInfo
                    (
                        referenceNameArray[i],
                        (ValidIndirectReference)Enum.Parse(typeof(ValidIndirectReference), referenceDefinitionArray[i]),
                        castReferenceAsArray[i]
                    )
                );
            }
            return list;
        }
    }
}
