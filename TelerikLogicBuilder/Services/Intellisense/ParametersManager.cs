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

        public ParametersManager(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ICollection<ParameterNodeInfoBase> GetParameterNodeInfos(IEnumerable<ParameterInfo> parameters) 
            => parameters.Select(p => ParameterNodeInfoBase.Create(p, this._contextProvider)).ToList();
    }
}
