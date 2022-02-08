using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ChildConstructorFinder : IChildConstructorFinder
    {
        private readonly IContextProvider _contextProvider;
        private readonly IConstructorManager _constructorManager;
        private readonly IParametersManager _parametersManager;
        private readonly IMemberAttributeReader _memberAttributeReader;

        public ChildConstructorFinder(IContextProvider contextProvider, IConstructorManager constructorManager, IParametersManager parametersManager, IMemberAttributeReader memberAttributeReader)
        {
            _contextProvider = contextProvider;
            _constructorManager = constructorManager;
            _parametersManager = parametersManager;
            _memberAttributeReader = memberAttributeReader;
        }

        public void AddChildConstructors(Dictionary<string, Constructor> existingConstructors, ParameterInfo[] parameters)
        {
            new ChildConstructorFinderUtility
            (
                existingConstructors,
                _contextProvider,
                _constructorManager,
                _parametersManager,
                _memberAttributeReader
            ).AddChildConstructors(parameters);
        }
    }
}
