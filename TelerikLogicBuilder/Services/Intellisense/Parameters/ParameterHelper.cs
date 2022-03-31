using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters
{
    internal class ParameterHelper : IParameterHelper
    {
        public bool IsParameterAny(ParameterBase parameter) 
            => parameter is LiteralParameter literalParameter && literalParameter.LiteralType == LiteralParameterType.Any;
    }
}
