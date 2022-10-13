using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfGenericsParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IParameterFactory _parameterFactory;
        private readonly ITypeHelper _typeHelper;

        internal ListOfGenericsParameterNodeInfo(
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

        private ListParameterInputStyle ListControl => _parameterAttributeReader.GetListInputStyle(PInfo);

        private Type UnderlyingType => _typeHelper.GetUndelyingTypeForValidList(this.PInfo.ParameterType);

        internal override ParameterBase Parameter => _parameterFactory.GetListOfGenericsParameter
        (
            this.Name,
            this.IsOptional,
            this.Comments,
            this.UnderlyingType.Name,
            _enumHelper.GetListType(this.PInfo.ParameterType),
            this.ListControl
        );
    }
}
