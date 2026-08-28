using LogicBuilder.Forms.Parameters.Expressions;

namespace Contoso.Test.Flow
{
    public interface IFilterHelper
    {
        void SetFilter(FilterLambdaOperatorParameters parameters, string filterId);
    }
}
