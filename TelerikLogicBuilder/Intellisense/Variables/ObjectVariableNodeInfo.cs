using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ObjectVariableNodeInfo : VariableNodeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;

        internal ObjectVariableNodeInfo(MemberInfo mInfo, Type memberType, IContextProvider contextProvider, IMemberAttributeReader memberAttributeReader)
            : base(mInfo, memberType, memberAttributeReader)
        {
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
        }

        internal override VariableBase GetVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory)
            => new ObjectVariable
            (
                name,
                memberName,
                variableCategory,
                castVariableAs,
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory,
                this.Comments,
                _typeHelper.ToId(this.MemberType),
                _contextProvider
            );
    }
}
