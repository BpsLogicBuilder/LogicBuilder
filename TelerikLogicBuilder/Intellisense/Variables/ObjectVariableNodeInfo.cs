using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ObjectVariableNodeInfo : VariableNodeInfoBase
    {
        private readonly ITypeHelper _typeHelper;
        private readonly IVariableFactory _variableFactory;

        internal ObjectVariableNodeInfo(
            ITypeHelper typeHelper,
            IMemberAttributeReader memberAttributeReader,
            IVariableFactory variableFactory,
            MemberInfo mInfo,
            Type memberType)
            : base(mInfo, memberType, memberAttributeReader)
        {
            _typeHelper = typeHelper;
            _variableFactory = variableFactory;
        }

        internal override VariableBase GetVariable(string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory)
            => _variableFactory.GetObjectVariable
            (
                Name,
                memberName,
                variableCategory,
                castVariableAs,
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory,
                this.Comments,
                _typeHelper.ToId(this.MemberType)
            );
    }
}
