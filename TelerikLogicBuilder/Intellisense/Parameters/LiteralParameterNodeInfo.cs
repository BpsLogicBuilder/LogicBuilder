using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class LiteralParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly IEnumHelper _enumHelper;

        internal LiteralParameterNodeInfo(ParameterInfo pInfo, IContextProvider contextProvider, IParameterAttributeReader parameterAttributeReader)
            : base(pInfo, parameterAttributeReader)
        {
            this._contextProvider = contextProvider;
            this._enumHelper = contextProvider.EnumHelper;
        }

        /// <summary>
        /// Parameter type
        /// </summary>
        internal LiteralParameterType Type
        {
            get
            {
                //return MembersInfo.GetParameterType(PInfo.ParameterType.GetUndelyingLiteralType());
                //For a literal parameter this should already be a literal type, its nullable counterpart or a string
                return this._enumHelper.GetLiteralParameterType(PInfo.ParameterType);
            }
        }

        /// <summary>
        /// Domain
        /// </summary>
        internal List<string> Domain
        {
            get { return this._parameterAttributeReader.GetDomain(PInfo); }
        }

        /// <summary>
        /// Control used in the flow diagram editor for a literal parameter
        /// </summary>
        internal LiteralParameterInputStyle LiteralControl
        {
            get
            {
                return this._parameterAttributeReader.GetLiteralInputStyle(PInfo);
            }
        }

        /// <summary>
        /// Use to calculate equality for the constructor object
        /// </summary>
        internal bool UseForEquality
        {
            get
            {
                if (!this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.USEFOREQUALITY, out string? value))
                    return true;

                return !bool.TryParse(value, out bool boolVal) || boolVal;
            }
        }

        /// <summary>
        /// Use in the hash code calculation.
        /// </summary>
        internal bool UseForHashCode
        {
            get
            {
                if (!this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.USEFORHASHCODE, out string? value))
                    return false;

                return bool.TryParse(value, out bool boolVal) && boolVal;
            }
        }

        /// <summary>
        /// Use as part of the object description e.g. when it appears in a list box
        /// </summary>
        internal bool UseForToString
        {
            get
            {
                if (!this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.USEFORTOSTRING, out string? value))
                    return true;

                return !bool.TryParse(value, out bool boolVal) || boolVal;
            }
        }

        /// <summary>
        /// The default value for this parameter when the constructor is created in the flow diagram
        /// </summary>
        internal string DefaultValue
        {
            get
            {
                this._parameterAttributeReader.GetNameValueTable(PInfo).TryGetValue(AttributeNames.DEFAULTVALUE, out string? fromAttribute);
                string fromOptionalParameter = PInfo.IsOptional && PInfo.DefaultValue != null 
                                                        ? PInfo.DefaultValue.ToString()!/*PInfo.DefaultValue is not null*/
                                                        : string.Empty;
                string defaultValue = string.IsNullOrEmpty(fromAttribute) ? fromOptionalParameter : fromAttribute;

                if (this.Type == LiteralParameterType.Boolean || this.Type == LiteralParameterType.NullableBoolean)
                    defaultValue = defaultValue.ToLowerInvariant();

                return defaultValue;
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
        /// The Property whose value will return the fully qualified class name whose properties will be used as a domain fro property selection
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
        internal override ParameterBase Parameter
        {
            get
            {
                return new LiteralParameter(this.Name,
                  this.IsOptional,
                  this.Comments,
                  this.Type,
                  this.LiteralControl,
                  this.UseForEquality,
                  this.UseForHashCode,
                  this.UseForToString,
                  this.PropertySource,
                  this.PropertySourceParameter,
                  this.DefaultValue,
                  this.Domain,
                  this._contextProvider);
            }
        }
    }
}
