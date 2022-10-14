using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal class FunctionNodeInfoFactory : IFunctionNodeInfoFactory
    {
        private readonly Func<MethodInfo, FunctionNodeInfo> _getFunctionNodeInfo;

        public FunctionNodeInfoFactory(Func<MethodInfo, FunctionNodeInfo> getFunctionNodeInfo)
        {
            _getFunctionNodeInfo = getFunctionNodeInfo;
        }

        public FunctionNodeInfo GetFunctionNodeInfo(MethodInfo mInfo)
            => _getFunctionNodeInfo(mInfo);
    }
}
