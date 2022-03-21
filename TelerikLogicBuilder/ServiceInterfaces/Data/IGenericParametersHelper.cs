using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IGenericParametersHelper
    {
        List<ParameterBase> GetConvertedParameters(IList<ParameterBase> parameters, IList<GenericConfigBase> genericParametersDictionary, ApplicationTypeInfo application);
    }
}
