using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariablesNodeInfoManager
    {
        VariableNodeInfoBase GetVariableNodeInfo(MemberInfo memberInfo, Type memberType);
    }
}
