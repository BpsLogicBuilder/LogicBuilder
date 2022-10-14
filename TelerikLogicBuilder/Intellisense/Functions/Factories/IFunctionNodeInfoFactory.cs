using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal interface IFunctionNodeInfoFactory
    {
        FunctionNodeInfo GetFunctionNodeInfo(MethodInfo mInfo);
    }
}
