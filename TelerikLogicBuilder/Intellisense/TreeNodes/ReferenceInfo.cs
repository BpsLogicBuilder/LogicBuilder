using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class ReferenceInfo
    {
        public ReferenceInfo(string memberName, ValidIndirectReference validIndirectReference, string castAs)
        {
            MemberName = memberName;
            ValidIndirectReference = validIndirectReference;
            CastAs = castAs;
        }

        public string MemberName { get; }
        public ValidIndirectReference ValidIndirectReference { get; }
        public string CastAs { get; }
    }
}
