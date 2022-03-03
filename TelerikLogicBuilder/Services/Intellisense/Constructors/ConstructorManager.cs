using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ConstructorManager : IConstructorManager
    {
        private readonly IParametersManager _parametersManager;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly ITypeHelper _typeHelper;
        private readonly IContextProvider _contextProvider;

        public ConstructorManager(IContextProvider contextProvider, IParametersManager parametersManager, IMemberAttributeReader memberAttributeReader)
        {
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
            _parametersManager = parametersManager;
            _memberAttributeReader = memberAttributeReader;
        }

        public Constructor? CreateConstructor(string name, ConstructorInfo cInfo)
        {
            if (cInfo.DeclaringType == null)
                return null;

            return new
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
                _memberAttributeReader.GetSummary(cInfo),
                _contextProvider
            );
        }
    }
}
