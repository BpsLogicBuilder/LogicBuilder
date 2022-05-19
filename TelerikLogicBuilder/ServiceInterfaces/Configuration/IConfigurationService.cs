using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IConfigurationService
    {
        Application? GetApplication(string applicationName);
        Application GetSelectedApplication();
        string GetSelectedApplicationKey();
        void SetSelectedApplication(string applicationName);
        ConstructorList ConstructorList { get; set; }
        ProjectProperties ProjectProperties { get; set; }
        FragmentList FragmentList { get; set; }
        FunctionList FunctionList { get; set; }
        VariableList VariableList { get; set; }
    }
}
