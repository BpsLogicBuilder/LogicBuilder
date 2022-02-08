using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class GenericParameterNodeInfo : ParameterNodeInfoBase
    {
        private readonly IContextProvider _contextProvider;

        internal GenericParameterNodeInfo(ParameterInfo pInfo, IContextProvider contextProvider, IParameterAttributeReader parameterAttributeReader)
            : base(pInfo, parameterAttributeReader)
        {
            _contextProvider = contextProvider;
        }

        internal override ParameterBase Parameter => new GenericParameter
        (
            this.Name,
            this.IsOptional,
            this.Comments,
            this.PInfo.ParameterType.Name,
            this._contextProvider
        );
    }
}
