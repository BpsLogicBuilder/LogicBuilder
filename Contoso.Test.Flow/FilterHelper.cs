using Contoso.Test.Flow.Cache;
using LogicBuilder.Forms.Parameters.Expressions;

namespace Contoso.Test.Flow
{
    public class FilterHelper : IFilterHelper
    {
        private readonly FlowDataCache flowDataCache;

        public FilterHelper(FlowDataCache flowDataCache)
        {
            this.flowDataCache = flowDataCache;
        }

        public void SetFilter(FilterLambdaOperatorParameters parameters, string filterId)
        {
            flowDataCache.Items[filterId] = parameters;
        }
    }
}
