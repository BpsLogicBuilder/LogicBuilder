using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters.Expressions;

namespace Contoso.Test.Flow
{
    public static class SelectorUtils
    {
        [AlsoKnownAs("Set Selector")]
        public static void SetSelector(ISelectorHelper selectorHelper, SelectorLambdaOperatorParameters parameters, string selectorId)
        {
            selectorHelper.SetSelector(parameters, selectorId);
        }
    }
}
