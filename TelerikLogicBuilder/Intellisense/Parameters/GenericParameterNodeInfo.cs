using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class GenericParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IParameterFactory _parameterFactory;

        internal GenericParameterNodeInfo(
            IParameterAttributeReader parameterAttributeReader,
            IParameterFactory parameterFactory,
            ParameterInfo pInfo)
            : base(pInfo, parameterAttributeReader)
        {
            _parameterFactory = parameterFactory;
        }

        internal override ParameterBase Parameter => _parameterFactory.GetGenericParameter
        (
            this.Name,
            this.IsOptional,
            this.Comments,
            this.PInfo.ParameterType.Name
        );
    }
}
