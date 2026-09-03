using LogicBuilder.App.Bsl.Flow.Interfaces;
using LogicBuilder.App.Bsl.Business.Requests;
using LogicBuilder.App.Bsl.Business.Responses;
using System.Collections.Generic;

namespace LogicBuilder.App.Bsl.Flow
{
    public class FlowDataCache : IFlowDataCache
    {
        public IBaseRequest? Request { get; set; }
        public BaseResponse? Response { get; set; }
        public Dictionary<string, object> Items { get; set; } = [];
    }
}
