using ABIS.LogicBuilder.FlowBuilder.Intellisense;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions
{
    internal interface IConfigureFunctionsForm : IConfigurationForm
    {
        HelperStatus? HelperStatus { get; }
    }
}
