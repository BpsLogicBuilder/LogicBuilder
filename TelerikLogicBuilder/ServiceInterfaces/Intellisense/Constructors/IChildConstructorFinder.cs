using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors
{
    internal interface IChildConstructorFinder
    {
        void AddChildConstructors(Dictionary<string, Constructor> existingConstructors, ParameterInfo[] parameters);
    }
}
