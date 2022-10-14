using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ListOfObjectsVariableNodeInfo : VariableNodeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IVariableFactory _variableFactory;

        internal ListOfObjectsVariableNodeInfo(
            IEnumHelper enumHelper,
            ITypeHelper typeHelper,
            IMemberAttributeReader memberAttributeReader,
            IVariableFactory variableFactory,
            MemberInfo mInfo,
            Type memberType)
            : base(mInfo, memberType, memberAttributeReader)
        {
            _typeHelper = typeHelper;
            _enumHelper = enumHelper;
            _variableFactory = variableFactory;
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
            => _variableFactory.GetListOfObjectsVariable
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
                this.ListControl
            );
    }
}
