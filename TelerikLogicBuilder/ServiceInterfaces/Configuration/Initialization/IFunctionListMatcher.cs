using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IFunctionListMatcher
    {
        bool IsBoolFunction(Function function);
        bool IsDialogFunction(Function function);
        bool IsTableFunction(Function function);
        bool IsValueFunction(Function function);
        bool IsVoidFunction(Function function);
    }
}
