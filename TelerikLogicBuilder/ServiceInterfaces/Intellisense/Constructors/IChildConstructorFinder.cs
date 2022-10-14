using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors
{
    internal interface IChildConstructorFinder
    {
        void AddChildConstructors(ParameterInfo[] parameters);
    }
}
