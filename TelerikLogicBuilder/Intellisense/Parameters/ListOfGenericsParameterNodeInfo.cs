using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfGenericsParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IParameterAttributeReader _parameterAttributeReader;

        internal ListOfGenericsParameterNodeInfo(ParameterInfo pInfo, IContextProvider contextProvider)
            : base(pInfo, contextProvider.ParameterAttributeReader)
        {
            _contextProvider = contextProvider;
            _typeHelper = contextProvider.TypeHelper;
            _enumHelper = contextProvider.EnumHelper;
            _parameterAttributeReader = contextProvider.ParameterAttributeReader;
        }

        private ListParameterInputStyle ListControl => _parameterAttributeReader.GetListInputStyle(PInfo);

        private Type UnderlyingType => _typeHelper.GetUndelyingTypeForValidList(this.PInfo.ParameterType);

        internal override ParameterBase Parameter => new ListOfGenericsParameter
        (
            this.Name,
            this.IsOptional,
            this.Comments,
            this.UnderlyingType.Name,
            _enumHelper.GetListType(this.PInfo.ParameterType),
            this.ListControl,
            this._contextProvider
        );
    }
}
