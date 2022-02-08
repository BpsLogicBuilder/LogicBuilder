using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface IReflectionHelper
    {
        int ComplexParameterCount(ParameterInfo[] parameters);
    }
}
