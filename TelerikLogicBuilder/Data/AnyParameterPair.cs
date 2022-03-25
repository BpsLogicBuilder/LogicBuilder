using System;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class AnyParameterPair
    {
        public AnyParameterPair(Type? parameterOneType, Type? parameterTwoType)
        {
            ParameterOneType = parameterOneType;
            ParameterTwoType = parameterTwoType;
        }

        public Type? ParameterOneType { get; }
        public Type? ParameterTwoType { get; }
    }
}
