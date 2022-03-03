using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfLiteralsParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;

        internal ListOfLiteralsParameterNodeInfo(ParameterInfo pInfo, IContextProvider contextProvider, IParameterAttributeReader parameterAttributeReader)
            : base(pInfo, parameterAttributeReader)
        {
            _contextProvider = contextProvider;
            _typeHelper = contextProvider.TypeHelper;
            _enumHelper = contextProvider.EnumHelper;
            _stringHelper = contextProvider.StringHelper;
        }

        #region Properties
        /// <summary>
        /// Parameter type
        /// </summary>
        internal LiteralParameterType Type =>
                //return MembersInfo.GetParameterType(PInfo.ParameterType.GetUndelyingLiteralType());
                //For a list of literals we need just the immediate underlying type
                this._enumHelper.GetLiteralParameterType
                (
                    this._typeHelper.GetUndelyingTypeForValidList(PInfo.ParameterType)
                );

        /// <summary>
        /// Domain
        /// </summary>
        internal List<string> Domain => this._parameterAttributeReader.GetDomain(PInfo);

        /// <summary>
        /// Control used in the flow diagram editor for a list parameter
        /// </summary>
        internal ListParameterInputStyle ListControl => this._parameterAttributeReader.GetListInputStyle(PInfo);

        /// <summary>
        /// Control used in the flow diagram editor for a literal parameter
        /// </summary>
        internal LiteralParameterInputStyle LiteralControl => this._parameterAttributeReader.GetLiteralInputStyle(PInfo);

        /// <summary>
        /// List Type
        /// </summary>
        internal ListType ListType => _enumHelper.GetListType(this.PInfo.ParameterType);

        internal char[] DefaultValueDelimiters
        {
            get
            {
                this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.DEFAULTVALUEDELIMITER, out string? delimiters);
                return delimiters?.Length > 0 ? delimiters.ToArray() : new char[] { ',', ';' };
            }
        }

        /// <summary>
        /// The default value for a list parameter when the constructor is created in the flow diagram
        /// </summary>
        internal List<string> DefaultValuesForLiteralList
        {
            get
            {
                this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.DEFAULTVALUE, out string? defaultValues);
                if (string.IsNullOrEmpty(defaultValues))
                    return new List<string>();

                return _stringHelper.SplitWithQuoteQualifier(defaultValues, this.DefaultValueDelimiters.Select(p => p.ToString()).ToArray()).ToList();
            }
        }

        /// <summary>
        /// The fully qualified class name whose properties will be used as a domain fro property selection
        /// </summary>
        internal string PropertySource
        {
            get
            {
                this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.PROPERTYSOURCE, out string? value);
                return value ?? string.Empty;
            }
        }

        // <summary>
        /// The parameter whose value will return the fully qualified class name whose properties will be used as a domain fro property selection
        /// </summary>
        internal string PropertySourceParameter
        {
            get
            {
                this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.PROPERTYSOURCEPARAMETER, out string? value);
                return value ?? string.Empty;
            }
        }

        /// <summary>
        /// The Parameter
        /// </summary>
        internal override ParameterBase Parameter => new ListOfLiteralsParameter(this.Name,
                  this.IsOptional,
                  this.Comments,
                  this.Type,
                  this.ListType,
                  this.ListControl,
                  this.LiteralControl,
                  this.PropertySource,
                  this.PropertySourceParameter,
                  this.DefaultValuesForLiteralList,
                  this.DefaultValueDelimiters,
                  this.Domain,
                  this._contextProvider);
        #endregion Properties
    }
}
