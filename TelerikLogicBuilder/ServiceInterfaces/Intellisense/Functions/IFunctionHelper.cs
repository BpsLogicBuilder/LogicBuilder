using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions
{
    internal interface IFunctionHelper
    {
        bool IsBoolean(Function function);
        bool IsDialog(Function function);
        bool IsVoid(Function function);
    }
}
