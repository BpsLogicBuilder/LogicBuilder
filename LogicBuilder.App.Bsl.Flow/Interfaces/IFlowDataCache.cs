using LogicBuilder.App.Bsl.Business.Requests;
using LogicBuilder.App.Bsl.Business.Responses;
using System.Collections.Generic;

namespace LogicBuilder.App.Bsl.Flow.Interfaces
{
    public interface IFlowDataCache
    {
        IBaseRequest? Request { get; set; }
        BaseResponse? Response { get; set; }
        Dictionary<string, object> Items { get; set; }
    }
}
