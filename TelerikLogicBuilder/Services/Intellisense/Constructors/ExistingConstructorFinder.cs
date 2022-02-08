using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ExistingConstructorFinder : IExistingConstructorFinder
    {
        private readonly IParametersManager _parametersManager;
        private readonly IParametersMatcher _parametersMatcher;
        private readonly ITypeHelper _typeHelper;

        public ExistingConstructorFinder(IParametersManager parametersManager, IParametersMatcher parametersMatcher, ITypeHelper typeHelper)
        {
            _parametersManager = parametersManager;
            _parametersMatcher = parametersMatcher;
            _typeHelper = typeHelper;
        }

        public Constructor FindExisting(ConstructorInfo cInfo, IDictionary<string, Constructor> existingConstructors) 
            => existingConstructors
                .Where(e => e.Value.TypeName == _typeHelper.ToId(cInfo.DeclaringType))
                .ToDictionary(e => e.Key, e => e.Value)
                .Values
                .FirstOrDefault
                (
                    constructor => _parametersMatcher.MatchParameters
                    (
                        this._parametersManager.GetParameterNodeInfos(cInfo.GetParameters()).ToList(),
                        constructor.Parameters
                    )
                );
    }
}
