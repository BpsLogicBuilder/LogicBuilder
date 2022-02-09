using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ListOfObjectsVariableNodeInfo : VariableNodeInfoBase
    {
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IContextProvider _contextProvider;

        internal ListOfObjectsVariableNodeInfo(MemberInfo mInfo, Type memberType, IContextProvider contextProvider, IMemberAttributeReader memberAttributeReader)
            : base(mInfo, memberType, memberAttributeReader)
        {
            _typeHelper = contextProvider.TypeHelper;
            _enumHelper = contextProvider.EnumHelper;
            _contextProvider = contextProvider;
        }

        #region Fields
        //private ConstructorManager constructorManager;
        #endregion Fields

        /// <summary>
        /// Control used in the flow diagram editor for a list parameter
        /// </summary>
        internal ListVariableInputStyle ListControl => _memberAttributeReader.GetListInputStyle(MInfo);

        /// <summary>
        /// List Type
        /// </summary>
        internal ListType ListType => _enumHelper.GetListType(this.MemberType);

        internal override VariableBase GetVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory)
            => new ListOfObjectsVariable
            (   name,
                memberName,
                variableCategory,
                castVariableAs,
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory,
                this.Comments,
                _typeHelper.ToId(_typeHelper.GetUndelyingTypeForValidList(this.MemberType)),
                this.ListType,
                this.ListControl,
                _contextProvider
            );
    }
}
