using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors
{
    internal interface IConstructorManager
    {
        Constructor CreateConstructor(string name, ConstructorInfo cInfo);
    }
}
