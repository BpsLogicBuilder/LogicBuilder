using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense
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

        public Constructor CreateConstructor(string name, ConstructorInfo cInfo) 
            => new 
            (
                name,
                _typeHelper.ToId(cInfo.DeclaringType),
                this._parametersManager
                    .GetParameterNodeInfos(cInfo.GetParameters())
                    .Select(p => p.Parameter)
                    .ToList(),
                new List<string>
                (
                    cInfo.DeclaringType.GetGenericArguments()
                        .Where(a => a.IsGenericParameter)
                        .Select(a => a.Name)
                ),
                this._memberAttributeReader.GetSummary(cInfo),
                _contextProvider
            );
    }
}
