using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class CustomVariableConfiguration
    {
        public CustomVariableConfiguration(
            VariableCategory variableCategory,
            string castAs = MiscellaneousConstants.TILDE,
            string memberName = MiscellaneousConstants.TILDE)
        {
            CastAs = castAs;
            MemberName = memberName;
            VariableCategory = variableCategory;
        }

        public string CastAs { get; }
        public string MemberName { get; }
        public VariableCategory VariableCategory { get; }
    }
}
