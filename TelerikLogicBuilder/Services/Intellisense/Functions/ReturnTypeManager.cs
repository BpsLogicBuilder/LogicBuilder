using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class ReturnTypeManager : IReturnTypeManager
    {
        private readonly IContextProvider _contextProvider;

        public ReturnTypeManager(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ReturnTypeInfoBase GetReturnTypeInfo(MethodInfo methodInfo) 
            => ReturnTypeInfoBase.Create(methodInfo, _contextProvider);
    }
}
