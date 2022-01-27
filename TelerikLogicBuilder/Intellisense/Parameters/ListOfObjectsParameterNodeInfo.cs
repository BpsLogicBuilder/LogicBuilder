using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfObjectsParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IParameterAttributeReader _parameterAttributeReader;

        public ListOfObjectsParameterNodeInfo(ParameterInfo pInfo, IContextProvider contextProvider)
            : base(pInfo, contextProvider.ParameterAttributeReader)
        {
            _contextProvider = contextProvider;
            _typeHelper = contextProvider.TypeHelper;
            _enumHelper = contextProvider.EnumHelper;
            _parameterAttributeReader = contextProvider.ParameterAttributeReader;
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
        internal override ParameterBase Parameter => new ListOfObjectsParameter(this.Name,
                  this.IsOptional,
                  this.Comments,
                  this._typeHelper.ToId(this._typeHelper.GetUndelyingTypeForValidList(this.PInfo.ParameterType)),
                  this.ListType,
                  this.ListControl,
                  this._contextProvider);
    }
}
