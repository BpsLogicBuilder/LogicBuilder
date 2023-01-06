using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class LiteralVariableNodeInfo : VariableNodeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IVariableFactory _variableFactory;

        internal LiteralVariableNodeInfo(
            IEnumHelper enumHelper,
            IMemberAttributeReader memberAttributeReader,
            IVariableFactory variableFactory,
            MemberInfo mInfo,
            Type memberType)
            : base(mInfo, memberType, memberAttributeReader)
        {
            _enumHelper = enumHelper;
            _variableFactory = variableFactory;
        }

        /// <summary>
        /// Literal type
        /// </summary>
        internal LiteralVariableType LiteralType => _enumHelper.GetLiteralVariableType(this.MemberType);

        /// <summary>
        /// Domain
        /// </summary>
        internal List<string> Domain => _memberAttributeReader.GetDomain(MInfo);

        /// <summary>
        /// Control used in the flow diagram editor for a literal field or property
        /// </summary>
        internal LiteralVariableInputStyle LiteralControl => _memberAttributeReader.GetLiteralInputStyle(MInfo);

        /// <summary>
        /// The default value for this variable when used in the flow diagram
        /// </summary>
        internal string DefaultValue
        {
            get
            {
                _memberAttributeReader.GetNameValueTable(MInfo).TryGetValue(AttributeNames.DEFAULTVALUE, out string? fromAttribute);
                return fromAttribute ?? string.Empty;
            }
        }

        /// <summary>
        /// The fully qualified class name whose properties will be used as a domain for property selection
        /// </summary>
        internal string PropertySource
        {
            get
            {
                _memberAttributeReader.GetNameValueTable(MInfo).TryGetValue(AttributeNames.PROPERTYSOURCE, out string? fromAttribute);
                return fromAttribute ?? string.Empty;
            }
        }

        internal override VariableBase GetVariable(string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory) 
            => _variableFactory.GetLiteralVariable
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
                this.LiteralType,
                this.LiteralControl,
                this.PropertySource,
                this.DefaultValue,
                this.Domain
            );
    }
}
