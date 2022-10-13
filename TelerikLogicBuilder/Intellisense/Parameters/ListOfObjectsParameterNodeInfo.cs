using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfObjectsParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IParameterFactory _parameterFactory;
        private readonly ITypeHelper _typeHelper;

        public ListOfObjectsParameterNodeInfo(
            IEnumHelper enumHelper,
            IParameterAttributeReader parameterAttributeReader,
            IParameterFactory parameterFactory,
            ITypeHelper typeHelper,
            ParameterInfo pInfo)
            : base(pInfo, parameterAttributeReader)
        {
            _parameterFactory = parameterFactory;
            _typeHelper = typeHelper;
            _enumHelper = enumHelper;
        }

        /// <summary>
        /// Control used in the flow diagram editor for a list parameter
        /// </summary>
        internal ListParameterInputStyle ListControl => _parameterAttributeReader.GetListInputStyle(PInfo);

        /// <summary>
        /// List Type
        /// </summary>
        internal ListType ListType => _enumHelper.GetListType(this.PInfo.ParameterType);

        /// <summary>
        /// The Parameter
        /// </summary>
        internal override ParameterBase Parameter => _parameterFactory.GetListOfObjectsParameter(this.Name,
                  this.IsOptional,
                  this.Comments,
                  this._typeHelper.ToId(this._typeHelper.GetUndelyingTypeForValidList(this.PInfo.ParameterType)),
                  this.ListType,
                  this.ListControl);
    }
}
