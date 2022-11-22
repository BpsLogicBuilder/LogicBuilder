using ABIS.LogicBuilder.FlowBuilder.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface IApplicationTypeInfoHelper
    {
        ApplicationTypeInfo CreateApplicationTypeInfo(string applicationName);
    }
}
