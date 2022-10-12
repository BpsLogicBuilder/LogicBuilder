using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ConstructorManager : IConstructorManager
    {
        private readonly IConstructorFactory _constructorFactory;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly IParametersManager _parametersManager;
        private readonly ITypeHelper _typeHelper;

        public ConstructorManager(
            IConstructorFactory constructorFactory,
            IMemberAttributeReader memberAttributeReader,
            IParametersManager parametersManager,
            ITypeHelper typeHelper)
        {
            _constructorFactory = constructorFactory;
            _memberAttributeReader = memberAttributeReader;
            _parametersManager = parametersManager;
            _typeHelper = typeHelper;
        }

        public Constructor? CreateConstructor(string name, ConstructorInfo cInfo)
        {
            if (cInfo.DeclaringType == null)
                return null;

            return _constructorFactory.GetConstructor
            (
                name,
                _typeHelper.ToId(cInfo.DeclaringType),
                _parametersManager
                    .GetParameterNodeInfos(cInfo.GetParameters())
                    .Select(p => p.Parameter)
                    .ToList(),
                cInfo.DeclaringType.GetGenericArguments()
                    .Where(a => a.IsGenericParameter)
                    .Select(a => a.Name)
                    .ToList(),
                _memberAttributeReader.GetSummary(cInfo)
            );
        }
    }
}
