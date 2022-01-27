using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IReflectionHelper
    {
        int ComplexParameterCount(ParameterInfo[] parameters);
    }
}
