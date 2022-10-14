using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories
{
    internal interface IVariableNodeInfoFactory
    {
        ListOfLiteralsVariableNodeInfo GetListOfLiteralsVariableNodeInfo(MemberInfo mInfo, Type memberType);
        ListOfObjectsVariableNodeInfo GetListOfObjectsVariableNodeInfo(MemberInfo mInfo, Type memberType);
        LiteralVariableNodeInfo GetLiteralVariableNodeInfo(MemberInfo mInfo, Type memberType);
        ObjectVariableNodeInfo GetObjectVariableNodeInfo(MemberInfo mInfo, Type memberType);
    }
}
