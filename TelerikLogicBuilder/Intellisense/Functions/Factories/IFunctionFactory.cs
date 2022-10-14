using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal interface IFunctionFactory
    {
        Function GetFunction(string name,
            string memberName,
            FunctionCategories functionCategory,
            string typeName,
            string referenceName,
            string referenceDefinition,
            string castReferenceAs,
            ReferenceCategories referenceCategory,
            ParametersLayout parametersLayout,
            List<ParameterBase> parameters,
            List<string> genericArguments,
            ReturnTypeBase returnType,
            string summary);
    }
}
