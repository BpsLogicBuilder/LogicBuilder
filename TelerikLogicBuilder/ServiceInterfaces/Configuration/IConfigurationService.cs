using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IConfigurationService
    {
        Application GetSelectedApplication();
        string GetSelectedApplicationKey();
        ProjectProperties ProjectProperties { get; set; }
    }
}
