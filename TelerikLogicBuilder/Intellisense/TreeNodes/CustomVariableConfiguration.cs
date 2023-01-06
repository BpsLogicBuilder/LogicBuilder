using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class CustomVariableConfiguration
    {
        public CustomVariableConfiguration(
            VariableCategory? variableCategory,
            string castAs = MiscellaneousConstants.TILDE,
            string memberName = MiscellaneousConstants.TILDE)
        {
            CastAs = castAs;
            MemberName = memberName;
            VariableCategory = variableCategory;
        }

        public string CastAs { get; }
        public string MemberName { get; }
        public VariableCategory? VariableCategory { get; }

        public override bool Equals(object? obj)
        {
            if (obj is not CustomVariableConfiguration customVariableConfiguration)
                return false;

            return customVariableConfiguration.CastAs == CastAs
                && customVariableConfiguration.MemberName == MemberName
                && customVariableConfiguration.VariableCategory == VariableCategory;
        }

        public override int GetHashCode()
        {
            return MemberName.GetHashCode();
        }
    }
}
