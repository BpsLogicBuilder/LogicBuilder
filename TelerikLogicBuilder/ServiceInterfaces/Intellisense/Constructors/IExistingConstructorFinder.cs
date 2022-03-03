using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors
{
    internal interface IExistingConstructorFinder
    {
        Constructor? FindExisting(ConstructorInfo cInfo, IDictionary<string, Constructor> existingConstructors);
    }
}
