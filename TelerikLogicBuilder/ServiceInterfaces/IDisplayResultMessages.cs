using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IDisplayResultMessages
    {
        void AppendMessage(ResultMessage message, MessageTab messageTab);
        void Clear(MessageTab messageTab);
        void DisplayMessages(IList<ResultMessage> results, MessageTab messageTab);
        void DisplayMessages(IList<string> results, MessageTab messageTab);
    }
}
