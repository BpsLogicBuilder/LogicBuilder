using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters
{
    internal interface IParametersMatcher
    {
        bool MatchParameters(IList<ParameterNodeInfoBase> parameterInfos, IList<ParameterBase> configuredParameters);
    }
}
