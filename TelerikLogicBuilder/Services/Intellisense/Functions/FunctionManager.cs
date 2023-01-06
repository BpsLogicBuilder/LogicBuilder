using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class FunctionManager : IFunctionManager
    {
        private readonly IFunctionNodeInfoFactory _functionNodeInfoFactory;

        public FunctionManager(IFunctionNodeInfoFactory functionNodeInfoFactory)
        {
            _functionNodeInfoFactory = functionNodeInfoFactory;
        }

        public Function? GetFunction(string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, ParametersLayout parametersLayout, MethodInfo methodInfo)
            => _functionNodeInfoFactory.GetFunctionNodeInfo(methodInfo).GetFunction
            (
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory,
                parametersLayout
            );
    }
}
