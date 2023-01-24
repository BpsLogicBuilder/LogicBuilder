using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType
{
    internal interface IConfigureReturnTypeControl
    {
        ReturnTypeBase ReturnType { get; }
        void SetValues(ReturnTypeBase returnType);
    }
}
