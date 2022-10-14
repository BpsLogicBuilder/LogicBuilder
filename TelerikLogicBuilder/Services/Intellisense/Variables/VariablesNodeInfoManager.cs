using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesNodeInfoManager : IVariablesNodeInfoManager
    {
        private readonly ITypeHelper _typeHelper;
        private readonly IVariableNodeInfoFactory _variableNodeInfoFactory;

        public VariablesNodeInfoManager(ITypeHelper typeHelper, IVariableNodeInfoFactory variableNodeInfoFactory)
        {
            _typeHelper = typeHelper;
            _variableNodeInfoFactory = variableNodeInfoFactory;
        }

        public VariableNodeInfoBase GetVariableNodeInfo(MemberInfo mInfo, Type memberType) 
            => Create(mInfo, memberType);

        private VariableNodeInfoBase Create(MemberInfo mInfo, Type memberType)
        {
            if (_typeHelper.IsLiteralType(memberType))
                return _variableNodeInfoFactory.GetLiteralVariableNodeInfo(mInfo, memberType);
            else if (_typeHelper.IsValidList(memberType))
            {
                Type underlyingType = _typeHelper.GetUndelyingTypeForValidList(memberType);
                if (_typeHelper.IsLiteralType(underlyingType))
                    return _variableNodeInfoFactory.GetListOfLiteralsVariableNodeInfo(mInfo, memberType);
                else
                    return _variableNodeInfoFactory.GetListOfObjectsVariableNodeInfo(mInfo, memberType);
            }
            else if (memberType.IsAbstract || memberType.IsInterface || memberType.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return _variableNodeInfoFactory.GetObjectVariableNodeInfo(mInfo, memberType);
            }
            else
            {
                return _variableNodeInfoFactory.GetObjectVariableNodeInfo(mInfo, memberType);
            }
        }
    }
}
