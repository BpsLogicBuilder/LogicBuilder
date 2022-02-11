using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions
{
    internal interface IFunctionNodeInfoManager
    {
        FunctionNodeInfo GetFunctionNodeInfo(MethodInfo methodInfo);
    }
}
