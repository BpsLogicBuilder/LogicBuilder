using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ChildConstructorFinder : IChildConstructorFinder
    {
        private readonly IContextProvider _contextProvider;

        public ChildConstructorFinder(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public void AddChildConstructors(Dictionary<string, Constructor> existingConstructors, ParameterInfo[] parameters)
        {
            new ChildConstructorFinderUtil
            (
                existingConstructors,
                _contextProvider
            ).AddChildConstructors(parameters);
        }
    }
}
