using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ConstructorManager : IConstructorManager
    {
        private readonly IParametersManager _parametersManager;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly ITypeHelper _typeHelper;

        public ConstructorManager(IContextProvider contextProvider)
        {
            _parametersManager = contextProvider.ParametersManager;
            _memberAttributeReader = contextProvider.MemberAttributeReader;
            _typeHelper = contextProvider.TypeHelper;
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
                this._memberAttributeReader.GetFunctionSummary(cInfo)
            );
    }
}
