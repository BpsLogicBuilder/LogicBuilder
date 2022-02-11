using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class FunctionNodeInfoManager : IFunctionNodeInfoManager
    {
        private readonly IContextProvider _contextProvider;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly IParametersManager _parametersManager;
        private readonly IReturnTypeManager _returnTypeManager;

        public FunctionNodeInfoManager(IContextProvider contextProvider, IMemberAttributeReader memberAttributeReader, IParametersManager parametersManager, IReturnTypeManager returnTypeManager)
        {
            _contextProvider = contextProvider;
            _memberAttributeReader = memberAttributeReader;
            _parametersManager = parametersManager;
            _returnTypeManager = returnTypeManager;
        }

        public FunctionNodeInfo GetFunctionNodeInfo(MethodInfo methodInfo)
            => new(methodInfo, _contextProvider, _memberAttributeReader, _parametersManager, _returnTypeManager);
    }
}
