using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesNodeInfoManager : IVariablesNodeInfoManager
    {
        private readonly IContextProvider _contextProvider;
        private readonly IMemberAttributeReader _memberAttributeReader;

        public VariablesNodeInfoManager(IContextProvider contextProvider, IMemberAttributeReader memberAttributeReader)
        {
            _contextProvider = contextProvider;
            _memberAttributeReader = memberAttributeReader;
        }

        public VariableNodeInfoBase GetVariableNodeInfo(MemberInfo mInfo, Type memberType) 
            => VariableNodeInfoBase.Create(mInfo, memberType, _contextProvider, _memberAttributeReader);
    }
}
