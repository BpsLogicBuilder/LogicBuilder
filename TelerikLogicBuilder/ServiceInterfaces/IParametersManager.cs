using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IParametersManager
    {
        ICollection<ParameterNodeInfoBase> GetParameterNodeInfos(IEnumerable<ParameterInfo> parameters);
    }
}
