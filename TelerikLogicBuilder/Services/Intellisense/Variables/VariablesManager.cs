using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesManager : IVariablesManager
    {
        private readonly IContextProvider _contextProvider;
        private readonly IMemberAttributeReader _memberAttributeReader;

        public VariablesManager(IContextProvider contextProvider, IMemberAttributeReader memberAttributeReader)
        {
            _contextProvider = contextProvider;
            _memberAttributeReader = memberAttributeReader;
        }

        public VariableNodeInfoBase GetVariableNodeInfo(MemberInfo mInfo, Type memberType)
        {
            return VariableNodeInfoBase.Create(mInfo, memberType, _contextProvider, _memberAttributeReader);
        }
    }
}
