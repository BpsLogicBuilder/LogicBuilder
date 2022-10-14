using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ListOfLiteralsVariableNodeInfo : VariableNodeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IVariableFactory _variableFactory;

        internal ListOfLiteralsVariableNodeInfo(
            IEnumHelper enumHelper,
            ITypeHelper typeHelper,
            IMemberAttributeReader memberAttributeReader,
            IStringHelper stringHelper,
            IVariableFactory variableFactory,
            MemberInfo mInfo,
            Type memberType)
            : base(mInfo, memberType, memberAttributeReader)
        {
            _enumHelper = enumHelper;
            _typeHelper = typeHelper;
            _stringHelper = stringHelper;
            _variableFactory = variableFactory;
        }

        internal LiteralVariableType LiteralType => _enumHelper.GetLiteralVariableType(_typeHelper.GetUndelyingTypeForValidList(this.MemberType));

        /// <summary>
        /// Domain
        /// </summary>
        internal List<string> Domain => _memberAttributeReader.GetDomain(MInfo);

        /// <summary>
        /// Control used in the flow diagram editor for a list variable
        /// </summary>
        internal ListVariableInputStyle ListControl => _memberAttributeReader.GetListInputStyle(MInfo);

        /// <summary>
        /// Control used in the flow diagram editor for the underlying literal element
        /// </summary>
        internal LiteralVariableInputStyle ElementControl => _memberAttributeReader.GetLiteralInputStyle(MInfo);

        /// <summary>
        /// List Type
        /// </summary>
        internal ListType ListType => _enumHelper.GetListType(this.MemberType);

        /// <summary>
        /// Default Value Delimiters
        /// </summary>
        internal char[] DefaultValueDelimiters
        {
            get
            {
                _memberAttributeReader.GetNameValueTable(MInfo).TryGetValue(AttributeNames.DEFAULTVALUEDELIMITER, out string? delimiters);
                return delimiters?.Length > 0 ? delimiters.ToArray() : new char[] { ',', ';' };
            }
        }

        /// <summary>
        /// The default value for a list parameter when the constructor is created in the flow diagram
        /// </summary>
        //internal List<string> DefaultValuesForLiteralList => AttributeParser.GetArgument(AttributeReader.GetVariableMetaDataList(MInfo), AttributeNames.DEFAULTVALUE).SplitWithQuoteQualifier(this.DefaultValueDelimiters.Select(p => p.ToString()).ToArray()).ToList();
        internal List<string> DefaultValuesForLiteralList
        {
            get
            {
                _memberAttributeReader.GetNameValueTable(MInfo).TryGetValue(AttributeNames.DEFAULTVALUE, out string? defaultValues);
                if (string.IsNullOrEmpty(defaultValues))
                    return new List<string>();

                return _stringHelper.SplitWithQuoteQualifier(defaultValues, this.DefaultValueDelimiters.Select(p => p.ToString()).ToArray()).ToList();
            }
        }

        /// <summary>
        /// The fully qualified class name whose properties will be used as a domain for property selection
        /// </summary>
        internal string PropertySource
        {
            get
            {
                _memberAttributeReader.GetNameValueTable(MInfo).TryGetValue(AttributeNames.PROPERTYSOURCE, out string? value);
                return value ?? string.Empty;
            }
        }

        internal override VariableBase GetVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory)
            => _variableFactory.GetListOfLiteralsVariable
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
                this.LiteralType,
                this.ListType,
                this.ListControl,
                this.ElementControl,
                this.PropertySource,
                this.DefaultValuesForLiteralList,
                this.Domain
            );
    }
}
