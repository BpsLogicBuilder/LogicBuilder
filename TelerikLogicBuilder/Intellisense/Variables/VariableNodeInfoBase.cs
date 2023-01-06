using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    abstract internal class VariableNodeInfoBase
    {
        protected readonly IMemberAttributeReader _memberAttributeReader;
        internal VariableNodeInfoBase(MemberInfo mInfo, Type memberType, IMemberAttributeReader memberAttributeReader)
        {
            this.MInfo = mInfo;
            this.MemberType = memberType;
            _memberAttributeReader = memberAttributeReader;
        }

        #region Properties
        internal MemberInfo MInfo { get; }//Represents MemberInfo for the referenced field or property
        internal Type MemberType { get; }//Needed when member has been cast (CastVariableAs is not empty)
        internal string AlsoKnownAs => _memberAttributeReader.GetAlsoKnownAs(MInfo);
        internal string Name => string.IsNullOrEmpty(AlsoKnownAs)
                                        ? $"{MInfo.DeclaringType?.Name}{MiscellaneousConstants.UNDERSCORE}{MInfo.Name}"
                                        : AlsoKnownAs;
        internal string Comments => _memberAttributeReader.GetVariableComments(MInfo);
        abstract internal VariableBase GetVariable(string memberName,
                                                VariableCategory variableCategory,
                                                string castVariableAs, 
                                                string typeName, 
                                                string referenceName, 
                                                string referenceDefinition, 
                                                string castReferenceAs, 
                                                ReferenceCategories referenceCategory);
        #endregion Properties
    }
}
