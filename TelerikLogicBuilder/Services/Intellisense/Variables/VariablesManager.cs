using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesManager : IVariablesManager
    {
        private readonly IVariablesNodeInfoManager _variablesNodeInfoManager;

        public VariablesManager(IVariablesNodeInfoManager variablesNodeInfoManager)
        {
            _variablesNodeInfoManager = variablesNodeInfoManager;
        }

        public VariableBase GetVariable(string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, MemberInfo memberInfo, Type memberType) 
            => _variablesNodeInfoManager.GetVariableNodeInfo(memberInfo, memberType).GetVariable
            (
                memberName,
                variableCategory,
                castVariableAs,
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory
            );
    }
}
