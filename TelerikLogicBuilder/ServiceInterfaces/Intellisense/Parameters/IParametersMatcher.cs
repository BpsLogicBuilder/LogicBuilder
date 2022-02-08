using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters
{
    internal interface IParametersMatcher
    {
        bool MatchParameters(List<ParameterNodeInfoBase> parameterInfos, List<ParameterBase> configuredParameters);
    }
}
