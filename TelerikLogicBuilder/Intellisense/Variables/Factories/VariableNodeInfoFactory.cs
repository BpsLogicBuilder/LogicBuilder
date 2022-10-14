using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories
{
    internal class VariableNodeInfoFactory : IVariableNodeInfoFactory
    {
        private readonly Func<MemberInfo, Type, ListOfLiteralsVariableNodeInfo> _getListOfLiteralsVariableNodeInfo;
        private readonly Func<MemberInfo, Type, ListOfObjectsVariableNodeInfo> _getListOfObjectsVariableNodeInfo;
        private readonly Func<MemberInfo, Type, LiteralVariableNodeInfo> _getLiteralVariableNodeInfo;
        private readonly Func<MemberInfo, Type, ObjectVariableNodeInfo> _getObjectVariableNodeInfo;

        public VariableNodeInfoFactory(
            Func<MemberInfo, Type, ListOfLiteralsVariableNodeInfo> getListOfLiteralsVariableNodeInfo,
            Func<MemberInfo, Type, ListOfObjectsVariableNodeInfo> getListOfObjectsVariableNodeInfo,
            Func<MemberInfo, Type, LiteralVariableNodeInfo> getLiteralVariableNodeInfo,
            Func<MemberInfo, Type, ObjectVariableNodeInfo> getObjectVariableNodeInfo)
        {
            _getListOfLiteralsVariableNodeInfo = getListOfLiteralsVariableNodeInfo;
            _getListOfObjectsVariableNodeInfo = getListOfObjectsVariableNodeInfo;
            _getLiteralVariableNodeInfo = getLiteralVariableNodeInfo;
            _getObjectVariableNodeInfo = getObjectVariableNodeInfo;
        }

        public ListOfLiteralsVariableNodeInfo GetListOfLiteralsVariableNodeInfo(MemberInfo mInfo, Type memberType)
            => _getListOfLiteralsVariableNodeInfo(mInfo, memberType);

        public ListOfObjectsVariableNodeInfo GetListOfObjectsVariableNodeInfo(MemberInfo mInfo, Type memberType)
            => _getListOfObjectsVariableNodeInfo(mInfo, memberType);

        public LiteralVariableNodeInfo GetLiteralVariableNodeInfo(MemberInfo mInfo, Type memberType)
            => _getLiteralVariableNodeInfo(mInfo, memberType);

        public ObjectVariableNodeInfo GetObjectVariableNodeInfo(MemberInfo mInfo, Type memberType)
            => _getObjectVariableNodeInfo(mInfo, memberType);
    }
}
