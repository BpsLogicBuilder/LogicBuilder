using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal class FunctionFactory : IFunctionFactory
    {
        private readonly Func<string, string, FunctionCategories, string, string, string, string, ReferenceCategories, ParametersLayout, List<ParameterBase>, List<string>, ReturnTypeBase, string, Function> _getFunction;

        public FunctionFactory(Func<string, string, FunctionCategories, string, string, string, string, ReferenceCategories, ParametersLayout, List<ParameterBase>, List<string>, ReturnTypeBase, string, Function> getFunction)
        {
            _getFunction = getFunction;
        }

        public Function GetFunction(
            string name,
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
            string summary)
             => _getFunction(name,
                        memberName,
                        functionCategory,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        parametersLayout,
                        parameters,
                        genericArguments,
                        returnType,
                        summary);
    }
}
