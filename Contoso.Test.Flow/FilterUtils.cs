using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters.Expressions;

namespace Contoso.Test.Flow
{
    public static class FilterUtils
    {
        [AlsoKnownAs("Set Filter")]
        public static void SetFilter(IFilterHelper filterHelper, FilterLambdaOperatorParameters parameters, string filterId)
        {
            filterHelper.SetFilter(parameters, filterId);
        }
    }
}
