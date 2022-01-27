using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ChildConstructorFinder : IChildConstructorFinder
    {
        private readonly IConstructorManager _constructorManager;
        private readonly IParametersManager _parametersManager;
        private readonly IReflectionHelper _reflectionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IStringHelper _stringHelper;

        public ChildConstructorFinder(IContextProvider contextProvider)
        {
            _constructorManager = contextProvider.ConstructorManager;
            _parametersManager = contextProvider.ParametersManager;
            _reflectionHelper = contextProvider.ReflectionHelper;
            _typeHelper = contextProvider.TypeHelper;
            _stringHelper = contextProvider.StringHelper;
        }

        public void AddChildConstructors(Dictionary<string, Constructor> existingConstructors, ParameterInfo[] parameters)
        {
            new ChildConstructorFinderUtil
            (
                existingConstructors, 
                _constructorManager,
                _parametersManager, 
                _reflectionHelper, 
                _typeHelper, 
                _stringHelper
            ).AddChildConstructors(parameters);
        }
    }
}
