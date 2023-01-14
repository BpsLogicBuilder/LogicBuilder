using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Factories
{
    internal interface IParametersControlFactory
    {
        IConfigureGenericListParameterControl GetConfigureGenericListParameterControl(IConfigurationForm configurationForm);
        IConfigureGenericParameterControl GetConfigureGenericParameterControl(IConfigurationForm configurationForm);
        IConfigureLiteralListParameterControl GetConfigureLiteralListParameterControl(IConfigurationForm configurationForm);
        IConfigureLiteralParameterControl GetConfigureLiteralParameterControl(IConfigurationForm configurationForm);
        IConfigureObjectListParameterControl GetConfigureObjectListParameterControl(IConfigurationForm configurationForm);
        IConfigureObjectParameterControl GetConfigureObjectParameterControl(IConfigurationForm configurationForm);
    }
}
