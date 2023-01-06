using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal interface IChildConstructorFinderFactory
    {
        IChildConstructorFinder GetChildConstructorFinder(IDictionary<string, Constructor> existingConstructors);
    }
}
