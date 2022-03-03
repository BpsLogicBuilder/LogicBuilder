using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class FunctionManager : IFunctionManager
    {
        private readonly IFunctionNodeInfoManager _functionNodeInfoManager;

        public FunctionManager(IFunctionNodeInfoManager functionNodeInfoManager)
        {
            _functionNodeInfoManager = functionNodeInfoManager;
        }

        public Function? GetFunction(string name, string memberName, FunctionCategories functionCategory, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, ParametersLayout parametersLayout, MethodInfo methodInfo) 
            => _functionNodeInfoManager.GetFunctionNodeInfo(methodInfo).GetFunction
            (
                name,
                memberName,
                functionCategory,
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory,
                parametersLayout
            );
    }
}
