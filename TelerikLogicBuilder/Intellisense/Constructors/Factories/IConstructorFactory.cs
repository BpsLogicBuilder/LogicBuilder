using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal interface IConstructorFactory
    {
        Constructor GetConstructor(string name, string typeName, List<ParameterBase> parameters, List<string> genericArguments, string summary);
    }
}
