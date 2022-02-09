using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariablesManager
    {
        VariableNodeInfoBase GetVariableNodeInfo(MemberInfo mInfo, Type memberType);
    }
}
