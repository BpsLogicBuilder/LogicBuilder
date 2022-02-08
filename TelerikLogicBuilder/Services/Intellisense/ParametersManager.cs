using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense
{
    internal class ParametersManager : IParametersManager
    {
        private readonly IContextProvider _contextProvider;
        private readonly IParameterAttributeReader _parameterAttributeReader;

        public ParametersManager(IContextProvider contextProvider, IParameterAttributeReader parameterAttributeReader)
        {
            _contextProvider = contextProvider;
            _parameterAttributeReader = parameterAttributeReader;
        }

        public ICollection<ParameterNodeInfoBase> GetParameterNodeInfos(IEnumerable<ParameterInfo> parameters) 
            => parameters.Select(p => ParameterNodeInfoBase.Create(p, this._contextProvider, _parameterAttributeReader)).ToList();
    }
}
