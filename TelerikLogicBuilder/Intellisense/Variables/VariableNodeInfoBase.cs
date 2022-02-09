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
        internal string Name => this.MInfo.Name;
        internal string Comments => _memberAttributeReader.GetVariableComments(MInfo);
        abstract internal VariableBase GetVariable(string name,
                                                string memberName,
                                                VariableCategory variableCategory,
                                                string castVariableAs, 
                                                string typeName, 
                                                string referenceName, 
                                                string referenceDefinition, 
                                                string castReferenceAs, 
                                                ReferenceCategories referenceCategory);
        #endregion Properties

        #region Methods
        internal static VariableNodeInfoBase Create(MemberInfo mInfo, Type memberType, IContextProvider contextProvider, IMemberAttributeReader memberAttributeReader)
        {
            if (contextProvider.TypeHelper.IsLiteralType(memberType))
                return new LiteralVariableNodeInfo(mInfo, memberType, contextProvider, memberAttributeReader);
            else if (contextProvider.TypeHelper.IsValidList(memberType))
            {
                Type underlyingType = contextProvider.TypeHelper.GetUndelyingTypeForValidList(memberType);
                if (contextProvider.TypeHelper.IsLiteralType(underlyingType))
                    return new ListOfLiteralsVariableNodeInfo(mInfo, memberType, contextProvider, memberAttributeReader);
                else
                    return new ListOfObjectsVariableNodeInfo(mInfo, memberType, contextProvider, memberAttributeReader);
            }
            else if (memberType.IsAbstract || memberType.IsInterface || memberType.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return new ObjectVariableNodeInfo(mInfo, memberType, contextProvider, memberAttributeReader);
            }
            else
            {
                return new ObjectVariableNodeInfo(mInfo, memberType, contextProvider, memberAttributeReader);
            }
        }
        #endregion Methods
    }
}
