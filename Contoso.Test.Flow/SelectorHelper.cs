using Contoso.Test.Flow.Cache;
using LogicBuilder.Forms.Parameters.Expressions;

namespace Contoso.Test.Flow
{
    public class SelectorHelper : ISelectorHelper
    {
        private readonly FlowDataCache flowDataCache;

        public SelectorHelper(FlowDataCache flowDataCache)
        {
            this.flowDataCache = flowDataCache;
        }

        public void SetSelector(SelectorLambdaOperatorParameters parameters, string selectorId)
        {
            flowDataCache.Items[selectorId] = parameters;
        }
    }
}
