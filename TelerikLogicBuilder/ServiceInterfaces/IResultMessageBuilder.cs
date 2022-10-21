using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IResultMessageBuilder
    {
        ResultMessage BuilderMessage(VisioFileSource source, string message);
        ResultMessage BuilderMessage(TableFileSource source, string message);
        ResultMessage BuilderMessage(string message);
    }
}
