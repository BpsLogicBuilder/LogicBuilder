using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ChildConstructorFinder : IChildConstructorFinder
    {
        private readonly IConstructorManager _constructorManager;
        private readonly IParametersManager _parametersManager;
        private readonly IReflectionHelper _reflectionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IMemberAttributeReader _memberAttributeReader;

        public ChildConstructorFinder(
            IConstructorManager constructorManager,
            IParametersManager parametersManager,
            IReflectionHelper reflectionHelper,
            ITypeHelper typeHelper,
            IStringHelper stringHelper,
            IMemberAttributeReader memberAttributeReader)
        {
            _constructorManager = constructorManager;
            _parametersManager = parametersManager;
            _reflectionHelper = reflectionHelper;
            _typeHelper = typeHelper;
            _stringHelper = stringHelper;
            _memberAttributeReader = memberAttributeReader;
        }

        public void AddChildConstructors(Dictionary<string, Constructor> existingConstructors, ParameterInfo[] parameters)
        {
            new ChildConstructorFinderUtility
            (
                existingConstructors,
                _constructorManager,
                _parametersManager,
                _reflectionHelper,
                _typeHelper,
                _stringHelper,
                _memberAttributeReader
            ).AddChildConstructors(parameters);
        }
    }
}
