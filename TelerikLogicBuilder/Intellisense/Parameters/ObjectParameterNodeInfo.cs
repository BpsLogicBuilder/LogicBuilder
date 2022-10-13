using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ObjectParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IParameterFactory _parameterFactory;
        private readonly ITypeHelper _typeHelper;

        public ObjectParameterNodeInfo(
            IParameterAttributeReader parameterAttributeReader,
            IParameterFactory parameterFactory,
            ITypeHelper typeHelper,
            ParameterInfo pInfo)
            : base(pInfo, parameterAttributeReader)
        {
            _parameterFactory = parameterFactory;
            this._typeHelper = typeHelper;
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
        /// The Parameter
        /// </summary>
        internal override ParameterBase Parameter => _parameterFactory.GetObjectParameter(this.Name,
                  this.IsOptional,
                  this.Comments,
                  this._typeHelper.ToId(this.PInfo.ParameterType),
                  this.UseForEquality,
                  this.UseForHashCode,
                  this.UseForToString);
    }
}
